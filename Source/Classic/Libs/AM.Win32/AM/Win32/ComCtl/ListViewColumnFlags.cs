// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListViewColumnFlags.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifying which members of LVCOLUMN structure
	/// contain valid information.
	/// </summary>
	[Flags]
	public enum ListViewColumnFlags
	{
		/// <summary>
		/// The fmt member is valid.
		/// </summary>
		LVCF_FMT = 0x0001,

		/// <summary>
		/// The cx member is valid.
		/// </summary>
		LVCF_WIDTH = 0x0002,

		/// <summary>
		/// The pszText member is valid.
		/// </summary>
		LVCF_TEXT = 0x0004,

		/// <summary>
		/// The iSubItem member is valid.
		/// </summary>
		LVCF_SUBITEM = 0x0008,

		/// <summary>
		/// Version 4.70. The iImage member is valid.
		/// </summary>
		LVCF_IMAGE = 0x0010,

		/// <summary>
		/// Version 4.70. The iOrder member is valid.
		/// </summary>
		LVCF_ORDER = 0x0020
	}
}
