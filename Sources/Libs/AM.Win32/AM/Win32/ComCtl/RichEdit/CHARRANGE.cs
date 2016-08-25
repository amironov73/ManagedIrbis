/* CHARRANGE.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// <para>The CHARRANGE structure specifies a range of characters 
    /// in a rich edit control. This structure is used with the EM_EXGETSEL 
    /// and EM_EXSETSEL messages.
    /// </para>
    /// <para>If the cpMin and cpMax members are equal, the range is empty. 
    /// The range includes everything if cpMin is 0 and cpMax is —1.
    /// </para>
    /// </summary>
	[StructLayout ( LayoutKind.Sequential )]
	public struct CHARRANGE
	{
        /// <summary>
        /// Character position index immediately preceding the first 
        /// character in the range.
        /// </summary>
		public int cpMin;

        /// <summary>
        /// Character position immediately following the last character 
        /// in the range. 
        /// </summary>
		public int cpMax;
	}
}
