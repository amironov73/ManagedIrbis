/* FrameKind.cs -- 
   Ars Magna project, http://library.istu.edu/am */

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
