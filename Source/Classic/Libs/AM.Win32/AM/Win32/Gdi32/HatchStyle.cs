/* HatchStyle.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies the orientation of the lines used to create the hatch.
	/// </summary>
	public enum HatchStyle
	{
		/// <summary>
		/// Horizontal hatch.
		/// </summary>
		HS_HORIZONTAL = 0,       /* ----- */

		/// <summary>
		/// Vertical hatch.
		/// </summary>
		HS_VERTICAL = 1,       /* ||||| */

		/// <summary>
		/// A 45-degree downward, left-to-right hatch.
		/// </summary>
		HS_FDIAGONAL = 2,       /* \\\\\ */

		/// <summary>
		/// A 45-degree upward, left-to-right hatch.
		/// </summary>
		HS_BDIAGONAL = 3,       /* ///// */

		/// <summary>
		/// Horizontal and vertical cross-hatch.
		/// </summary>
		HS_CROSS = 4,       /* +++++ */

		/// <summary>
		/// 45-degree crosshatch.
		/// </summary>
		HS_DIAGCROSS = 5        /* xxxxx */
	}
}
