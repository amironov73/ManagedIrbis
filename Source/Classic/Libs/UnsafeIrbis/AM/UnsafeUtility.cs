// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UnsafeUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM
{
    /// <summary>
    /// Utility for unsafe tricks.
    /// </summary>
    [PublicAPI]
    public static unsafe class UnsafeUtility
    {
        #region Public methods

        /// <summary>
        /// Allocate unmanaged memory.
        /// </summary>
        public static Span<byte> Alloc
            (
                int size
            )
        {
            Code.Positive(size, nameof(size));

            IntPtr handle = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);
            void* pointer = handle.ToPointer();
            Span<byte> result = new Span<byte>(pointer, size);

            return result;
        }

        /// <summary>
        /// Convert the object to <see cref="Span{T}"/>.
        /// </summary>
        public static Span<byte> AsSpan<T>
            (
                ref T value,
                int size
            )
        {
            void* ptr = Unsafe.AsPointer(ref value);
            Span<byte> span = new Span<byte>(ptr, size);

            return span;
        }

        /// <summary>
        /// Convert the object to <see cref="Span{T}"/>.
        /// </summary>
        public static Span<byte> AsSpan<T>
            (
                ref T value
            )
        {
            void* ptr = Unsafe.AsPointer(ref value);
            int size = Unsafe.SizeOf<T>();
            Span<byte> result = new Span<byte>(ptr, size);

            return result;
        }

        /// <summary>
        /// Free unmanaged memory block allocated by <see cref="Alloc"/>.
        /// </summary>
        public static void Free
            (
                Span<byte> block
            )
        {
            void* pointer = Unsafe.AsPointer(ref block[0]);
            GC.RemoveMemoryPressure(block.Length);
            IntPtr handle = new IntPtr(pointer);
            Marshal.FreeHGlobal(handle);
        }

        #endregion
    }
}
