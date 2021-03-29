using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Linq;
using Template.Base;
using Template.Configurations;
using Template.EntityFrameworkCore;
using Template.Extensions;
using Template.Filters;
using Template.Middleware;
using Template.MultiTenancy;
using Template.Swagger;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace Template
{
    [DependsOn(
        typeof(TemplateHttpApiModule),
        typeof(TemplateApplicationModule),
        typeof(TemplateEntityFrameworkCoreDbMigrationsModule),
        typeof(TemplateSwaggerModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule)
    )]
    public class TemplateHttpApiHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            ConfigureUrls(configuration);
            ConfigureConventionalControllers();
            ConfigureAuthentication(context, configuration);
            ConfigureLocalization();
            ConfigureVirtualFileSystem(context);
            ConfigureCors(context, configuration);
            ConfigureAuthorization(context);
            ConfigureHttpClient(context);
            ConfigureMvcOptions();
        }

        //配置URl跳转
        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
                options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"].Split(','));
            });
        }
        //配置虚拟文件
        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<TemplateDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}Template.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<TemplateDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}Template.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<TemplateApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}Template.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<TemplateApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}Template.Application"));
                });
            }
        }
        //!!!重要 从Application按规约自动生成Api
        private void ConfigureConventionalControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(TemplateApplicationModule).Assembly);
            });
        }
        //认证
        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, //是否验证颁发者
                        ValidateAudience = true, //是否验证访问群体
                        ValidateLifetime = true, //是否验证生存期
                        ClockSkew = TimeSpan.FromSeconds(30), //验证Token的时间偏移量
                        ValidateIssuerSigningKey = true, //是否验证安全密钥
                        ValidAudience = AppSettings.JWT.Domain, //访问群体
                        ValidIssuer = AppSettings.JWT.Domain, //颁发者
                        IssuerSigningKey = new SymmetricSecurityKey(AppSettings.JWT.SecurityKey.GetBytes()) //安全密钥
                    };
                    //应用程序提供的对象，用于处理承载引发的事件，身份验证处理程序
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            //跳过默认的处理逻辑，返回下面的模型数据
                            context.HandleResponse();
                            context.Response.ContentType = "application/json;charset=utf-8";
                            context.Response.StatusCode = StatusCodes.Status200OK;
                            var result = new ServiceResult();
                            result.IsFailed("UnAuthorized");
                            await context.Response.WriteAsync(result.ToJson());
                        }
                    };
                });
        }
        //授权
        private void ConfigureAuthorization(ServiceConfigurationContext context)
        {
            context.Services.AddAuthorization();
        }
        //http请求
        private void ConfigureHttpClient(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClient();
        }

        /*
         * 按理说，他应该会执行到我们写的ExceptionHandlerMiddleware中间件中去
             *但是被我们的Filter进行拦截了，现在我们移除默认的拦截器AbpExceptionFilter
         */
        private void ConfigureMvcOptions()
        {
            Configure<MvcOptions>(options =>
            {
                var filterMetadata = options.Filters.FirstOrDefault(x => x is ServiceFilterAttribute attribute
                                                                         && attribute.ServiceType.Equals(typeof(AbpExceptionFilter)));
                //移除 AbpExceptionFilter
                options.Filters.Remove(filterMetadata);
                //添加自己实现的 TemplateExceptionFilter
                options.Filters.Add(typeof(TemplateExceptionFilter));
            });
        }

        //配置本地资源
        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            });
        }
        //配置跨域
        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                        //.WithOrigins(
                        //    configuration["App:CorsOrigins"]
                        //        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        //        .Select(o => o.RemovePostFix("/"))
                        //        .ToArray()
                        //)
                        //.WithAbpExposedHeaders()
                        //.SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    //.AllowCredentials();
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //本地化资源
            app.UseAbpRequestLocalization();

            app.UseCorrelationId();
            //虚拟文件系统
            app.UseVirtualFiles();
            //路由
            app.UseRouting();
            // 异常处理中间件
            app.UseMiddleware<ExceptionHandleMiddleware>();
            //跨域
            app.UseCors(DefaultCorsPolicyName);
            //身份验证
            app.UseAuthentication();

            //多租户
            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }
            //开启工作单元
            app.UseUnitOfWork();
            //认证授权
            app.UseAuthorization();
            //审核日志
            app.UseAuditing();
            //路由映射
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
