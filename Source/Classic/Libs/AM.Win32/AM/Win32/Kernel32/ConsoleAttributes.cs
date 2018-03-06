// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConsoleAttributes.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Attributes for console text.
	/// </summary>
	[Flags]
	public enum ConsoleAttributes : short
	{
		/// <summary>
		/// Text color contains blue.
		/// </summary>
		FOREGROUND_BLUE = 0x0001,

		/// <summary>
		/// Text color contains green.
		/// </summary>
		FOREGROUND_GREEN = 0x0002,

		/// <summary>
		/// Text color contains red.
		/// </summary>
		FOREGROUND_RED = 0x0004, 

		/// <summary>
		/// Text color is intensified.
		/// </summary>
		FOREGROUND_INTENSITY = 0x0008, 

		/// <summary>
		/// Background color contains blue.
		/// </summary>
		BACKGROUND_BLUE = 0x0010, 

		/// <summary>
		/// Background color contains green.
		/// </summary>
		BACKGROUND_GREEN = 0x0020, 

		/// <summary>
		/// Background color contains red.
		/// </summary>
		BACKGROUND_RED = 0x0040, 

		/// <summary>
		/// Background color is intensified.
		/// </summary>
		BACKGROUND_INTENSITY = 0x0080, 

		/// <summary>
		/// Leading Byte of DBCS.
		/// </summary>
		COMMON_LVB_LEADING_BYTE = 0x0100, 

		/// <summary>
		/// Trailing Byte of DBCS.
		/// </summary>
		COMMON_LVB_TRAILING_BYTE = 0x0200, 

		/// <summary>
		/// DBCS: Grid attribute: top horizontal.
		/// </summary>
		COMMON_LVB_GRID_HORIZONTAL = 0x0400,
 
		/// <summary>
		/// DBCS: Grid attribute: left vertical.
		/// </summary>
		COMMON_LVB_GRID_LVERTICAL = 0x0800, 

		/// <summary>
		/// DBCS: Grid attribute: right vertical.
		/// </summary>
		COMMON_LVB_GRID_RVERTICAL = 0x1000,
 
		/// <summary>
		/// DBCS: Reverse fore/back ground attribute.
		/// </summary>
		COMMON_LVB_REVERSE_VIDEO = 0x4000, 

		/// <summary>
		/// DBCS: Underscore.
		/// </summary>
		COMMON_LVB_UNDERSCORE = unchecked((short)0x8000)
	}
}
