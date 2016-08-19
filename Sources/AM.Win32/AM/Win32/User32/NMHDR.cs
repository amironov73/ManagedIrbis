/* NMHDR.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Contains information about a notification message.
	/// </summary>
	[Serializable]
	[StructLayout ( LayoutKind.Sequential )]
	public struct NMHDR
	{
		/// <summary>
		/// Window handle to the control sending a message.
		/// </summary>
		public IntPtr hwndFrom;

		/// <summary>
		/// Identifier of the control sending a message.
		/// </summary>
        [CLSCompliant ( false )]
        public uint idFrom;

		/// <summary>
		/// Notification code. This member can be a control-specific 
		/// notification code or it can be one of the common notification 
		/// codes.
		/// </summary>
		public int code;
	}
}
