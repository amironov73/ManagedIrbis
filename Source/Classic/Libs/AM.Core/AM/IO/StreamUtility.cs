// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StreamUtility.cs -- stream manipulation routines.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    /// <summary>
    /// Stream manipulation routines.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class StreamUtility
    {
        #region Private members

#if FW45

        private const MethodImplOptions Aggressive
            = MethodImplOptions.AggressiveInlining;

#else

        private const MethodImplOptions Aggressive
            = (MethodImplOptions)0;

#endif

        #endregion

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
            Code.NotNull(sourceStream, "sourceStream");
            Code.NotNull(destinationStream, "destinationStream");

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
            Code.NotNull(firstStream, "firstStream");
            Code.NotNull(secondStream, "secondStream");

            const int bufferSize = 1024;
            while (true)
            {
                byte[] firstBuffer = new byte[bufferSize];
                int firstReaded = firstStream.Read(firstBuffer, 0, bufferSize);
                byte[] secondBuffer = new byte[bufferSize];
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
            Code.NotNull(stream, "stream");

            if (maximum < 0)
            {
                Log.Error
                    (
                        "StreamUtility::ReadAsMuchAsPossible: "
                        + "maximum="
                        + maximum
                    );

                throw new ArgumentOutOfRangeException("maximum");
            }

            byte[] result = new byte[maximum];
            int readed = stream.Read(result, 0, maximum);
            if (readed <= 0)
            {
                return new byte[0];
            }
            Array.Resize(ref result, readed);

            return result;
        }

        /// <summary>
        /// Reads <see cref="Boolean"/> value from the <see cref="Stream"/>.
        /// </summary>
        public static bool ReadBoolean
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

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
            Code.NotNull(stream, "stream");
            Code.Positive(count, "count");

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
        /// Reads <see cref="Int16"/> value from the <see cref="Stream"/>.
        /// </summary>
        public static short ReadInt16
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            return BitConverter.ToInt16(ReadExact(stream, sizeof(short)), 0);
        }

        /// <summary>
        /// Reads <see cref="UInt16"/> value from the <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static ushort ReadUInt16
            (
                [NotNull] Stream stream
            )
        {
            return BitConverter.ToUInt16(ReadExact(stream, sizeof(ushort)), 0);
        }

        /// <summary>
        /// Reads <see cref="Int32"/> value from the <see cref="Stream"/>.
        /// </summary>
        public static int ReadInt32
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            return BitConverter.ToInt32(ReadExact(stream, sizeof(int)), 0);
        }

        /// <summary>
        /// Reads <see cref="UInt32"/> value from the <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static uint ReadUInt32
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            return BitConverter.ToUInt32(ReadExact(stream, sizeof(uint)), 0);
        }

        /// <summary>
        /// Reads <see cref="Int64"/> value from the <see cref="Stream"/>.
        /// </summary>
        public static long ReadInt64
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            return BitConverter.ToInt64(ReadExact(stream, sizeof(long)), 0);
        }

        /// <summary>
        /// Reads <see cref="UInt64"/> value from the <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static ulong ReadUInt64
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            return BitConverter.ToUInt64(ReadExact(stream, sizeof(ulong)), 0);
        }

        /// <summary>
        /// Reads <see cref="Single"/> value from the <see cref="Stream"/>.
        /// </summary>
        public static float ReadSingle
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            return BitConverter.ToSingle(ReadExact(stream, sizeof(float)), 0);
        }

        /// <summary>
        /// Reads <see cref="Double"/> value from the <see cref="Stream"/>.
        /// </summary>
        public static double ReadDouble
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            return BitConverter.ToDouble(ReadExact(stream, sizeof(double)), 0);
        }

        /// <summary>
        /// Reads <see cref="String"/> value from the <see cref="Stream"/>
        /// using specified <see cref="Encoding"/>.
        /// </summary>
        /// <seealso cref="Write(Stream,string,Encoding)"/>
        public static string ReadString
            (
                [NotNull] Stream stream,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(stream, "stream");
            Code.NotNull(encoding, "encoding");

            int count = ReadInt32(stream);
            byte[] bytes = ReadExact(stream, count);

            string result = EncodingUtility.GetString
                (
                    encoding,
                    bytes
                );

            return result;
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
            Code.NotNull(stream, "stream");

            return ReadString(stream, Encoding.UTF8);
        }

        /// <summary>
        /// Reads array of <see cref="Int16"/> values from the 
        /// <see cref="Stream"/>.
        /// </summary>
        public static short[] ReadInt16Array
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            int length = ReadInt32(stream);
            short[] result = new short[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = ReadInt16(stream);
            }

            return result;
        }

        /// <summary>
        /// Reads array of <see cref="UInt16"/> values from the 
        /// <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static ushort[] ReadUInt16Array
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            int length = ReadInt32(stream);
            ushort[] result = new ushort[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = ReadUInt16(stream);
            }

            return result;
        }

        /// <summary>
        /// Reads array of <see cref="Int32"/> values from the 
        /// <see cref="Stream"/>.
        /// </summary>
        public static int[] ReadInt32Array
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            int length = ReadInt32(stream);
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = ReadInt32(stream);
            }

            return result;
        }

        /// <summary>
        /// Reads array of <see cref="UInt32"/> values from the 
        /// <see cref="Stream"/>.
        /// </summary>
        [NotNull]
        [CLSCompliant(false)]
        public static uint[] ReadUInt32Array
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            int length = ReadInt32(stream);
            uint[] result = new uint[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = ReadUInt32(stream);
            }

            return result;
        }

        /// <summary>
        /// Reads array of <see cref="String"/>'s from the given stream until the end
        /// of the stream using specified <see cref="Encoding"/>.
        /// </summary>
        [NotNull]
        public static string[] ReadStringArray
            (
                [NotNull] Stream stream,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(stream, "stream");
            Code.NotNull(encoding, "encoding");

            int length = ReadInt32(stream);
            string[] result = new string[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = ReadString(stream, encoding);
            }

            return result;
        }

        /// <summary>
        /// Reads array of <see cref="String"/>'s from the <see cref="Stream"/>
        /// using UTF-8 <see cref="Encoding"/>.
        /// </summary>
        [NotNull]
        public static string[] ReadStringArray
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            return ReadStringArray(stream, Encoding.UTF8);
        }

        /// <summary>
        /// Reads the <see cref="Decimal"/> from the specified 
        /// <see cref="Stream"/>.
        /// </summary>
        public static decimal ReadDecimal
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            int[] bits = ReadInt32Array(stream);
            return new decimal(bits);
        }


        /// <summary>
        /// Reads the date time.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static DateTime ReadDateTime
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

