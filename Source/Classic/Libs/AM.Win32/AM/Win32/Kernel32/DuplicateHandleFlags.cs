// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DuplicateHandleFlags.cs -- options for DuplicateHandle function.
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Options for DuplicateHandle function.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum DuplicateHandleFlags
    {
        /// <summary>
        /// Closes the source handle. This occurs regardless of 
        /// any error status returned.
        /// </summary>
        DUPLICATE_CLOSE_SOURCE = 0x00000001,

        /// <summary>
        /// Ignores the dwDesiredAccess parameter. The duplicate handle 
        /// has the same access as the source handle.
        /// </summary>
        DUPLICATE_SAME_ACCESS = 0x00000002
    }
}
