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

        /// <inheritdoc />
        public ConsoleColor BackgroundColor
        {
            get { return Console.BackgroundColor; }
            set { Console.BackgroundColor = value; }
        }

        /// <inheritdoc />
        public ConsoleColor ForegroundColor
        {
            get { return Console.ForegroundColor; }
            set { Console.ForegroundColor = value; }
        }

        /// <inheritdoc />
        public bool KeyAvailable 
        {
            get { return Console.KeyAvailable; }
        }

        /// <inheritdoc />
        public string Title
        {
            get { return Console.Title; }
            set { Console.Title = value; }
        }

        /// <inheritdoc />
        public void Clear()
        {
            Console.Clear();
        }

        /// <inheritdoc />
        public int Read()
        {
            return Console.Read();
        }

        /// <inheritdoc />
        public ConsoleKeyInfo ReadKey
            (
                bool intercept
            )
        {
            return Console.ReadKey(intercept);
        }

        /// <inheritdoc />
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        /// <inheritdoc />
        public void Write
            (
                string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                Console.Write(text);
            }
        }

        /// <inheritdoc />
        public void WriteLine()
        {
            Console.WriteLine();
        }

        #endregion
    }
}
