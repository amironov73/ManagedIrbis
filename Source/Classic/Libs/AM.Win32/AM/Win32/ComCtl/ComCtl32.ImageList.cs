// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ComCtl32.ImageList.cs -- 
   Ars Magna project, http://arsmagna.ru */

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
        /// <param name="himl"></param>
        /// <param name="hbmImage"></param>
        /// <param name="hbmMask"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern int ImageList_Add
			(
				IntPtr himl,
				IntPtr hbmImage,
				IntPtr hbmMask
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="hicon"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern int ImageList_AddIcon
			(
				IntPtr himl,
				IntPtr hicon
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="hbmImage"></param>
        /// <param name="crMask"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern int ImageList_AddMasked
			(
				IntPtr himl,
				IntPtr hbmImage,
				COLORREF crMask
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himlDst"></param>
        /// <param name="himlSrc"></param>
        /// <param name="iDst"></param>
        /// <param name="iSrc"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ImageList_Copy
			(
				IntPtr himlDst,
				IntPtr himlSrc,
				int iDst,
				int iSrc,
				int uFlags
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="flags"></param>
        /// <param name="cInitial"></param>
        /// <param name="cGrow"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern IntPtr ImageList_Create
			(
				int cx,
				int cy,
				int flags,
				int cInitial,
				int cGrow
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ImageList_Destroy
			(
				IntPtr himl
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="i"></param>
        /// <param name="hdcDst"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fStyle"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ImageList_Draw
			(
				IntPtr himl,
				int i,
				IntPtr hdcDst,
				int x,
				int y,
				ImageListFlags fStyle
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="i"></param>
        /// <param name="hdcDst"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="rgbBk"></param>
        /// <param name="rgbFg"></param>
        /// <param name="fStyle"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ImageList_DrawEx
			(
				IntPtr himl,
				int i,
				IntPtr hdcDst,
				int x,
				int y,
				int dx,
				int dy,
				COLORREF rgbBk,
				COLORREF rgbFg,
				ImageListFlags fStyle
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern IntPtr ImageList_Duplicate
			(
				IntPtr himl
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hInstance"></param>
        /// <param name="himl"></param>
        /// <param name="i"></param>
        /// <returns></returns>
		public static IntPtr ImageList_ExtractIcon
			(
				IntPtr hInstance,
				IntPtr himl,
				int i
			)
		{
			return ImageList_GetIcon ( himl, i, ImageListFlags.ILD_NORMAL );
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern COLORREF ImageList_GetBkColor
			(
				IntPtr himl
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ppt"></param>
        /// <param name="pptHotspot"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern IntPtr ImageList_GetDragImage
			(
				ref Point ppt,
				ref Point pptHotspot
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="i"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern IntPtr ImageList_GetIcon
			(
				IntPtr himl,
				int i,
				ImageListFlags flags
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ImageList_GetIconSize
			(
				IntPtr himl,
				out int cx,
				out int cy
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern int ImageList_GetImageCount
			(
				IntPtr himl
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hInstance"></param>
        /// <param name="lpbmp"></param>
        /// <param name="cx"></param>
        /// <param name="cGrow"></param>
        /// <param name="crMask"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern IntPtr ImageList_LoadBitmap
			(
				IntPtr hInstance,
				string lpbmp,
				int cx,
				int cGrow,
				COLORREF crMask
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hInstance"></param>
        /// <param name="lpbmp"></param>
        /// <param name="cx"></param>
        /// <param name="cGrow"></param>
        /// <param name="crMask"></param>
        /// <param name="uType"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern IntPtr ImageList_LoadImage
			(
				IntPtr hInstance,
				string lpbmp,
				int cx,
				int cGrow,
				COLORREF crMask,
				int uType,
				int uFlags
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl1"></param>
        /// <param name="i1"></param>
        /// <param name="himl2"></param>
        /// <param name="i2"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern IntPtr ImageList_Merge
			(
				IntPtr himl1,
				int i1,
				IntPtr himl2,
				int i2,
				int dx,
				int dy
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pstm"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern IntPtr ImageList_Read
			(
				IntPtr pstm
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="pstm"></param>
        /// <param name="riid"></param>
        /// <param name="ppv"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern int ImageList_ReadEx
			(
				int dwFlags,
				IntPtr pstm,
				ref Guid riid,
				out IntPtr ppv
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="i"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ImageList_Remove
			(
				IntPtr himl,
				int i
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ImageList_RemoveAll
			(
				IntPtr himl
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="i"></param>
        /// <param name="hbmImage"></param>
        /// <param name="hbmMask"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ImageList_Replace
			(
				IntPtr himl,
				int i,
				IntPtr hbmImage,
				IntPtr hbmMask
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="i"></param>
        /// <param name="hicon"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern int ImageList_ReplaceIcon
			(
				IntPtr himl,
				int i,
				IntPtr hicon
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="clrBk"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern COLORREF ImageList_SetBkColor
			(
				IntPtr himl,
				COLORREF clrBk
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ImageList_SetIconSize
			(
				IntPtr himl,
				int cx,
				int cy
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="uNewCount"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ImageList_SetImageCount
			(
				IntPtr himl,
				int uNewCount
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="iImage"></param>
        /// <param name="iOverlay"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ImageList_SetOverlayImage
			(
				IntPtr himl,
				int iImage,
				int iOverlay
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="pstm"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern bool ImageList_Write
			(
				IntPtr himl,
				IntPtr pstm
			);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="himl"></param>
        /// <param name="dwFlags"></param>
        /// <param name="pstm"></param>
        /// <returns></returns>
		[DllImport ( DllName )]
		public static extern int ImageList_WriteEx
			(
				IntPtr himl,
				int dwFlags,
				IntPtr pstm
			);
	}
}
