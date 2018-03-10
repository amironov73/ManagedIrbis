// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CHARFORMAT.cs -- contains information about character formatting in a rich edit control
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Contains information about character formatting in a rich edit control.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [StructLayout ( LayoutKind.Sequential )]
    public struct CHARFORMAT
    {
        /// <summary>
        /// Size in bytes of the specified structure.
        /// This member must be set before passing the structure
        /// to the rich edit control.
        /// </summary>
        public int cbSize;

        /// <summary>
        /// Members containing valid information or attributes to set.
        /// </summary>
        public int dwMask;

        /// <summary>
        /// Character effects.
        /// </summary>
        public int dwEffects;

        /// <summary>
        /// Character height, in twips (1/1440 of an inch or 1/20 of a printer's point).
        /// </summary>
        public int yHeight;

        /// <summary>
        /// Character offset, in twips, from the baseline.
        /// If the value of this member is positive, the character is a superscript;
        /// if it is negative, the character is a subscript.
        /// </summary>
        public int yOffset;

        /// <summary>
        /// Text color. This member is ignored if the CFE_AUTOCOLOR
        /// character effect is specified. To generate a COLORREF,
        /// use the RGB macro.
        /// </summary>
        public int crTextColor;

        /// <summary>
        /// Character set value. The bCharSet member can be one
        /// of the values specified for the lfCharSet member
        /// of the LOGFONT structure. Microsoft Rich Edit 3.0
        /// may override this value if it is invalid for the target characters.
        /// </summary>
        public byte bCharSet;

        /// <summary>
        /// Font family and pitch. This member is the same as the
        /// lfPitchAndFamily member of the LOGFONT structure.
        /// </summary>
        public byte bPitchAndFamily;

        /// <summary>
        ///Null-terminated character array specifying the font name.
        /// </summary>
        [MarshalAs ( UnmanagedType.ByValTStr, SizeConst = 32 )]
        public string szFaceName;
    }
}
