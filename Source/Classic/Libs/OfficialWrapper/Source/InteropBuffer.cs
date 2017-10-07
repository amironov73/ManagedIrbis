// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InteropBuffer.cs -- helper class for interop with irbis64_client.dll
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace OfficialWrapper
{
    public sealed class InteropBuffer
        : IDisposable
    {
        #region Constants

        public const int DefaultSize = 30*1024;

        #endregion

        #region Construction

        public InteropBuffer()
            : this(DefaultSize)
        {
        }

        public InteropBuffer( int size )
        {
            Handle = Marshal.AllocHGlobal(size);
        }

        #endregion

        #region Private members

        #endregion

        #region Public members

        public IntPtr Handle;

        public int Size
        {
            get { return (int) NativeMethods64.GlobalSize(Handle); }
        }

        public byte[] Bytes
        {
            get
            {
                int size = Size;
                byte[] result = new byte[size];
                Marshal.Copy(Handle, result, 0, size);
                return result;
            }
            set
            {
                int size = value.Length;
                if (size > Size)
                {
                    throw new ArgumentException("Bytes");
                }
                Marshal.Copy
                    (
                        value,
                        0,
                        Handle,
                        size
                    );
            }
        }

        public string AnsiString
        {
            get
            {
                byte[] bytes = Bytes;
                string result = Encoding.Default.GetString(bytes);
                return result;
            }
            set
            {
                Bytes = Encoding.Default.GetBytes(value);
            }
        }

        public string AnsiZString
        {
            get 
            { 
                byte[] bytes = Bytes;
                int zero = Array.IndexOf<byte>(bytes,0);
                string result = Encoding.Default.GetString
                    (
                        bytes,
                        0,
                        (zero >= 0) ? zero : bytes.Length
                    );
                return result;
            }
            set
            {
                Bytes = StringToBytesZ
                    (
                        value,
                        Encoding.Default
                    );
            }
        }

        public string UnicodeString
        {
            get
            {
                byte[] bytes = Bytes;
                string result = Encoding.Unicode.GetString(bytes);
                return result;
            }
            set
            {
                Bytes = new UnicodeEncoding(false, false)
                    .GetBytes(value);
            }
        }

        public string UnicodeZString
        {
            get
            {
                string result = UnicodeString;
                result = NativeUtility.TrimAtZero(result);
                return result;
            }
            set
            {
                Bytes = StringToBytesZ
                    (
                        value,
                        new UnicodeEncoding(false,false)
                    );
            }
        }

        public string Utf8String
        {
            get
            {
                byte[] bytes = Bytes;
                string result = Encoding.UTF8.GetString(bytes);
                return result;
            }
            set
            {
                Bytes = new UTF8Encoding(false)
                    .GetBytes(value);
            }
        }

        public string Utf8ZString
        {
            get
            {
                string result = Utf8String;
                result = NativeUtility.TrimAtZero(result);
                return result;
            }
            set
            {
                Bytes = StringToBytesZ
                    (
                        value,
                        new UTF8Encoding(false)
                    );
            }
        }

        public bool Increase ()
        {
            int newSize = Size*2;
            Marshal.FreeHGlobal (Handle);
            Handle = Marshal.AllocHGlobal (newSize);
            return true;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Конвертирует строку в массив байтов с завершающим нулем.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public byte[] StringToBytesZ 
            (
                string text,
                Encoding encoding
            )
        {
            int length = encoding.GetByteCount(text);
            byte [] result = new byte[length + 2];
            Array.Copy
                (
                    encoding.GetBytes(text),
                    result,
                    length
                );
            result[length] = result[length + 1] = 0;
            return result;
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            Marshal.FreeHGlobal(Handle);
        }

        #endregion
    }
}
