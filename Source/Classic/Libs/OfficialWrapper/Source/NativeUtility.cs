// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NativeUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.ConsoleIO;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace OfficialWrapper
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class NativeUtility
    {
        #region Public methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IntPtr AllocateMemory(int size)
        {
            return Marshal.AllocHGlobal(size);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pointer"></param>
        public static void FreeMemory(IntPtr pointer)
        {
            Marshal.FreeHGlobal(pointer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static Encoding GetUtfEncoding()
        {
            return new UTF8Encoding(false, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static byte[] ToUtf(string text)
        {
            Encoding utf = GetUtfEncoding();
            int len = utf.GetByteCount(text);
            byte[] result = new byte[len + 5];
            utf.GetBytes(text, 0, text.Length, result, 0);
            //DumpBuffer(result,0,result.Length);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static byte[] ToUtfLen(string text)
        {
            Encoding utf = GetUtfEncoding();
            int len = utf.GetByteCount(text);
            byte[] result = new byte[len + 5];
            Array.Copy
                (
                    BitConverter.GetBytes(len),
                    0,
                    result,
                    0,
                    4
                );
            utf.GetBytes
                (
                    text.ToCharArray(),
                    0,
                    text.Length,
                    result,
                    4
                );
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FromUtf(byte[] bytes)
        {
            return GetUtfEncoding().GetString(bytes);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FromUtfZ(byte[] bytes)
        {
            int index = Array.IndexOf<byte>(bytes, 0);
            if (index < 0)
            {
                index = bytes.Length;
            }
            return GetUtfEncoding().GetString
                (
                    bytes,
                    0,
                    index
                );
        }

        /// <summary>
        /// Поиск нуля-ограничителя в строке.
        /// </summary>
        [NotNull]
        public static string TrimAtZero
            (
                [NotNull] string text
            )
        {
            int index = text.IndexOf((char)0);
            return index >= 0
                       ? text.Substring(0, index)
                       : text;
        }

        /// <summary>
        /// Дамп буфера в консоль (для отладочных целей).
        /// </summary>
        public static void DumpBuffer
            (
                byte[] buffer,
                int start,
                int length
            )
        {
            length = Math.Max(0, Math.Min(buffer.Length - start, length));
            for (int i = 0; i < length; i++)
            {
                byte b = buffer[start + i];
                ConsoleInput.Write
                    (
                        string.Format
                        (
                            "{0} ",
                            b.ToString("X2")
                        )
                    );
            }
        }

        #endregion
    }
}
