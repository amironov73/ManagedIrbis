// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LowLevelKeyboardHookFlags.cs -- flags for low-level keyboard hook
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Specifies the extended-key flag, event-injected flag, 
    /// context code, and transition-state flag for low-level
    /// keyboard hook procedure.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum LowLevelKeyboardHookFlags
    {
        /// <summary>
        /// ???
        /// </summary>
        KF_EXTENDED = 0x0100,

        /// <summary>
        /// ???
        /// </summary>
        KF_DLGMODE = 0x0800,

        /// <summary>
        /// ???
        /// </summary>
        KF_MENUMODE = 0x1000,

        /// <summary>
        /// ???
        /// </summary>
        KF_ALTDOWN = 0x2000,

        /// <summary>
        /// ???
        /// </summary>
        KF_REPEAT = 0x4000,

        /// <summary>
        /// ???
        /// </summary>
        KF_UP = 0x8000,

        /// <summary>
        /// Test the extended-key flag.
        /// </summary>
        LLKHF_EXTENDED = KF_EXTENDED >> 8,

        /// <summary>
        /// Test the event-injected flag.
        /// </summary>
        LLKHF_INJECTED = 0x00000010,

        /// <summary>
        /// Test the context code.
        /// </summary>
        LLKHF_ALTDOWN = KF_ALTDOWN >> 8,

        /// <summary>
        /// Test the transition-state flag.
        /// </summary>
        LLKHF_UP = KF_UP >> 8
    }
}
