// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StreamPacker.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

/*
 * Layout of packed UInt32 value
 *
 * bit number
 * -----------------------------------------------------------
 * | 31  30         |                                2  1  0 |
 * | length minus 1 |                                        |
 * -----------------------------------------------------------
 *
 * Layout of packed UInt64 value
 *
 * bit number
 * -----------------------------------------------------------
 * | 63  62  61     |                                2  1  0 |
 * | length minus 1 |                                        |
 * -----------------------------------------------------------
 *
 */

namespace AM.IO
{
    /// <summary>
    /// Упаковщик: пытается записать данные в поток, 
    /// используя по возможности меньше байт ( но до
    /// архиватора не дотягивает ).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class StreamPacker
    {
        #region Public methods

        /// <summary>
        /// Выводит в поток 4-байтовое целое.
        /// </summary>
        /// <param name="stream">Поток. Может равняться null.</param>
        /// <param name="val">Целое.</param>
        /// <returns>Количество байт, необходимых для вывода.</returns>
        [CLSCompliant(false)]
        public static int PackUInt32
            (
                [NotNull] Stream stream,
                uint val
            )
        {
            Code.NotNull(stream, "stream");

            byte[] bytes = BitConverter.GetBytes(val);
            byte c = bytes[3];
            bytes[3] = bytes[0];
            bytes[0] = c;
            c = bytes[2];
            bytes[2] = bytes[1];
            bytes[1] = c;

            int len;
            unchecked
            {
                if (val <= 63) /* 0x3F */
                {
                    len = 1;
                }
                else if (val <= 16383) /* 0x3FFF */
                {
                    len = 2;
                    bytes[2] |= 0x40;
                }
                else if (val <= 4193303) /* 0x3FFFFF */
                {
                    len = 3;
                    bytes[1] |= 0x80;
                }
                else if (val <= 1073741823) /* 0x3FFFFFFF */
                {
                    len = 4;
                    bytes[0] |= 0xC0;
                }
                else
                {
                    Log.Error
                        (
                            "StreamPacker::PackUInt32: "
                            + "value too big="
                            + val
                        );

                    throw new ArgumentException("too big", "val");
                }
            }

            stream.Write(bytes, 4 - len, len);

            return len;
        }

        /// <summary>
        /// Считывает 4-байтовое целое из потока.
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <returns>Считанное значение.</returns>
        [CLSCompliant(false)]
        public static uint UnpackUInt32
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            uint res;
            int fb = stream.ReadByte();

            if (fb < 0)
            {
                Log.Error
                    (
                        "StreamPacker::UnpackUInt32: "
                        + "unexpected end of stream"
                    );

                throw new IOException("end of stream");
            }
            unchecked
            {
                res = (uint)(fb & 0x3F);
                for (int len = fb >> 6; len > 0; len--)
                {
                    fb = stream.ReadByte();
                    if (fb < 0)
                    {
                        Log.Error
                            (
                                "StreamPacker::UnpackUInt32: "
                                + "unexpected end of stream"
                            );

                        throw new IOException("end of stream");
                    }
                    res = (uint)((res << 8) + fb);
                }
            }

            return res;
        }

        /// <summary>
        /// Выводит в поток 8-байтовое целое.
        /// </summary>
        /// <param name="stream">Поток. Может равняться null.</param>
        /// <param name="val">Целое.</param>
        /// <returns>Количество байт, необходимых для вывода.</returns>
        [CLSCompliant(false)]
        public static int PackUInt64
            (
                [NotNull] Stream stream,
                ulong val
            )
        {
            Code.NotNull(stream, "stream");

            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            int length;

            unchecked
            {
                if (val <= 0x1F)
                {
                    length = 1;
                }
                else if (val <= 0x1FFF)
                {
                    length = 2;
                }
                else if (val <= 0x1FFFFF)
                {
                    length = 3;
                }
                else if (val <= 0x1FFFFFFFUL)
                {
                    length = 4;
                }
                else if (val <= 0x1FFFFFFFFFUL)
                {
                    length = 5;
                }
                else if (val <= 0x1FFFFFFFFFFFUL)
                {
                    length = 6;
                }
                else if (val <= 0x1FFFFFFFFFFFFFUL)
                {
                    length = 7;
                }
                else if (val <= 0x1FFFFFFFFFFFFFFFUL)
                {
                    length = 8;
                }
                else
                {
                    Log.Error
                        (
                            "StreamPacker::PackUInt64: "
                            + "value too big="
                            + val
                        );

                    throw new ArgumentException("too big", "val");
                }
                bytes[8 - length] |= (byte)((length - 1) << 5);
            }

            stream.Write(bytes, 8 - length, length);

            return length;
        }

