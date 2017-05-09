// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BusyGuard.cs --
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

namespace AM.Threading
{
    /// <summary>
    /// Обёртка для ожидания и освобождения <see cref="BusyState"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BusyGuard
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// State.
        /// </summary>
        [NotNull]
        public BusyState State { get { return _state; } }

        /// <summary>
        /// Timeout.
        /// </summary>
        public TimeSpan Timeout { get { return _timeout; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BusyGuard
            (
                [NotNull] BusyState state
            )
        {
            Code.NotNull(state, "state");

            _state = state;
            _timeout = TimeSpan.Zero;

            _Grab();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BusyGuard
            (
                [NotNull] BusyState state,
                TimeSpan timeout
            )
        {
            Code.NotNull(state, "state");

            _state = state;
            _timeout = timeout;

            _Grab();
        }

        #endregion

        #region Private members

        private readonly BusyState _state;

        private readonly TimeSpan _timeout;

        private void _Grab()
        {
            if (Timeout.IsZeroOrLess())
            {
                State.WaitAndGrab();
            }
            else
            {
                if (!State.WaitAndGrab(Timeout))
                {
                    throw new TimeoutException();
                }
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            State.SetState(false);
        }

        #endregion
    }
}
