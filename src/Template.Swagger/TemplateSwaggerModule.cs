/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：TemplateSwaggerModule.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using Microsoft.AspNetCore.Builder;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Template.Swagger
{
    [DependsOn(
        typeof(TemplateDomainModule)
        )]
    public class TemplateSwaggerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSwagger();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.GetApplicationBuilder().UseSwagger().UseSwaggerUI();
        }
    }
}
