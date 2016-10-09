/* DRIVETRYPE.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Describes available types for local drives.
	/// </summary>
	public enum DRIVETYPE
	{
		/// <summary>
		/// The drive type cannot be determined.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// The root path is invalid. For example, no volume 
		/// is mounted at the path.
		/// </summary>
		NotRootDir = 1,

		/// <summary>
		/// The disk can be removed from the drive.
		/// </summary>
		Removable = 2,

		/// <summary>
		/// The disk cannot be removed from the drive.
		/// </summary>
		Fixed = 3,

		/// <summary>
		/// The drive is a remote (network) drive.
		/// </summary>
		Remote = 4,

		/// <summary>
		/// The drive is a CD-ROM drive.
		/// </summary>
		CDROM = 5,

		/// <summary>
		/// The drive is a RAM disk.
		/// </summary>
		RamDisk = 6,
	}
}
