// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InMemoryCollector.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Performance
{
    /// <summary>
    /// Сборщик данных о производительности.
    /// Результат хранится в оперативной памяти.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class InMemoryCollector
        : PerformanceCollector
    {
        #region Events

        /// <summary>
        /// Raised on record addition.
        /// </summary>
        public event EventHandler RecordAdded;

        #endregion

        #region Properties

        /// <summary>
        /// Limit.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// List of collected records.
        /// </summary>
        [NotNull]
        public IList<PerfRecord> List { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public InMemoryCollector()
        {
            List = new List<PerfRecord>();
            _syncRoot = new object();
        }

        #endregion

        #region Private members

        private readonly object _syncRoot;

        #endregion

        #region PerformanceCollector members

        /// <inheritdoc cref="PerformanceCollector.Collect" />
        public override void Collect
            (
                PerfRecord record
            )
        {
            Code.NotNull(record, "record");

            lock (_syncRoot)
            {
                List.Add(record);

                int limit = Limit;
                if (limit > 0)
                {
                    while (List.Count > limit)
                    {
                        List.RemoveAt(0);
                    }
                }
            }

            RecordAdded.Raise(this);
        }

        #endregion
    }
}
