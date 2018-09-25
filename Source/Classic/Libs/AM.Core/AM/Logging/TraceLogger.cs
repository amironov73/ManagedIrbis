// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TraceLogger.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC || DESKTOP || NETCORE

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using SysTrace=System.Diagnostics.Trace;

#endregion

namespace AM.Logging
{
    /// <summary>
    /// <see cref="IAmLogger"/> that uses
    /// <see cref="System.Diagnostics.Trace.WriteLine(string)"/>
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [ExcludeFromCodeCoverage]
    public sealed class TraceLogger
        : IAmLogger
    {
        #region IAmLogger members

        /// <inheritdoc cref="IAmLogger.Debug" />
        public void Debug
        (
            string text
        )
        {
            if (!string.IsNullOrEmpty(text))
            {
                SysTrace.WriteLine(text, "Debug");
            }
        }

        /// <inheritdoc cref="IAmLogger.Error" />
        public void Error
        (
            string text
        )
        {
            if (!string.IsNullOrEmpty(text))
            {
                SysTrace.WriteLine(text, "Error");
            }
        }

        /// <inheritdoc cref="IAmLogger.Fatal" />
        public void Fatal
        (
            string text
        )
        {
            if (!string.IsNullOrEmpty(text))
            {
                SysTrace.WriteLine(text, "Fatal");
            }
        }

        /// <inheritdoc cref="IAmLogger.Info" />
        public void Info
        (
            string text
        )
        {
            if (!string.IsNullOrEmpty(text))
            {
                SysTrace.WriteLine(text, "Info");
            }
        }

        /// <inheritdoc cref="IAmLogger.Trace" />
        public void Trace
        (
            string text
        )
        {
            if (!string.IsNullOrEmpty(text))
            {
                SysTrace.WriteLine(text, "Trace");
            }
        }

        /// <inheritdoc cref="IAmLogger.Warn" />
        public void Warn
        (
            string text
        )
        {
            if (!string.IsNullOrEmpty(text))
            {
                SysTrace.WriteLine(text, "Warn");
            }
        }

        #endregion
    }
}

#endif