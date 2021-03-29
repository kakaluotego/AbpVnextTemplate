/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：PagedList.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using Template.Configurations;

namespace Template.GitHub
{
    public class AccessTokenRequest
    {
        public  string Client_ID = GitHubConfig.Client_ID;
        public  string Client_Secret = GitHubConfig.Client_Secret;
        /// <summary>
        /// 调用API_Authorize获取到的Code值
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Authorization callback URL
        /// </summary>
        public string Redirect_Uri = GitHubConfig.Redirect_Uri;
        public string State { get; set; }


    }
}
