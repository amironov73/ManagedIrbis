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
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolboxBitmap(typeof(BusyStripe),
        "Images.BusyStripe.bmp")]
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

        #endregion

        #region Control members

        /// <summary>
        /// Releases the unmanaged resources used by the 
        /// <see cref="T:System.Windows.Forms.Control"/>
        /// and its child controls and optionally releases the 
        /// managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed 
        /// and unmanaged resources; false to release only 
        /// unmanaged resources. </param>
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

        /// <inheritdoc />
        protected override void OnPaint
            (
                PaintEventArgs e
            )
        {
            Graphics g = e.Graphics;
            Rectangle r = ClientRectangle;
            r.X -= 2;
            r.Width += 4;
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
