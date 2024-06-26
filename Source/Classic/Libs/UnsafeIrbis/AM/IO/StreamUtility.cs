﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StreamUtility.cs -- stream manipulation routines.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

using UnsafeAM.Logging;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.IO
{
    /// <summary>
    /// Stream manipulation routines.
    /// </summary>
    [PublicAPI]
    public static class StreamUtility
    {
        #region Public methods

        /// <summary>
        /// Appends one's stream contents (starting from current position)
        /// to another stream.
        /// </summary>
        public static void AppendTo
            (
                [NotNull] Stream sourceStream,
                [NotNull] Stream destinationStream,
                int chunkSize
            )
        {
            Code.NotNull(sourceStream, nameof(sourceStream));
            Code.NotNull(destinationStream, nameof(destinationStream));

            if (chunkSize <= 0)
            {
                chunkSize = 4 * 1024;
            }

            byte[] buffer = new byte[chunkSize];
            destinationStream.Seek(0, SeekOrigin.End);
            while (true)
            {
                int readed = sourceStream.Read(buffer, 0, chunkSize);
                if (readed <= 0)
                {
                    break;
                }

                destinationStream.Write(buffer, 0, readed);
            }
        }

        /// <summary>
        /// Compares two <see cref="Stream"/>'s from current position.
        /// </summary>
        public static int CompareTo
            (
                [NotNull] Stream firstStream,
                [NotNull] Stream secondStream
            )
        {
            Code.NotNull(firstStream, nameof(firstStream));
            Code.NotNull(secondStream, nameof(secondStream));

            const int bufferSize = 1024;
            byte[] firstBuffer = new byte[bufferSize];
            byte[] secondBuffer = new byte[bufferSize];
            while (true)
            {
                int firstReaded = firstStream.Read(firstBuffer, 0, bufferSize);
                int secondReaded = secondStream.Read(secondBuffer, 0, bufferSize);
                int difference = firstReaded - secondReaded;
                if (difference != 0)
                {
                    return difference;
                }

                if (firstReaded == 0)
                {
                    return 0;
                }

                for (int i = 0; i < firstReaded; i++)
                {
                    difference = firstBuffer[i] - secondBuffer[i];
                    if (difference != 0)
                    {
                        return difference;
                    }
                }
            }
        }

        /// <summary>
        /// Read as up to <paramref name="maximum"/> bytes
        /// from the given stream.
        /// </summary>
        public static byte[] ReadAsMuchAsPossible
            (
                [NotNull] Stream stream,
                int maximum
            )
        {
            Code.NotNull(stream, nameof(stream));

            if (maximum < 0)
            {
                Log.Error
                    (
                        "StreamUtility::ReadAsMuchAsPossible: "
                        + "maximum="
                        + maximum
                    );

                throw new ArgumentOutOfRangeException(nameof(maximum));
            }

            byte[] result;
            byte[] temporary = ArrayPool<byte>.Shared.Rent(maximum);
            try
            {
                int readed = stream.Read(temporary, 0, maximum);
                if (readed <= 0)
                {
                    return new byte[0];
                }

                result = new byte[readed];
                Array.Copy(temporary, result, readed);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(temporary);
            }

            return result;
        }

        /// <summary>
        /// Read <see cref="bool"/> value from the <see cref="Stream"/>.
        /// </summary>
        public static bool ReadBoolean
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, nameof(stream));

            int value = stream.ReadByte();
            switch (value)
            {
                case 0:
                    return false;

                case -1:
                    throw new IOException();

                default:
                    return true;
            }
        }

        /// <summary>
        /// Read some bytes from the stream.
        /// </summary>
        [CanBeNull]
        public static byte[] ReadBytes
            (
                [NotNull] Stream stream,
                int count
            )
        {
            Code.NotNull(stream, nameof(stream));
            Code.Positive(count, nameof(count));

            byte[] result = new byte[count];
            int read = stream.Read(result, 0, count);
            if (read <= 0)
            {
                return null;
            }

            if (read != count)
            {
                Array.Resize(ref result, read);
            }

            return result;
        }

        /// <summary>
        /// Reads <see cref="short"/> value from the <see cref="Stream"/>.
        /// </summary>
        public static unsafe short ReadInt16
            (
                [NotNull] Stream stream
            )
        {
            ShortStruct value = new ShortStruct();
            ReadExact(stream, value.Array, 2);

            return value.SignedValue;
        }

        /// <summary>
        /// Reads <see cref="ushort"/> value from the <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static unsafe ushort ReadUInt16
            (
                [NotNull] Stream stream
            )
        {
            ShortStruct value = new ShortStruct();
            ReadExact(stream, value.Array, 2);

            return value.UnsignedValue;
        }

        /// <summary>
        /// Reads <see cref="int"/> value from the <see cref="Stream"/>.
        /// </summary>
        public static unsafe int ReadInt32
            (
                [NotNull] Stream stream
            )
        {
            MiddleStruct value = new MiddleStruct();
            ReadExact(stream, value.Array, 4);

            return value.SignedValue;
        }

        /// <summary>
        /// Reads <see cref="uint"/> value from the <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static unsafe uint ReadUInt32
            (
                [NotNull] Stream stream
            )
        {
            MiddleStruct value = new MiddleStruct();
            ReadExact(stream, value.Array, 4);

            return value.UnsignedValue;
        }

        /// <summary>
        /// Reads <see cref="long"/> value from the <see cref="Stream"/>.
        /// </summary>
        public static unsafe long ReadInt64
            (
                [NotNull] Stream stream
            )
        {
            LongStruct value = new LongStruct();
            ReadExact(stream, value.Array, 8);

            return value.SignedValue;
        }

        /// <summary>
        /// Reads <see cref="ulong"/> value from the <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static unsafe ulong ReadUInt64
            (
                [NotNull] Stream stream
            )
        {
            LongStruct value = new LongStruct();
            ReadExact(stream, value.Array, 8);

            return value.UnsignedValue;
        }

        /// <summary>
        /// Reads <see cref="float"/> value from the <see cref="Stream"/>.
        /// </summary>
        public static unsafe float ReadSingle
            (
                [NotNull] Stream stream
            )
        {
            MiddleStruct value = new MiddleStruct();
            ReadExact(stream, value.Array, 4);

            return value.FloatValue;
        }

        /// <summary>
        /// Reads <see cref="double"/> value from the <see cref="Stream"/>.
        /// </summary>
        public static unsafe double ReadDouble
            (
                [NotNull] Stream stream
            )
        {
            LongStruct value = new LongStruct();
            ReadExact(stream, value.Array, 8);

            return value.DoubleValue;
        }

        /// <summary>
        /// Reads <see cref="string"/> value from the <see cref="Stream"/>
        /// using specified <see cref="Encoding"/>.
        /// </summary>
        /// <seealso cref="Write(Stream,string,Encoding)"/>
        public static unsafe string ReadString
            (
                [NotNull] Stream stream,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(stream, nameof(stream));
            Code.NotNull(encoding, nameof(encoding));

            int byteCount = ReadInt32(stream);
            if (byteCount < 256)
            {
                byte* buffer = stackalloc byte[byteCount];
                ReadExact(stream, buffer, byteCount);
                int charCount = encoding.GetCharCount(buffer, byteCount);
                char* chars = stackalloc char[charCount];
                encoding.GetChars(buffer, byteCount, chars, charCount);
                string result = new string(chars, 0, charCount);

                return result;
            }
            else
            {
                ArrayPool<byte> pool = ArrayPool<byte>.Shared;
                byte[] buffer = pool.Rent(byteCount);
                try
                {
                    ReadExact(stream, buffer, byteCount);
                    string result = encoding.GetString(buffer, 0, byteCount);

                    return result;
                }
                finally
                {
                    pool.Return(buffer);
                }
            }
        }

        /// <summary>
        /// Reads <see cref="Boolean"/> value from the <see cref="Stream"/>
        /// using UTF-8 <see cref="Encoding"/>.
        /// </summary>
        public static string ReadString
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, nameof(stream));

            return ReadString(stream, Encoding.UTF8);
        }

        /// <summary>
        /// Reads array of <see cref="short"/> values from the
        /// <see cref="Stream"/>.
        /// </summary>
        public static short[] ReadInt16Array
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, nameof(stream));

            int length = ReadInt32(stream);
            short[] result = new short[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = ReadInt16(stream);
            }

            return result;
        }

        /// <summary>
        /// Reads array of <see cref="ushort"/> values from the
        /// <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static ushort[] ReadUInt16Array
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, nameof(stream));

            int length = ReadInt32(stream);
            ushort[] result = new ushort[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = ReadUInt16(stream);
            }

            return result;
        }

        /// <summary>
        /// Reads array of <see cref="int"/> values from the
        /// <see cref="Stream"/>.
        /// </summary>
        public static int[] ReadInt32Array
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, nameof(stream));

            int length = ReadInt32(stream);
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = ReadInt32(stream);
            }

            return result;
        }

        /// <summary>
        /// Reads array of <see cref="uint"/> values from the
        /// <see cref="Stream"/>.
        /// </summary>
        [NotNull]
        [CLSCompliant(false)]
        public static uint[] ReadUInt32Array
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, nameof(stream));

            int length = ReadInt32(stream);
            uint[] result = new uint[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = ReadUInt32(stream);
            }

            return result;
        }

        /// <summary>
        /// Reads array of <see cref="string"/>'s from the given stream until the end
        /// of the stream using specified <see cref="Encoding"/>.
        /// </summary>
        [NotNull]
        public static string[] ReadStringArray
            (
                [NotNull] Stream stream,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(stream, nameof(stream));
            Code.NotNull(encoding, nameof(encoding));

            int length = ReadInt32(stream);
            string[] result = new string[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = ReadString(stream, encoding);
            }

            return result;
        }

        /// <summary>
        /// Reads array of <see cref="string"/>'s from the <see cref="Stream"/>
        /// using UTF-8 <see cref="Encoding"/>.
        /// </summary>
        [NotNull]
        public static string[] ReadStringArray
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, nameof(stream));

            return ReadStringArray(stream, Encoding.UTF8);
        }

        /// <summary>
        /// Reads the <see cref="decimal"/> from the specified
        /// <see cref="Stream"/>.
        /// </summary>
        public static unsafe decimal ReadDecimal
            (
                [NotNull] Stream stream
            )
        {
            ExtraStruct value = new ExtraStruct();
            ReadExact(stream, value.Array, 16);

            return value.DecimalValue;
        }

        /// <summary>
        /// Reads the date time.
        /// </summary>
        public static unsafe DateTime ReadDateTime
            (
                [NotNull] Stream stream
            )
        {
            LongStruct value = new LongStruct();
            ReadExact(stream, value.Array, 8);

            return value.DateValue;
        }

        /// <summary>
        /// Чтение точного числа байт.
        /// </summary>
        [CLSCompliant(false)]
        public static unsafe void ReadExact
            (
                [NotNull] Stream stream,
                [NotNull] byte* buffer,
                int length
            )
        {
            byte* stop = buffer + length;
            for (byte* ptr = buffer; ptr < stop; ptr++)
            {
                int value = stream.ReadByte();
                if (value < 0)
                {
                    throw new IOException();
                }

                unchecked
                {
                    *ptr = (byte)value;
                }
            }
        }

        /// <summary>
        /// Чтение точного числа байт.
        /// </summary>
        [NotNull]
        public static byte[] ReadExact
            (
                [NotNull] Stream stream,
                int length
            )
        {
            byte[] buffer = new byte[length];
            if (stream.Read(buffer, 0, length) != length)
            {
                Log.Error
                    (
                        "StreamUtility::ReadExact: "
                        + "unexpected end of stream"
                    );

                throw new IOException();
            }

            return buffer;
        }

        /// <summary>
        /// Чтение точного числа байт.
        /// </summary>
        public static void ReadExact
            (
                [NotNull] Stream stream,
                [NotNull] byte[] buffer
            )
        {
            int length = buffer.Length;
            if (stream.Read(buffer, 0, length) != length)
            {
                Log.Error
                    (
                        "StreamUtility::ReadExact: "
                        + "unexpected end of stream"
                    );

                throw new IOException();
            }
        }

        /// <summary>
        /// Чтение точного числа байт.
        /// </summary>
        public static void ReadExact
            (
                [NotNull] Stream stream,
                [NotNull] byte[] buffer,
                int length
            )
        {
            if (stream.Read(buffer, 0, length) != length)
            {
                Log.Error
                    (
                        "StreamUtility::ReadExact: "
                        + "unexpected end of stream"
                    );

                throw new IOException();
            }
        }

        /// <summary>
        /// Write the buffer.
        /// </summary>
        [CLSCompliant(false)]
        public static unsafe void Write
            (
                [NotNull] Stream stream,
                [NotNull] byte* buffer,
                int length
            )
        {
            byte* stop = buffer + length;
            for (byte* ptr = buffer; ptr < stop; ptr++)
            {
                stream.WriteByte(*ptr);
            }
        }

        /// <summary>
        /// Writes the <see cref="bool"/> value to the <see cref="Stream"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                bool value
            )
        {
            Code.NotNull(stream, nameof(stream));

            stream.WriteByte
                (
                    value
                    ? (byte)1
                    : (byte)0
                );
        }

        /// <summary>
        /// Writes the <see cref="short"/> value to the <see cref="Stream"/>.
        /// </summary>
        public static unsafe void Write
            (
                [NotNull] Stream stream,
                short value
            )
        {
            ShortStruct buffer = new ShortStruct { SignedValue = value };
            Write(stream, buffer.Array, 2);
        }

        /// <summary>
        /// Writes the <see cref="ushort"/> value to the <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static unsafe void Write
            (
                [NotNull] Stream stream,
                ushort value
            )
        {
            ShortStruct buffer = new ShortStruct { UnsignedValue = value };
            Write(stream, buffer.Array, 2);
        }

        /// <summary>
        /// Writes the <see cref="int"/> to the <see cref="Stream"/>.
        /// </summary>
        public static unsafe void Write
            (
                [NotNull] Stream stream,
                int value
            )
        {
            MiddleStruct buffer = new MiddleStruct { SignedValue = value };
            Write(stream, buffer.Array, 4);
        }

        /// <summary>
        /// Writes the <see cref="uint"/> to the <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static unsafe void Write
            (
                [NotNull] Stream stream,
                uint value
            )
        {
            MiddleStruct buffer = new MiddleStruct { UnsignedValue = value };
            Write(stream, buffer.Array, 4);
        }

        /// <summary>
        /// Writes the <see cref="long"/> to the <see cref="Stream"/>.
        /// </summary>
        public static unsafe void Write
            (
                [NotNull] Stream stream,
                long value
            )
        {
            LongStruct buffer = new LongStruct { SignedValue = value };
            Write(stream, buffer.Array, 8);
        }

        /// <summary>
        /// Writes the <see cref="ulong"/> to the <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static unsafe void Write
            (
                [NotNull] Stream stream,
                ulong value
            )
        {
            LongStruct buffer = new LongStruct { UnsignedValue = value };
            Write(stream, buffer.Array, 8);
        }

        /// <summary>
        /// Writes the <see cref="float"/> to the <see cref="Stream"/>.
        /// </summary>
        public static unsafe void Write
            (
                [NotNull] Stream stream,
                float value
            )
        {
            MiddleStruct buffer = new MiddleStruct { FloatValue = value };
            Write(stream, buffer.Array, 4);
        }

        /// <summary>
        /// Writes the <see cref="double"/> to the <see cref="Stream"/>.
        /// </summary>
        public static unsafe void Write
            (
                [NotNull] Stream stream,
                double value
            )
        {
            LongStruct buffer = new LongStruct { DoubleValue = value };
            Write(stream, buffer.Array, 8);
        }

        /// <summary>
        /// Writes the <see cref="string"/> to the <see cref="Stream"/>
        /// using specified <see cref="Encoding"/>.
        /// </summary>
        public static unsafe void Write
            (
                [NotNull] Stream stream,
                [NotNull] string value,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(stream, nameof(stream));
            Code.NotNull(value, nameof(value));
            Code.NotNull(encoding, nameof(encoding));

            int byteCount = encoding.GetByteCount(value);
            Write(stream, byteCount);
            if (byteCount < 10)
            {
                byte* buffer = stackalloc byte[byteCount];
                fixed (char* chars = value)
                {
                    encoding.GetBytes(chars, value.Length, buffer, byteCount);
                    Write(stream, buffer, byteCount);
                }
            }
            else
            {
                var pool = ArrayPool<byte>.Shared;
                byte[] buffer = pool.Rent(byteCount);
                try
                {
                    encoding.GetBytes(value, 0, value.Length, buffer, 0);
                    stream.Write(buffer, 0, byteCount);
                }
                finally
                {
                    pool.Return(buffer);
                }
            }
        }

        /// <summary>
        /// Writes the <see cref="string"/> to the <see cref="Stream"/>
        /// using UTF-8 <see cref="Encoding"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                [NotNull] string value
            )
        {
            Code.NotNull(stream, nameof(stream));
            Code.NotNull(value, nameof(value));

            Write(stream, value, Encoding.UTF8);
        }

        /// <summary>
        /// Writes the array of <see cref="short"/> to the <see cref="Stream"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                [NotNull] short[] values
            )
        {
            Code.NotNull(stream, nameof(stream));
            Code.NotNull(values, nameof(values));

            Write(stream, values.Length);
            foreach (var value in values)
            {
                Write(stream, value);
            }
        }

        /// <summary>
        /// Writes the array of <see cref="UInt16"/> to the <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static void Write
            (
                [NotNull] Stream stream,
                [NotNull] ushort[] values
            )
        {
            Code.NotNull(stream, nameof(stream));
            Code.NotNull(values, nameof(values));

            Write(stream, values.Length);
            foreach (var value in values)
            {
                Write(stream, value);
            }
        }

        /// <summary>
        /// Writes the array of <see cref="Int32"/> to the <see cref="Stream"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                [NotNull] int[] values
            )
        {
            Code.NotNull(stream, nameof(stream));
            Code.NotNull(values, nameof(values));

            Write(stream, values.Length);
            foreach (var value in values)
            {
                Write(stream, value);
            }
        }

        /// <summary>
        /// Writes the array of <see cref="uint"/> to the <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static void Write
            (
                [NotNull] Stream stream,
                [NotNull] uint[] values
            )
        {
            Code.NotNull(stream, nameof(stream));
            Code.NotNull(values, nameof(values));

            Write(stream, values.Length);
            foreach (var value in values)
            {
                Write(stream, value);
            }
        }

        /// <summary>
        /// Writes the array of <see cref="string"/> to the <see cref="Stream"/>
        /// using specified <see cref="Encoding"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                [NotNull] string[] values,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(stream, nameof(stream));
            Code.NotNull(values, nameof(values));
            Code.NotNull(encoding, nameof(encoding));

            Write(stream, values.Length);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < values.Length; i++)
            {
                Write(stream, values[i], encoding);
            }
        }

        /// <summary>
        /// Writes the array of <see cref="string"/> to the <see cref="Stream"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                [NotNull] string[] values
            )
        {
            Code.NotNull(stream, nameof(stream));
            Code.NotNull(values, nameof(values));

            Write(stream, values, Encoding.UTF8);
        }

        /// <summary>
        /// Writes the <see cref="decimal"/> to the specified <see cref="Stream"/>.
        /// </summary>
        public static unsafe void Write
            (
                [NotNull] Stream stream,
                decimal value
            )
        {
            ExtraStruct buffer = new ExtraStruct { DecimalValue = value };
            Write(stream, buffer.Array, 16);
        }

        /// <summary>
        /// Writes the <see cref="DateTime"/> to the specified
        /// <see cref="Stream"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                DateTime value
            )
        {
            Code.NotNull(stream, nameof(stream));

            Write(stream, value.ToBinary());
        }

        /// <summary>
        /// Network to host byte conversion for 16-bit integer.
        /// </summary>
        public static void NetworkToHost16
            (
                [NotNull] byte[] array,
                int offset
            )
        {
            byte temp = array[offset];
            array[offset] = array[offset + 1];
            array[offset + 1] = temp;
        }

        /// <summary>
        /// Network to host byte conversion for 16-bit integer.
        /// </summary>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void NetworkToHost16
            (
                [NotNull] byte* array
            )
        {
            byte temp = *array;
            *array = array[1];
            array[1] = temp;
        }

        /// <summary>
        /// Network to host byte conversion for 32-bit integer.
        /// </summary>
        public static void NetworkToHost32
            (
                [NotNull] byte[] array,
                int offset
            )
        {
            byte temp1 = array[offset];
            byte temp2 = array[offset + 1];
            array[offset] = array[offset + 3];
            array[offset + 1] = array[offset + 2];
            array[offset + 3] = temp1;
            array[offset + 2] = temp2;
        }

        /// <summary>
        /// Network to host byte conversion for 32-bit integer.
        /// </summary>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void NetworkToHost32
            (
                [NotNull] byte* array
            )
        {
            byte temp1 = *array;
            byte temp2 = array[1];
            *array = array[3];
            array[1] = array[2];
            array[3] = temp1;
            array[2] = temp2;
        }

        /// <summary>
        /// Network to host byte conversion for 64-bit integer.
        /// </summary>
        /// <remarks>IRBIS64-oriented!</remarks>
        public static void NetworkToHost64
            (
                [NotNull] byte[] array,
                int offset
            )
        {
            NetworkToHost32(array, offset);
            NetworkToHost32(array, offset + 4);
        }

        /// <summary>
        /// Network to host byte conversion for 64-bit integer.
        /// </summary>
        /// <remarks>IRBIS64-oriented!</remarks>
        [CLSCompliant(false)]
        public static unsafe void NetworkToHost64
            (
                [NotNull] byte* array
            )
        {
            NetworkToHost32(array);
            NetworkToHost32(array + 4);
        }

        /// <summary>
        /// Host to network byte conversion for 16-bit integer.
        /// </summary>
        public static void HostToNetwork16
            (
                [NotNull] byte[] array,
                int offset
            )
        {
            byte temp = array[offset];
            array[offset] = array[offset + 1];
            array[offset + 1] = temp;
        }

        /// <summary>
        /// Host to network byte conversion for 16-bit integer.
        /// </summary>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void HostToNetwork16
            (
                [NotNull] byte* array
            )
        {
            byte temp = *array;
            *array = array[1];
            array[1] = temp;
        }

        /// <summary>
        /// Host to network byte conversion for 32-bit integer.
        /// </summary>
        public static void HostToNetwork32
            (
                [NotNull] byte[] array,
                int offset
            )
        {
            byte temp1 = array[offset];
            byte temp2 = array[offset + 1];
            array[offset] = array[offset + 3];
            array[offset + 1] = array[offset + 2];
            array[offset + 3] = temp1;
            array[offset + 2] = temp2;
        }

        /// <summary>
        /// Host to network byte conversion for 32-bit integer.
        /// </summary>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void HostToNetwork32
            (
                [NotNull] byte* array
            )
        {
            byte temp1 = *array;
            byte temp2 = array[1];
            *array = array[3];
            array[1] = array[2];
            array[3] = temp1;
            array[2] = temp2;
        }

        /// <summary>
        /// Host to network byte conversion for 64-bit integer.
        /// </summary>
        /// <remarks>IRBIS64-oriented!</remarks>
        public static void HostToNetwork64
            (
                [NotNull] byte[] array,
                int offset
            )
        {
            HostToNetwork32(array, offset);
            HostToNetwork32(array, offset + 4);
        }

        /// <summary>
        /// Host to network byte conversion for 64-bit integer.
        /// </summary>
        /// <remarks>IRBIS64-oriented!</remarks>
        [CLSCompliant(false)]
        public static unsafe void HostToNetwork64
            (
                [NotNull] byte* array
            )
        {
            HostToNetwork32(array);
            HostToNetwork32(array + 4);
        }

        /// <summary>
        /// Read 16-bit integer in network byte order.
        /// </summary>
        public static unsafe short ReadInt16Network
            (
                [NotNull] this Stream stream
            )
        {
            ShortStruct result = new ShortStruct();
            ReadExact(stream, result.Array, 2);
            NetworkToHost16(result.Array);

            return result.SignedValue;
        }

        /// <summary>
        /// Read 16-bit integer in host byte order.
        /// </summary>
        public static unsafe short ReadInt16Host
            (
                [NotNull] this Stream stream
            )
        {
            ShortStruct result = new ShortStruct();
            ReadExact(stream, result.Array, 2);

            return result.SignedValue;
        }

        /// <summary>
        /// Read 32-bit integer in network byte order.
        /// </summary>
        public static unsafe int ReadInt32Network
            (
                [NotNull] this Stream stream
            )
        {
            MiddleStruct result = new MiddleStruct();
            ReadExact(stream, result.Array, 4);
            NetworkToHost32(result.Array);

            return result.SignedValue;
        }

        /// <summary>
        /// Read 32-bit integer in host byte order.
        /// </summary>
        public static unsafe int ReadInt32Host
            (
                [NotNull] this Stream stream
            )
        {
            MiddleStruct result = new MiddleStruct();
            ReadExact(stream, result.Array, 4);

            return result.SignedValue;
        }

        /// <summary>
        /// Read 32-bit integer in network byte order.
        /// </summary>
        public static unsafe long ReadInt64Network
            (
                [NotNull] this Stream stream
            )
        {
            LongStruct result = new LongStruct();
            ReadExact(stream, result.Array, 8);
            NetworkToHost64(result.Array);

            return result.SignedValue;
        }

        /// <summary>
        /// Read 64-bit integer in host byte order.
        /// </summary>
        public static unsafe long ReadInt64Host
            (
                [NotNull] this Stream stream
            )
        {
            LongStruct result = new LongStruct();
            ReadExact(stream, result.Array, 8);

            return result.SignedValue;
        }

        /// <summary>
        /// Считывает из потока максимально возможное число байт.
        /// </summary>
        /// <remarks>Полезно для считывания из сети (сервер высылает
        /// ответ, после чего закрывает соединение).</remarks>
        /// <param name="stream">Поток для чтения.</param>
        /// <returns>Массив считанных байт.</returns>
        [NotNull]
        public static byte[] ReadToEnd
            (
                [NotNull] this Stream stream
            )
        {
            Code.NotNull(stream, nameof(stream));

            ChunkedBuffer result = new ChunkedBuffer();
            {
                byte[] buffer = new byte[4 * 1024];
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                    {
                        break;
                    }

                    result.Write(buffer, 0, read);
                }

                return result.ToBigArray();
            }
        }

        /// <summary>
        /// Write 16-bit integer to the stream in network byte order.
        /// </summary>
        public static unsafe void WriteInt16Network
            (
                [NotNull] this Stream stream,
                short value
            )
        {
            ShortStruct buffer = new ShortStruct() { SignedValue = value };
            HostToNetwork16(buffer.Array);
            Write(stream, buffer.Array, 2);
        }

        /// <summary>
        /// Write 32-bit integer to the stream in network byte order.
        /// </summary>
        public static unsafe void WriteInt32Network
            (
                [NotNull] this Stream stream,
                int value
            )
        {
            MiddleStruct buffer = new MiddleStruct() { SignedValue = value };
            HostToNetwork32(buffer.Array);
            Write(stream, buffer.Array, 4);
        }

        /// <summary>
        /// Write 64-bit integer to the stream in network byte order.
        /// </summary>
        public static unsafe void WriteInt64Network
            (
                [NotNull] this Stream stream,
                long value
            )
        {
            LongStruct buffer = new LongStruct() { SignedValue = value };
            HostToNetwork64(buffer.Array);
            Write(stream, buffer.Array, 8);
        }

        #endregion
    }
}
