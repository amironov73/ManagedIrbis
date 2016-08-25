/* BinaryWriterUtility.cs --
 * Ars Magna project, http://arsmagna.ru
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
    public static class BinaryWriterUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Write the <see cref="NonNullCollection{T}"/>
        /// to the stream.
        /// </summary>
        public static BinaryWriter Write<T>
            (
                [NotNull] this BinaryWriter writer,
                [NotNull] NonNullCollection<T> collection
            )
            where T: class, IHandmadeSerializable, new()
        {
            Code.NotNull(writer, "writer");

            writer.WriteArray(collection.ToArray());

            return writer;
        }

        /// <summary>
        /// Write nullable 8-bit integer.
        /// </summary>
        [NotNull]
        public static BinaryWriter Write
            (
                [NotNull] this BinaryWriter writer,
                [CanBeNull] byte? value
            )
        {
            Code.NotNull(writer, "writer");

            if (value != null)
            {
                writer.Write(true);
                writer.Write(value.Value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write nullable 16-bit integer.
        /// </summary>
        [NotNull]
        public static BinaryWriter Write
            (
                [NotNull] this BinaryWriter writer,
                [CanBeNull] short? value
            )
        {
            Code.NotNull(writer, "writer");

            if (value != null)
            {
                writer.Write(true);
                writer.Write(value.Value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write nullable 32-bit integer.
        /// </summary>
        [NotNull]
        public static BinaryWriter Write
            (
                [NotNull] this BinaryWriter writer,
                [CanBeNull] int? value
            )
        {
            Code.NotNull(writer, "writer");

            if (value != null)
            {
                writer.Write(true);
                writer.Write(value.Value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write nullable 64-bit integer.
        /// </summary>
        [NotNull]
        public static BinaryWriter Write
            (
                [NotNull] this BinaryWriter writer,
                [CanBeNull] long? value
            )
        {
            Code.NotNull(writer, "writer");

            if (value != null)
            {
                writer.Write(true);
                writer.Write(value.Value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write nullable decimal number.
        /// </summary>
        [NotNull]
        public static BinaryWriter Write
            (
                [NotNull] this BinaryWriter writer,
                [CanBeNull] decimal? value
            )
        {
            Code.NotNull(writer, "writer");

            if (value != null)
            {
                writer.Write(true);
                writer.Write(value.Value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write <see cref="DateTime"/>.
        /// </summary>
        [NotNull]
        public static BinaryWriter Write
            (
                [NotNull] this BinaryWriter writer,
                DateTime value
            )
        {
            long ticks = value.ToBinary();
            writer.Write(ticks);

            return writer;
        }

        /// <summary>
        /// Write nullable DateTime.
        /// </summary>
        [NotNull]
        public static BinaryWriter Write
            (
                [NotNull] this BinaryWriter writer,
                [CanBeNull] DateTime? value
            )
        {
            Code.NotNull(writer, "writer");

            if (value != null)
            {
                writer.Write(true);
                writer.Write(value.Value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write nullable double precision number.
        /// </summary>
        [NotNull]
        public static BinaryWriter Write
            (
                [NotNull] this BinaryWriter writer,
                [CanBeNull] double? value
            )
        {
            Code.NotNull(writer, "writer");

            if (value != null)
            {
                writer.Write(true);
                writer.Write(value.Value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

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
            Code.NotNull(writer, "writer");
            Code.NotNull(array, "array");

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
            Code.NotNull(writer, "writer");
            Code.NotNull(array, "array");

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
            Code.NotNull(writer, "writer");
            Code.NotNull(array, "writer");

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
            Code.NotNull(writer, "writer");
            Code.NotNull(array, "array");

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
            Code.NotNull(writer, "writer");
            Code.NotNull(array, "array");

            writer.WritePackedInt32(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                writer.Write(array[i]);
            }

            return writer;
        }

        /// <summary>
        /// Write the array.
        /// </summary>
        [NotNull]
        public static BinaryWriter WriteArray<T>
            (
                [NotNull] this BinaryWriter writer,
                [NotNull][ItemNotNull] T[] array
            )
            where T: IHandmadeSerializable, new ()
        {
            Code.NotNull(writer, "writer");
            Code.NotNull(array, "array");

            writer.WritePackedInt32(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                T item = array[i];
                item.SaveToStream(writer);
            }

            return writer;
        }

        /// <summary>
        /// Writes the collection to the stream.
        /// </summary>
        [NotNull]
        public static BinaryWriter WriteCollection<T>
            (
                [NotNull] this BinaryWriter writer,
                [NotNull][ItemNotNull] NonNullCollection<T> collection
            )
            where T: class, IHandmadeSerializable, new()
        {
            Code.NotNull(writer, "writer");
            Code.NotNull(collection, "collection");

            writer.WritePackedInt32(collection.Count);
            for (int i = 0; i < collection.Count; i++)
            {
                T item = collection[i];
                item.SaveToStream(writer);
            }

            return writer;
        }

        /// <summary>
        /// Write the list to the stream.
        /// </summary>
        [NotNull]
        public static BinaryWriter WriteList<T>
            (
                [NotNull] this BinaryWriter writer,
                [NotNull][ItemNotNull] List<T> list
            )
            where T: IHandmadeSerializable, new ()
        {
            Code.NotNull(writer, "writer");
            Code.NotNull(list, "list");

            writer.WritePackedInt32(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                T item = list[i];
                item.SaveToStream(writer);
            }

            return writer;
        }

        /// <summary>
        /// Write nullable string.
        /// </summary>
        [NotNull]
        public static BinaryWriter WriteNullable
            (
                [NotNull] this BinaryWriter writer,
                [CanBeNull] string value
            )
        {
            Code.NotNull(writer, "writer");

            if (value != null)
            {
                writer.Write(true);
                writer.Write(value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write array of 32-bit integers.
        /// </summary>
        [NotNull]
        public static BinaryWriter WriteNullableArray
            (
                [NotNull] this BinaryWriter writer,
                [CanBeNull] int[] array
            )
        {
            Code.NotNull(writer, "writer");

            if (ReferenceEquals(array, null))
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                writer.WritePackedInt32(array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    writer.Write(array[i]);
                }
            }

            return writer;
        }

        /// <summary>
        /// Write 32-bit integer in packed format.
        /// </summary>
        /// <remarks>Borrowed from
        /// http://referencesource.microsoft.com/
        /// </remarks>
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

        /// <summary>
        /// Write 64-bit integer in packed format.
        /// </summary>
        /// <remarks>Inspired by
        /// http://referencesource.microsoft.com/
        /// </remarks>
        public static BinaryWriter WritePackedInt64
            (
                [NotNull] this BinaryWriter writer,
                long value
            )
        {
            ulong v = (ulong)value;
            while (v >= 0x80)
            {
                writer.Write((byte)(v | 0x80));
                v >>= 7;
            }
            writer.Write((byte)v);

            return writer;
        }

        #endregion
    }
}
