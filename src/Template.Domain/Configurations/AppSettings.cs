/*
 * Copyright © 2020，CloudRoam.com
 * All rights reserved.
 *  
 * 文件名称：AppSettings.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Template.Configurations
{
    public class AppSettings
    {
        private static readonly IConfigurationRoot _config;
        static AppSettings()
        {
            // 加载appsettings.json，并构建IConfigurationRoot
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true);
            _config = builder.Build();

        }
        public static string EnableDb => _config["ConnectionStrings:Enable"];   //MySQL

        public static string ConnectionStrings => _config.GetConnectionString(EnableDb);
        public static string ApiVersion => _config["ApiVersion"];
        /// <summary>
        /// 监听端口
        /// </summary>
        public static string ListenPort => _config["listenPort"];

        public static class JWT
        {
            public static string Domain => _config["JWT:Domain"];
            public static string SecurityKey => _config["JWT:SecurityKey"];
            public static int Expires => Convert.ToInt32(_config["JWT:Expires"]);
        }
        /// <summary>
        /// Caching
        /// </summary>
        public static class Caching
        {
            public static string RedisConnectionString => _config["Caching:RedisConnectionString"];
            public static bool IsOpen => Convert.ToBoolean(_config["Caching:IsOpen"]);
        }

        /// <summary>
        /// GitHub
        /// </summary>
        public static class Github
        {
            public static int UserId => Convert.ToInt32(_config["Github:UserId"]);

            public static string Client_ID => _config["Github:ClientID"];
            public static string Client_Secret => _config["Github:ClientSecret"];
            public static string Redirect_Uri => _config["Github:RedirectUri"];
            public static string ApplicationName => _config["Github:ApplicationName"];

        }

        /// <summary>
        /// Email配置
        /// </summary>
        public static class Email
        {
            public static string Host => _config["Settings:Abp.Mailing.Smtp.Host"];
            public static int Port => Convert.ToInt32(_config["Settings:Abp.Mailing.Smtp.Port"]);
            public static bool UseSsl => Convert.ToBoolean(_config["Settings:Abp.Mailing.Smtp.EnableSsl"]);
            public static class From
            {
                public static string Username => _config["Settings:Abp.Mailing.Smtp.UserName"];
                public static string Password => _config["Settings:Abp.Mailing.Smtp.Password"];
                public static string Name => _config["Settings:Abp.Mailing.DefaultFromDisplayName"];
                public static string Address => _config["Settings:Abp.Mailing.DefaultFromAddress"];

            }

        }

    }
}
