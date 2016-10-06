/* TEXTMETRIC.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The TEXTMETRIC structure contains basic information about a 
	/// physical font. All sizes are specified in logical units; that is, 
	/// they depend on the current mapping mode of the display context. 
	/// </summary>
	[Serializable]
	[StructLayout ( LayoutKind.Sequential )]
	public struct TEXTMETRIC
	{
		/// <summary>
		/// Specifies the height (ascent + descent) of characters.
		/// </summary>
		public int tmHeight;

		/// <summary>
		/// Specifies the ascent (units above the base line) of characters.
		/// </summary>
		public int tmAscent;

		/// <summary>
		/// Specifies the descent (units below the base line) of characters.
		/// </summary>
		public int tmDescent;

		/// <summary>
		/// Specifies the amount of leading (space) inside the bounds 
		/// set by the tmHeight member. Accent marks and other diacritical 
		/// characters may occur in this area. The designer may set this 
		/// member to zero. 
		/// </summary>
		public int tmInternalLeading;

		/// <summary>
		/// Specifies the amount of extra leading (space) that the application 
		/// adds between rows. Since this area is outside the font, it 
		/// contains no marks and is not altered by text output calls 
		/// in either OPAQUE or TRANSPARENT mode. The designer may set 
		/// this member to zero.
		/// </summary>
		public int tmExternalLeading;

		/// <summary>
		/// Specifies the average width of characters in the font 
		/// (generally defined as the width of the letter x). 
		/// This value does not include the overhang required 
		/// for bold or italic characters.
		/// </summary>
		public int tmAveCharWidth;

		/// <summary>
		/// Specifies the width of the widest character in the font.
		/// </summary>
		public int tmMaxCharWidth;

		/// <summary>
		/// Specifies the weight of the font.
		/// </summary>
		public int tmWeight;

		/// <summary>
		/// Specifies the extra width per string that may be added to 
		/// some synthesized fonts. When synthesizing some attributes, 
		/// such as bold or italic, graphics device interface (GDI) 
		/// or a device may have to add width to a string on both a 
		/// per-character and per-string basis. For example, GDI makes 
		/// a string bold by expanding the spacing of each character 
		/// and overstriking by an offset value; it italicizes a font 
		/// by shearing the string. In either case, there is an overhang 
		/// past the basic string. For bold strings, the overhang is the 
		/// distance by which the overstrike is offset. For italic strings, 
		/// the overhang is the amount the top of the font is sheared past 
		/// the bottom of the font.
		/// </summary>
		public int tmOverhang;

		/// <summary>
		/// Specifies the horizontal aspect of the device for which 
		/// the font was designed.
		/// </summary>
		public int tmDigitizedAspectX;

		/// <summary>
		/// Specifies the vertical aspect of the device for which the font 
		/// was designed. The ratio of the tmDigitizedAspectX and 
		/// tmDigitizedAspectY members is the aspect ratio of the device 
		/// for which the font was designed. 
		/// </summary>
		public int tmDigitizedAspectY;

		/// <summary>
		/// Specifies the value of the first character defined in the font.
		/// </summary>
		public char tmFirstChar;

		/// <summary>
		/// Specifies the value of the character to be substituted for 
		/// characters not in the font.
		/// </summary>
		public char tmLastChar;

		/// <summary>
		/// Specifies the value of the character that will be used to define 
		/// word breaks for text justification.
		/// </summary>
		public char tmDefaultChar;

		/// <summary>
		/// Specifies an italic font if it is nonzero.
		/// </summary>
		public char tmBreakChar;

		/// <summary>
		/// Specifies an italic font if it is nonzero.
		/// </summary>
		public byte tmItalic;

		/// <summary>
		/// Specifies a strikeout font if it is nonzero. 
		/// </summary>
		public byte tmUnderlined;

		/// <summary>
		/// Specifies a strikeout font if it is nonzero.
		/// </summary>
		public byte tmStruckOut;

		/// <summary>
		/// Specifies information about the pitch, the technology, 
		/// and the family of a physical font.
		/// </summary>
		public byte tmPitchAndFamily;

		/// <summary>
		/// Specifies the character set of the font. The character 
		/// set can be one of the following values.
		/// </summary>
		public byte tmCharSet;
	}
}
