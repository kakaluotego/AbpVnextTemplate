/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：IDependency1.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using Microsoft.AspNetCore.Mvc.Filters;
using Template.Helper;

namespace Template.Filters
{
    public class TemplateExceptionFilter : IExceptionFilter
    {
        public TemplateExceptionFilter()
        {
            
        }

        public void OnException(ExceptionContext context)
        {
            //日志记录
            LoggerHelper.WriteToFile($"{context.HttpContext.Request.Path} | {context.Exception.Message}", context.Exception);
        }


    }
}
