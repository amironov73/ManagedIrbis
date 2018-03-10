// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GetObjectOptions.cs -- flags for IRichEditOle::GetObject method
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Flags for IRichEditOle::GetObject method.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum GetObjectOptions
    {
        /// <summary>
        /// Get no interfaces.
        /// </summary>
        REO_GETOBJ_NO_INTERFACES = 0x00000000,

        /// <summary>
        /// Get object interface.
        /// </summary>
        REO_GETOBJ_POLEOBJ = 0x00000001,

        /// <summary>
        /// Get storage interface.
        /// </summary>
        REO_GETOBJ_PSTG = 0x00000002,

        /// <summary>
        /// Get site interface.
        /// </summary>
        REO_GETOBJ_POLESITE = 0x00000004,

        /// <summary>
        /// Get all interfaces.
        /// </summary>
        REO_GETOBJ_ALL_INTERFACES = 0x00000007
    }
}
