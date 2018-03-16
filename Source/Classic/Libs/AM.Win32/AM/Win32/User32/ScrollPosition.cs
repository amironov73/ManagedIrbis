// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ScrollPosition.cs --
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

// ReSharper disable InconsistentNaming

#endregion

namespace AM.Win32
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public enum ScrollPosition
    {
        /// <summary>
        /// Scrolls one line up.
        /// </summary>
        SB_LINEUP = 0,

        /// <summary>
        /// Scrolls left by one unit.
        /// </summary>
        SB_LINELEFT = 0,

        /// <summary>
        /// Scrolls one line down.
        /// </summary>
        SB_LINEDOWN = 1,

        /// <summary>
        /// Scrolls right by one unit.
        /// </summary>
        SB_LINERIGHT = 1,

        /// <summary>
        /// Scrolls one page up.
        /// </summary>
        SB_PAGEUP = 2,

        /// <summary>
        /// Scrolls left by the width of the window.
        /// </summary>
        SB_PAGELEFT = 2,

        /// <summary>
        /// Scrolls one page down.
        /// </summary>
        SB_PAGEDOWN = 3,

        /// <summary>
        /// Scrolls right by the width of the window.
        /// </summary>
        SB_PAGERIGHT = 3,

        /// <summary>
        /// The user has dragged the scroll box (thumb) and released
        /// the mouse button. The high-order word indicates the position
        /// of the scroll box at the end of the drag operation.
        /// </summary>
        SB_THUMBPOSITION = 4,

        /// <summary>
        /// The user is dragging the scroll box. This message is sent
        /// repeatedly until the user releases the mouse button. The
        /// high-order word indicates the position that the scroll box
        /// has been dragged to.
        /// </summary>
        SB_THUMBTRACK = 5,

        /// <summary>
        /// Scrolls to the upper left.
        /// </summary>
        SB_TOP = 6,

        /// <summary>
        /// Scrolls to the upper left.
        /// </summary>
        SB_LEFT = 6,

        /// <summary>
        /// Scrolls to the lower right.
        /// </summary>
        SB_BOTTOM = 7,

        /// <summary>
        /// Scrolls to the lower right.
        /// </summary>
        SB_RIGHT = 7,

        /// <summary>
        /// Ends scroll.
        /// </summary>
        SB_ENDSCROLL = 8
    }
}
