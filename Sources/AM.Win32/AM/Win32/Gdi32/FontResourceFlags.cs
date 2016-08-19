/* FontResourceFlags.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies characteristics of the font to be added to the system.
	/// </summary>
	[Flags]
	public enum FontResourceFlags
	{
		/// <summary>
		/// Specifies that only the process that called the AddFontResourceEx 
		/// function can use this font. When the font name matches a public 
		/// font, the private font will be chosen. When the process terminates, 
		/// the system will remove all fonts installed by the process with 
		/// the AddFontResourceEx function.
		/// </summary>
		FR_PRIVATE = 0x10,

		/// <summary>
		/// Specifies that no process, including the process that called 
		/// the AddFontResourceEx function, can enumerate this font.
		/// </summary>
		FR_NOT_ENUM = 0x20
	}
}
