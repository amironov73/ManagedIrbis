// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LVBKIMAGE.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Contains information about the background image of a list-view control. 
	/// This structure is used for both setting and retrieving background image 
	/// information.
	/// </summary>
	[Serializable]
	[StructLayout ( LayoutKind.Sequential )]
	public struct LVBKIMAGE
	{
		/// <summary>
		/// This member may be one or more of the following flags.
		/// </summary>
		public ListViewBackImageFlags ulFlags;

		/// <summary>
		/// Not currently used.
		/// </summary>
		public IntPtr hbm;

		/// <summary>
		/// Address of a NULL-terminated string that contains the URL of 
		/// the background image. This member is only valid if the 
		/// LVBKIF_SOURCE_URL flag is set in ulFlags. This member must 
		/// be initialized to point to the buffer that contains or receives 
		/// the text before sending the message.
		/// </summary>
		[MarshalAs ( UnmanagedType.LPTStr )]
		public string pszImage;

		/// <summary>
		/// Size of the buffer at the address in pszImage. If information is 
		/// being sent to the control, this member is ignored.
		/// </summary>
		public int cchImageMax;

		/// <summary>
		/// Percentage of the control's client area that the image should 
		/// be offset horizontally. For example, at 0 percent, the image 
		/// will be displayed against the left edge of the control's client 
		/// area. At 50 percent, the image will be displayed horizontally 
		/// centered in the control's client area. At 100 percent, the image 
		/// will be displayed against the right edge of the control's client 
		/// area. This member is only valid when LVBKIF_STYLE_NORMAL is 
		/// specified in ulFlags.If both LVBKIF_FLAG_TILEOFFSET and 
		/// LVBKIF_STYLE_TILE are specified in ulFlags, then the value 
		/// specifies the pixel, not percentage offset, of the first tile. 
		/// Otherwise, the value is ignored.
		/// </summary>
		public int xOffsetPercent;

		/// <summary>
		/// Percentage of the control's client area that the image should 
		/// be offset vertically. For example, at 0 percent, the image will 
		/// be displayed against the top edge of the control's client area. 
		/// At 50 percent, the image will be displayed vertically centered 
		/// in the control's client area. At 100 percent, the image will 
		/// be displayed against the bottom edge of the control's client area. 
		/// This member is only valid when LVBKIF_STYLE_NORMAL is specified 
		/// in ulFlags.If both LVBKIF_FLAG_TILEOFFSET and LVBKIF_STYLE_TILE 
		/// are specified in ulFlags, then the value specifies the pixel, 
		/// not percentage offset, of the first tile. Otherwise, the value 
		/// is ignored.
		/// </summary>
		public int yOffsetPercent;

	}
}
