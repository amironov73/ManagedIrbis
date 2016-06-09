/* BusyState.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Threading
{
    /// <summary>
    /// Индикатор занятости.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Busy = {Busy}")]
    public sealed class BusyState
    {
        #region Events

        /// <summary>
        /// Вызывается, когда меняется состояние.
        /// </summary>
        public event EventHandler StateChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Состояние
        /// </summary>
        public bool Busy { get { return _currentState; } }

        /// <summary>
        /// Использовать асинхронный обработчик события.
        /// </summary>
        public bool UseAsync { get; set; }

        /// <summary>
        /// Хэндл для ожидания.
        /// </summary>
        public WaitHandle WaitHandle { get { return _waitHandle; } }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BusyState()
        {
            _waitHandle = new ManualResetEvent(true);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BusyState
            (
                bool initialState
            )
            : this ()
        {
            _currentState = initialState;
        }

        #endregion

        #region Private members

        private bool _currentState;

        private ManualResetEvent _waitHandle;

        #endregion

        #region Public methods

        /// <summary>
        /// Смена состояния.
        /// </summary>
        public void SetState
            (
                bool newState
            )
        {
            if (newState != _currentState)
            {
                if (newState)
                {
                    _waitHandle.Reset();
                }
                else
                {
                    _waitHandle.Set();
                }

                _currentState = newState;

                if (UseAsync)
                {
                    StateChanged.RaiseAsync(this);
                }
                else
                {
                    StateChanged.Raise(this);
                }
            }
        }

        /// <summary>
        /// Ожидаем, пока не освободится.
        /// </summary>
        public void WaitFreeState()
        {
            while (true)
            {
                if (!Busy)
                {
                    return;
                }

                WaitHandle.WaitOne();
            }
        }

        /// <summary>
        /// Ожидаем, пока не освободится.
        /// </summary>
        public bool WaitFreeState
            (
                TimeSpan timeout
            )
        {
            if (!Busy)
            {
                return true;
            }
            return WaitHandle.WaitOne(timeout);
        }

        public static implicit operator bool
            (
                [NotNull] BusyState state
            )
        {
            return state.Busy;
        }

        public static implicit operator BusyState
            (
                bool value
            )
        {
            return new BusyState(value);
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("Busy: {0}", Busy);
        }

        #endregion
    }
}
