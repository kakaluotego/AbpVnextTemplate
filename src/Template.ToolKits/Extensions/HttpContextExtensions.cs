/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：ServiceResult.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Template.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 获取客户端Ip
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientIp(this HttpRequest request)
        {
            var ip = request.Headers["X-Real-IP"].FirstOrDefault() ??
                     request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                     request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            return ip;
        }


    }
}
