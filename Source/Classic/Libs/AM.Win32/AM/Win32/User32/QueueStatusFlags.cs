// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* QueueStatusFlags.cs -- specifies the types of messages for which to check the queue
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Specifies the types of messages for which to check the queue.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum QueueStatusFlags
    {
        /// <summary>
        /// A WM_KEYUP, WM_KEYDOWN, WM_SYSKEYUP, or WM_SYSKEYDOWN message
        /// is in the queue.
        /// </summary>
        QS_KEY = 0x0001,

        /// <summary>
        /// A WM_MOUSEMOVE message is in the queue.
        /// </summary>
        QS_MOUSEMOVE = 0x0002,

        /// <summary>
        /// A mouse-button message (WM_LBUTTONUP, WM_RBUTTONDOWN, and so on).
        /// </summary>
        QS_MOUSEBUTTON = 0x0004,

        /// <summary>
        /// A posted message (other than those listed here) is in the queue.
        /// </summary>
        QS_POSTMESSAGE = 0x0008,

        /// <summary>
        /// A WM_TIMER message is in the queue.
        /// </summary>
        QS_TIMER = 0x0010,

        /// <summary>
        /// A WM_PAINT message is in the queue.
        /// </summary>
        QS_PAINT = 0x0020,

        /// <summary>
        /// A message sent by another thread or application is in the queue.
        /// </summary>
        QS_SENDMESSAGE = 0x0040,

        /// <summary>
        /// A WM_HOTKEY message is in the queue.
        /// </summary>
        QS_HOTKEY = 0x0080,

        /// <summary>
        /// A posted message (other than those listed here) is in the queue.
        /// </summary>
        QS_ALLPOSTMESSAGE = 0x0100,

        /// <summary>
        /// Windows XP: A raw input message is in the queue.
        /// </summary>
        QS_RAWINPUT = 0x0400,

        /// <summary>
        /// A WM_MOUSEMOVE message or mouse-button message (WM_LBUTTONUP,
        /// WM_RBUTTONDOWN, and so on).
        /// </summary>
        QS_MOUSE = QS_MOUSEMOVE
                   | QS_MOUSEBUTTON,

        /// <summary>
        /// <para>An input message is in the queue. This is composed
        /// of QS_KEY and QS_MOUSE.</para>
        /// <para>Windows XP: This also includes QS_RAWINPUT.</para>
        /// </summary>
        QS_INPUT = QS_MOUSE
                   | QS_KEY
                   | QS_RAWINPUT,

        /// <summary>
        /// An input, WM_TIMER, WM_PAINT, WM_HOTKEY, or posted message
        /// is in the queue.
        /// </summary>
        QS_ALLEVENTS = QS_INPUT
                       | QS_POSTMESSAGE
                       | QS_TIMER
                       | QS_PAINT
                       | QS_HOTKEY,

        /// <summary>
        /// Any message is in the queue.
        /// </summary>
        QS_ALLINPUT = QS_INPUT
                      | QS_POSTMESSAGE
                      | QS_TIMER
                      | QS_PAINT
                      | QS_HOTKEY
                      | QS_SENDMESSAGE
    }
}
