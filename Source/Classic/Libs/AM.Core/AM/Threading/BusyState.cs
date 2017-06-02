// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
using System.Threading.Tasks;

using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Threading.Tasks;

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
        : IHandmadeSerializable
    {
        #region Events

        /// <summary>
        /// Raised when the state has changed.
        /// </summary>
        public event EventHandler StateChanged;

        #endregion

        #region Properties

        /// <summary>
        /// The state itself.
        /// </summary>
        public bool Busy { get { return _currentState; } }

        /// <summary>
        /// Whether to use asynchronous event handler.
        /// </summary>
        public bool UseAsync { get; set; }

        /// <summary>
        /// Хэндл для ожидания.
        /// </summary>
        public WaitHandle WaitHandle
        {
            get
            {
                // coverity[missing_lock]
                return _waitHandle;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BusyState()
        {
            Log.Trace("BusyState::Constructor");

            _lock = new object();
            _waitHandle = new ManualResetEvent(true);
        }

        /// <summary>
        /// Constructor.
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
        /// Run some code.
        /// </summary>
        public void Run
            (
                [NotNull] Action action
            )
        {
            Code.NotNull(action, "action");

            WaitAndGrab();

            try
            {
                action();
            }
            finally
            {
                SetState(false);
            }
        }

        /// <summary>
        /// Run some code in asychronous manner.
        /// </summary>
        public Task RunAsync
            (
                [NotNull] Action action
            )
        {
            Code.NotNull(action, "action");

            Task result = Task.Factory.StartNew
                (
                    () => Run(action)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Change the state.
        /// </summary>
        public void SetState
            (
                bool newState
            )
        {
            Log.Trace
                (
                    "BusyState::SetState: newState="
                    + newState
                );

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
                        goto DONE;
                    }

                    WaitHandle.WaitOne();
                }
            }

            DONE:
            Log.Trace("BusyState::WaitAndGrab: return");
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
                    goto DONE;
                }

                WaitHandle.WaitOne();
            }

            DONE:
            Log.Trace("BusyState::WaitFreeState: return");
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

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            _currentState = reader.ReadBoolean();
            UseAsync = reader.ReadBoolean();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
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

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format("Busy: {0}", Busy);
        }

        #endregion
    }
}
