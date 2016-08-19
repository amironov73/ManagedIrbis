/* VARIANT.cs -- 
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
	public sealed class VARIANT
	{
		/// <summary>
		/// 
		/// </summary>
		[MarshalAs ( UnmanagedType.I2 )]
		public short vt;
		
		/// <summary>
		/// 
		/// </summary>
		[MarshalAs ( UnmanagedType.I2 )]
		public short reserved1;
		
		/// <summary>
		/// 
		/// </summary>
		[MarshalAs ( UnmanagedType.I2 )]
		public short reserved2;
		
		/// <summary>
		/// 
		/// </summary>
		[MarshalAs ( UnmanagedType.I2 )]
		public short reserved3;
		
		/// <summary>
		/// 
		/// </summary>
		public IntPtr data1;
		
		/// <summary>
		/// 
		/// </summary>
		public IntPtr data2;
	}

}
