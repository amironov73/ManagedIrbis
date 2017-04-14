// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InteropUtility.cs -- set of interop helper routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !SILVERLIGHT && !UAP && !WIN81

#region Using directives

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Runtime
{
    /// <summary>
    /// Set of interop helper routines.
    /// </summary>
    [PublicAPI]
    public static class InteropUtility
    {
        /// <summary>
        /// Copies buffer to unmanaged memory and returns
        /// pointer to it.
        /// </summary>
        /// <param name="buffer">The buffer to copy.</param>
        /// <returns>Pointer to copy in unmanaged memory.
        /// This pointer must be released by 
        /// <see cref="Marshal.FreeHGlobal"/>.
        /// </returns>
        public static IntPtr BufferToPtr
            (
                [NotNull] byte[] buffer
            )
        {
            Code.NotNull(buffer, "buffer");

            IntPtr result = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, result, buffer.Length);

            return result;
        }

        /// <summary>
        /// Превращает структуру в массив байтов.
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        /// <remarks>Годится только для простых структур.</remarks>
        public static byte[] StructToPtr
            (
                [NotNull] object structure
            )
        {
            Code.NotNull(structure, "structure");

            int size = Marshal.SizeOf(structure);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structure, ptr, false);
            byte[] result = new byte[size];
            Marshal.Copy(ptr, result, 0, size);
            Marshal.FreeHGlobal(ptr);

            return result;
        }

        /// <summary>
        /// Превращает массив байтов в структуру.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="offset"></param>
        /// <param name="structure"></param>
        /// <remarks>Годится только для простых структур.</remarks>
        public static void PtrToStruct
            (
                [NotNull] byte[] block,
                int offset,
                [NotNull] object structure
            )
        {
            Code.NotNull(block, "block");
            Code.Nonnegative(offset, "offset");
            Code.NotNull(structure, "structure");

            int size = Marshal.SizeOf(structure);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(block, offset, ptr, size);
            Marshal.PtrToStructure(ptr, structure);
            Marshal.FreeHGlobal(ptr);
        }

        /// <summary>
        /// Borrowed from StackOverflow:
        /// http://stackoverflow.com/questions/2871/reading-a-c-c-data-structure-in-c-sharp-from-a-byte-array
        /// </summary>
        public static T ByteArrayToStructure<T>
            (
                [NotNull] byte[] bytes
            )
            where T : struct
        {
            Code.NotNull(bytes, "bytes");

            GCHandle handle = GCHandle.Alloc
                (
                    bytes,
                    GCHandleType.Pinned
                );
            T stuff = (T)Marshal.PtrToStructure
                (
                    handle.AddrOfPinnedObject(),
                    typeof(T)
                );
            handle.Free();

            return stuff;
        }

        /// <summary>
        /// Get block of bytes from given pointer.
        /// </summary>
        [NotNull]
        public static byte[] GetBlock
            (
                this IntPtr pointer,
                int blockLength
            )
        {
            byte[] result = new byte[blockLength];

            Marshal.Copy(pointer, result, 0, blockLength);

            return result;
        }

        /// <summary>
        /// Dump bytes from specified pointer.
        /// </summary>
        public static void DumpAddress
            (
                TextWriter writer,
                IntPtr pointer,
                int count
            )
        {
            byte[] bytes = new byte[count];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Marshal.ReadByte(pointer, i);
            }
            DumpUtility.Dump(writer, bytes);
        }

        /// <summary>
        /// Get zero-ended string from specified pointer.
        /// </summary>
        public static string GetZeroTerminatedString
            (
                IntPtr pointer,
                Encoding encoding,
                int maxLength
            )
        {
            int pos;
            for (pos = 0; pos < maxLength; pos++)
            {
                if (Marshal.ReadByte(pointer, pos) == 0)
                {
                    break;
                }
            }

            byte[] buffer = new byte[pos];
            Marshal.Copy(pointer, buffer, 0, pos);
            string result = encoding.GetString(buffer, 0, pos);

            return result;
        }

    }
}

#endif

