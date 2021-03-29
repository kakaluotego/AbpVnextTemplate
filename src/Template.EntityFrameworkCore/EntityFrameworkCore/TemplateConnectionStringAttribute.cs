/*
 * Copyright © 2020，CloudRoam.com
 * All rights reserved.
 *  
 * 文件名称：TemplateConnectionStringAttribute.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using System;
using Template.Configurations;
using Volo.Abp.Data;

namespace Template.EntityFrameworkCore
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateConnectionStringAttribute : ConnectionStringNameAttribute
    {
        private static readonly string db = AppSettings.EnableDb;
        public TemplateConnectionStringAttribute(string name = "") : base(name)
        {
            Name = string.IsNullOrEmpty(name) ? db : name;

        }

        public new string Name { get; }
    }
}
