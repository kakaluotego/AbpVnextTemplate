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

        //??????URl??????
        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
                options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"].Split(','));
            });
        }
        //??????????????????
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
        //!!!?????? ???Application?????????????????????Api
        private void ConfigureConventionalControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(TemplateApplicationModule).Assembly);
            });
        }
        //??????
        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, //?????????????????????
                        ValidateAudience = true, //????????????????????????
                        ValidateLifetime = true, //?????????????????????
                        ClockSkew = TimeSpan.FromSeconds(30), //??????Token??????????????????
                        ValidateIssuerSigningKey = true, //????????????????????????
                        ValidAudience = AppSettings.JWT.Domain, //????????????
                        ValidIssuer = AppSettings.JWT.Domain, //?????????
                        IssuerSigningKey = new SymmetricSecurityKey(AppSettings.JWT.SecurityKey.GetBytes()) //????????????
                    };
                    //??????????????????????????????????????????????????????????????????????????????????????????
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            //?????????????????????????????????????????????????????????
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
        //??????
        private void ConfigureAuthorization(ServiceConfigurationContext context)
        {
            context.Services.AddAuthorization();
        }
        //http??????
        private void ConfigureHttpClient(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClient();
        }

        /*
         * ?????????????????????????????????????????????ExceptionHandlerMiddleware???????????????
             *??????????????????Filter??????????????????????????????????????????????????????AbpExceptionFilter
         */
        private void ConfigureMvcOptions()
        {
            Configure<MvcOptions>(options =>
            {
                var filterMetadata = options.Filters.FirstOrDefault(x => x is ServiceFilterAttribute attribute
                                                                         && attribute.ServiceType.Equals(typeof(AbpExceptionFilter)));
                //?????? AbpExceptionFilter
                options.Filters.Remove(filterMetadata);
                //????????????????????? TemplateExceptionFilter
                options.Filters.Add(typeof(TemplateExceptionFilter));
            });
        }

        //??????????????????
        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "????????????"));
            });
        }
        //????????????
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
            //???????????????
            app.UseAbpRequestLocalization();

            app.UseCorrelationId();
            //??????????????????
            app.UseVirtualFiles();
            //??????
            app.UseRouting();
            // ?????????????????????
            app.UseMiddleware<ExceptionHandleMiddleware>();
            //??????
            app.UseCors(DefaultCorsPolicyName);
            //????????????
            app.UseAuthentication();

            //?????????
            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }
            //??????????????????
            app.UseUnitOfWork();
            //????????????
            app.UseAuthorization();
            //????????????
            app.UseAuditing();
            //????????????
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
