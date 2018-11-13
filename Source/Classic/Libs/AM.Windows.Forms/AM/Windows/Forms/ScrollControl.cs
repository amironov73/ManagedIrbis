// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ScrollControl.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using RC = AM.Windows.Forms.Properties.Resources;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Simplest scroll control.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ScrollControl
        : Control
    {
        #region Events

        /// <summary>
        /// Raised on scroll.
        /// </summary>
        public event ScrollEventHandler Scroll;

        #endregion

        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScrollControl()
        {
            SetStyle
                (
                    ControlStyles.AllPaintingInWmPaint
                    | ControlStyles.UserPaint
                    | ControlStyles.StandardClick,
                    true
                );
            SetStyle
                (
                    ControlStyles.Selectable,
                    false
                );
            _timer = new Timer
            {
                Interval = 90,
                Enabled = true
            };
            _timer.Tick += _timer_Tick;
        }

        #endregion

        #region Private members

        private readonly Timer _timer;

        private bool _pressed;

        private ScrollEventArgs _eventArgs;

        private void _timer_Tick
            (
                object sender,
                EventArgs e
            )
        {
            if (_pressed)
            {
                ScrollEventHandler handler = Scroll;
                if (!ReferenceEquals(handler, null))
                {
                    handler(this, _eventArgs);
                }
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region Control members

        /// <inheritdoc cref="Control.DefaultMaximumSize" />
        protected override Size DefaultMaximumSize
        {
            get { return new Size(16, 0); }
        }

        /// <inheritdoc cref="Control.DefaultMinimumSize" />
        protected override Size DefaultMinimumSize
        {
            get { return new Size(16, 0); }
        }

        /// <inheritdoc cref="Control.DefaultSize" />
        protected override Size DefaultSize
        {
            get { return new Size(16, 200); }
        }

        /// <inheritdoc cref="Control.OnMouseDown" />
        protected override void OnMouseDown
            (
                MouseEventArgs e
            )
        {
            base.OnMouseDown(e);

            if (_pressed)
            {
                return;
            }

            ScrollEventType type = e.Y > Height / 2
                ? ScrollEventType.LargeIncrement
                : ScrollEventType.LargeDecrement;

            if (e.Y < 16)
            {
                type = ScrollEventType.SmallDecrement;
            }
            else if (e.Y > (Height - 16))
            {
                type = ScrollEventType.SmallIncrement;
            }

            _eventArgs = new ScrollEventArgs
                (
                    type,
                    0
                );

            ScrollEventHandler handler = Scroll;
            if (!ReferenceEquals(handler, null))
            {
                _pressed = true;
                handler(this, _eventArgs);
            }
        }

        /// <inheritdoc cref="Control.OnMouseUp"/>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _pressed = false;
        }

        /// <inheritdoc cref="Control.OnMouseLeave"/>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _pressed = false;
        }

        /// <inheritdoc cref="Control.OnPaint" />
        protected override void OnPaint
            (
                PaintEventArgs e
            )
        {
            Graphics graphics = e.Graphics;

            using (Brush brush = new SolidBrush(BackColor))
            {
                graphics.FillRectangle
                    (
                        brush,
                        e.ClipRectangle
                    );
            }

            using (Image upArrow = RC.arrowUp)
            {
                graphics.DrawImage
                (
                    upArrow,
                    0,
                    0,
                    16,
                    16
                );
            }

            using (Image downArrow = RC.arrowDown)
            {
                graphics.DrawImage
                (
                    downArrow,
                    0,
                    Height - downArrow.Height,
                    16,
                    16
                );
            }

            using (Pen pen = new Pen(ForeColor, 0.5f))
            {
                graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
                graphics.DrawRectangle(pen, 0, 0, Width, 16);
                graphics.DrawRectangle(pen, 0, Height-15, Width, 16);
            }
        }

        /// <inheritdoc cref="Control.Dispose" />
        protected override void Dispose
            (
                bool disposing
            )
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _timer.Dispose();
            }
        }

        #endregion
    }
}
