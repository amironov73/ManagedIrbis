// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IRichEditOle.cs -- 
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
    [InterfaceType ( ComInterfaceType.InterfaceIsIUnknown )]
    [Guid ( "00020D00-0000-0000-c000-000000000046" )]
    public interface IRichEditOle
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="lplpolesite"></param>
		/// <returns></returns>
        int GetClientSite ( IntPtr lplpolesite );

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        int GetObjectCount ();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        int GetLinkCount ();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iob"></param>
		/// <param name="lpreobject"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
        int GetObject ( int iob, REOBJECT lpreobject, [MarshalAs ( UnmanagedType.U4 )]GetObjectOptions flags );

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lpreobject"></param>
		/// <returns></returns>
        int InsertObject ( REOBJECT lpreobject );

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iob"></param>
		/// <param name="rclsidNew"></param>
		/// <param name="lpstrUserTypeNew"></param>
		/// <returns></returns>
        int ConvertObject ( int iob, Guid rclsidNew, string lpstrUserTypeNew );

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rclsid"></param>
		/// <param name="rclsidAs"></param>
		/// <returns></returns>
        int ActivateAs ( Guid rclsid, Guid rclsidAs );

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lpstrContainerApp"></param>
		/// <param name="lpstrContainerObj"></param>
		/// <returns></returns>
        int SetHostNames ( string lpstrContainerApp, string lpstrContainerObj );

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iob"></param>
		/// <param name="fAvailable"></param>
		/// <returns></returns>
        int SetLinkAvailable ( int iob, int fAvailable );

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iob"></param>
		/// <param name="dvaspect"></param>
		/// <returns></returns>
        int SetDvaspect ( int iob, int dvaspect );

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iob"></param>
		/// <returns></returns>
        int HandsOffStorage ( int iob );

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iob"></param>
		/// <param name="lpstg"></param>
		/// <returns></returns>
        int SaveCompleted ( int iob, IntPtr lpstg );

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        int InPlaceDeactivate ();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fEnterMode"></param>
		/// <returns></returns>
        int ContextSensitiveHelp ( int fEnterMode );
        //int GetClipboardData(CHARRANGE FAR * lpchrg, uint reco, IntPtr lplpdataobj);
        //int ImportDataObject(IntPtr lpdataobj, CLIPFORMAT cf, HGLOBAL hMetaPict);
    }
}
