﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Ascii85.cs -- Base85 implementation
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives


using System;
using System.IO;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM
{
    //
    // Borrowed from https://github.com/LogosBible/Logos.Utility
    //

    /// <summary>
    /// Converts between binary data and an Ascii85-encoded string.
    /// </summary>
    /// <remarks>See <a href="http://en.wikipedia.org/wiki/Ascii85">Ascii85 at Wikipedia</a>.</remarks>
    public static class Ascii85
    {
        /// <summary>
        /// Encodes the specified byte array in Ascii85.
        /// </summary>
        /// <param name="bytes">The bytes to encode.</param>
        /// <returns>An Ascii85-encoded string representing the input byte array.</returns>
        [NotNull]
        public static string Encode
            (
                [NotNull] byte[] bytes
            )
        {
            Code.NotNull(bytes, "bytes");

            if (bytes.Length == 0)
            {
                return string.Empty;
            }

            // preallocate a StringBuilder with enough room to store the encoded bytes
            StringBuilder sb = new StringBuilder(bytes.Length * 5 / 4);

            // walk the bytes
            int count = 0;
            uint value = 0;
            foreach (byte b in bytes)
            {
                // build a 32-bit value from the bytes
                value |= ((uint)b) << (24 - (count * 8));
                count++;

                // every 32 bits, convert the previous 4 bytes into 5 Ascii85 characters
                if (count == 4)
                {
                    if (value == 0)
                    {
                        sb.Append('z');
                    }
                    else
                    {
                        EncodeValue(sb, value, 0);
                    }
                    count = 0;
                    value = 0;
                }
            }

            // encode any remaining bytes (that weren't a multiple of 4)
            if (count > 0)
            {
                EncodeValue(sb, value, 4 - count);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Decodes the specified Ascii85 string into the corresponding byte array.
        /// </summary>
        /// <param name="encoded">The Ascii85 string.</param>
        /// <returns>The decoded byte array.</returns>
        [NotNull]
        public static byte[] Decode
            (
                [NotNull] string encoded
            )
        {
            Code.NotNull(encoded, "encoded");

            if (encoded.Length == 0)
            {
                return EmptyArray<byte>.Value;
            }

            // preallocate a memory stream with enough capacity to hold the decoded data
            using (MemoryStream stream = new MemoryStream(encoded.Length * 4 / 5))
            {
                // walk the input string
                int count = 0;
                uint value = 0;
                foreach (char ch in encoded)
                {
                    if (ch == 'z' && count == 0)
                    {
                        // handle "z" block specially
                        DecodeValue(stream, value, 0);
                    }
                    else if (ch < FirstCharacter || ch > LastCharacter)
                    {
                        throw new FormatException
                            (
                                string.Format
                                (
                                    "Invalid character '{0}' in Ascii85 block",
                                    ch
                                )
                            );
                    }
                    else
                    {
                        // build a 32-bit value from the input characters
                        try
                        {
                            checked { value += (uint)(PowersOf85[count] * (ch - FirstCharacter)); }
                        }
                        catch (OverflowException ex)
                        {
                            throw new FormatException("The current group of characters decodes to a value greater than UInt32.MaxValue.", ex);
                        }

                        count++;

                        // every five characters, convert the characters into the equivalent byte array
                        if (count == 5)
                        {
                            DecodeValue(stream, value, 0);
                            count = 0;
                            value = 0;
                        }
                    }
                }

                if (count == 1)
                {
                    throw new FormatException("The final Ascii85 block must contain more than one character.");
                }

                if (count > 1)
                {
                    // decode any remaining characters
                    for (int padding = count; padding < 5; padding++)
                    {
                        try
                        {
                            checked { value += 84 * PowersOf85[padding]; }
                        }
                        catch (OverflowException ex)
                        {
                            throw new FormatException("The current group of characters decodes to a value greater than UInt32.MaxValue.", ex);
                        }
                    }
                    DecodeValue(stream, value, 5 - count);
                }

                return stream.ToArray();
            }
        }

        // Writes the Ascii85 characters for a 32-bit value to a StringBuilder.
        private static void EncodeValue
            (
                StringBuilder sb,
                uint value,
                int paddingBytes
            )
        {
            char[] encoded = new char[5];

            for (int index = 4; index >= 0; index--)
            {
                encoded[index] = (char)((value % 85) + FirstCharacter);
                value /= 85;
            }

            if (paddingBytes != 0)
            {
                Array.Resize(ref encoded, 5 - paddingBytes);
            }

            sb.Append(encoded);
        }

        // Writes the bytes of a 32-bit value to a stream.
        private static void DecodeValue
            (
                Stream stream,
                uint value,
                int paddingChars
            )
        {
            stream.WriteByte((byte)(value >> 24));
            if (paddingChars == 3)
            {
                return;
            }

            stream.WriteByte((byte)((value >> 16) & 0xFF));
            if (paddingChars == 2)
            {
                return;
            }

            stream.WriteByte((byte)((value >> 8) & 0xFF));
            if (paddingChars == 1)
            {
                return;
            }

            stream.WriteByte((byte)(value & 0xFF));
        }

        // the first and last characters used in the Ascii85 encoding character set
        private const char FirstCharacter = '!';
        private const char LastCharacter = 'u';

        static readonly uint[] PowersOf85 =
        {
            unchecked (85u * 85u * 85u * 85u),
            unchecked (85u * 85u * 85u),
            unchecked (85u * 85u),
            85u,
            1
        };
    }
}
