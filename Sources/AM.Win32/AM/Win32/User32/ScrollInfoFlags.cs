/* ScrollInfoFlags.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies the scroll bar parameters to set or retrieve for
	/// functions GetScrollInfo/SetScrollInfo.
	/// </summary>
	[Flags]
	public enum ScrollInfoFlags
	{
		/// <summary>
		/// The nMin and nMax members contain the minimum and maximum values 
		/// for the scrolling range.
		/// </summary>
		SIF_RANGE = 0x0001,

		/// <summary>
		/// The nPage member contains the page size for a proportional 
		/// scroll bar.
		/// </summary>
		SIF_PAGE = 0x0002,

		/// <summary>
		/// The nPos member contains the scroll box position, which is not 
		/// updated while the user drags the scroll box.
		/// </summary>
		SIF_POS = 0x0004,

		/// <summary>
		/// This value is used only when setting a scroll bar's parameters. 
		/// If the scroll bar's new parameters make the scroll bar unnecessary, 
		/// disable the scroll bar instead of removing it.
		/// </summary>
		SIF_DISABLENOSCROLL = 0x0008,

		/// <summary>
		/// The nTrackPos member contains the current position of the scroll 
		/// box while the user is dragging it.
		/// </summary>
		SIF_TRACKPOS = 0x0010,

		/// <summary>
		/// Combination of SIF_PAGE, SIF_POS, SIF_RANGE, and SIF_TRACKPOS.
		/// </summary>
		SIF_ALL = ( SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS )
	}
}
