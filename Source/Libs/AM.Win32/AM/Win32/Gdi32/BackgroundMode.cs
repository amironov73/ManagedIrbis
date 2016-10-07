/* BackgroundMode.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Background mix mode for text, hatch brush drawing on device context.
	/// </summary>
	[Flags]
	public enum BackgroundMode
	{
		/// <summary>
		/// Error.
		/// </summary>
		ERROR = 0,

		/// <summary>
		/// Transparent.
		/// </summary>
		TRANSPARENT = 1,

		/// <summary>
		/// Opaque.
		/// </summary>
		OPAQUE = 2
	}
}
