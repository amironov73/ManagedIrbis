// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GlobalAllocFlags.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Memory allocation flags.
	/// </summary>
	[Flags]
	public enum GlobalAllocFlags
	{
		/// <summary>
		/// Allocates fixed memory. The return value is a pointer.
		/// </summary>
		GMEM_FIXED = 0x0000,

		/// <summary>
		/// <para>Allocates movable memory. Memory blocks are never 
		/// moved in physical memory, but they can be moved within the 
		/// default heap.</para>
		/// <para>The return value is a handle to the memory object. 
		/// To translate the handle into a pointer, use the GlobalLock 
		/// function.</para>
		/// <para>This value cannot be combined with GMEM_FIXED.</para>
		/// </summary>
		GMEM_MOVEABLE = 0x0002,

		/// <summary>
		/// Obsolete, but are provided for compatibility with 16-bit 
		/// Windows. Ignored.
		/// </summary>
		GMEM_NOCOMPACT = 0x0010,

		/// <summary>
		/// Obsolete, but are provided for compatibility with 16-bit 
		/// Windows. Ignored.
		/// </summary>
		GMEM_NODISCARD = 0x0020,

		/// <summary>
		/// Initializes memory contents to zero.
		/// </summary>
		GMEM_ZEROINIT = 0x0040,

		/// <summary>
		/// Obsolete, but are provided for compatibility with 16-bit 
		/// Windows. Ignored.
		/// </summary>
		GMEM_MODIFY = 0x0080,

		/// <summary>
		/// Obsolete, but are provided for compatibility with 16-bit 
		/// Windows. Ignored.
		/// </summary>
		GMEM_DISCARDABLE = 0x0100,

		/// <summary>
		/// Obsolete, but are provided for compatibility with 16-bit 
		/// Windows. Ignored.
		/// </summary>
		GMEM_NOT_BANKED = 0x1000,

		/// <summary>
		/// Obsolete, but are provided for compatibility with 16-bit 
		/// Windows. Ignored.
		/// </summary>
		GMEM_SHARE = 0x2000,

		/// <summary>
		/// Obsolete, but are provided for compatibility with 16-bit 
		/// Windows. Ignored.
		/// </summary>
		GMEM_DDESHARE = 0x2000,

		/// <summary>
		/// Obsolete, but are provided for compatibility with 16-bit 
		/// Windows. Ignored.
		/// </summary>
		GMEM_NOTIFY = 0x4000,

		/// <summary>
		/// Obsolete, but are provided for compatibility with 16-bit 
		/// Windows. Ignored.
		/// </summary>
		GMEM_LOWER = GMEM_NOT_BANKED,

		/// <summary>
		/// ???
		/// </summary>
		GMEM_VALID_FLAGS = 0x7F72,

		/// <summary>
		/// ???
		/// </summary>
		GMEM_INVALID_HANDLE = 0x8000,

		/// <summary>
		/// Combines GMEM_MOVEABLE and GMEM_ZEROINIT.
		/// </summary>
		GHND = ( GMEM_MOVEABLE | GMEM_ZEROINIT ),

		/// <summary>
		/// Combines GMEM_FIXED and GMEM_ZEROINIT.
		/// </summary>
		GPTR = ( GMEM_FIXED | GMEM_ZEROINIT ),
	}
}