        /// <summary>
        /// Считывает 8-байтовое целое из потока.
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <returns>Считанное целое.</returns>
        [CLSCompliant(false)]
        public static ulong UnpackUInt64
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            ulong res;
            int fb = stream.ReadByte();

            if (fb < 0)
            {
                Log.Error
                    (
                        "StreamPacker::UnpackUInt64: "
                        + "unexpected end of stream"
                    );

                throw new IOException("end of stream");
            }
            unchecked
            {
                res = (ulong)(fb & 0x1F);
                for (int len = fb >> 5; len > 0; len--)
                {
                    fb = stream.ReadByte();
                    if (fb < 0)
                    {
                        Log.Error
                            (
                                "StreamPacker::UnpackUInt64: "
                                + "unexpected end of stream"
                            );

                        throw new IOException("end of stream");
                    }
                    res = (res << 8) + (ulong)fb;
                }
            }

            return res;
        }

        /// <summary>
        /// Записывает массив байт в поток.
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <param name="bytes">Массив.</param>
        /// <returns>Количество байт, необходимых для вывода.</returns>
        public static int PackBytes
            (
                [NotNull] Stream stream,
                [NotNull] byte[] bytes
            )
        {
            Code.NotNull(stream, "stream");
            Code.NotNull(bytes, "bytes");

            int len = unchecked
                (
                    bytes.Length
                    + PackUInt32(stream, (uint)bytes.Length)
                );

            stream.Write(bytes, 0, bytes.Length);

            return len;
        }

        /// <summary>
        /// Считывает массив байт из потока.
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <returns>Считанный массив.</returns>
        [NotNull]
        public static byte[] UnpackBytes
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            int len = unchecked((int)UnpackUInt32(stream));
            if (len == 0)
            {
                return new byte[0];
            }

            byte[] bytes = new byte[len];
            if (stream.Read(bytes, 0, len) != len)
            {
                Log.Error
                    (
                        "StreamPacker::UnpackBytes: "
                        + "unexpected end of stream"
                    );

                throw new IOException("end of stream");
            }

            return bytes;
        }

        /// <summary>
        /// Записывает строку в поток в указанной кодировке.
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <param name="encoding">Кодировка.</param>
        /// <param name="text">Строка.</param>
        /// <returns>Количество байт, необходимых для вывода.</returns>
        public static int PackString
            (
                [NotNull] Stream stream,
                [CanBeNull] Encoding encoding,
                [NotNull] string text
            )
        {
            Code.NotNull(stream, "stream");
            Code.NotNull(text, "text");

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = encoding.GetBytes(text);

            return PackBytes(stream, bytes);
        }

        /// <summary>
        /// Записывает строку в поток в UTF8.
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <param name="value">Строка.</param>
        /// <returns>Количество байт, необходимых для вывода.</returns>
        public static int PackString
            (
                [NotNull] Stream stream,
                [NotNull] string value
            )
        {
            return PackString
                (
                    stream,
                    null,
                    value
                );
        }

        /// <summary>
        /// Считывает строку из потока в заданной кодировке.
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <param name="encoding">Кодировка.</param>
        /// <returns>Считанная строка.</returns>
        [CanBeNull]
        public static string UnpackString
            (
                [NotNull] Stream stream,
                [CanBeNull] Encoding encoding
            )
        {
            Code.NotNull(stream, "stream");

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = UnpackBytes(stream);
            string result = EncodingUtility.GetString
                (
                    encoding,
                    bytes
                );

            return result;
        }

        /// <summary>
        /// Считывает строку из потока в UTF8.
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <returns>Считанная строка.</returns>
        [CanBeNull]
        public static string UnpackString
            (
                [NotNull] Stream stream
            )
        {
            return UnpackString
                (
                    stream,
                    null
                );
        }

        #endregion
    }
}
