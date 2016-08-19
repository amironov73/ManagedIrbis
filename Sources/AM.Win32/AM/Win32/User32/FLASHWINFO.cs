/* FLASHWINFO.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The FLASHWINFO structure contains the flash status for a 
	/// window and the number of times the system should flash 
	/// the window.
	/// </summary>
	[StructLayout ( LayoutKind.Sequential, Size = FLASHWINFO.StructureSize )]
	public struct FLASHWINFO
	{
		/// <summary>
		/// Structure size.
		/// </summary>
		public const int StructureSize = 20;

		/// <summary>
		/// Size of the structure, in bytes.
		/// </summary>
        [CLSCompliant ( false )]
        public uint cbSize;

		/// <summary>
		/// Handle to the window to be flashed. 
		/// The window can be either opened or minimized.
		/// </summary>
		public IntPtr hwnd;

		/// <summary>
		/// Flash status.
		/// </summary>
		public FlashWindowFlags dwFlags;

		/// <summary>
		/// Number of times to flash the window.
		/// </summary>
        [CLSCompliant ( false )]
        public uint uCount; 
		
		/// <summary>
		/// Rate at which the window is to be flashed, in milliseconds. 
		/// If dwTimeout is zero, the function uses the default cursor 
		/// blink rate.
		/// </summary>
        [CLSCompliant ( false )]
        public uint dwTimeout;
	}
}
