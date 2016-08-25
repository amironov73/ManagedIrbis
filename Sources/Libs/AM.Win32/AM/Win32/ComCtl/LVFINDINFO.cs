/* LVFINDINFO.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Contains information used when searching for a list-view item. 
	/// This structure is identical to LVFINDINFO but has been renamed 
	/// to fit standard naming conventions.
	/// </summary>
	[Serializable]
	[StructLayout ( LayoutKind.Sequential )]
	public struct LVFINDINFO
	{
		/// <summary>
		/// Type of search to perform.
		/// </summary>
		public ListViewFindFlags flags;

		/// <summary>
		/// Address of a null-terminated string to compare with the 
		/// item text. It is valid only if LVFI_STRING or LVFI_PARTIAL 
		/// is set in the flags member. 
		/// </summary>
		[MarshalAs ( UnmanagedType.LPTStr )]
		public string psz;

		/// <summary>
		/// Value to compare with the lParam member of a list-view item's 
		/// LVITEM structure. It is valid only if LVFI_PARAM is set in the 
		/// flags member. 
		/// </summary>
		public int lParam;

		/// <summary>
		/// POINT structure with the initial search position. It is valid 
		/// only if LVFI_NEARESTXY is set in the flags member. 
		/// </summary>
		public Point pt;
		
		/// <summary>
		/// <para>Virtual key code that specifies the direction to search. 
		/// The following codes are supported:</para>
		/// <list type="bullet">
		/// <item>VK_LEFT</item>
		/// <item>VK_RIGHT</item>
		/// <item>VK_UP</item>
		/// <item>VK_DOWN</item>
		/// <item>VK_HOME</item>
		/// <item>VK_END</item>
		/// <item>VK_PRIOR</item>
		/// <item>VK_NEXT</item>
		/// </list>
		/// <para>This member is valid only if LVFI_NEARESTXY is set 
		/// in the flags member.</para>
		/// </summary>
		public int vkDirection;
	}
}
