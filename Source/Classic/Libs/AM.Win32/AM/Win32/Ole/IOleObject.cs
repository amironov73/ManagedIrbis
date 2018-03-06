// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IOleObject.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// 
	/// </summary>
	[ComImport]
	[Guid ( "00000112-0000-0000-C000-000000000046" )]
	[InterfaceTypeAttribute ( ComInterfaceType.InterfaceIsIUnknown )]
	internal interface IOleObject
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pClientSite"></param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int SetClientSite
			(
			[In] [MarshalAs ( UnmanagedType.Interface )] IOleClientSite pClientSite
			);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int GetClientSite
			(
			[Out] [MarshalAs ( UnmanagedType.Interface )] out IOleClientSite site
			);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="szContainerApp"></param>
		/// <param name="szContainerObj"></param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int SetHostNames
			(
			[In] [MarshalAs ( UnmanagedType.LPWStr )] String szContainerApp,
			[In] [MarshalAs ( UnmanagedType.LPWStr )] String szContainerObj
			);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dwSaveOption"></param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int Close
			(
			[In] [MarshalAs ( UnmanagedType.U4 )] uint dwSaveOption
			);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dwWhichMoniker"></param>
		/// <param name="pmk"></param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int SetMoniker
			(
			[In] [MarshalAs ( UnmanagedType.U4 )] uint dwWhichMoniker,
			[In] [MarshalAs ( UnmanagedType.Interface )] Object pmk
			);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dwAssign"></param>
		/// <param name="dwWhichMoniker"></param>
		/// <param name="moniker"></param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int GetMoniker
			(
			[In] [MarshalAs ( UnmanagedType.U4 )] uint dwAssign,
			[In] [MarshalAs ( UnmanagedType.U4 )] uint dwWhichMoniker,
			[Out] [MarshalAs ( UnmanagedType.Interface )] out Object moniker
			);

		/// <summary>
		/// Inits from data.
		/// </summary>
		/// <param name="pDataObject">The p data object.</param>
		/// <param name="fCreation">if set to <c>true</c> [f creation].</param>
		/// <param name="dwReserved">The dw reserved.</param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int InitFromData
			(
			[In] [MarshalAs ( UnmanagedType.Interface )] Object pDataObject,
			[In] [MarshalAs ( UnmanagedType.Bool )] Boolean fCreation,
			[In] [MarshalAs ( UnmanagedType.U4 )] uint dwReserved
			);

		/// <summary>
		/// Gets the clipboard data.
		/// </summary>
		/// <param name="dwReserved">The dw reserved.</param>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int GetClipboardData
			(
			[In] [MarshalAs ( UnmanagedType.U4 )] uint dwReserved,
			out Object data
			);

		/// <summary>
		/// Does the verb.
		/// </summary>
		/// <param name="iVerb">The i verb.</param>
		/// <param name="lpmsg">The LPMSG.</param>
		/// <param name="pActiveSite">The p active site.</param>
		/// <param name="lindex">The lindex.</param>
		/// <param name="hwndParent">The HWND parent.</param>
		/// <param name="lprcPosRect">The LPRC pos rect.</param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int DoVerb
			(
			[In] [MarshalAs ( UnmanagedType.I4 )] int iVerb,
			[In] IntPtr lpmsg,
			[In] [MarshalAs ( UnmanagedType.Interface )] IOleClientSite pActiveSite,
			[In] [MarshalAs ( UnmanagedType.I4 )] int lindex,
			[In] IntPtr hwndParent,
			[In] Rectangle lprcPosRect
			);

		/// <summary>
		/// Enums the verbs.
		/// </summary>
		/// <param name="e">The e.</param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int EnumVerbs ( out Object e ); // IEnumOLEVERB

		/// <summary>
		/// OLEs the update.
		/// </summary>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int OleUpdate ();

		/// <summary>
		/// Determines whether [is up to date].
		/// </summary>
		/// <returns></returns>
		[return : MarshalAs ( UnmanagedType.I4 )]
		[PreserveSig]
		int IsUpToDate ();

		/// <summary>
		/// Gets the user class ID.
		/// </summary>
		/// <param name="pClsid">The p CLSID.</param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int GetUserClassID ( [In] [Out] ref Guid pClsid );

		/// <summary>
		/// Gets the type of the user.
		/// </summary>
		/// <param name="dwFormOfType">Type of the dw form of.</param>
		/// <param name="userType">Type of the user.</param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int GetUserType
			(
			[In] [MarshalAs ( UnmanagedType.U4 )] uint dwFormOfType,
			[Out] [MarshalAs ( UnmanagedType.LPWStr )] out String userType
			);

		/// <summary>
		/// Sets the extent.
		/// </summary>
		/// <param name="dwDrawAspect">The dw draw aspect.</param>
		/// <param name="pSizel">The p sizel.</param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int SetExtent
			(
			[In] [MarshalAs ( UnmanagedType.U4 )] uint dwDrawAspect,
			[In] Object pSizel
			); // tagSIZEL

		/// <summary>
		/// Gets the extent.
		/// </summary>
		/// <param name="dwDrawAspect">The dw draw aspect.</param>
		/// <param name="pSizel">The p sizel.</param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int GetExtent
			(
			[In] [MarshalAs ( UnmanagedType.U4 )] uint dwDrawAspect,
			[Out] Object pSizel
			); // tagSIZEL

		/// <summary>
		/// Advises the specified p adv sink.
		/// </summary>
		/// <param name="pAdvSink">The p adv sink.</param>
		/// <param name="cookie">The cookie.</param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int Advise
			(
			[In] [MarshalAs ( UnmanagedType.Interface )] IAdviseSink pAdvSink,
			out int cookie
			);

		/// <summary>
		/// Unadvises the specified dw connection.
		/// </summary>
		/// <param name="dwConnection">The dw connection.</param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int Unadvise ( [In] [MarshalAs ( UnmanagedType.U4 )] int dwConnection );

		/// <summary>
		/// Enums the advise.
		/// </summary>
		/// <param name="e">The e.</param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int EnumAdvise ( out Object e );

		/// <summary>
		/// Gets the misc status.
		/// </summary>
		/// <param name="dwAspect">The dw aspect.</param>
		/// <param name="misc">The misc.</param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int GetMiscStatus
			(
			[In] [MarshalAs ( UnmanagedType.U4 )] uint dwAspect,
			out int misc
			);

		/// <summary>
		/// Sets the color scheme.
		/// </summary>
		/// <param name="pLogpal">The p logpal.</param>
		/// <returns></returns>
		[PreserveSig]
		[return : MarshalAs ( UnmanagedType.I4 )]
		int SetColorScheme ( [In] Object pLogpal ); // tagLOGPALETTE
	}
}