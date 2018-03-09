// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HeapFlags.cs -- heap creation and allocation options
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Heap creation and allocation options.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum HeapFlags
    {
        /// <summary>
        /// Mutual exclusion will not be used when the heap functions 
        /// allocate and free memory from this heap. The default, 
        /// when HEAP_NO_SERIALIZE is not specified, is to serialize 
        /// access to the heap. Serialization of heap access allows two 
        /// or more threads to simultaneously allocate and free memory 
        /// from the same heap.
        /// </summary>
        HEAP_NO_SERIALIZE = 0x00000001,

        /// <summary>
        /// ??
        /// </summary>
        HEAP_GROWABLE = 0x00000002,

        /// <summary>
        /// The system will raise an exception to indicate a function 
        /// failure, such as an out-of-memory condition, instead of 
        /// returning NULL.
        /// </summary>
        HEAP_GENERATE_EXCEPTIONS = 0x00000004,

        /// <summary>
        /// The allocated memory will be initialized to zero. Otherwise, 
        /// the memory is not initialized to zero.
        /// </summary>
        HEAP_ZERO_MEMORY = 0x00000008,

        /// <summary>
        /// There can be no movement when reallocating a memory block. 
        /// If this value is not specified, the function may move the 
        /// block to a new location. If this value is specified and 
        /// the block cannot be resized without moving, the function 
        /// fails, leaving the original memory block unchanged.
        /// </summary>
        HEAP_REALLOC_IN_PLACE_ONLY = 0x00000010,

        /// <summary>
        /// ???
        /// </summary>
        HEAP_TAIL_CHECKING_ENABLED = 0x00000020,

        /// <summary>
        /// ???
        /// </summary>
        HEAP_FREE_CHECKING_ENABLED = 0x00000040,

        /// <summary>
        /// ???
        /// </summary>
        HEAP_DISABLE_COALESCE_ON_FREE = 0x00000080,

        /// <summary>
        /// ???
        /// </summary>
        HEAP_CREATE_ALIGN_16 = 0x00010000,

        /// <summary>
        /// ???
        /// </summary>
        HEAP_CREATE_ENABLE_TRACING = 0x00020000,

        /// <summary>
        /// ???
        /// </summary>
        HEAP_MAXIMUM_TAG = 0x0FFF,

        /// <summary>
        /// ???
        /// </summary>
        HEAP_PSEUDO_TAG_FLAG = 0x8000,
    }
}
