/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：IHasTotalCount.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Base.Paged
{
    public interface IHasTotalCount
    {
        int Total { get; set; }
    }
}
