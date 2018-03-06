// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FrameKind.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Flags used by DrawFrameControl().
	/// </summary>
	[Flags]
	public enum FrameKind
	{
		/// <summary>
		/// Title bar.
		/// </summary>
		DFC_CAPTION = 1,

		/// <summary>
		/// Menu bar.
		/// </summary>
		DFC_MENU = 2,

		/// <summary>
		/// Scroll bar.
		/// </summary>
		DFC_SCROLL = 3,

		/// <summary>
		/// Standard button.
		/// </summary>
		DFC_BUTTON = 4,

		/// <summary>
		/// Windows 98/Me, Windows 2000/XP: Popup menu item.
		/// </summary>
		DFC_POPUPMENU = 5
	}
}
