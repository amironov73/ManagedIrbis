// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PerformanceCollector.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Performance
{
    /// <summary>
    /// Сборщик данных о производительности.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PerformanceCollector
        : IDisposable
    {
        #region Public methods

        /// <summary>
        /// Collect the record.
        /// </summary>
        public virtual void Collect
            (
                [NotNull] PerfRecord record
            )
        {
            Code.NotNull(record, "record");

            // Nothing to do here
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public virtual void Dispose()
        {
            // Nothing to do here
        }

        #endregion
    }
}
