// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NMMOUSE.cs -- mouse notification message
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Contains information used with mouse notification messages.
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct NMMOUSE
    {
        /// <summary>
        /// <see cref="NMHDR" /> structure that contains additional
        /// information about this notification.
        /// </summary>
        public NMHDR hdr;

        /// <summary>
        /// Control-specific item identifier.
        /// </summary>
        public IntPtr dwItemSpec;

        /// <summary>
        /// Control-specific item data.
        /// </summary>
        public IntPtr dwItemData;

        /// <summary>
        /// <see cref="Point" /> structure that contains the screen
        /// coordinates of the mouse when the click occurred.
        /// </summary>
        public Point pt;

        /// <summary>
        /// Carries information about where on the item or control the
        /// cursor is pointing.
        /// </summary>
        public IntPtr dwHitInfo;
    }
}
