// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CompressionUtility.cs -- useful routines that simplifies data compression 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !SILVERLIGHT

#region Using directives

using System.IO;
using System.IO.Compression;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.IO
{
    /// <summary>
    /// Useful routines that simplifies data compression/decompression.
    /// </summary>
    public static class CompressionUtility
    {
        #region Public methods

        /// <summary>
        /// Compress the data.
        /// </summary>
        [NotNull]
        public static byte[] Compress
            (
                [NotNull] byte[] data
            )
        {
            Code.NotNull(data, "data");

            MemoryStream memory = new MemoryStream();
            using (DeflateStream compressor = new DeflateStream
                (
                    memory,
                    CompressionMode.Compress
                ))
            {
                compressor.Write(data, 0, data.Length);
            }

            return memory.ToArray();
        }

        /// <summary>
        /// Decompress the data.
        /// </summary>
        [NotNull]
        public static byte[] Decompress
            (
                [NotNull] byte[] data
            )
        {
            Code.NotNull(data, "data");

            MemoryStream memory = new MemoryStream(data);
            using (DeflateStream decompresser = new DeflateStream
                (
                    memory,
                    CompressionMode.Decompress
                ))
            {
                MemoryStream result = new MemoryStream();
                StreamUtility.Copy(decompresser, result);
                decompresser.Dispose();

                return result.ToArray();
            }
        }

        #endregion
    }
}

#endif

