/* DocInfoFlags.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Specifies additional information about the print job.
	/// </summary>
	[Flags]
	public enum DocInfoFlags
	{
		/// <summary>
		/// Applications that use banding should set this flag for optimal 
		/// performance during printing.
		/// </summary>
		DI_APPBANDING = 0x00000001,

		/// <summary>
		/// The application will use raster operations that involve reading 
		/// from the destination surface.
		/// </summary>
		DI_ROPS_READ_DESTINATION = 0x00000002
	}
}
