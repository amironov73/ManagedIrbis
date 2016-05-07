using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AM.Runtime
{
    static class SerializationUtility
    {
        #region Private members
        #endregion

        #region Public methods

        /// <summary>
        /// Deserialize object from byte array.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="bytes">Array of bytes.</param>
        /// <returns>Deserialized object.</returns>
        [CLSCompliant(false)]
        public static T BinDeserialize<T>(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes, false);
            return BinDeserialize<T>(stream);
        }

        /// <summary>
        /// Deserialize object from disk file.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="fileName">File name.</param>
        /// <returns>Deseriazlized object.</returns>
        [CLSCompliant(false)]
        public static T BinDeserialize<T>(string fileName)
        {
            using (Stream stream = File.OpenRead(fileName))
            {
                return BinDeserialize<T>(stream);
            }
        }

        /// <summary>
        /// Deserialize object from stream.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="stream">Stream.</param>
        /// <returns>Deserialized object.</returns>
        [CLSCompliant(false)]
        public static T BinDeserialize<T>(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (T)formatter.Deserialize(stream);
        }

        /// <summary>
        /// Serialize object to stream.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="obj">Object.</param>
        public static void BinSerialize(Stream stream, object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
        }

        /// <summary>
        /// Serialize object to disk file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="obj">Object.</param>
        public static void BinSerialize(string fileName, object obj)
        {
            using (Stream stream = File.Create(fileName))
            {
                BinSerialize(stream, obj);
            }
        }

        /// <summary>
        /// Serialize object to byte array.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns>Array of bytes.</returns>
        public static byte[] BinSerialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinSerialize(stream, obj);
            return stream.ToArray();
        }

        #endregion
    }
}
