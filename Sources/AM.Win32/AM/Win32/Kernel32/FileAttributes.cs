/* FileAttributes.cs --  
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Attributes for shell entities
	/// </summary>
	[Flags]
	public enum FileAttributes
	{
		/// <summary>
		/// File is Read-only.
		/// </summary>
		FILE_ATTRIBUTE_READONLY = 0x00000001,

		/// <summary>
		/// Hidden file.
		/// </summary>
		FILE_ATTRIBUTE_HIDDEN = 0x00000002,

		/// <summary>
		/// System file.
		/// </summary>
		FILE_ATTRIBUTE_SYSTEM = 0x00000004,

		/// <summary>
		/// File is a directory.
		/// </summary>
		FILE_ATTRIBUTE_DIRECTORY = 0x00000010,

		/// <summary>
		/// File archive bit is turned on.
		/// </summary>
		FILE_ATTRIBUTE_ARCHIVE = 0x00000020,

		/// <summary>
		/// File is a device.
		/// </summary>
		FILE_ATTRIBUTE_DEVICE = 0x00000040,

		/// <summary>
		/// Not readonly, hidden, system or directory.
		/// </summary>
		FILE_ATTRIBUTE_NORMAL = 0x00000080,

		/// <summary>
		/// Temporary file.
		/// </summary>
		FILE_ATTRIBUTE_TEMPORARY = 0x00000100,

		/// <summary>
		/// ???
		/// </summary>
		FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200,

		/// <summary>
		/// ???
		/// </summary>
		FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400,

		/// <summary>
		/// File is compressed.
		/// </summary>
		FILE_ATTRIBUTE_COMPRESSED = 0x00000800,

		/// <summary>
		/// File is offline.
		/// </summary>
		FILE_ATTRIBUTE_OFFLINE = 0x00001000,

		/// <summary>
		/// File is not indexed by content.
		/// </summary>
		FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000,

		/// <summary>
		/// Encrypted file.
		/// </summary>
		FILE_ATTRIBUTE_ENCRYPTED = 0x00004000
	}
}
