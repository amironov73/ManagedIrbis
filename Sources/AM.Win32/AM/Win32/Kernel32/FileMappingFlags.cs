/* FileMappingFlags.cs -- file mapping flags for OpenFileMapping function. 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// File mapping flags for OpenFileMapping function.
	/// </summary>
	[Flags]
	public enum FileMappingFlags
	{
		/// <summary>
		/// ???
		/// </summary>
		SECTION_QUERY = 0x0001,

		/// <summary>
		/// ???
		/// </summary>
		SECTION_MAP_WRITE = 0x0002,

		/// <summary>
		/// ???
		/// </summary>
		SECTION_MAP_READ = 0x0004,

		/// <summary>
		/// ???
		/// </summary>
		SECTION_MAP_EXECUTE = 0x0008,

		/// <summary>
		/// ???
		/// </summary>
		SECTION_EXTEND_SIZE = 0x0010,

		/// <summary>
		/// ???
		/// </summary>
		STANDARD_RIGHTS_REQUIRED = 0x000F0000,

		/// <summary>
		/// ???
		/// </summary>
		SECTION_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED
		                     | SECTION_QUERY
		                     | SECTION_MAP_WRITE
		                     | SECTION_MAP_READ
		                     | SECTION_MAP_EXECUTE
		                     | SECTION_EXTEND_SIZE,

		/// <summary>
		/// <para>Copy-on-write access. If you create the map with 
		/// PAGE_WRITECOPY and the view with FILE_MAP_COPY, you will 
		/// receive a view to the file.</para>
		/// <para>If you share the mapping between multiple processes 
		/// using DuplicateHandle or OpenFileMapping and one process 
		/// writes to a view, the modification is not propagated to 
		/// the other process.</para>
		/// <para>Windows Me/98/95:  If you share the mapping between 
		/// multiple processes using DuplicateHandle or OpenFileMapping 
		/// and one process writes to a view, the modification is 
		/// propagated to the other process.</para>
		/// </summary>
		FILE_MAP_COPY = SECTION_QUERY,

		/// <summary>
		/// Read and write access. The file mapping object must be 
		/// created with PAGE_READWRITE protection. A read/write view 
		/// of the file is mapped.
		/// </summary>
		FILE_MAP_WRITE = SECTION_MAP_WRITE,

		/// <summary>
		/// Read-only access. The file mapping object must be created 
		/// with PAGE_READWRITE or PAGE_READONLY protection. A read-only 
		/// view of the file is mapped.
		/// </summary>
		FILE_MAP_READ = SECTION_MAP_READ,

		/// <summary>
		/// Includes all access rights to a file mapping object. 
		/// The MapViewOfFile and MapViewOfFileEx functions treat this 
		/// the same as if you had specified FILE_MAP_WRITE.
		/// </summary>
		FILE_MAP_ALL_ACCESS = SECTION_ALL_ACCESS
	}
}