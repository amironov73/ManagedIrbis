/* ConsoleDisplayMode.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Console display mode flags.
	/// </summary>
	[Flags]
	public enum ConsoleDisplayMode
	{
		/// <summary>
		/// Full-screen console. The console is in this mode as soon as the 
		/// window is maximized. At this point, the transition to full-screen 
		/// mode can still fail.
		/// </summary>
		CONSOLE_FULLSCREEN = 1,

		/// <summary>
		/// Full-screen console communicating directly with the video hardware. 
		/// This mode is set after the console is in CONSOLE_FULLSCREEN mode to 
		/// indicate that the transition to full-screen mode has completed.
		/// </summary>
		CONSOLE_FULLSCREEN_HARDWARE = 2
	}
}
