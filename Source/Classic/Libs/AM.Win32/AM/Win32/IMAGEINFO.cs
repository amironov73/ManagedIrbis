// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IMAGEINFO.cs -- 
   Ars Magna project, http://arsmagna.ru */

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
	public class IMAGEINFO
	{
		/// <summary>
		/// 
		/// </summary>
		public IntPtr hbmImage;
		
		/// <summary>
		/// 
		/// </summary>
		public IntPtr hbmMask;
		
		/// <summary>
		/// 
		/// </summary>
		public int Unused1;
		
		/// <summary>
		/// 
		/// </summary>
		public int Unused2;
		
		/// <summary>
		/// 
		/// </summary>
		public int rcImage_left;
		
		/// <summary>
		/// 
		/// </summary>
		public int rcImage_top;
		
		/// <summary>
		/// 
		/// </summary>
		public int rcImage_right;
		
		/// <summary>
		/// 
		/// </summary>
		public int rcImage_bottom;
	}

}
