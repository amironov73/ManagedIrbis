// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NullConsole.cs -- 
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
    public sealed class NullConsole
        : IConsoleDriver
    {
        #region IConsoleDriver members

        /// <inheritdoc />
        public ConsoleColor BackgroundColor { get; set; }

        /// <inheritdoc />
        public ConsoleColor ForegroundColor { get; set; }

        /// <inheritdoc />
        public bool KeyAvailable { get { return false; } }

        /// <inheritdoc />
        public string Title { get; set; }

        /// <inheritdoc />
        public void Clear()
        {
        }

        /// <inheritdoc />
        public ConsoleKeyInfo ReadKey
            (
                bool intercept
            )
        {
            return new ConsoleKeyInfo();
        }

        /// <inheritdoc />
        public int Read()
        {
            return -1;
        }

        /// <inheritdoc />
        public string ReadLine()
        {
            return null;
        }

        /// <inheritdoc />
        public void Write
            (
                string text
            )
        {
        }

        /// <inheritdoc />
        public void WriteLine()
        {
        }

        #endregion
    }
}
