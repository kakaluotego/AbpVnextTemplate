/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：KakaBlogSwaggerExtensions.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Template.Configurations;
using Template.Swagger.Filters;

namespace Template.Swagger
{
    public static class KakaBlogSwaggerExtensions
    {
        public static string version = AppSettings.ApiVersion;
        public static string description = "这是我的接口";

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                //options.SwaggerDoc("v1", new OpenApiInfo
                //{
                //    Version = "1.0.0",
                //    Title = "我的接口",
                //    Description = "接口描述"
                //});
                //遍历并应用Swagger分组信息
                ApiInfos.ForEach(x =>
                {
                    options.SwaggerDoc(x.UrlPrefix, x.OpenApiInfo);
                });

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Template.HttpApi.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Template.Domain.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Template.Application.Contracts.xml"));
                //应用Controller的API文档描述信息
                options.DocumentFilter<SwaggerDocumentFilter>();
                //开启小绿锁
                /*
                 *请注意认证与授权是不同的意思，简单理解：认证，是证明你的身份，你有账号密码，你可以登录进我们的系统，
                 * 说明你认证成功了；授权，即权限，分配给用户某一权限标识，用户得到什么什么权限，才能使用系统的某一功能，就是授权
                 */
                var security = new OpenApiSecurityScheme
                {
                    Description = "JWT模式授权，请输入 Bearer {Token} 进行身份验证",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                options.AddSecurityDefinition("oauth2", security);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement { { security, new List<string>() } });
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();

            });
        }

        public static void UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(options =>
            {
                //options.SwaggerEndpoint($"/swagger/v1/swagger.json", "默认接口");
                // 遍历分组信息，生成Json
                ApiInfos.ForEach(x =>
                {
                    options.SwaggerEndpoint($"/swagger/{x.UrlPrefix}/swagger.json", x.Name);
                });
                // 模型的默认扩展深度，设置为 -1 完全隐藏模型
                options.DefaultModelsExpandDepth(-1);
                // API文档仅展开标记
                options.DocExpansion(DocExpansion.List);
                // API前缀设置为空
                options.RoutePrefix = string.Empty;
                // API页面Title
                options.DocumentTitle = "😍接口文档 - kakaluote⭐⭐⭐";

            });

        }


        internal class SwaggerApiInfo
        {
            /// <summary>
            /// URL前缀
            /// </summary>
            public string UrlPrefix { get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// <see cref="Microsoft.OpenApi.Models.OpenApiInfo"/>
            /// </summary>
            public OpenApiInfo OpenApiInfo { get; set; }
        }

        private static readonly List<SwaggerApiInfo> ApiInfos = new List<SwaggerApiInfo>()
        {
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.GroupName_v1,
                Name = "博客前台接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "kaka - 博客前台接口",
                    Description = description
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.GroupName_v2,
                Name = "博客后台接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "kaka - 博客后台接口",
                    Description = description
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.GroupName_v3,
                Name = "通用公共接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "kaka - 通用公共接口",
                    Description = description
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.GroupName_v4,
                Name = "JWT授权接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "kaka - JWT授权接口",
                    Description = description
                }
            }
        };


    }
}
