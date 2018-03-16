// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NMHDR.cs -- notification message
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Contains information about a notification message.
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct NMHDR
    {
        /// <summary>
        /// Window handle to the control sending a message.
        /// </summary>
        public IntPtr hwndFrom;

        /// <summary>
        /// Identifier of the control sending a message.
        /// </summary>
        [CLSCompliant(false)]
        public uint idFrom;

        /// <summary>
        /// Notification code. This member can be a control-specific
        /// notification code or it can be one of the common notification
        /// codes.
        /// </summary>
        public int code;
    }
}
