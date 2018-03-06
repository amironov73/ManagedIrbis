// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IOleClientSite.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// 
	/// </summary>
	[ComImport]
	[Guid ( "00000118-0000-0000-C000-000000000046" )]
	[InterfaceType ( ComInterfaceType.InterfaceIsIUnknown )]
	public interface IOleClientSite
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[PreserveSig]
		int SaveObject ();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dwAssign"></param>
		/// <param name="dwWhichMoniker"></param>
		/// <param name="moniker"></param>
		/// <returns></returns>
		[PreserveSig]
		int GetMoniker
			(
			[In] [MarshalAs ( UnmanagedType.U4 )] int dwAssign,
			[In] [MarshalAs ( UnmanagedType.U4 )] int dwWhichMoniker,
			[MarshalAs ( UnmanagedType.Interface )] out object moniker
			);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="container"></param>
		/// <returns></returns>
		[PreserveSig]
		int GetContainer ( out object container );

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[PreserveSig]
		int ShowObject ();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fShow"></param>
		/// <returns></returns>
		[PreserveSig]
		int OnShowWindow ( int fShow );

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[PreserveSig]
		int RequestNewObjectLayout ();
	}
}