#if WINMOBILE || PocketPC

            throw new System.NotImplementedException();

#else

            long binary = ReadInt64(stream);

            return DateTime.FromBinary(binary);
#endif
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
                        "StreamUtility::_Read: "
                        + "unexpected end of stream"
                    );

                throw new IOException();
            }

            return buffer;
        }

        /// <summary>
        /// Writes the <see cref="Boolean"/> value to the <see cref="Stream"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                bool value
            )
        {
            Code.NotNull(stream, "stream");

            stream.WriteByte
                (
                    value
                    ? (byte)1
                    : (byte)0
                );
        }

        /// <summary>
        /// Writes the <see cref="Int16"/> value to the <see cref="Stream"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                short value
            )
        {
            Code.NotNull(stream, "stream");

            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the <see cref="UInt16"/> value to the <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static void Write
            (
                [NotNull] Stream stream,
                ushort value
            )
        {
            Code.NotNull(stream, "stream");

            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the <see cref="Int32"/> to the <see cref="Stream"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                int value
            )
        {
            Code.NotNull(stream, "stream");

            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the <see cref="UInt32"/> to the <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static void Write
            (
                [NotNull] Stream stream,
                uint value
            )
        {
            Code.NotNull(stream, "stream");

            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the <see cref="Int64"/> to the <see cref="Stream"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                long value
            )
        {
            Code.NotNull(stream, "stream");

            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the <see cref="UInt64"/> to the <see cref="Stream"/>.
        /// </summary>
        [CLSCompliant(false)]
        public static void Write
            (
                [NotNull] Stream stream,
                ulong value
            )
        {
            Code.NotNull(stream, "stream");

            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the <see cref="Single"/> to the <see cref="Stream"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                float value
            )
        {
            Code.NotNull(stream, "stream");

            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the <see cref="Double"/> to the <see cref="Stream"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                double value
            )
        {
            Code.NotNull(stream, "stream");

            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the <see cref="String"/> to the <see cref="Stream"/>
        /// using specified <see cref="Encoding"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                [NotNull] string value,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(stream, "stream");
            Code.NotNull(value, "value");
            Code.NotNull(encoding, "encoding");

            byte[] bytes = encoding.GetBytes(value);
            Write(stream, bytes.Length);
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the <see cref="String"/> to the <see cref="Stream"/>
        /// using UTF-8 <see cref="Encoding"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                [NotNull] string value
            )
        {
            Code.NotNull(stream, "stream");
            Code.NotNull(value, "value");

            Write(stream, value, Encoding.UTF8);
        }

        /// <summary>
        /// Writes the array of <see cref="Int16"/> to the <see cref="Stream"/>.
        /// </summary>
        public static void Write
            (
                [NotNull] Stream stream,
                [NotNull] short[] values
            )
        {
            Code.NotNull(stream, "stream");
            Code.NotNull(values, "values");

            Write(stream, values.Length);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < values.Length; i++)
            {
                Write(stream, values[i]);
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
            Code.NotNull(stream, "stream");
            Code.NotNull(values, "values");

            Write(stream, values.Length);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < values.Length; i++)
            {
                Write(stream, values[i]);
            }
        }

        /// <summary>
        /// Writes the array of <see cref="Int32"/> to the <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="values">Array of signed integer numbers.</param>
        /// <remarks>Value can be readed with 
        /// <see cref="ReadInt32Array"/> or compatible method.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Either 
        /// <paramref name="stream"/> or <paramref name="values"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="IOException">An error during stream
        /// output happens.</exception>
        /// <see cref="Write(Stream,uint[])"/>
        /// <see cref="ReadInt32Array"/>
        public static void Write
            (
                [NotNull] Stream stream,
                [NotNull] int[] values
            )
        {
            Code.NotNull(stream, "stream");
            Code.NotNull(values, "values");

            Write(stream, values.Length);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < values.Length; i++)
            {
                Write(stream, values[i]);
            }
        }

        /// <summary>
        /// Writes the array of <see cref="UInt32"/> to the <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="values">Array of unsigned integer numbers.</param>
        /// <remarks>Value can be readed with 
        /// <see cref="ReadUInt32Array"/> or compatible method.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Either 
        /// <paramref name="stream"/> or <paramref name="values"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="IOException">An error during stream
        /// output happens.</exception>
        /// <seealso cref="Write(Stream,int[])"/>
        /// <see cref="ReadUInt32Array"/>
        [CLSCompliant(false)]
        public static void Write
            (
                [NotNull] Stream stream,
                [NotNull] uint[] values
            )
        {
            Code.NotNull(stream, "stream");
            Code.NotNull(values, "values");

            Write(stream, values.Length);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < values.Length; i++)
            {
                Write(stream, values[i]);
            }
        }

        /// <summary>
        /// Writes the array of <see cref="String"/> to the <see cref="Stream"/>
        /// using specified <see cref="Encoding"/>.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="values">Array of strings to write.</param>
        /// <param name="encoding">Encoding to use.</param>
        /// <remarks>Value can be readed with 
        /// <see cref="ReadStringArray(Stream,Encoding)"/> or compatible method.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Either 
        /// <paramref name="stream"/> or <paramref name="values"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="IOException">An error during stream
        /// output happens.</exception>
        /// <seealso cref="Write(Stream,string[])"/>
        /// <see cref="ReadStringArray(Stream,Encoding)"/>
        public static void Write
            (
                [NotNull] Stream stream,
                [NotNull] string[] values,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(stream, "stream");
            Code.NotNull(values, "values");
            Code.NotNull(encoding, "encoding");

            Write(stream, values.Length);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < values.Length; i++)
            {
                Write(stream, values[i], encoding);
            }
        }

        /// <summary>
        /// Writes the array of <see cref="String"/> to the <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="values">Array of strings to write.</param>
        /// <remarks>Value can be readed with 
        /// <see cref="ReadStringArray(Stream)"/> or compatible method.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Either 
        /// <paramref name="stream"/> or <paramref name="values"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="IOException">An error during stream
        /// output happens.</exception>
        /// <seealso cref="Write(Stream,string[],Encoding)"/>
        /// <seealso cref="ReadStringArray(Stream)"/>
        public static void Write
            (
                [NotNull] Stream stream,
                [NotNull] string[] values
            )
        {
            Code.NotNull(stream, "stream");
            Code.NotNull(values, "values");

            Write(stream, values, Encoding.UTF8);
        }

        /// <summary>
        /// Writes the <see cref="Decimal"/> to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value.</param>
        /// <remarks>Value can be readed with <see cref="ReadDecimal"/>
        /// or compatible method.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> is <c>null</c>.</exception>
        /// <exception cref="IOException">Error during stream output
        /// happens.</exception>
        /// <seealso cref="ReadDecimal"/>
        public static void Write
            (
                [NotNull] Stream stream,
                decimal value
            )
        {
            Code.NotNull(stream, "stream");

            Write(stream, decimal.GetBits(value));
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
            Code.NotNull(stream, "stream");

#if WINMOBILE || PocketPC || SILVERLIGHT

            throw new System.NotImplementedException();

#else

            Write(stream, value.ToBinary());

#endif
        }

        /// <summary>
        /// Network to host byte conversion.
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
        /// Network to host byte conversion.
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
        /// Network to host byte conversion.
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
        /// Host to network byte conversion.
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
        /// Host to network byte conversion.
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
        /// Host to network byte conversion.
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
        /// Read integer in network byte order.
        /// </summary>
        public static short ReadInt16Network
            (
                [NotNull] this Stream stream
            )
        {
            byte[] buffer = ReadExact(stream, 2);
            NetworkToHost16(buffer, 0);
            short result = BitConverter.ToInt16(buffer, 0);

            return result;
        }

        /// <summary>
        /// Read integer in host byte order.
        /// </summary>
        public static short ReadInt16Host
            (
                [NotNull] this Stream stream
            )
        {
            byte[] buffer = ReadExact(stream, 2);
            short result = BitConverter.ToInt16(buffer, 0);

            return result;
        }

        /// <summary>
        /// Read integer in network byte order.
        /// </summary>
        public static int ReadInt32Network
            (
                [NotNull] this Stream stream
            )
        {
            byte[] buffer = ReadExact(stream, 4);
            NetworkToHost32(buffer, 0);
            int result = BitConverter.ToInt32(buffer, 0);

            return result;
        }

        /// <summary>
        /// Read integer in host byte order.
        /// </summary>
        public static int ReadInt32Host
            (
                [NotNull] this Stream stream
            )
        {
            byte[] buffer = ReadExact(stream, 4);
            int result = BitConverter.ToInt32(buffer, 0);

            return result;
        }

        /// <summary>
        /// Read integer in network byte order.
        /// </summary>
        public static long ReadInt64Network
            (
                [NotNull] this Stream stream
            )
        {
            byte[] buffer = ReadExact(stream, 8);
            NetworkToHost64(buffer, 0);
            long result = BitConverter.ToInt64(buffer, 0);

            return result;
        }

        /// <summary>
        /// Read integer in host byte order.
        /// </summary>
        public static long ReadInt64Host
            (
                [NotNull] this Stream stream
            )
        {
            byte[] buffer = ReadExact(stream, 8);
            long result = BitConverter.ToInt64(buffer, 0);

            return result;
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
            Code.NotNull(stream, "stream");

            MemoryStream result = new MemoryStream(); //-V3114

            while (true)
            {
                byte[] buffer = new byte[50 * 1024];
                int read = stream.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    break;
                }
                result.Write(buffer, 0, read);
            }

            return result.ToArray();
        }

