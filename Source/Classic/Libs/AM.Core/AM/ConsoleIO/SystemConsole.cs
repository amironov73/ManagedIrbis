// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SystemConsole.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.ConsoleIO
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SystemConsole
        : IConsoleDriver
    {
        #region IConsoleDriver members

        /// <inheritdoc cref="IConsoleDriver.BackgroundColor" />
        public ConsoleColor BackgroundColor
        {
            get
            {
#if UAP || WIN81 || PORTABLE || SILVERLIGHT || WINMOBILE
                return ConsoleColor.Black;
#else
                return Console.BackgroundColor;
#endif
            }
            set
            {
#if !UAP && !WIN81 && !PORTABLE && !SILVERLIGHT && !WINMOBILE
                Console.BackgroundColor = value;
#endif
            }
        }

        /// <inheritdoc cref="IConsoleDriver.ForegroundColor" />
        public ConsoleColor ForegroundColor
        {
            get
            {
#if UAP || WIN81 || PORTABLE || SILVERLIGHT || WINMOBILE
                return ConsoleColor.White;
#else
                return Console.ForegroundColor;
#endif
            }
            set
            {
#if !UAP && !WIN81 && !PORTABLE && !SILVERLIGHT && !WINMOBILE
                Console.ForegroundColor = value;
#endif
            }
        }

        /// <inheritdoc cref="IConsoleDriver.KeyAvailable" />
        public bool KeyAvailable 
        {
            get
            {
#if UAP || WIN81 || PORTABLE || SILVERLIGHT || WINMOBILE
                return false;
#else
                return Console.KeyAvailable;
#endif
            }
        }

        /// <inheritdoc cref="IConsoleDriver.Title" />
        public string Title
        {
            get
            {
#if UAP || WIN81 || PORTABLE || SILVERLIGHT || WINMOBILE
                return string.Empty;
#else
                return Console.Title;
#endif
            }
            set
            {
#if !UAP && !WIN81 && !PORTABLE && !SILVERLIGHT && !WINMOBILE
                Console.Title = value;
#endif
            }
        }

        /// <inheritdoc cref="IConsoleDriver.Clear" />
        public void Clear()
        {
#if !UAP && !WIN81 && !PORTABLE && !SILVERLIGHT && !WINMOBILE
            Console.Clear();
#endif
        }

        /// <inheritdoc cref="IConsoleDriver.Read" />
        public int Read()
        {
#if UAP || WIN81 || PORTABLE || SILVERLIGHT || WINMOBILE
            return -1;
#else
            return Console.Read();
#endif
        }

        /// <inheritdoc cref="IConsoleDriver.ReadKey" />
        public ConsoleKeyInfo ReadKey
            (
                bool intercept
            )
        {
#if UAP || WIN81 || PORTABLE || SILVERLIGHT || WINMOBILE
            return new ConsoleKeyInfo();
#else
            return Console.ReadKey(intercept);
#endif
        }

        /// <inheritdoc cref="IConsoleDriver.ReadLine" />
        public string ReadLine()
        {
#if UAP || WIN81 || PORTABLE || SILVERLIGHT || WINMOBILE
            return null;
#else
            return Console.ReadLine();
#endif
        }

        /// <inheritdoc cref="IConsoleDriver.Write" />
        public void Write
            (
                string text
            )
        {
#if !UAP && !WIN81 && !PORTABLE && !SILVERLIGHT && !WINMOBILE
            if (!string.IsNullOrEmpty(text))
            {
                Console.Write(text);
            }
#endif
        }

        /// <inheritdoc cref="IConsoleDriver.WriteLine" />
        public void WriteLine()
        {
#if !UAP && !WIN81 && !PORTABLE && !SILVERLIGHT && !WINMOBILE
            Console.WriteLine();
#endif
        }

        #endregion
    }
}
