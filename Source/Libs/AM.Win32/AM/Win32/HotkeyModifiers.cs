/* HotkeyModifiers.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Hotkey modifiers.
    /// </summary>
	[Flags]
	public enum HotkeyModifiers
	{
        /// <summary>
        /// None.
        /// </summary>
		None = 0,

        /// <summary>
        /// Alt.
        /// </summary>
		Alt = 1,

        /// <summary>
        /// Control.
        /// </summary>
		Control = 2,

        /// <summary>
        /// Shift.
        /// </summary>
		Shift = 4,

        /// <summary>
        /// Win.
        /// </summary>
		Win = 8
	}
}
