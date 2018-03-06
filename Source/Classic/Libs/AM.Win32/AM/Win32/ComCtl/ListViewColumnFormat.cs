// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListViewColumnFormat.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Alignment of the column header and the subitem text in the column.
	/// </summary>
	[Flags]
	public enum ListViewColumnFormat
	{
		/// <summary>
		/// Text is left-aligned.
		/// </summary>
		LVCFMT_LEFT = 0x0000,

		/// <summary>
		/// Text is right-aligned.
		/// </summary>
		LVCFMT_RIGHT = 0x0001,

		/// <summary>
		/// Text is centered.
		/// </summary>
		LVCFMT_CENTER = 0x0002,

		/// <summary>
		/// A bitmask used to select those bits of fmt that control field 
		/// justification. To check the format of a column, use a logical 
		/// "and" to combine LCFMT_JUSTIFYMASK with fmt. You can then use 
		/// a switch statement to determine whether the LVCFMT_LEFT, 
		/// LVCFMT_RIGHT, or LVCFMT_CENTER bits are set.
		/// </summary>
		LVCFMT_JUSTIFYMASK = 0x0003,

		/// <summary>
		/// Version 4.70. The item displays an image from an image list.
		/// </summary>
		LVCFMT_IMAGE = 0x0800,

		/// <summary>
		/// Version 4.70. The bitmap appears to the right of text. 
		/// This does not affect an image from an image list assigned 
		/// to the header item.
		/// </summary>
		LVCFMT_BITMAP_ON_RIGHT = 0x1000,

		/// <summary>
		/// Version 4.70. The header item contains an image in the image list.
		/// </summary>
		LVCFMT_COL_HAS_IMAGES = 0x8000
	}
}
