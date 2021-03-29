/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：ServiceResult.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using System;

namespace Template.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
        /// <summary>
        /// 判断对象是否不为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        public static void ThrowIfNull(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }



    }
}
