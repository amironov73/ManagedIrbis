// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListViewFindFlags.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Type of search to perform by ListView_FindItem function.
	/// </summary>
	public enum ListViewFindFlags
	{
		/// <summary>
		/// Searches for a match between this structure's lParam member 
		/// and the lParam member of an item's LVITEM structure. 
		/// If LVFI_PARAM is specified, all other flags are ignored.
		/// </summary>
		LVFI_PARAM = 0x0001,

		/// <summary>
		/// Searches based on the item text. Unless additional values 
		/// are specified, the item text of the matching item must exactly 
		/// match the string pointed to by the psz member.
		/// </summary>
		LVFI_STRING = 0x0002,

		/// <summary>
		/// Checks to see if the item text begins with the string pointed 
		/// to by the psz member. This value implies use of LVFI_STRING.
		/// </summary>
		LVFI_PARTIAL = 0x0008,

		/// <summary>
		/// Continues the search at the beginning if no match is found.
		/// </summary>
		LVFI_WRAP = 0x0020,

		/// <summary>
		/// Finds the item nearest to the position specified in the pt 
		/// member, in the direction specified by the vkDirection member. 
		/// This flag is supported only by large icon and small icon modes. 
		/// </summary>
		LVFI_NEARESTXY = 0x0040
	}
}
