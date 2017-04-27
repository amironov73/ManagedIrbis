// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IConnectionStatisticsService.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

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

namespace ManagedIrbis.Infrastructure.Statistics
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConnectionStatisticsService
    {
        /// <summary>
        /// Add event.
        /// </summary>
        void AddEvent(double duration);

        /// <summary>
        /// Clear.
        /// </summary>
        void Clear();

        /// <summary>
        /// Get cumulated statistics.
        /// </summary>
        [NotNull]
        ConnectionStatistics GetCumulatedStatistics();
    }
}
