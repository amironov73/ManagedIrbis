/* DuplicateHandleFlags.cs -- options for DuplicateHandle function.
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Options for DuplicateHandle function.
	/// </summary>
	[Flags]
	public enum DuplicateHandleFlags
	{
		/// <summary>
		/// Closes the source handle. This occurs regardless of 
		/// any error status returned.
		/// </summary>
		DUPLICATE_CLOSE_SOURCE = 0x00000001,

		/// <summary>
		/// Ignores the dwDesiredAccess parameter. The duplicate handle 
		/// has the same access as the source handle.
		/// </summary>
		DUPLICATE_SAME_ACCESS = 0x00000002
	}
}
