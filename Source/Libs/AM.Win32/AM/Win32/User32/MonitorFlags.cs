/* MonitorFlags.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Determines the function's return value if the window does not 
	/// intersect any display monitor.
	/// </summary>
	public enum MonitorFlags
	{
		/// <summary>
		/// Returns NULL.
		/// </summary>
		MONITOR_DEFAULTTONULL = 0x00000000,

		/// <summary>
		/// Returns a handle to the primary display monitor. 
		/// </summary>
		MONITOR_DEFAULTTOPRIMARY = 0x00000001,

		/// <summary>
		/// Returns a handle to the display monitor that 
		/// is nearest to the window.
		/// </summary>
		MONITOR_DEFAULTTONEAREST = 0x00000002
	}
}
