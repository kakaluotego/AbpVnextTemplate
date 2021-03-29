/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：ICreationAuditedObjectNoGuid.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using Volo.Abp.Auditing;

namespace Template.Auditing
{
    public interface ICreationAuditedObjectNoGuid : IHasCreationTime,IMayHaveCreatorNoGuid
    {

    }
}
