// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CONSOLE_CURSOR_INFO.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Contains information about the console cursor.
	/// </summary>
	[Serializable]
	[StructLayout ( LayoutKind.Explicit, Size = 8 )]
	public struct CONSOLE_CURSOR_INFO
	{
		/// <summary>
		/// Percentage of the character cell that is filled 
		/// by the cursor. This value is between 1 and 100. 
		/// The cursor appearance varies, ranging from completely 
		/// filling the cell to showing up as a horizontal line 
		/// at the bottom of the cell. 
		/// </summary>
		[FieldOffset ( 0 )]
		int dwSize;  

		/// <summary>
		/// Visibility of the cursor. If the cursor is visible, 
		/// this member is TRUE.
		/// </summary>
		[FieldOffset ( 4 )]
		bool bVisible;
	}
}
