// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MemoryFlags.cs --  memory allocation options
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Memory allocation options.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum MemoryFlags
    {
        /// <summary>
        /// Allocates physical storage in memory or in the 
        /// paging file on disk for the specified region of memory 
        /// pages. Windows initializes the memory to zero.
        /// </summary>
        MEM_COMMIT = 0x1000,

        /// <summary>
        /// Reserves a range of the process's virtual address space 
        /// without allocating any actual physical storage in memory 
        /// or in the paging file on disk. 
        /// </summary>
        MEM_RESERVE = 0x2000,

        /// <summary>
        /// <para>Decommits the specified region of committed pages. 
        /// After this operation, the pages are in the reserved state.
        /// </para>
        /// <para>The function does not fail if you attempt to decommit 
        /// an uncommitted page. This means that you can decommit a 
        /// range of pages without first determining their current 
        /// commitment state.</para>
        /// <para>Do not use this value with MEM_RELEASE.</para>
        /// </summary>
        MEM_DECOMMIT = 0x4000,

        /// <summary>
        /// <para>Releases the specified region of pages. After this 
        /// operation, the pages are in the free state.</para>
        /// <para>If you specify this value, dwSize must be zero, and 
        /// lpAddress must point to the base address returned by the 
        /// VirtualAllocEx function when the region was reserved. The 
        /// function fails if either of these conditions is not met.
        /// </para>
        /// <para>If any pages in the region are currently committed, 
        /// the function first decommits and then releases them.</para>
        /// <para>The function does not fail if you attempt to release 
        /// pages that are in different states, some reserved and some 
        /// committed. This means that you can release a range of pages 
        /// without first determining their current commitment state.
        /// </para>
        /// <para>Do not use this value with MEM_DECOMMIT.</para>
        /// </summary>
        MEM_RELEASE = 0x8000,

        /// <summary>
        /// Indicates free pages not accessible to the calling process 
        /// and available to be allocated. For free pages, the 
        /// information in the AllocationBase, AllocationProtect, 
        /// Protect, and Type members is undefined.
        /// </summary>
        MEM_FREE = 0x10000,

        /// <summary>
        /// Indicates that the memory pages within the region are 
        /// private (that is, not shared by other processes).
        /// </summary>
        MEM_PRIVATE = 0x20000,

        /// <summary>
        /// Indicates that the memory pages within the region are mapped 
        /// into the view of a section.
        /// </summary>
        MEM_MAPPED = 0x40000,

        /// <summary>
        /// <para>Specifies that the data in the memory range specified 
        /// by lpAddress and dwSize is no longer of interest. The pages 
        /// should not be read from or written to the paging file. 
        /// However, the memory block will be used again later, so it 
        /// should not be decommitted. This value cannot be used with 
        /// any other value.</para>
        /// <para>Using this value does not guarantee that the range 
        /// operated on with MEM_RESET will contain zeroes. If you want 
        /// the range to contain zeroes, decommit the memory and then 
        /// recommit it.</para>
        /// <para>When you specify MEM_RESET, the VirtualAlloc function 
        /// ignores the value of fProtect. However, you must still set 
        /// fProtect to a valid protection value, such as PAGE_NOACCESS.
        /// </para>
        /// <para>VirtualAlloc returns an error if you use MEM_RESET 
        /// and the range of memory is mapped to a file. A shared view 
        /// is only acceptable if it is mapped to a paging file.</para>
        /// <para>Windows Me/98/95: This flag is not supported.</para>
        /// </summary>
        MEM_RESET = 0x80000,

        /// <summary>
        /// <para>Allocates memory at the highest possible address.
        /// </para>
        /// <para>Windows Me/98/95:   This flag is not supported.</para>
        /// </summary>
        MEM_TOP_DOWN = 0x100000,

        /// <summary>
        /// <para>Causes the system to track pages that are written 
        /// to in the allocated region. If you specify this value, 
        /// you must also specify MEM_RESERVE.</para>
        /// <para>To retrieve the addresses of the pages that have 
        /// been written to since the region was allocated or the 
        /// write-tracking state was reset, call the GetWriteWatch 
        /// function. To reset the write-tracking state, call 
        /// GetWriteWatch or ResetWriteWatch. The write-tracking 
        /// feature remains enabled for the memory region until the 
        /// region is freed.</para>
        /// </summary>
        MEM_WRITE_WATCH = 0x200000,

        /// <summary>
        /// <para>Allocates physical memory with read-write access. 
        /// This value is solely for use with Address Windowing 
        /// Extensions (AWE) memory.</para>
        /// <para>This value must be used with MEM_RESERVE and 
        /// no other values.</para>
        /// </summary>
        MEM_PHYSICAL = 0x400000,

        /// <summary>
        /// <para>Allocates memory using large page support.</para>
        /// <para>The size and alignment must be a multiple of the 
        /// large-page minimum. To obtain this value, use the 
        /// GetLargePageMinimum function.</para>
        /// </summary>
        MEM_LARGE_PAGES = 0x20000000,

        /// <summary>
        /// 
        /// </summary>
        MEM_4MB_PAGES = unchecked((int)0x80000000),

        /// <summary>
        /// 
        /// </summary>
        SEC_FILE = 0x800000,

        /// <summary>
        /// Indicates that the memory pages within the region are mapped 
        /// into the view of an image section.
        /// </summary>
        SEC_IMAGE = 0x1000000,

        /// <summary>
        /// 
        /// </summary>
        SEC_RESERVE = 0x4000000,

        /// <summary>
        /// 
        /// </summary>
        SEC_COMMIT = 0x8000000,

        /// <summary>
        /// 
        /// </summary>
        SEC_NOCACHE = 0x10000000,

        /// <summary>
        /// Indicates that the memory pages within the region are mapped 
        /// into the view of an image section.
        /// </summary>
        MEM_IMAGE = SEC_IMAGE,

        /// <summary>
        /// ???
        /// </summary>
        WRITE_WATCH_FLAG_RESET = 0x01
    }
}