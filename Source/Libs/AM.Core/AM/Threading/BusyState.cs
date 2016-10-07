/* BusyState.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

using AM.IO;
using AM.Runtime;

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
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("Busy = {Busy}")]
#endif
    public sealed class BusyState
        : IHandmadeSerializable
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
        public WaitHandle WaitHandle
        {
            get { return _waitHandle; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BusyState()
        {
            _lock = new object();
            _waitHandle = new ManualResetEvent(true);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BusyState
            (
                bool initialState
            )
            : this()
        {
            _currentState = initialState;
        }

        #endregion

        #region Private members

        private readonly object _lock;

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
            lock (_lock)
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
#if WINMOBILE || PocketPC

                        StateChanged.Raise(this);
#else

                        StateChanged.RaiseAsync(this);

#endif
                    }
                    else
                    {
                        StateChanged.Raise(this);
                    }
                }
            }
        }

        /// <summary>
        /// Ожидаем, пока не освободится, и захватываем.
        /// </summary>
        public void WaitAndGrab()
        {
            lock (_lock)
            {
                while (true)
                {
                    if (!Busy)
                    {
                        SetState(true);
                        return;
                    }

                    WaitHandle.WaitOne();
                }
            }
        }

        /// <summary>
        /// Ожидаем, пока не освободится, затем захватываем.
        /// </summary>
        public bool WaitAndGrab
            (
                TimeSpan timeout
            )
        {
            lock (_lock)
            {
                if (!Busy)
                {
                    SetState(true);
                    return true;
                }

                bool result;

#if WINMOBILE || PocketPC

                result = WaitHandle.WaitOne
                    (
                        (int) timeout.TotalMilliseconds,
                        false
                    );

#else

                result = WaitHandle.WaitOne(timeout);

#endif

                if (result)
                {
                    SetState(true);
                }

                return result;
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

#if WINMOBILE || PocketPC

            return WaitHandle.WaitOne
                (
                    (int) timeout.TotalMilliseconds,
                    false
                );

#else

            return WaitHandle.WaitOne(timeout);

#endif
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator bool
            (
                [NotNull] BusyState state
            )
        {
            return state.Busy;
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator BusyState
            (
                bool value
            )
        {
            return new BusyState(value);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            _currentState = reader.ReadBoolean();
            UseAsync = reader.ReadBoolean();
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.Write(_currentState);
            writer.Write(UseAsync);
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
