// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EventedTextBox.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Windows.Forms;

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
    public sealed class EventedTextBox
        : TextBox
    {
        #region Events

        /// <summary>
        /// Delayed version of TextChanged event.
        /// </summary>
        public event EventHandler DelayedTextChanged;

        /// <summary>
        /// Fired when Enter key pressed.
        /// </summary>
        public event EventHandler EnterPressed;

        #endregion

        #region Properties

        /// <summary>
        /// Delay, milliseconds.
        /// </summary>
        public int Delay
        {
            get { return _delay; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException
                        (
                            "value"
                        );
                }

                _timer.Enabled = false;
                _delay = value;
                _count = 0;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public EventedTextBox()
        {
            _delay = 750;
            _timer = new Timer
            {
                Interval = 50
            };
            _timer.Tick += _timer_Tick;
        }

        #endregion

        #region Private members

        private int _delay;

        private int _count;

        private Timer _timer;

        /// <inheritdoc />
        protected override void Dispose
        (
            bool disposing
        )
        {
            base.Dispose(disposing);

            if (!ReferenceEquals(_timer, null))
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        /// <inheritdoc />
        protected override void OnKeyDown
            (
                KeyEventArgs e
            )
        {
            base.OnKeyDown(e);

            if (e.KeyData == Keys.Enter)
            {
                _timer.Enabled = false;
                _count = 0;

                EnterPressed.Raise
                    (
                        this,
                        EventArgs.Empty
                    );
            }
        }

        /// <inheritdoc />
        protected override void OnTextChanged
            (
                EventArgs e
            )
        {
            _count = 0;
            _timer.Enabled = true;
        }

        private void _timer_Tick
            (
                object sender,
                EventArgs e
            )
        {
            _count += _timer.Interval;

            if (_count >= _delay)
            {
                _timer.Enabled = false;
                _count = 0;

                DelayedTextChanged.Raise
                    (
                        this,
                        EventArgs.Empty
                    );
            }
        }

        #endregion
    }
}
