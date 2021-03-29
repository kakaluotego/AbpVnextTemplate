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

using System;

namespace Template.GitHub
{
    public class UserResponse
    {
        public string Login { get; set; }
        public int Id { get; set; }
        public string Avatar_url { get; set; }
        public string Html_url { get; set; }
        public string Repos_url { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Blog { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public int Public_repos { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }

    }
}
