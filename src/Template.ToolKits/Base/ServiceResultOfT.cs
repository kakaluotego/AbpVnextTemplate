/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：ServiceResultOfT.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using Template.Base.Enum;

namespace Template.Base
{
    public class ServiceResult<T> : ServiceResult where T : class
    {
        public T Result { get; set; }

        public void IsSuccess(T result = null, string message = "")
        {
            Message = message;
            Code = ServiceResultCode.Succeed;
            Result = result;

        }
    }
}
