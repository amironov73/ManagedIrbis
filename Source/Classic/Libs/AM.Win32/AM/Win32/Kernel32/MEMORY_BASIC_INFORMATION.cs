// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MEMORY_BASIC_INFORMATION.cs -- information about a range of pages
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Contains information about a range of pages in the virtual 
    /// address space of a process. The VirtualQuery and VirtualQueryEx 
    /// functions use this structure.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 28)]
    public struct MEMORY_BASIC_INFORMATION
    {
        /// <summary>
        /// Pointer to the base address of the region of pages.
        /// </summary>
        public IntPtr BaseAddress;

        /// <summary>
        /// Pointer to the base address of a range of pages allocated 
        /// by the VirtualAlloc function. The page pointed to by the 
        /// BaseAddress member is contained within this allocation range.
        /// </summary>
        public IntPtr AllocationBase;

        /// <summary>
        /// Memory protection when the region was initially allocated. 
        /// This member can be one of the memory protection options, 
        /// along with PAGE_GUARD or PAGE_NOCACHE, as needed.
        /// </summary>
        public MemoryProtectionFlags AllocationProtect;

        /// <summary>
        /// Size of the region beginning at the base address in which 
        /// all pages have identical attributes, in bytes.
        /// </summary>
        [CLSCompliant(false)]
        public uint RegionSize;

        /// <summary>
        /// State of the pages in the region.
        /// </summary>
        public MemoryFlags State;

        /// <summary>
        /// Access protection of the pages in the region.
        /// </summary>
        public MemoryFlags Protect;

        /// <summary>
        /// Type of pages in the region.
        /// </summary>
        public MemoryFlags Type;
    }
}
