// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* VARIANT.cs -- 
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
