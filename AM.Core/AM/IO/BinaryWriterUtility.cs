/* BinaryWriterUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

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
    public static class BinaryWriterUtility
    {
        #region Private members

        //private static Encoding _GetUtf8()
        //{
        //    return new UTF8Encoding(false,true);
        //}

        #endregion

        #region Public methods

        /// <summary>
        /// Write array of bytes.
        /// </summary>
        [NotNull]
        public static BinaryWriter WriteArray
            (
                [NotNull] this BinaryWriter writer,
                [NotNull] byte[] array
            )
        {
            Code.NotNull(() => writer);
            Code.NotNull(() => array);

            writer.WritePackedInt32(array.Length);
            writer.Write(array);

            return writer;
        }

        /// <summary>
        /// Write array of 16-bit integers.
        /// </summary>
        [NotNull]
        public static BinaryWriter WriteArray
            (
                [NotNull] this BinaryWriter writer,
                [NotNull] short[] array
            )
        {
            Code.NotNull(() => writer);
            Code.NotNull(() => array);

            writer.WritePackedInt32(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                writer.Write(array[i]);
            }

            return writer;
        }

        /// <summary>
        /// Write array of 32-bit integers.
        /// </summary>
        [NotNull]
        public static BinaryWriter WriteArray
            (
                [NotNull] this BinaryWriter writer,
                [NotNull] int[] array
            )
        {
            Code.NotNull(() => writer);
            Code.NotNull(() => array);

            writer.WritePackedInt32(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                writer.Write(array[i]);
            }

            return writer;
        }

        /// <summary>
        /// Write array of 64-bit integers.
        /// </summary>
        [NotNull]
        public static BinaryWriter WriteArray
            (
                [NotNull] this BinaryWriter writer,
                [NotNull] long[] array
            )
        {
            Code.NotNull(() => writer);
            Code.NotNull(() => array);

            writer.WritePackedInt32(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                writer.Write(array[i]);
            }

            return writer;
        }

        /// <summary>
        /// Write array of strings.
        /// </summary>
        [NotNull]
        public static BinaryWriter WriteArray
            (
                [NotNull] this BinaryWriter writer,
                [NotNull] string[] array
            )
        {
            Code.NotNull(() => writer);
            Code.NotNull(() => array);

            writer.WritePackedInt32(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                writer.Write(array[i]);
            }

            return writer;
        }

        /// <summary>
        /// Write 32-bit integer in packed format.
        /// </summary>
        public static BinaryWriter WritePackedInt32
            (
                [NotNull] this BinaryWriter writer,
                int value
            )
        {
            uint v = (uint) value;
            while (v >= 0x80)
            {
                writer.Write((byte) (v | 0x80));
                v >>= 7;
            }
            writer.Write((byte)v);

            return writer;
        }

        #endregion
    }
}
