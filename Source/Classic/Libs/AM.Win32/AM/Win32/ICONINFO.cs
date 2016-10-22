/* ICONINFO.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// 
	/// </summary>
	[StructLayout ( LayoutKind.Sequential )]
	public class ICONINFO
	{
		/// <summary>
		/// 
		/// </summary>
		public int fIcon;
		
		/// <summary>
		/// 
		/// </summary>
		public int xHotspot;
		
		/// <summary>
		/// 
		/// </summary>
		public int yHotspot;
		
		/// <summary>
		/// 
		/// </summary>
		public IntPtr hbmMask;
		
		/// <summary>
		/// 
		/// </summary>
		public IntPtr hbmColor;
	}
}
