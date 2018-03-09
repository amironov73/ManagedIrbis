// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LocalAllocFlags.cs -- memory allocation flags
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;


#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Memory allocation flags.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum LocalAllocFlags
    {
        /// <summary>
        /// Allocates fixed memory. The return value is a pointer.
        /// </summary>
        LMEM_FIXED = 0x0000,

        /// <summary>
        /// <para>Allocates movable memory. Memory blocks are never 
        /// moved in physical memory, but they can be moved within the 
        /// default heap.</para>
        /// <para>The return value is a handle to the memory object. 
        /// To translate the handle into a pointer, use the LocalLock 
        /// function.</para>
        /// <para>This value cannot be combined with GMEM_FIXED.</para>
        /// </summary>
        LMEM_MOVEABLE = 0x0002,

        /// <summary>
        /// Obsolete, but are provided for compatibility with 16-bit 
        /// Windows. Ignored.
        /// </summary>
        LMEM_NOCOMPACT = 0x0010,

        /// <summary>
        /// Obsolete, but are provided for compatibility with 16-bit 
        /// Windows. Ignored.
        /// </summary>
        LMEM_NODISCARD = 0x0020,

        /// <summary>
        /// Initializes memory contents to zero.
        /// </summary>
        LMEM_ZEROINIT = 0x0040,

        /// <summary>
        /// Obsolete, but are provided for compatibility with 16-bit 
        /// Windows. Ignored.
        /// </summary>
        LMEM_MODIFY = 0x0080,

        /// <summary>
        /// Obsolete, but are provided for compatibility with 16-bit 
        /// Windows. Ignored.
        /// </summary>
        LMEM_DISCARDABLE = 0x0100,

        /// <summary>
        /// Obsolete, but are provided for compatibility with 16-bit 
        /// Windows. Ignored.
        /// </summary>
        LMEM_NOT_BANKED = 0x1000,

        /// <summary>
        /// Obsolete, but are provided for compatibility with 16-bit 
        /// Windows. Ignored.
        /// </summary>
        LMEM_SHARE = 0x2000,

        /// <summary>
        /// Obsolete, but are provided for compatibility with 16-bit 
        /// Windows. Ignored.
        /// </summary>
        LMEM_DDESHARE = 0x2000,

        /// <summary>
        /// Obsolete, but are provided for compatibility with 16-bit 
        /// Windows. Ignored.
        /// </summary>
        LMEM_NOTIFY = 0x4000,

        /// <summary>
        /// Obsolete, but are provided for compatibility with 16-bit 
        /// Windows. Ignored.
        /// </summary>
        LMEM_LOWER = LMEM_NOT_BANKED,

        /// <summary>
        /// ???
        /// </summary>
        LMEM_VALID_FLAGS = 0x7F72,

        /// <summary>
        /// ???
        /// </summary>
        LMEM_INVALID_HANDLE = 0x8000,

        /// <summary>
        /// Combines LMEM_MOVEABLE and LMEM_ZEROINIT.
        /// </summary>
        LHND = LMEM_MOVEABLE | LMEM_ZEROINIT,

        /// <summary>
        /// Combines LMEM_FIXED and LMEM_ZEROINIT.
        /// </summary>
        LPTR = LMEM_FIXED | LMEM_ZEROINIT,
    }
}
