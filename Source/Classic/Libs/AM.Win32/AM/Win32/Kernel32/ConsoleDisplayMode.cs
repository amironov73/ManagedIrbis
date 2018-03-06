// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConsoleDisplayMode.cs --  
   Ars Magna project, http://arsmagna.ru */

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
