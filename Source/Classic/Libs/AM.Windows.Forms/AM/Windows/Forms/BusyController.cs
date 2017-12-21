// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BusyController.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW45

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM.Collections;
using AM.Logging;
using AM.Text.Output;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class BusyController
        : Component
    {
        #region Events

        /// <summary>
        /// Raised when exception occur.
        /// </summary>
        public event EventHandler<ExceptionEventArgs> ExceptionOccur;

        /// <summary>
        /// Raised when state changed.
        /// </summary>
        public event EventHandler<BusyStateEventArgs> StateChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Control collection.
        /// </summary>
        public NonNullCollection<Control> Controls { get; private set; }

        /// <summary>
        /// For error messages.
        /// </summary>
        [CanBeNull]
        public AbstractOutput Output { get; set; }

        /// <summary>
        /// State.
        /// </summary>
        [CanBeNull]
        public BusyState State
        {
            get { return _state; }
            set { SetupState(value); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BusyController()
        {
            Controls = new NonNullCollection<Control>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BusyController
            (
                [CanBeNull] BusyState state
            )
        {
            State = state;
            Controls = new NonNullCollection<Control>();
        }

        #endregion

        #region Private members

        private BusyState _state;

        /// <summary>
        /// Initialize <see cref="State"/> with specified value.
        /// </summary>
        protected void SetupState
            (
                [CanBeNull] BusyState state
            )
        {
            if (!ReferenceEquals(_state, null))
            {
                _state.StateChanged -= _StateChanged;
            }

            _state = state;
            if (!ReferenceEquals(state, null))
            {
                state.StateChanged += _StateChanged;
            }
        }

        private void _StateChanged
            (
                object sender,
                EventArgs e
            )
        {
            OnStateChanged();
        }

        /// <summary>
        /// Raises <see cref="ExceptionOccur"/> event.
        /// </summary>
        protected virtual void OnExceptionOccur
            (
                [NotNull] Exception exception
            )
        {
            Code.NotNull(exception, "exception");

            ExceptionEventArgs eventArgs
                = new ExceptionEventArgs(exception);
            ExceptionOccur.Raise(this, eventArgs);
        }

        /// <summary>
        /// Raises <see cref="StateChanged"/> event.
        /// </summary>
        protected virtual void OnStateChanged()
        {
            BusyState state = State;
            if (!ReferenceEquals(state, null))
            {
                Log.Trace
                    (
                        "BusyController::OnStateChanged: "
                        + "busy="
                        + state.Busy
                    );

                BusyStateEventArgs eventArgs
                    = new BusyStateEventArgs(state);
                StateChanged.Raise(this, eventArgs);
            }
        }

        /// <summary>
        /// Update control state.
        /// </summary>
        protected void UpdateControlState
            (
                bool enabled
            )
        {
            BusyState state = State;
            if (!ReferenceEquals(state, null))
            {
                Log.Trace
                    (
                        "BusyController::UpdateControlState: "
                        + "enabled="
                        + enabled
                    );

                foreach (Control control in Controls)
                {
                    control.Enabled = enabled;
                }

                Application.DoEvents();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Disable associated controls.
        /// </summary>
        public void DisableControls()
        {
            UpdateControlState(false);
        }

        /// <summary>
        /// Enable associated controls.
        /// </summary>
        public void EnableControls()
        {
            UpdateControlState(true);
        }

        /// <summary>
        /// Run the specified action.
        /// </summary>
        public bool Run
            (
                [NotNull] Action action
            )
        {
            Code.NotNull(action, "action");

            bool result = false;

            BusyState state = State;
            if (!ReferenceEquals(state, null))
            {
                try
                {
                    Log.Trace
                    (
                        "BusyController::Run: "
                        + "before"
                    );

                    UpdateControlState(false);
                    PseudoAsync.Run
                        (
                            () => state.Run(action)
                        );

                    Log.Trace
                    (
                        "BusyController::Run: "
                        + "normal after"
                    );

                    result = true;
                }
                catch (Exception exception)
                {
                    Log.TraceException
                        (
                            "BusyController::Run",
                            exception
                        );
                    WriteLine
                        (
                            "{0}: {1}",
                            exception.GetType().Name,
                            exception.Message
                        );

                    OnExceptionOccur(exception);
                }
                finally
                {
                    UpdateControlState(true);
                }
            }

            return result;
        }

        /// <summary>
        /// Run the specified action.
        /// </summary>
        public async Task<bool> RunAsync
            (
                [NotNull] Action action
            )
        {
            Code.NotNull(action, "action");

            bool result = false;

            BusyState state = State;
            if (!ReferenceEquals(state, null))
            {
                try
                {
                    UpdateControlState(false);
                    await state.RunAsync(action);

                    result = true;
                }
                catch (Exception exception)
                {
                    Exception unwrapped
                        = ExceptionUtility.Unwrap(exception);

                    Log.TraceException
                        (
                            "BusyController::RunAsync",
                            unwrapped
                        );
                    WriteLine
                        (
                            "{0}: {1}",
                            exception.GetType().Name,
                            exception.Message
                        );

                    OnExceptionOccur(unwrapped);
                }
                finally
                {
                    UpdateControlState(true);
                }
            }

            return result;
        }

        /// <summary>
        /// Write message to <see cref="Output"/> if present.
        /// </summary>
        public void WriteLine
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(format, "format");

            AbstractOutput output = Output;
            if (!ReferenceEquals(output, null))
            {
                output.WriteLine(format, args);
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        protected override void Dispose
            (
                bool disposing
            )
        {
            base.Dispose(disposing);

            BusyState state = State;
            if (!ReferenceEquals(state, null))
            {
                state.StateChanged -= _StateChanged;
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}

#endif
