/* CONSOLE_FONT_INFO.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Contains information for a console font.
	/// </summary>
	[Serializable]
	[StructLayout ( LayoutKind.Explicit, Size = 8 )]
	public struct CONSOLE_FONT_INFO
	{
		/// <summary>
		/// Index of the font in the system's console font table. 
		/// </summary>
		[FieldOffset ( 0 )]
		public int nFont; 

		/// <summary>
		/// A COORD structure that contains the width and height 
		/// of each character in the font. The X member contains 
		/// the width, while the Y member contains the height.
		/// </summary>
		[FieldOffset ( 4 )]
		public COORD dwFontSize;
	}
}
