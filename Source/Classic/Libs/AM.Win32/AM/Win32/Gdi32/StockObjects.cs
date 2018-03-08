// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StockObjects.cs -- stock objects of GDI
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Stock objects of GDI.
    /// </summary>
    [PublicAPI]
    public enum StockObjects
    {
        /// <summary>
        /// White brush.
        /// </summary>
        WHITE_BRUSH = 0,

        /// <summary>
        /// Light gray brush.
        /// </summary>
        LTGRAY_BRUSH = 1,

        /// <summary>
        /// Gray brush.
        /// </summary>
        GRAY_BRUSH = 2,

        /// <summary>
        /// Dark gray brush.
        /// </summary>
        DKGRAY_BRUSH = 3,

        /// <summary>
        /// Black brush.
        /// </summary>
        BLACK_BRUSH = 4,

        /// <summary>
        /// Null brush.
        /// </summary>
        NULL_BRUSH = 5,

        /// <summary>
        /// Hollow brush.
        /// </summary>
        HOLLOW_BRUSH = 5,

        /// <summary>
        /// White pen.
        /// </summary>
        WHITE_PEN = 6,

        /// <summary>
        /// Black pen.
        /// </summary>
        BLACK_PEN = 7,

        /// <summary>
        /// Null pen.
        /// </summary>
        NULL_PEN = 8,

        /// <summary>
        /// Original equipment manufacturer (OEM) dependent fixed-pitch 
        /// (monospace) font.
        /// </summary>
        OEM_FIXED_FONT = 10,

        /// <summary>
        /// Windows fixed-pitch (monospace) system font.
        /// </summary>
        ANSI_FIXED_FONT = 11,

        /// <summary>
        /// Windows variable-pitch (proportional space) system font.
        /// </summary>
        ANSI_VAR_FONT = 12,

        /// <summary>
        /// System font. By default, the system uses the system font 
        /// to draw menus, dialog box controls, and text. 
        /// </summary>
        SYSTEM_FONT = 13,

        /// <summary>
        /// Windows NT/2000/XP: Device-dependent font.
        /// </summary>
        DEVICE_DEFAULT_FONT = 14,

        /// <summary>
        /// Default palette. This palette consists of the static colors 
        /// in the system palette.
        /// </summary>
        DEFAULT_PALETTE = 15,

        /// <summary>
        /// Fixed-pitch (monospace) system font. This stock object is 
        /// provided only for compatibility with 16-bit Windows versions 
        /// earlier than 3.0. 
        /// </summary>
        SYSTEM_FIXED_FONT = 16,

        /// <summary>
        /// Default font for user interface objects such as menus and 
        /// dialog boxes. This is MS Sans Serif. Compare this with SYSTEM_FONT.
        /// </summary>
        DEFAULT_GUI_FONT = 17
    }
}
