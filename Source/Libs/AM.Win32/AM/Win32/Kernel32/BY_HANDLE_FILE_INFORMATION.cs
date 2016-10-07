/* BY_HANDLE_FILE_INFORMATION.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

using FILETIME=System.Runtime.InteropServices.ComTypes.FILETIME;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Contains information retrieved by the 
	/// GetFileInformationByHandle function.
	/// </summary>
	[StructLayout ( LayoutKind.Sequential, Size = 52 )]
	public struct BY_HANDLE_FILE_INFORMATION
	{
		/// <summary>
		/// File attributes.
		/// </summary>
		public FileAttributes dwFileAttributes;

		/// <summary>
		/// A FILETIME structure that specifies when the file or directory 
		/// was created. If the underlying file system does not support 
		/// creation time, this member is zero.
		/// </summary>
		public FILETIME ftCreationTime;

		/// <summary>
		/// A FILETIME structure. For a file, the structure specifies when 
		/// the file was last read from or written to. For a directory, 
		/// the structure specifies when the directory was created. 
		/// For both files and directories, the specified date will be correct, 
		/// but the time of day will always be set to midnight. If the underlying 
		/// file system does not support last access time, this member is zero. 
		/// </summary>
		public FILETIME ftLastAccessTime;

		/// <summary>
		/// A FILETIME structure. For a file, the structure specifies when 
		/// the file was last written to. For a directory, the structure 
		/// specifies when the directory was created. If the underlying 
		/// file system does not support last write time, this member is zero.
		/// </summary>
		public FILETIME ftLastWriteTime;

		/// <summary>
		/// Serial number of the volume that contains the file. 
		/// </summary>
		[CLSCompliant ( false )]
		public uint dwVolumeSerialNumber;

		/// <summary>
		/// High-order part of the file size.
		/// </summary>
		public int nFileSizeHigh;

		/// <summary>
		/// Low-order part of the file size. 
		/// </summary>
		[CLSCompliant ( false )]
		public uint nFileSizeLow;

		/// <summary>
		/// Number of links to this file. For the FAT file system this 
		/// member is always 1. For NTFS, it may be more than 1. 
		/// </summary>
		public int nNumberOfLinks;

		/// <summary>
		/// High-order part of a unique identifier associated with 
		/// the file. For more information, see nFileIndexLow.
		/// </summary>
		public int nFileIndexHigh;

		/// <summary>
		/// <para>Low-order part of a unique identifier associated 
		/// with the file.</para>
		/// <para>Note that this value is useful only while the file 
		/// is open by at least one process. If no processes have it 
		/// open, the index may change the next time the file is opened.
		/// </para>
		/// <para>The identifier (low and high parts) and the volume 
		/// serial number uniquely identify a file on a single computer. 
		/// To determine whether two open handles represent the same file, 
		/// combine this identifier and the volume serial number for each 
		/// file and compare them.</para>
		/// </summary>
		[CLSCompliant ( false )]
		public uint nFileIndexLow;
	}
}