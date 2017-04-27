// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConnectionStatisticsService.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Statistics
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ConnectionStatisticsService
        : IConnectionStatisticsService,
        IDisposable
    {
        #region Properties

        /// <summary>
        /// Accumulated statistics.
        /// </summary>
        [NotNull]
        public ConnectionStatistics Statistics { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConnectionStatisticsService()
        {
            Log.Trace("ConnectionStatisticsService::Constructor");

            Statistics = new ConnectionStatistics();
        }

        #endregion

        #region IConnectionStatisticsService members

        /// <inheritdoc cref="IConnectionStatisticsService.AddEvent"/>
        public void AddEvent
            (
                double duration
            )
        {
            Code.Positive(duration, "duration");

            Statistics.Count++;
            Statistics.Total += duration;

            if (Statistics.Count == 1)
            {
                Statistics.Minimum = duration;
                Statistics.Maximum = duration;
            }
            else
            {
                Statistics.Minimum = Math.Min
                    (
                        Statistics.Minimum,
                        duration
                    );
                Statistics.Maximum = Math.Max
                    (
                        Statistics.Maximum,
                        duration
                    );
            }
        }

        /// <inheritdoc cref="IConnectionStatisticsService.Clear"/>
        public void Clear()
        {
            Statistics.Count = 0;
            Statistics.Maximum = 0.0;
            Statistics.Minimum = 0.0;
            Statistics.Total = 0.0;
        }

        /// <inheritdoc cref="IConnectionStatisticsService.GetCumulatedStatistics"/>
        public ConnectionStatistics GetCumulatedStatistics()
        {
            return Statistics;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Log.Trace("ConnectionStatisticsService::Dispose");
        }

        #endregion
    }
}
