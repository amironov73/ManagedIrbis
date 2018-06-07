// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SerializableAttribute.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if UAP

using System;

namespace System
{
    /// <summary>
    /// For compatibility.
    /// </summary>
    public sealed class SerializableAttribute
        : Attribute
    {
    }

    /// <summary>
    /// For compatibility.
    /// </summary>
    public sealed class NonSerializedAttribute
        : Attribute
    {

    }
}

#endif
