/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：IListResult.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using System.Collections.Generic;

namespace Template.Base.Paged
{
    public interface IListResult<T>
    {
        IReadOnlyList<T> Item { get; set; }
    }
}
