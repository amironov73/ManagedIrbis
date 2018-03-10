// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DialogCodes.cs -- used with WM_GETDLGCODE
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Used with WM_GETDLGCODE to say what task messages a control wants
    /// to process.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum DialogCodes
    {
        /// <summary>
        /// Arrow keys.
        /// </summary>
        DLGC_WANTARROWS = 0x0001,

        /// <summary>
        /// Tab key.
        /// </summary>
        DLGC_WANTTAB = 0x0002,

        /// <summary>
        /// All keyboard input.
        /// </summary>
        DLGC_WANTALLKEYS = 0x0004,

        /// <summary>
        /// All keboard input. Passes this message on to the control.
        /// </summary>
        DLGC_WANTMESSAGE = 0x0004,

        /// <summary>
        /// EM_SETSEL messages.
        /// </summary>
        DLGC_HASSETSEL = 0x0008,

        /// <summary>
        /// Default pushbutton messages.
        /// </summary>
        DLGC_DEFPUSHBUTTON = 0x0010,

        /// <summary>
        /// No default pushbutton processing.
        /// </summary>
        DLGC_UNDEFPUSHBUTTON = 0x0020,

        /// <summary>
        /// Radio button.
        /// </summary>
        DLGC_RADIOBUTTON = 0x0040,

        /// <summary>
        /// WM_CHAR messages.
        /// </summary>
        DLGC_WANTCHARS = 0x0080,

        /// <summary>
        /// Static control.
        /// </summary>
        DLGC_STATIC = 0x0100,

        /// <summary>
        /// Button (generic).
        /// </summary>
        DLGC_BUTTON = 0x2000
    }
}
