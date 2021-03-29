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

using log4net;
using log4net.Config;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace Template.Extensions
{
    /// <summary>
    /// 在日志记录中使用的静态方法有人指出写法不是很优雅，遂优化一下上一篇中日志记录的方法，具体操作如下
    /// </summary>
    public static class Log4NetExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IHostBuilder UseLog4Net(this IHostBuilder hostBuilder)
        {
            var log4netRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(log4netRepository, new FileInfo("Resources/log4net.config"));
            return hostBuilder;

        }
    }
}
