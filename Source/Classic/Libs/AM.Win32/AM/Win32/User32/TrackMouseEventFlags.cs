// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TrackMouseEventFlags.cs -- specifies the services requested for TrackMouseEvent function
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Specifies the services requested for TrackMouseEvent
    /// function.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum TrackMouseEventFlags
    {
        /// <summary>
        /// <para>The caller wants hover notification. Notification
        /// is delivered as a WM_MOUSEHOVER message.</para>
        /// <para>If the caller requests hover tracking while hover
        /// tracking is already active, the hover timer will be reset.
        /// </para>
        /// <para>This flag is ignored if the mouse pointer is not over
        /// the specified window or area.</para>
        /// </summary>
        TME_HOVER = 0x00000001,

        /// <summary>
        /// The caller wants leave notification. Notification is
        /// delivered as a WM_MOUSELEAVE message. If the mouse is
        /// not over the specified window or area, a leave notification
        /// is generated immediately and no further tracking is
        /// performed.
        /// </summary>
        TME_LEAVE = 0x00000002,

        /// <summary>
        /// Windows 98/Me, Windows 2000/XP: The caller wants hover and
        /// leave notification for the nonclient areas. Notification
        /// is delivered as WM_NCMOUSEHOVER and WM_NCMOUSELEAVE messages.
        /// </summary>
        TME_NONCLIENT = 0x00000010,

        /// <summary>
        /// The function fills in the structure instead of treating it
        /// as a tracking request. The structure is filled such that
        /// had that structure been passed to TrackMouseEvent, it would
        /// generate the current tracking. The only anomaly is that the
        /// hover time-out returned is always the actual time-out and
        /// not HOVER_DEFAULT, if HOVER_DEFAULT was specified during
        /// the original TrackMouseEvent request.
        /// </summary>
        TME_QUERY = 0x40000000,

        /// <summary>
        /// The caller wants to cancel a prior tracking request.
        /// The caller should also specify the type of tracking that
        /// it wants to cancel. For example, to cancel hover tracking,
        /// the caller must pass the TME_CANCEL and TME_HOVER flags.
        /// </summary>
        TME_CANCEL = unchecked ( (int) 0x80000000 )
    }
}
