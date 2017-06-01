// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BusyStripe.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

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
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolboxBitmap(typeof(BusyStripe), "Images.BusyStripe.bmp")]
    public class BusyStripe
        : Control
    {
        #region Properties

        private bool _moving;

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="BusyStripe"/> is moving.
        /// </summary>
        /// <value><c>true</c> if moving; otherwise,
        /// <c>false</c>.</value>
        [System.ComponentModel.Category("Behavior")]
        public bool Moving
        {
            [DebuggerStepThrough]
            get
            {
                return _moving;
            }
            [DebuggerStepThrough]
            set
            {
                _moving = value;
                _timer.Enabled = true;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BusyStripe()
        {
            ResizeRedraw = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            _timer = new Timer
            {
                Interval = 100
            };
            _timer.Tick += _timer_Tick;
        }

        #endregion

        #region Private members

        private Timer _timer;
        private float _position;
        private float _speed = 0.05f;
        private bool _back;

        private void _timer_Tick
            (
                object sender,
                EventArgs e
            )
        {
            if (Moving && Visible && !DesignMode)
            {
                _position += _speed;
                if (_position >= 0.95f)
                {
                    _back = !_back;
                    _position = 0.0f;
                }
                Invalidate();
            }
        }

        private void Busy_StateChanged
            (
                object sender,
                EventArgs e
            )
        {
            BusyState state = (BusyState)sender;

            this.InvokeIfRequired
            (
                () =>
                {
                    Moving = state;
                    Invalidate();
                }
            );
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Subscribe.
        /// </summary>
        public void SubscribeTo
            (
                [NotNull] BusyState busyState
            )
        {
            Code.NotNull(busyState, "busyState");

            busyState.StateChanged += Busy_StateChanged;
        }

        /// <summary>
        /// Unsubscribe.
        /// </summary>
        public void UnsubscribeFrom
            (
                [NotNull] BusyState busyState
            )
        {
            Code.NotNull(busyState, "busyState");

            busyState.StateChanged -= Busy_StateChanged;
        }

        #endregion

        #region Control members

        /// <inheritdoc cref="Control.Dispose(bool)" />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <inheritdoc cref="Control.OnPaint" />
        protected override void OnPaint
            (
                PaintEventArgs e
            )
        {
            Graphics g = e.Graphics;
            Rectangle r = ClientRectangle;
            r.X -= 2;
            r.Width += 4;

            if (Moving)
            {
                using (LinearGradientBrush brush
                    = new LinearGradientBrush
                    (
                        r,
                        BackColor,
                        ForeColor,
                        _back ? 0 : 180
                    ))
                {
                    brush.SetBlendTriangularShape(_position, 0.5f);
                    g.FillRectangle(brush, ClientRectangle);
                }
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(BackColor))
                {
                    g.FillRectangle(brush, ClientRectangle);
                }
            }

            if (!string.IsNullOrEmpty(Text))
            {
                using (Brush brush = new SolidBrush(ForeColor))
                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    g.DrawString
                        (
                            Text,
                            Font,
                            brush,
                            ClientRectangle,
                            format
                        );
                }
            }
            base.OnPaint(e);
        }

        #endregion
    }
}
