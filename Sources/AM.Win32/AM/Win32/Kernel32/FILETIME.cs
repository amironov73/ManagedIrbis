/* FILETIME.cs -- 64-bit time 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The FILETIME structure is a 64-bit value representing the 
	/// number of 100-nanosecond intervals since January 1, 1601 (UTC).
	/// </summary>
	[Serializable]
	[StructLayout ( LayoutKind.Sequential )]
	public struct FILETIME
	{
		/// <summary>
		/// Low-order part of the file time.
		/// </summary>
		[CLSCompliant ( false )]
		public uint dwLowDateTime;

		/// <summary>
		/// High-order part of the file time.
		/// </summary>
		[CLSCompliant ( false )]
		public uint dwHighDateTime;
	}
}