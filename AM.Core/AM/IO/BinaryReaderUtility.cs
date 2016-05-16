/* BinaryReaderUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

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
        /// Read array of bytes.
        /// </summary>
        [NotNull]
        public static byte[] ReadByteArray
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(() => reader);

            int length = reader.ReadPackedInt32();
            byte[] result = new byte[length];
            reader.Read(result, 0, length);

            return result;
        }

        /// <summary>
        /// Read array of 16-bit integers.
        /// </summary>
        [NotNull]
        public static short[] ReadInt16Array
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(() => reader);

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
            Code.NotNull(() => reader);

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
            Code.NotNull(() => reader);

            int length = reader.ReadPackedInt32();
            long[] result = new long[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = reader.ReadInt64();
            }

            return result;
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
            Code.NotNull(() => reader);

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
            Code.NotNull(() => reader);

            bool flag = reader.ReadBoolean();
            return flag
                ? (double?)reader.ReadDouble()
                : null;
        }

        /// <summary>
        /// Read nullable decimal.
        /// </summary>
        [CanBeNull]
        public static decimal? ReadNullableDecimal
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(() => reader);

            bool flag = reader.ReadBoolean();
            return flag
                ? (decimal?)reader.ReadDecimal()
                : null;
        }

        /// <summary>
        /// Read nullable 16-bit integer.
        /// </summary>
        [CanBeNull]
        public static short? ReadNullableInt16
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(() => reader);

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
            Code.NotNull(() => reader);

            bool flag = reader.ReadBoolean();
            return flag
                ? (int?)reader.ReadInt32()
                : null;
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
            Code.NotNull(() => reader);

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
            Code.NotNull(() => reader);

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
        /// Read array of strings.
        /// </summary>
        [NotNull]
        public static string[] ReadStringArray
            (
                [NotNull] this BinaryReader reader
            )
        {
            Code.NotNull(() => reader);

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
