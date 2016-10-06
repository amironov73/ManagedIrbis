/* ArrangeWindowsFlags.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies how the system arranges minimized windows.
	/// </summary>
	[Flags]
	public enum ArrangeWindowsFlags
	{
		/// <summary>
		/// Start at the lower-left corner of the screen 
		/// (default position).
		/// </summary>
		ARW_BOTTOMLEFT = 0x0000,

		/// <summary>
		/// Start at the lower-right corner of the screen. 
		/// Equivalent to ARW_STARTRIGHT.
		/// </summary>
		ARW_BOTTOMRIGHT = 0x0001,

		/// <summary>
		/// Start at the upper-left corner of the screen. 
		/// Equivalent to ARV_STARTTOP.
		/// </summary>
		ARW_TOPLEFT = 0x0002,

		/// <summary>
		/// Start at the upper-right corner of the screen. 
		/// Equivalent to ARW_STARTTOP | SRW_STARTRIGHT.
		/// </summary>
		ARW_TOPRIGHT = 0x0003,

		/// <summary>
		/// ???
		/// </summary>
		ARW_STARTMASK = 0x0003,

		/// <summary>
		/// Start at the lower-right corner of the screen. 
		/// Equivalent to ARW_BOTTOMRIGHT.
		/// </summary>
		ARW_STARTRIGHT = 0x0001,

		/// <summary>
		/// Start at the upper-left corner of the screen. 
		/// Equivalent to ARV_TOPLEFT.
		/// </summary>
		ARW_STARTTOP = 0x0002,

		/// <summary>
		/// Arrange horizontally, left to right.
		/// </summary>
		ARW_LEFT = 0x0000,

		/// <summary>
		/// Arrange horizontally, right to left.
		/// </summary>
		ARW_RIGHT = 0x0000,

		/// <summary>
		/// Arrange vertically, bottom to top.
		/// </summary>
		ARW_UP = 0x0004,

		/// <summary>
		/// Arrange vertically, top to bottom.
		/// </summary>
		ARW_DOWN = 0x0004,

		/// <summary>
		/// Hide minimized windows by moving them off 
		/// the visible area of the screen.
		/// </summary>
		ARW_HIDE = 0x0008,
	}
}
