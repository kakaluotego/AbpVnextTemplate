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

using System.Collections.Generic;
using Template.Base.Paged;

namespace Template.Base
{
    public class PagedList<T> : ListResult<T>, IPagedList<T>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }

        public PagedList()
        {

        }

        public PagedList(int total, IReadOnlyList<T> result) : base(result)
        {
            Total = total;
        }


    }
}
