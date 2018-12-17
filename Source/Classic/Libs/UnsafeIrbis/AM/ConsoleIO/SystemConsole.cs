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

#endregion

namespace UnsafeAM.ConsoleIO
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public sealed class SystemConsole
        : IConsoleDriver
    {
        #region IConsoleDriver members

        /// <inheritdoc cref="IConsoleDriver.BackgroundColor" />
        public ConsoleColor BackgroundColor
        {
            get
            {
#if WINMOBILE || PocketPC || UAP

                return ConsoleColor.Black;

#else

                return Console.BackgroundColor;

#endif
            }
            set
            {
#if !WINMOBILE && !PocketPC && !UAP

                Console.BackgroundColor = value;

#endif
            }
        }

        /// <inheritdoc cref="IConsoleDriver.ForegroundColor" />
        public ConsoleColor ForegroundColor
        {
            get
            {
#if WINMOBILE || PocketPC || UAP

                return ConsoleColor.White;

#else

                return Console.ForegroundColor;

#endif
            }
            set
            {
#if !WINMOBILE && !PocketPC && !UAP

                Console.ForegroundColor = value;

#endif
            }
        }

        /// <inheritdoc cref="IConsoleDriver.KeyAvailable" />
        public bool KeyAvailable
        {
            get
            {
#if WINMOBILE || PocketPC || UAP

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
#if WINMOBILE || PocketPC || UAP

                return string.Empty;

#else

                return Console.Title;

#endif
            }
            set
            {
#if !WINMOBILE && !PocketPC && !UAP

                Console.Title = value;

#endif
            }
        }

        /// <inheritdoc cref="IConsoleDriver.Clear" />
        public void Clear()
        {
#if !WINMOBILE && !PocketPC && !UAP

            Console.Clear();

#endif
        }

        /// <inheritdoc cref="IConsoleDriver.Read" />
        public int Read()
        {
#if WINMOBILE || PocketPC || UAP

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
#if WINMOBILE || PocketPC || UAP

            return new ConsoleKeyInfo();

#else

            return Console.ReadKey(intercept);

#endif
        }

        /// <inheritdoc cref="IConsoleDriver.ReadLine" />
        public string ReadLine()
        {
#if WINMOBILE || PocketPC || UAP

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
#if !WINMOBILE && !PocketPC && !UAP

            Console.Write(text);

#endif
        }

        /// <inheritdoc cref="IConsoleDriver.WriteLine" />
        public void WriteLine()
        {
#if !WINMOBILE && !PocketPC && !UAP

            Console.WriteLine();

#endif
        }

        #endregion
    }
}
