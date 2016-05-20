/* SerializationUtility.cs -- сериализация
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.IO.Compression;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Runtime
{
    /// <summary>
    /// Хелперы, связанные с сериализацией и десериализацией.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class SerializationUtility
    {
        #region Private members
        #endregion

        #region Public methods

        /// <summary>
        /// Считывание массива из потока.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        public static T[] RestoreArray<T>
            (
                [NotNull] this BinaryReader reader
            )
            where T: IHandmadeSerializable, new()
        {
            bool isNull = !reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }

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
        /// Считывание массива из файла.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        public static T[] RestoreFromFile<T>
            (
                [NotNull] string fileName
            )
            where T: IHandmadeSerializable, new()
        {
            using (Stream stream = File.OpenRead(fileName))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                return reader.RestoreArray<T>();
            }
        }

        /// <summary>
        /// Считывание массива из файла.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        public static T[] RestoreFromZipFile<T>
            (
                [NotNull] string fileName
            )
            where T: IHandmadeSerializable, new ()
        {
            using (Stream stream = File.OpenRead(fileName))
            using (DeflateStream deflate = new DeflateStream
                (
                    stream,
                    CompressionMode.Decompress
                ))
            using (BinaryReader reader = new BinaryReader(deflate))
            {
                return reader.RestoreArray<T>();
            }
        }

        /// <summary>
        /// Считывание массива из памяти.
        /// </summary>
        public static T[] RestoreFromMemory<T>
            (
                [NotNull] this byte[] array
            )
            where T: IHandmadeSerializable, new ()
        {
            using (Stream stream = new MemoryStream(array))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                return reader.RestoreArray<T>();
            }
        }

        /// <summary>
        /// Считывание массива из памяти.
        /// </summary>
        public static T[] RestoreFromZipMemory<T>
            (
                [NotNull] this byte[] array,
                [NotNull] Func<BinaryReader, T> func
            )
            where T: IHandmadeSerializable, new ()
        {
            using (Stream stream = new MemoryStream(array))
            using (DeflateStream deflate = new DeflateStream
                (
                    stream,
                    CompressionMode.Decompress
                ))
            using (BinaryReader reader = new BinaryReader(deflate))
            {
                return reader.RestoreArray<T>();
            }
        }

        /// <summary>
        /// Считывание из потока обнуляемого объекта.
        /// </summary>
        [CanBeNull]
        public static T RestoreNullable<T>
            (
                [NotNull] this BinaryReader reader
            )
            where T : class, IHandmadeSerializable, new()
        {
            Code.NotNull(() => reader);

            bool isNull = !reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }

            T result = new T();
            result.RestoreFromStream(reader);

            return result;
        }

        /// <summary>
        /// Сохранение в поток массива элементов.
        /// </summary>
        public static void SaveToStream<T>
            (
                [CanBeNull][ItemNotNull] this T[] array,
                [NotNull] BinaryWriter writer
            )
            where T : IHandmadeSerializable
        {
            Code.NotNull(() => writer);

            if (ReferenceEquals(array, null))
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                writer.WritePackedInt32(array.Length);
                foreach (T item in array)
                {
                    item.SaveToStream(writer);
                }
            }
        }

        /// <summary>
        /// Сохранение в файл массива объектов,
        /// умеющих сериализоваться вручную.
        /// </summary>
        public static void SaveToFile<T>
            (
                [NotNull] [ItemNotNull] this T[] array,
                [NotNull] string fileName
            )
            where T : IHandmadeSerializable
        {
            Code.NotNull(() => array);
            Code.NotNullNorEmpty(() => fileName);

            using (Stream stream = File.Create(fileName))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                array.SaveToStream(writer);
            }
        }

        /// <summary>
        /// Сохранение массива объектов.
        /// </summary>
        public static byte[] SaveToMemory<T>
            (
                [NotNull][ItemNotNull] this T[] array
            )
            where T : IHandmadeSerializable
        {
            Code.NotNull(() => array);

            using (MemoryStream stream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                array.SaveToStream(writer);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Сохранение в файл массива объектов
        /// с одновременной упаковкой.
        /// </summary>
        public static void SaveToZipFile<T>
            (
                [NotNull] [ItemNotNull] this T[] array,
                [NotNull] string fileName
            )
            where T : IHandmadeSerializable
        {
            Code.NotNull(() => array);
            Code.NotNullNorEmpty(() => fileName);

            using (Stream stream = File.Create(fileName))
            using (DeflateStream deflate = new DeflateStream
                (
                    stream,
                    CompressionMode.Compress
                ))
            using (BinaryWriter writer = new BinaryWriter(deflate))
            {
                array.SaveToStream(writer);
            }
        }

        /// <summary>
        /// Сохранение массива объектов.
        /// </summary>
        public static byte[] SaveToZipMemory<T>
            (
                [NotNull][ItemNotNull] this T[] array
            )
            where T : IHandmadeSerializable
        {
            Code.NotNull(() => array);

            using (MemoryStream stream = new MemoryStream())
            using (DeflateStream deflate = new DeflateStream
                (
                    stream,
                    CompressionMode.Compress
                ))
            using (BinaryWriter writer = new BinaryWriter(deflate))
            {
                array.SaveToStream(writer);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Сохранение в поток обнуляемого объекта.
        /// </summary>
        public static BinaryWriter WriteNullable<T>
            (
                [NotNull] this BinaryWriter writer,
                [CanBeNull] T obj
            )
            where T : class, IHandmadeSerializable
        {
            Code.NotNull(() => writer);

            if (ReferenceEquals(obj, null))
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                obj.SaveToStream(writer);
            }

            return writer;
        }

        #endregion
    }
}