#if !WIN81 && !PORTABLE

        /// <summary>
        /// Lock the file.
        /// </summary>
        /// <remarks>For WinMobile compatibility.</remarks>
        [MethodImpl(Aggressive)]
        [ExcludeFromCodeCoverage]
        public static void Lock
            (
                [NotNull] FileStream stream,
                long position,
                long length
            )
        {
#if CLASSIC || DROID

            stream.Lock(position, length);

#endif
        }

        /// <summary>
        /// Unlock the file.
        /// </summary>
        /// <remarks>For WinMobile compatibility.</remarks>
        [MethodImpl(Aggressive)]
        [ExcludeFromCodeCoverage]
        public static void Unlock
            (
                [NotNull] FileStream stream,
                long position,
                long length
            )
        {
#if CLASSIC || DROID

            stream.Unlock(position, length);

#endif
        }

#endif

        /// <summary>
        /// Write 16-bit integer to the stream in network byte order.
        /// </summary>
        public static void WriteInt16Network
            (
                [NotNull] this Stream stream,
                short value
            )
        {
            byte[] buffer = BitConverter.GetBytes(value);
            HostToNetwork16(buffer, 0);
            stream.Write(buffer, 0, 2);
        }

        /// <summary>
        /// Write 32-bit integer to the stream in network byte order.
        /// </summary>
        public static void WriteInt32Network
            (
                [NotNull] this Stream stream,
                int value
            )
        {
            byte[] buffer = BitConverter.GetBytes(value);
            HostToNetwork32(buffer, 0);
            stream.Write(buffer, 0, 4);
        }

        /// <summary>
        /// Write 64-bit integer to the stream in network byte order.
        /// </summary>
        public static void WriteInt64Network
            (
                [NotNull] this Stream stream,
                long value
            )
        {
            byte[] buffer = BitConverter.GetBytes(value);
            HostToNetwork64(buffer, 0);
            stream.Write(buffer, 0, 8);
        }

        #endregion
    }
}
