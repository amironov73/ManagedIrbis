/* BinaryReaderUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using AM.Collections;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class BinaryReaderUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Read <see cref="NonNullCollection{T}"/>
        /// </summary>
        [NotNull]
        public static NonNullCollection<T> ReadNonNullCollection<T>
            (
                [NotNull] this BinaryReader reader
            )
            where T: class, IHandmadeSerializable, new ()
        {
            Code.NotNull(reader, "reader");

            T[] array = reader.ReadArray<T>();
            NonNullCollection<T> result = new NonNullCollection<T>();
            result.AddRange(array);

            return result;
        }

        /// <summary>
        /// Read array from stream
        /// </summary>
        public static T[] ReadArray<T>
            (
                [NotNull] this BinaryReader reader
            )
            where T: IHandmadeSerializable, new()
        {
            Code.NotNull(reader, "reader");

            int count = reader.ReadPackedInt32();
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
            {
                T item = new T();
                item.RestoreFromStream(reader);
                result[i] = item;
            }

            return result;
        }

        /// <summary>
        /// Read array of bytes.
        /// </summary>
        [NotNull]
        public static byte[] ReadByteArray
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            int length = reader.ReadPackedInt32();
            byte[] result = new byte[length];
            reader.Read(result, 0, length);

            return result;
        }

        /// <summary>
        /// Reads collection of items from the stream.
        /// </summary>
        [NotNull]
        public static BinaryReader ReadCollection<T>
            (
                [NotNull] this BinaryReader reader,
                [NotNull] NonNullCollection<T> collection
            )
            where T : class, IHandmadeSerializable, new()
        {
            Code.NotNull(reader, "reader");
            Code.NotNull(collection, "collection");

            collection.Clear();

            int count = reader.ReadPackedInt32();
            for (int i = 0; i < count; i++)
            {
                T item = new T();
                item.RestoreFromStream(reader);
                collection.Add(item);
            }

            return reader;
        }

#if !WINMOBILE && !PocketPC

        /// <summary>
        /// Read <see cref="DateTime"/> from the stream.
        /// </summary>
        public static DateTime ReadDateTime
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            DateTime result = DateTime.FromBinary(reader.ReadInt64());

            return result;
        }

#endif

        /// <summary>
        /// Read array of 16-bit integers.
        /// </summary>
        [NotNull]
        public static short[] ReadInt16Array
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            int length = reader.ReadPackedInt32();
            short[] result = new short[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = reader.ReadInt16();
            }

            return result;
        }

        /// <summary>
        /// Read array of 32-bit integers.
        /// </summary>
        [NotNull]
        public static int[] ReadInt32Array
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            int length = reader.ReadPackedInt32();
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = reader.ReadInt32();
            }

            return result;
        }

        /// <summary>
        /// Read array of 64-bit integers.
        /// </summary>
        [NotNull]
        public static long[] ReadInt64Array
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            int length = reader.ReadPackedInt32();
            long[] result = new long[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = reader.ReadInt64();
            }

            return result;
        }

        /// <summary>
        /// Reads list of items from the stream.
        /// </summary>
        [NotNull]
        public static List<T> ReadList<T>
            (
                [NotNull] this BinaryReader reader
            )
            where T: IHandmadeSerializable, new()
        {
            Code.NotNull(reader, "reader");

            int count = reader.ReadPackedInt32();
            List<T> result = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                T item = new T();
                item.RestoreFromStream(reader);
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Reads list of items from the stream.
        /// </summary>
        [NotNull]
        public static BinaryReader ReadList<T>
            (
                [NotNull] this BinaryReader reader,
                [NotNull] List<T> list 
            )
            where T : IHandmadeSerializable, new()
        {
            Code.NotNull(reader, "reader");
            Code.NotNull(list, "list");

            int count = reader.ReadPackedInt32();
            for (int i = 0; i < count; i++)
            {
                T item = new T();
                item.RestoreFromStream(reader);
                list.Add(item);
            }

            return reader;
        }

        /// <summary>
        /// Read nullable byte.
        /// </summary>
        [CanBeNull]
        public static byte? ReadNullableByte
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            bool flag = reader.ReadBoolean();
            return flag
                ? (byte?)reader.ReadByte()
                : null;
        }

        /// <summary>
        /// Read nullable double precision number.
        /// </summary>
        [CanBeNull]
        public static double? ReadNullableDouble
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            bool flag = reader.ReadBoolean();
            return flag
                ? (double?)reader.ReadDouble()
                : null;
        }

#if !SILVERLIGHT

        /// <summary>
        /// Read nullable decimal.
        /// </summary>
        [CanBeNull]
        public static decimal? ReadNullableDecimal
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            bool flag = reader.ReadBoolean();
            return flag
                ? (decimal?)reader.ReadDecimal()
                : null;
        }

#endif

        /// <summary>
        /// Read nullable 16-bit integer.
        /// </summary>
        [CanBeNull]
        public static short? ReadNullableInt16
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            bool flag = reader.ReadBoolean();
            return flag
                ? (short?)reader.ReadInt16()
                : null;
        }

        /// <summary>
        /// Read nullable 32-bit integer.
        /// </summary>
        [CanBeNull]
        public static int? ReadNullableInt32
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            bool flag = reader.ReadBoolean();
            return flag
                ? (int?)reader.ReadInt32()
                : null;
        }

        /// <summary>
        /// Read array of 32-bit integers.
        /// </summary>
        [CanBeNull]
        public static int[] ReadNullableInt32Array
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            bool isNull = !reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }

            int length = reader.ReadPackedInt32();
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = reader.ReadInt32();
            }

            return result;
        }


        /// <summary>
        /// Read nullable 64-bit integer.
        /// </summary>
        [CanBeNull]
        public static long? ReadNullableInt64
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            bool flag = reader.ReadBoolean();
            return flag
                ? (long?)reader.ReadInt64()
                : null;
        }

        /// <summary>
        /// Read nullable string.
        /// </summary>
        [CanBeNull]
        public static string ReadNullableString
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            bool flag = reader.ReadBoolean();
            return flag
                ? reader.ReadString()
                : null;
        }

        /// <summary>
        /// Read 32-bit integer in packed format.
        /// </summary>
        /// <remarks>Borrowed from
        /// http://referencesource.microsoft.com/
        /// </remarks>
        public static int ReadPackedInt32
            (
                [NotNull] this BinaryReader reader
            )
        {
            int count = 0;
            int shift = 0;
            byte b;
            do
            {
                if (shift == 5*7)
                {
                    throw new FormatException();
                }

                b = reader.ReadByte();
                count |= (b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return count;
        }

        /// <summary>
        /// Read 64-bit integer in packed format.
        /// </summary>
        /// <remarks>Inspired by
        /// http://referencesource.microsoft.com/
        /// </remarks>
        public static long ReadPackedInt64
            (
                [NotNull] this BinaryReader reader
            )
        {
            long count = 0;
            int shift = 0;
            long b;
            do
            {
                b = reader.ReadByte();
                count |= (b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return count;
        }

        /// <summary>
        /// Read string with given length.
        /// </summary>
        [NotNull]
        public static string ReadString
            (
                [NotNull] this BinaryReader reader,
                int count
            )
        {
            Code.NotNull(reader, "reader");
            Code.Positive(count, "count");

            char[] characters = reader.ReadChars(count);
            string result = new string(characters);

            return result;
        }

        /// <summary>
        /// Read array of strings.
        /// </summary>
        [NotNull]
        public static string[] ReadStringArray
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            int length = reader.ReadPackedInt32();
            string[] result = new string[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = reader.ReadString();
            }

            return result;
        }

        #endregion
    }
}
