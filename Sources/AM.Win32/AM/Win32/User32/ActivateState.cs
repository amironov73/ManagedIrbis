/* ActivateState.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Flags for WM_ACTIVATE message (low word of wparam).
	/// </summary>
    [CLSCompliant ( false )]
    public enum ActivateState : ushort
	{
		/// <summary>
		/// Window has been deactivated.
		/// </summary>
		WA_INACTIVE = 0,

		/// <summary>
		/// Window activated by other than a mouse click, 
		/// like call to SetActiveWindow.
		/// </summary>
		WA_ACTIVE = 1,

		/// <summary>
		/// Window activated by a mouse click.
		/// </summary>
		WA_CLICKACTIVE = 2
	}
}
