// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IOleContainer.cs -- 
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
	[ComImport]
	[CLSCompliant ( false )]
	[Guid ( "0000011B-0000-0000-C000-000000000046" )]
	[InterfaceTypeAttribute ( ComInterfaceType.InterfaceIsIUnknown )]
	public interface IOleContainer
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pbc"></param>
		/// <param name="pszDisplayName"></param>
		/// <param name="pchEaten"></param>
		/// <param name="ppmkOut"></param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int ParseDisplayName
			(
			[In] [MarshalAs ( UnmanagedType.Interface )] Object pbc,
			[In] [MarshalAs ( UnmanagedType.LPWStr )] String pszDisplayName,
			[Out] [MarshalAs ( UnmanagedType.LPArray )] int[] pchEaten,
			[Out] [MarshalAs ( UnmanagedType.LPArray )] Object[] ppmkOut
			);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="grfFlags"></param>
		/// <param name="ppenum"></param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int EnumObjects
			(
			[In] [MarshalAs ( UnmanagedType.U4 )] uint grfFlags,
			[Out] [MarshalAs ( UnmanagedType.LPArray )] Object[] ppenum
			);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fLock"></param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int LockContainer
			(
			[In] [MarshalAs ( UnmanagedType.Bool )] Boolean fLock
			);
	}
}