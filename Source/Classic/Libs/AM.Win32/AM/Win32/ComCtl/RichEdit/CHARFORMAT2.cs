// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CHARFORMAT2.cs -- contains information about character formatting in a rich edit control
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
    [StructLayout(LayoutKind.Sequential)]
    public struct CHARFORMAT2
    {
        /// <summary>
        /// Specifies the size, in bytes, of the structure.
        /// </summary>
        public int cbSize;

        /// <summary>
        /// Specifies the parts of the CHARFORMAT2 structure
        /// that contain valid information.
        /// </summary>
        public int dwMask;

        /// <summary>
        /// A set of bit flags that specify character effects.
        /// Some of the flags are included only for compatibility
        /// with Microsoft Text Object Model (TOM) interfaces;
        /// the rich edit control stores the value but does
        /// not use it to display text.
        /// </summary>
        public int dwEffects;

        /// <summary>
        /// Specifies the character height, in twips (1/1440 of an inch,
        /// or 1/20 of a printer's point). To use this member,
        /// set the CFM_SIZE flag in the dwMask member.
        /// </summary>
        public int yHeight;

        /// <summary>
        /// Character offset from the baseline, in twips.
        /// If the value of this member is positive, the character
        /// is a superscript; if the value is negative, the character
        /// is a subscript. To use this member, set the CFM_OFFSET
        /// flag in the dwMask member.
        /// </summary>
        public int yOffset;

        /// <summary>
        /// Text color. To use this member, set the CFM_COLOR flag
        /// in the dwMask member. This member is ignored if the
        /// CFE_AUTOCOLOR character effect is specified.
        /// To generate a COLORREF, use the RGB macro.
        /// </summary>
        public int crTextColor;

        /// <summary>
        /// Character set value. It can be one of the values specified
        /// for the lfCharSet member of the LOGFONT structure.
        /// To use this member, set the CFM_CHARSET flag in the dwMask member.
        /// </summary>
        public byte bCharSet;

        /// <summary>
        /// Specifies the font family and pitch. This member is the same
        /// as the lfPitchAndFamily member of the LOGFONT structure.
        /// </summary>
        public byte bPitchAndFamily;

        /// <summary>
        /// A null-terminated character array specifying the font name.
        /// To use this member, set the CFM_FACE flag in the dwMask member.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szFaceName;

        /// <summary>
        /// Font weight. This member is the same as the lfWeight member
        /// of the LOGFONT structure. To use this member,
        /// set the CFM_WEIGHT flag in the dwMask member.
        /// </summary>
        public short wWeight;

        /// <summary>
        /// Horizontal space between letters, in twips.
        /// This value has no effect on the text displayed by
        /// a rich edit control; it is included for compatibility
        /// with Windows TOM interfaces. To use this member,
        /// set the CFM_SPACING flag in the dwMask member.
        /// </summary>
        public short sSpacing;

        /// <summary>
        /// Background color. To use this member, set the CFM_BACKCOLOR
        /// flag in the dwMask member. This member is ignored if the
        /// CFE_AUTOBACKCOLOR character effect is specified.
        /// </summary>
        public int crBackColor;

        /// <summary>
        /// A 32-bit locale identifier that contains a language identifier
        /// in the lower word and a sorting identifier and reserved value
        /// in the upper word. This member has no effect on the text
        /// displayed by a rich edit control, but spelling and grammar
        /// checkers can use it to deal with language-dependent problems.
        /// You can use the macro to create an LCID value.
        /// To use this member, set the CFM_LCID flag in the dwMask member.
        /// </summary>
        public int lcid;

        /// <summary>
        /// Reserved; the value must be zero.
        /// </summary>
        public int dwReserved;

        /// <summary>
        /// Character style handle. This value has no effect on the
        /// text displayed by a rich edit control; it is included
        /// for compatibility with WindowsTOM interfaces.
        /// To use this member, set the CFM_STYLE flag in the
        /// dwMask member. For more information see the TOM documentation.
        /// </summary>
        public short sStyle;

        /// <summary>
        /// Value of the font size, above which to kern the character (yHeight).
        /// This value has no effect on the text displayed by a rich edit control;
        /// it is included for compatibility with TOM interfaces.
        /// To use this member, set the CFM_KERNING flag in the dwMask member.
        /// </summary>
        public short wKerning;

        /// <summary>
        /// Specifies the underline type. To use this member,
        /// set the CFM_UNDERLINETYPE flag in the dwMask member.
        /// </summary>
        public byte bUnderlineType;

        /// <summary>
        /// Text animation type. This value has no effect on the text displayed
        /// by a rich edit control; it is included for compatibility with
        /// TOM interfaces. To use this member, set the CFM_ANIMATION flag
        /// in the dwMask member.
        /// </summary>
        public byte bAnimation;

        /// <summary>
        /// An index that identifies the author making a revision. The rich edit
        /// control uses different text colors for each different author index.
        /// To use this member, set the CFM_REVAUTHOR flag in the dwMask member.
        /// </summary>
        public byte bRevAuthor;
    }
}
