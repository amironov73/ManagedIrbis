// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DrawingOptions.cs -- options for WM_PRINT and WM_PRINTCLIENT messagea
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Options for WM_PRINT and WM_PRINTCLIENT messages
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum DrawingOptions
    {
        /// <summary>
        /// Draws the window only if it is visible.
        /// </summary>
        PRF_CHECKVISIBLE = 0x00000001,

        /// <summary>
        /// Draws the nonclient area of the window.
        /// </summary>
        PRF_NONCLIENT = 0x00000002,

        /// <summary>
        /// Draws the client area of the window.
        /// </summary>
        PRF_CLIENT = 0x00000004,

        /// <summary>
        /// Erases the background before drawing the window.
        /// </summary>
        PRF_ERASEBKGND = 0x00000008,

        /// <summary>
        /// Draws all visible children windows.
        /// </summary>
        PRF_CHILDREN = 0x00000010,

        /// <summary>
        /// Draws all owned windows.
        /// </summary>
        PRF_OWNED = 0x00000020
    }
}
