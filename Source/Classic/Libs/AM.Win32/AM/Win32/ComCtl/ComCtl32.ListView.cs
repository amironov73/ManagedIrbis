// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ComCtl32.ListView.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    partial class ComCtl32
    {
        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        [CLSCompliant(false)]
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
        [DllImport(DllName)]
        public static extern bool ListView_Arrange
            (
                IntPtr hwnd,
                ListViewAlignment code
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern void ListView_CancelEditLabel
            (
                IntPtr hwnd
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern IntPtr ListView_CreateDragImage
            (
                IntPtr hwnd,
                int iItem,
                ref Point lpptUpLeft
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern bool ListView_DeleteAllItems
            (
                IntPtr hwnd
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern bool ListView_DeleteColumn
            (
                IntPtr hwnd,
                int iCol
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern bool ListView_DeleteItem
            (
                IntPtr hwnd,
                int iItem
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern IntPtr ListView_EditLabel
            (
                IntPtr hwnd,
                int iItem
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern int ListView_EnableGroupView
            (
                IntPtr hwnd,
                bool fEnable
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern bool ListView_EnsureVisible
            (
                IntPtr hwnd,
                int i,
                bool fPartialOK
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern int ListView_FindItem
            (
                IntPtr hwnd,
                int iStart,
                ref LVFINDINFO plvfi
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern COLORREF ListView_GetBkColor
            (
                IntPtr hwnd
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern bool ListView_GetBkImage
            (
                IntPtr hwndLV,
                ref LVBKIMAGE plvbki
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        [CLSCompliant(false)]
        public static extern uint ListView_GetCallbackMask
            (
                IntPtr hwnd
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern bool ListView_GetCheckState
            (
                IntPtr hwndLV,
                int iIndex
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern bool ListView_GetColumn
            (
                IntPtr hwnd,
                int iCol,
                ref LVCOLUMN pcol
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
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
        [DllImport(DllName)]
        public static extern int ListView_GetColumnWidth
            (
                IntPtr hwnd,
                int iCol
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern int ListView_GetCountPerPage
            (
                IntPtr hwnd
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        public static extern IntPtr ListView_GetEditControl
            (
                IntPtr hwnd
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName)]
        [CLSCompliant(false)]
        public static extern uint ListView_GetExtendedListViewStyle
            (
                IntPtr hwndLV
            );

    }
}
