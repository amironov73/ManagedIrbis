// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TraceLogger.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using SysTrace=System.Diagnostics.Trace;

#endregion

namespace UnsafeAM.Logging
{
    /// <summary>
    /// <see cref="IAmLogger"/> that uses
    /// <see cref="System.Diagnostics.Trace.WriteLine(string)"/>
    /// </summary>
    [PublicAPI]
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
