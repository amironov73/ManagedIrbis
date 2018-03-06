// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ScrollBarFlags.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Флаги для функций вроде GetScrollPos, SetScrollPos.
	/// </summary>
	public enum ScrollBarFlags
	{
		/// <summary>
		/// Retrieves the position of the scroll box in a window's 
		/// standard horizontal scroll bar.
		/// </summary>
		SB_HORZ = 0,

		/// <summary>
		/// Retrieves the position of the scroll box in a window's 
		/// standard vertical scroll bar.
		/// </summary>
		SB_VERT = 1,

		/// <summary>
		/// Retrieves the position of the scroll box in a scroll bar control. 
		/// The hWnd parameter must be the handle to the scroll bar control.
		/// </summary>
		SB_CTL = 2,

		/// <summary>
		/// ???
		/// </summary>
		SB_BOTH = 3
	}
}
