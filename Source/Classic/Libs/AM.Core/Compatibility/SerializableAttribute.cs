// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SerializableAttribute.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !CLASSIC && !NETCORE

using System;

namespace System
{
    public sealed class SerializableAttribute
        : Attribute
    {
    }

    public sealed class NonSerializedAttribute
        : Attribute
    {
        
    }
}

#endif
