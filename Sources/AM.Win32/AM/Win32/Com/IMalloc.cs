/* IMalloc.cs --  allocates, frees and manages memory
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// The IMalloc interface allocates, frees, and manages memory.
	/// </summary>
	[ComImport]
	[Guid ( "00000002-0000-0000-C000-000000000046" )]
	[InterfaceType ( ComInterfaceType.InterfaceIsIUnknown )]
	public interface IMalloc : IUnknown
	{
		/// <summary>
		/// Allocates a block of memory.
		/// </summary>
		/// <param name="cb">Size, in bytes, of the memory block 
		/// to be allocated.</param>
		/// <returns>If successful, Alloc returns a pointer to the 
		/// allocated memory block. If insufficient memory is available, 
		/// Alloc returns NULL.</returns>
		[PreserveSig]
		IntPtr Alloc
			(
			int cb
			);

		/// <summary>
		/// Changes the size of a previously allocated memory block.
		/// </summary>
		/// <param name="pv">Pointer to the memory block to be reallocated. 
		/// The pointer can have a NULL value.</param>
		/// <param name="cb">Size of the memory block (in bytes) 
		/// to be reallocated. It can be zero.</param>
		/// <returns>Memory block successfully reallocated. If insufficient 
		/// memory, or cb is zero and pv is not NULL, returns NULL.</returns>
		[PreserveSig]
		IntPtr Realloc
			(
			IntPtr pv,
			int cb
			);

		/// <summary>
		/// Frees a previously allocated block of memory.
		/// </summary>
		/// <param name="pv">Pointer to the memory block to be freed.</param>
		[PreserveSig]
		void Free
			(
			IntPtr pv
			);

		/// <summary>
		/// Returns the size (in bytes) of a memory block previously allocated 
		/// with IMalloc::Alloc or IMalloc::Realloc.
		/// </summary>
		/// <param name="pv">Pointer to the memory block for which the size 
		/// is requested.</param>
		/// <returns>The size of the allocated memory block in bytes or, 
		/// if pv is a NULL pointer, -1.</returns>
		[PreserveSig]
		int GetSize
			(
			IntPtr pv
			);

		/// <summary>
		/// Determines whether this allocator was used to allocate the 
		/// specified block of memory.
		/// </summary>
		/// <param name="pv">Pointer to the memory block; can be a NULL 
		/// pointer, in which case, -1 is returned.</param>
		/// <returns><list type="bullet">
		/// <item>1 - the memory block was allocated by this IMalloc 
		/// instance.</item>
		/// <item>0 - the memory block was not allocated by this IMalloc 
		/// instance.</item>
		/// <item>-1 - DidAlloc is unable to determine whether or not it 
		/// allocated the memory block.</item>
		/// </list></returns>
		[PreserveSig]
		int DidAlloc
			(
			IntPtr pv
			);

		/// <summary>
		/// Minimizes the heap as much as possible by releasing unused memory 
		/// to the operating system, coalescing adjacent free blocks and 
		/// committing free pages.
		/// </summary>
		[PreserveSig]
		void HeapMinimize ();
	}
}