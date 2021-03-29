/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：GitHubConfig.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

namespace Template.Configurations
{
    public class GitHubConfig
    {
        /// <summary>
        /// GET请求，跳转GitHub登录界面，获取用户授权，得到code
        /// </summary>
        public static string API_Authorize = "https://github.com/login/oauth/authorize";
        /// <summary>
        /// POST请求，根据code得到access_token
        /// </summary>
        public static string API_AccessToken = "https://github.com/login/oauth/access_token";

        /// <summary>
        /// GET请求，根据access_token得到用户信息
        /// </summary>
        public static string API_User = "https://api.github.com/user";
        /// <summary>
        /// Github UserId
        /// </summary>
        public static int UserId = AppSettings.Github.UserId;
        /// <summary>
        /// Client ID
        /// </summary>
        public static string Client_ID = AppSettings.Github.Client_ID;
        /// <summary>
        /// Client Secret
        /// </summary>
        public static string Client_Secret = AppSettings.Github.Client_Secret;
        /// <summary>
        /// Authorization callback URL
        /// </summary>
        public static string Redirect_Uri = AppSettings.Github.Redirect_Uri;
        /// <summary>
        /// Application name
        /// </summary>
        public static string ApplicationName = AppSettings.Github.ApplicationName;


    }
}
