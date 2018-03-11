// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EnableScrollBarFlags.cs -- options for EnableScrollBar method
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Options for EnableScrollBar method.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum EnableScrollBarFlags
    {
        /// <summary>
        /// Enables both arrows on a scroll bar.
        /// </summary>
        ESB_ENABLE_BOTH = 0x0000,

        /// <summary>
        /// Disables both arrows on a scroll bar.
        /// </summary>
        ESB_DISABLE_BOTH = 0x0003,

        /// <summary>
        /// Disables the left arrow on a horizontal scroll bar.
        /// </summary>
        ESB_DISABLE_LEFT = 0x0001,

        /// <summary>
        /// Disables the right arrow on a horizontal scroll bar.
        /// </summary>
        ESB_DISABLE_RIGHT = 0x0002,

        /// <summary>
        /// Disables the up arrow on a vertical scroll bar.
        /// </summary>
        ESB_DISABLE_UP = 0x0001,

        /// <summary>
        /// Disables the down arrow on a vertical scroll bar.
        /// </summary>
        ESB_DISABLE_DOWN = 0x0002,

        /// <summary>
        /// Disables the left arrow on a horizontal scroll bar
        /// or the up arrow of a vertical scroll bar.
        /// </summary>
        ESB_DISABLE_LTUP = ESB_DISABLE_LEFT,

        /// <summary>
        /// Disables the right arrow on a horizontal scroll bar
        /// or the down arrow of a vertical scroll bar.
        /// </summary>
        ESB_DISABLE_RTDN = ESB_DISABLE_RIGHT
    }
}
