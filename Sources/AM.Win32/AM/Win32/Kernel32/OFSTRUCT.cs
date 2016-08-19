/* OFSTRUCT.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// </summary>
	[StructLayout ( LayoutKind.Sequential, Size = OFSTRUCT.SIZE )]
	public struct OFSTRUCT
	{
		/// <summary>
		/// Size of the structure, in bytes.
		/// </summary>
		public const int SIZE = 136;

		/// <summary>
		/// 
		/// </summary>
		public const int OFS_MAXPATHNAME = 128;

		/// <summary>
		/// Length of the structure, in bytes.
		/// </summary>
		public byte cBytes;

		/// <summary>
		/// If this member is nonzero, the file is on a hard 
		/// (fixed) disk. Otherwise, it is not.
		/// </summary>
		public byte fFixedDisk;

		/// <summary>
		/// MS-DOS error code if the OpenFile function failed.
		/// </summary>
		public short nErrCode;

		/// <summary>
		/// Reserved; do not use.
		/// </summary>
		public short Reserved1;

		/// <summary>
		/// Reserved; do not use.
		/// </summary>
		public short Reserved2;

		/// <summary>
		/// Path and file name of the file.
		/// </summary>
		[MarshalAs ( UnmanagedType.ByValTStr, SizeConst = OFS_MAXPATHNAME )]
		public string szPathName;
	}
}