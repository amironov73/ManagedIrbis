/* ListViewBackImageFlags.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies how to draw ListView back image.
	/// </summary>
	[Flags]
	public enum ListViewBackImageFlags
	{
		/// <summary>
		/// The list-view control has no background image.
		/// </summary>
		LVBKIF_SOURCE_NONE = 0x00000000,

		/// <summary>
		/// ???
		/// </summary>
		LVBKIF_SOURCE_HBITMAP = 0x00000001,

		/// <summary>
		/// The pszImage member contains the URL of the background image.
		/// </summary>
		LVBKIF_SOURCE_URL = 0x00000002,

		/// <summary>
		/// ???
		/// </summary>
		LVBKIF_SOURCE_MASK = 0x00000003,

		/// <summary>
		/// The background image is displayed normally.
		/// </summary>
		LVBKIF_STYLE_NORMAL = 0x00000000,

		/// <summary>
		/// ???
		/// </summary>
		LVBKIF_STYLE_TILE = 0x00000010,

		/// <summary>
		/// ???
		/// </summary>
		LVBKIF_STYLE_MASK = 0x00000010,

		/// <summary>
		/// <para>You use this flag to specify the coordinates of the 
		/// first tile.</para>
		/// <para>This flag is valid only if the LVBKIF_STYLE_TILE 
		/// flag is also specified. If this flag is not specified, 
		/// the first tile begins at the upper-left corner of the 
		/// client area.</para>
		/// <para>If you use ComCtl32.dll Version 6.0 the xOffsetPercent 
		/// and yOffsetPercent fields contain pixel, not percentage values, 
		/// to specify the coordinates of the first tile.</para>
		/// </summary>
		LVBKIF_FLAG_TILEOFFSET = 0x00000100,

		/// <summary>
		/// ???
		/// </summary>
		LVBKIF_TYPE_WATERMARK = 0x10000000
	}
}
