// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConsoleLogger.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC || DESKTOP || NETCORE

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ConsoleLogger
        : IAmLogger
    {
        #region IAmLogger members

        /// <inheritdoc cref="IAmLogger.Debug" />
        public void Debug
            (
                string text
            )
        {
            Console.WriteLine(text);
        }

        /// <inheritdoc cref="IAmLogger.Error" />
        public void Error
            (
                string text
            )
        {
            Console.WriteLine(text);
        }

        /// <inheritdoc cref="IAmLogger.Fatal" />
        public void Fatal
            (
                string text
            )
        {
            Console.WriteLine(text);
        }

        /// <inheritdoc cref="IAmLogger.Info" />
        public void Info
            (
                string text
            )
        {
            Console.WriteLine(text);
        }

        /// <inheritdoc cref="IAmLogger.Trace" />
        public void Trace
            (
                string text
            )
        {
            Console.WriteLine(text);
        }

        /// <inheritdoc cref="IAmLogger.Warn" />
        public void Warn
            (
                string text
            )
        {
            Console.WriteLine(text);
        }

        #endregion
    }
}

#endif
