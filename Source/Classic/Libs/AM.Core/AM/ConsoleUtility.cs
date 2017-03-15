// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConsoleUtility.cs -- useful routines for console manipulation
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC || NETCORE

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Useful routines for console manipulation.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ConsoleUtility
    {
        /// <summary>
        /// Перенаправление стандартного вывода в файл.
        /// </summary>
        public static void RedirectStandardOutput
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            //StreamWriter stdOutput = new StreamWriter
            //    (
            //        fileName,
            //        false,
            //        encoding
            //    )
            StreamWriter stdOutput = new StreamWriter
                (
                    File.Create(fileName),
                    encoding
                )
            {
                AutoFlush = true
            };

            Console.SetOut(stdOutput);
        }

        /// <summary>
        /// Переключение кодовой страницы вывода консоли.
        /// </summary>
        public static void SetOutputCodePage
            (
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(encoding, "encoding");

            StreamWriter stdOutput = new StreamWriter
                (
                    Console.OpenStandardOutput(),
                    encoding
                )
            {
                AutoFlush = true
            };
            Console.SetOut(stdOutput);

            StreamWriter stdError = new StreamWriter
                (
                    Console.OpenStandardError(),
                    encoding
                )
            {
                AutoFlush = true
            };
            Console.SetError(stdError);
        }

        /// <summary>
        /// Переключение кодовой страницы вывода консоли.
        /// </summary>
        public static void SetOutputCodePage
            (
                int codePage
            )
        {
            SetOutputCodePage
                (
                    Encoding.GetEncoding(codePage)
                );
        }

        /// <summary>
        /// Переключение кодовой страницы вывода консоли.
        /// </summary>
        /// <param name="codePage"></param>
        public static void SetOutputCodePage
            (
                [NotNull] string codePage
            )
        {
            Code.NotNullNorEmpty(codePage, "codePage");

            SetOutputCodePage
                (
                    Encoding.GetEncoding(codePage)
                );
        }

        /// <summary>
        /// Исправление бага Visual Studio 2005, когда в окне Console
        /// вместо кириллицы показываются кракозябрики.
        /// </summary>
        public static void FixVisualStudio2005Bug()
        {
            if (Debugger.IsAttached)
            {
                SetOutputCodePage(Encoding.GetEncoding(0));
            }
        }
    }
}

#endif

