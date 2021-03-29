/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：SwaggerDocumentFilter.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Template.Swagger.Filters
{
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var tags = new List<OpenApiTag>()
            {
                new OpenApiTag
                {
                    Name = "Blog",
                    Description = "个人博客相关接口",
                    ExternalDocs = new OpenApiExternalDocs() {Description = "包含：文章/标签/分类/友链"}
                },
                new OpenApiTag
                {
                    Name = "HelloWorld",
                    Description = "通用公共接口",
                    ExternalDocs = new OpenApiExternalDocs {Description = "这里是一些通用的公共接口"}
                }
             
            };

            #region 实现添加自定义描述时过滤不属于同一个分组的API

            var groupName = context.ApiDescriptions.FirstOrDefault().GroupName;

            var apis = context.ApiDescriptions.GetType().GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(context.ApiDescriptions) as IEnumerable<ApiDescription>;

            var controllers = apis.Where(x => x.GroupName != groupName).Select(x => ((ControllerActionDescriptor)x.ActionDescriptor).ControllerName).Distinct();
            swaggerDoc.Tags = tags.Where(x => !controllers.Contains(x.Name)).OrderBy(x => x.Name).ToList();

            #endregion

        }
    }
}
