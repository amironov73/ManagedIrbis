/* ComCtl32.ListView.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	partial class ComCtl32
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwndLV"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
        [CLSCompliant ( false )]
		public static extern uint ListView_ApproximateViewRect 
			(
				IntPtr hwndLV,
				int cx,
				int cy,
				int iCount
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="code"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ListView_Arrange 
			(
				IntPtr hwnd,
				ListViewAlignment code
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
		[DllImport ( DllName )]
		public static extern void ListView_CancelEditLabel 
			(
				IntPtr hwnd
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="iItem"></param>
        /// <param name="lpptUpLeft"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern IntPtr ListView_CreateDragImage 
			(
				IntPtr hwnd,
				int iItem,
				ref Point lpptUpLeft
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ListView_DeleteAllItems 
			(
				IntPtr hwnd
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="iCol"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ListView_DeleteColumn 
			(
				IntPtr hwnd,
				int iCol
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="iItem"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ListView_DeleteItem 
			(
				IntPtr hwnd,
				int iItem
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="iItem"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern IntPtr ListView_EditLabel 
			(
				IntPtr hwnd,
				int iItem
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="fEnable"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern int ListView_EnableGroupView 
			(
				IntPtr hwnd,
				bool fEnable
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="i"></param>
        /// <param name="fPartialOK"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ListView_EnsureVisible 
			(
				IntPtr hwnd,
				int i,
				bool fPartialOK
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="iStart"></param>
        /// <param name="plvfi"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern int ListView_FindItem
			(
				IntPtr hwnd,
				int iStart,
				ref LVFINDINFO plvfi
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern COLORREF ListView_GetBkColor 
			(
				IntPtr hwnd
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwndLV"></param>
        /// <param name="plvbki"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ListView_GetBkImage 
			(
				IntPtr hwndLV,
				ref LVBKIMAGE plvbki
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
        [CLSCompliant ( false )]
        public static extern uint ListView_GetCallbackMask 
			(
				IntPtr hwnd
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwndLV"></param>
        /// <param name="iIndex"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ListView_GetCheckState 
			(
				IntPtr hwndLV,
				int iIndex
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="iCol"></param>
        /// <param name="pcol"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ListView_GetColumn 
			(
				IntPtr hwnd,
				int iCol,
				ref LVCOLUMN pcol
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwndLV"></param>
        /// <param name="iCount"></param>
        /// <param name="lpiArray"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ListView_GetColumnOrderArray 
			(
				IntPtr hwndLV,
				int iCount,
				[MarshalAs ( UnmanagedType.LPArray, SizeParamIndex = 1 )]
				int [] lpiArray
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="iCol"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern int ListView_GetColumnWidth 
			(
				IntPtr hwnd,
				int iCol
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern int ListView_GetCountPerPage 
			(
				IntPtr hwnd
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern IntPtr ListView_GetEditControl
			(
				IntPtr hwnd
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwndLV"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
        [CLSCompliant ( false )]
        public static extern uint ListView_GetExtendedListViewStyle 
			(
				IntPtr hwndLV
			);

	}
}
