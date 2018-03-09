// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* KernelUtil.cs -- kernel32.dll helper
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Kernel32.dll helper.
    /// </summary>
    [PublicAPI]
    public static class KernelUtility
    {
        #region Nested classes

        private class CodePageEnumerator
        {
            private List<string> _result;

            private bool _Callback(string codePage)
            {
                _result.Add(codePage);
                return true;
            }

            public string[] Enumerate(bool onlyInstalled)
            {
                _result = new List<string>();
                Kernel32.EnumSystemCodePages
                    (
                        _Callback,
                        onlyInstalled
                         ? CodePageEnumFlags.CP_INSTALLED
                         : CodePageEnumFlags.CP_SUPPORTED
                    );
                return _result.ToArray();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Convert Win32 error code to string.
        /// </summary>
        [CLSCompliant(false)]
        public static string ErrorCodeToString
            (
                uint errorCode
            )
        {
            const int bufferSize = 4 * 1024;
            StringBuilder builder = new StringBuilder(bufferSize);
            Kernel32.FormatMessage
                (
                    FormatMessageFlags.FORMAT_MESSAGE_FROM_SYSTEM,
                    0,
                    errorCode,
                    0,
                    builder,
                    bufferSize,
                    0
                );

            return builder.ToString();
        }

        /// <summary>
        /// Fatal exit.
        /// </summary>
        public static void FatalExit
            (
                string format,
                params object[] args
            )
        {
            Kernel32.FatalAppExit(0, string.Format(format, args));
        }

        /// <summary>
        /// Enumerates system code pages.
        /// </summary>
        /// <remarks>
        /// Типичный вывод:
        /// 10000, 10006, 10007, 10010, 10017, 10029, 10079, 10081,
        /// 10082, 1026, 1250, 1251, 1252, 1253, 1254, 1255, 1256,
        /// 1257, 1258, 1361, 20127, 20261, 20866, 21866, 28591, 28592,
        /// 28594, 28595, 28597, 28599, 28605, 37, 437, 500, 737, 775,
        /// 850, 852, 855, 857, 860, 861, 863, 865, 866, 869, 874, 875,
        /// 932, 936, 949, 950, 28603, 65000, 65001
        /// </remarks>
        public static string[] GetSystemCodePages
            (
                bool onlyInstalled
            )
        {
            return new CodePageEnumerator().Enumerate(onlyInstalled);
        }

        /// <summary>
        /// Retrieves the size of a specified file.
        /// </summary>
        /// <param name="handle">Handle to the file whose size is to 
        /// be returned. This handle must have been created with either 
        /// the GENERIC_READ or GENERIC_WRITE access right.</param>
        /// <returns>File size.</returns>
        public static long GetFileSize
            (
                IntPtr handle
            )
        {
            int highPart = 0;
            uint lowPart = Kernel32.GetFileSize(handle, ref highPart);

            return ((long)highPart << 32) | lowPart;
        }

        /// <summary>
        /// Gets the name of the short path.
        /// </summary>
        public static string GetShortPathName
            (
                string longName
            )
        {
            StringBuilder buffer = new StringBuilder { Capacity = 300 };
            if (Kernel32.GetShortPathName(longName, buffer, buffer.Capacity) <= 0)
            {
                throw new Win32Exception();
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Retrieves handle for main module.
        /// </summary>
        public static IntPtr MainModuleHandle()
        {
            return Kernel32.GetModuleHandle(null);
        }

        #endregion
    }
}
