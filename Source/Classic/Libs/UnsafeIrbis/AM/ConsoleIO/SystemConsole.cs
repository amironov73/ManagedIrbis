// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SystemConsole.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.ConsoleIO
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public sealed class SystemConsole
        : IConsoleDriver
    {
        #region IConsoleDriver members

        /// <inheritdoc cref="IConsoleDriver.BackgroundColor" />
        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        /// <inheritdoc cref="IConsoleDriver.ForegroundColor" />
        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        /// <inheritdoc cref="IConsoleDriver.KeyAvailable" />
        public bool KeyAvailable => Console.KeyAvailable;

        /// <inheritdoc cref="IConsoleDriver.Title" />
        public string Title
        {
            get => Console.Title;
            set => Console.Title = value;
        }

        /// <inheritdoc cref="IConsoleDriver.Clear" />
        public void Clear()
        {
            Console.Clear();
        }

        /// <inheritdoc cref="IConsoleDriver.Read" />
        public int Read()
        {
            return Console.Read();
        }

        /// <inheritdoc cref="IConsoleDriver.ReadKey" />
        public ConsoleKeyInfo ReadKey
            (
                bool intercept
            )
        {
            return Console.ReadKey(intercept);
        }

        /// <inheritdoc cref="IConsoleDriver.ReadLine" />
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        /// <inheritdoc cref="IConsoleDriver.Write" />
        public void Write
            (
                string text
            )
        {
            Console.Write(text);
        }

        /// <inheritdoc cref="IConsoleDriver.WriteLine" />
        public void WriteLine()
        {
            Console.WriteLine();
        }

        #endregion
    }
}
