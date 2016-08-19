/* EditMargins.cs -- specifies what margin of edit control to set.
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies what margin of edit control to set.
	/// </summary>
	[Flags]
	public enum EditMargins
	{
		/// <summary>
		/// Sets the left margin.
		/// </summary>
		EC_LEFTMARGIN = 1,

		/// <summary>
		/// Sets the right margin.
		/// </summary>
		EC_RIGHTMARGIN = 2,

		/// <summary>
		/// Sets the left and right margins to a narrow
		/// width calculated using the text metrics of the
		/// control's current font. If no font has been
		/// set for the control, the margins are set to
		/// zero.
		/// </summary>
		EC_USEFONTINFO = 0xFFFF
	}
}