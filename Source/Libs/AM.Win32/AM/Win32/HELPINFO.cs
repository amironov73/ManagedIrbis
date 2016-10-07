/* HELPINFO.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// 
	/// </summary>
	[StructLayout ( LayoutKind.Sequential )]
	public struct HELPINFO
	{
		/// <summary>
		/// 
		/// </summary>
		public int cbSize;
		
		/// <summary>
		/// 
		/// </summary>
		public int iContextType;
		
		/// <summary>
		/// 
		/// </summary>
		public int iCtrlId;
		
		/// <summary>
		/// 
		/// </summary>
		public IntPtr hItemHandle;
		
		/// <summary>
		/// 
		/// </summary>
		public int dwContextId;
		
		/// <summary>
		/// 
		/// </summary>
		public Point MousePos;
	}

}
