/* IrbisIdleManager.cs -- sends NOP command to the server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// Sends NOP command to the server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisIdleManager
        : IDisposable
    {
        #region Events

        /// <summary>
        /// Raised when connection is idle.
        /// </summary>
        public event EventHandler Idle;

        #endregion

        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Idle interval, milliseconds.
        /// </summary>
        public int Interval { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisIdleManager
            (
                [NotNull] IrbisConnection connection,
                int interval
            )
        {
            Code.NotNull(connection, "connection");
            if (interval < 10)
            {
                throw new ArgumentOutOfRangeException("interval");
            }

            Connection = connection;
            Interval = interval;

            Connection.Disposing += Connection_Disposing;
            _timer = new Timer
            {
                Interval = interval,
                Enabled = true
            };
            _timer.Tick += _timer_Tick;
            _lock = new object();
        }

        #endregion

        #region Private members

        private readonly Timer _timer;
        private readonly object _lock;

        private void Connection_Disposing
            (
                object sender,
                EventArgs e
            )
        {
            Dispose();
        }

        private void _timer_Tick
            (
                object sender,
                EventArgs e
            )
        {
            lock (_lock)
            {
                if (Connection.Connected
                    && !Connection.Busy)
                {
                    Connection.NoOp();

                    Idle.Raise(this);
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Manually raise <see cref="Idle"/> event.
        /// </summary>
        public void Raise()
        {
            _timer_Tick(this, EventArgs.Empty);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc />
        public void Dispose()
        {
            _timer.Dispose();
        }

        #endregion
    }
}
