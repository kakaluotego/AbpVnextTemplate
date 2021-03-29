/*
 * Copyright © 2021，Company.com
 * All rights reserved.
 *  
 * 文件名称：IMayHaveCreatorNoGuid.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

namespace Template.Auditing
{
    public interface IMayHaveCreatorNoGuid
    {
        long? CreatorId { get; }
    }
}
