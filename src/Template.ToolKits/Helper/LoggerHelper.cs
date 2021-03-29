/*
 * Copyright © 2020，CloudRoam.com
 * All rights reserved.
 *  
 * 文件名称：LoggerHelper.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;

namespace Template.Helper
{
    public static class LoggerHelper
    {
        private static readonly ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
        private static readonly ILog log = LogManager.GetLogger(repository.Name, "NETCorelog4net");

        static LoggerHelper()
        {
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message"></param>
        public static void WriteToFile(string message)
        {
            log.Info(message);
        }
        /// <summary>
        /// 写错误日志 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void WriteToFile(string message, Exception ex)
        {
            if (string.IsNullOrEmpty(message))
                message = ex.Message;
            log.Error(message,ex);
        }

    }
}
