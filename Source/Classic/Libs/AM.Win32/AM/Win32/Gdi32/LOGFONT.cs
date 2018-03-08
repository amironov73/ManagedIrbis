// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LOGFONT.cs -- defines the attributes of a font
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// The LOGFONT structure defines the attributes of a font.
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LOGFONT
    {
        /// <summary>
        /// The height, in logical units, of the font's character
        /// cell or character. The character height value
        /// (also known as the em height) is the character cell
        /// height value minus the internal-leading value.
        /// The font mapper interprets the value specified
        /// in lfHeight in the following manner.
        ///
        /// &gt; 0 The font mapper transforms this value into
        /// device units and matches it against the cell height
        /// of the available fonts.
        /// 0 The font mapper uses a default height value when
        /// it searches for a match.
        /// &lt; 0 The font mapper transforms this value into device
        /// units and matches its absolute value against
        /// the character height of the available fonts.
        ///
        /// For all height comparisons, the font mapper looks
        /// for the largest font that does not exceed the requested size.
        /// This mapping occurs when the font is used for the first time.
        /// </summary>
        public int lfHeight;

        /// <summary>
        /// The average width, in logical units, of characters in the font.
        /// If lfWidth is zero, the aspect ratio of the device
        /// is matched against the digitization aspect ratio
        /// of the available fonts to find the closest match,
        /// determined by the absolute value of the difference.
        /// </summary>
        public int lfWidth;

        /// <summary>
        /// The angle, in tenths of degrees, between the escapement
        /// vector and the x-axis of the device. The escapement vector
        /// is parallel to the base line of a row of text.
        /// When the graphics mode is set to GM_ADVANCED,
        /// you can specify the escapement angle of the string
        /// independently of the orientation angle of the string's characters.
        /// When the graphics mode is set to GM_COMPATIBLE,
        /// lfEscapement specifies both the escapement and orientation.
        /// You should set lfEscapement and lfOrientation to the same value.
        /// </summary>
        public int lfEscapement;

        /// <summary>
        /// The angle, in tenths of degrees, between each character's
        /// base line and the x-axis of the device.
        /// </summary>
        public int lfOrientation;

        /// <summary>
        /// The weight of the font in the range 0 through 1000.
        /// For example,  400 is normal and 700 is bold.
        /// If this value is zero, a default weight is used.
        /// </summary>
        public int lfWeight;

        /// <summary>
        /// An italic font if set to TRUE.
        /// </summary>
        public byte lfItalic;

        /// <summary>
        /// An underlined font if set to TRUE.
        /// </summary>
        public byte lfUnderline;

        /// <summary>
        /// A strikeout font if set to TRUE.
        /// </summary>
        public byte lfStrikeOut;

        /// <summary>
        /// The character set.
        /// </summary>
        public byte lfCharSet;

        /// <summary>
        /// The output precision. The output precision defines
        /// how closely the output must match the requested
        /// font's height, width, character orientation,
        /// escapement, pitch, and font type.
        /// </summary>
        public byte lfOutPrecision;

        /// <summary>
        /// The clipping precision. The clipping precision defines
        /// how to clip characters that are partially outside
        /// the clipping region.
        /// </summary>
        public byte lfClipPrecision;

        /// <summary>
        /// The output quality. The output quality defines
        /// how carefully the graphics device interface (GDI)
        /// must attempt to match the logical-font attributes
        /// to those of an actual physical font. 
        /// </summary>
        public byte lfQuality;

        /// <summary>
        /// The pitch and family of the font.
        /// </summary>
        public byte lfPitchAndFamily;

        /// <summary>
        /// A null-terminated string that specifies the typeface
        /// name of the font. The length of this string must
        /// not exceed 32 TCHAR values, including the terminating NULL.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string lfFaceName;
    }
}