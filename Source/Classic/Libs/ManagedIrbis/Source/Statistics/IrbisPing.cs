// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisPing.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Statistics
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisPing
        : IDisposable
    {
        #region Events

        /// <summary>
        /// Raised when the <see cref="Statistics"/> is updated.
        /// </summary>
        public event EventHandler StatisticsUpdated;

        #endregion

        #region Properties

        /// <summary>
        /// Whether the <see cref="IrbisPing"/> is active?
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Statistics.
        /// </summary>
        [NotNull]
        public PingStatistics Statistics { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisPing
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
            Statistics = new PingStatistics();
            _timer = new Timer(1000)
            {
                AutoReset = true,
                Enabled = true
            };
            _timer.Elapsed += _timer_Elapsed;
        }

        #endregion

        #region Private members

        private readonly Timer _timer;

        private bool _busy;

        private void _timer_Elapsed
            (
                object sender,
                ElapsedEventArgs e
            )
        {
            if (!Active
                || _busy
                || Connection.Busy)
            {
                return;
            }

            _busy = true;
            try
            {
                PingData ping = PingOnce();
                Statistics.Add(ping);
                StatisticsUpdated.Raise(this);
            }
            finally
            {
                _busy = false;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Ping once.
        /// </summary>
        public PingData PingOnce()
        {
            PingData result = new PingData
            {
                Moment = DateTime.Now
            };

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Connection.NoOp();

                stopwatch.Stop();
                unchecked
                {
                    result.RoundTripTime
                        = (int) stopwatch.ElapsedMilliseconds;
                }
            }
            catch
            {
                result.Success = false;
            }

            return result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _timer.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}
