// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TeeMonitoringSink.cs -- sink delegating to collection of sink
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Collections;
using AM.Logging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Monitoring
{
    /// <summary>
    /// Sink delegating to collection of sink.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TeeMonitoringSink
        : MonitoringSink
    {
        #region Properties

        /// <summary>
        /// Collection of sinks.
        /// </summary>
        [NotNull]
        public NonNullCollection<MonitoringSink> Sinks { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TeeMonitoringSink()
        {
            Sinks = new NonNullCollection<MonitoringSink>();
        }

        #endregion

        #region MonitoringSink members

        /// <inheritdoc cref="MonitoringSink.WriteData" />
        public override bool WriteData
            (
                MonitoringData data
            )
        {
            bool result = true;

            foreach (MonitoringSink sink in Sinks)
            {
                try
                {
                    if (!sink.WriteData(data))
                    {
                        result = false;
                        break;
                    }
                }
                catch (Exception exception)
                {
                    Log.TraceException
                        (
                            "TeeMonitoringSink::WriteData",
                            exception
                        );
                }
            }

            return result;
        }

        #endregion
    }
}
