/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：ListResult.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template.Base.Paged
{
    public class ListResult<T> : IListResult<T>
    {
        IReadOnlyList<T> item;

        public ListResult()
        {

        }

        public ListResult(IReadOnlyList<T> item)
        {
            Item = item;
        }
        public IReadOnlyList<T> Item
        {
            get => item ?? (item = new List<T>());
            set => item = value;
        }
    }
}
