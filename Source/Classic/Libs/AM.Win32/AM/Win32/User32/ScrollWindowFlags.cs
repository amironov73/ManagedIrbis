// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ScrollWindowFlags.cs -- flags for ScrollWindowEx function
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Flags for ScrollWindowEx function.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum ScrollWindowFlags
    {
        /// <summary>
        /// Scrolls all child windows that intersect the rectangle pointed
        /// to by the prcScroll parameter. The child windows are scrolled
        /// by the number of pixels specified by the dx and dy parameters.
        /// The system sends a WM_MOVE message to all child windows that
        /// intersect the prcScroll rectangle, even if they do not move.
        /// </summary>
        SW_SCROLLCHILDREN = 0x0001,

        /// <summary>
        /// Invalidates the region identified by the hrgnUpdate parameter
        /// after scrolling.
        /// </summary>
        SW_INVALIDATE = 0x0002,

        /// <summary>
        /// Erases the newly invalidated region by sending a WM_ERASEBKGND
        /// message to the window when specified with the SW_INVALIDATE flag.
        /// </summary>
        SW_ERASE = 0x0004,

        /// <summary>
        /// Windows 98/Me, Windows 2000/XP: Scrolls using smooth scrolling.
        /// Use the HIWORD portion of the flags parameter to indicate how
        /// much time the smooth-scrolling operation should take.
        /// </summary>
        SW_SMOOTHSCROLL = 0x0010
    }
}
