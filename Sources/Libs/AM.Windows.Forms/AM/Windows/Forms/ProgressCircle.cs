/* ProgressCircle.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using AM.Drawing;

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
    public class ProgressCircle
        : Control
    {
        #region Properties

        private Color _fillColor = Color.White;

        /// <summary>
        /// Gets or sets the color of the fill.
        /// </summary>
        /// <value>The color of the fill.</value>
        [DefaultValue(typeof(Color), "White")]
        public Color FillColor
        {
            [DebuggerStepThrough]
            get
            {
                return _fillColor;
            }
            [DebuggerStepThrough]
            set
            {
                _fillColor = value;
                Invalidate();
            }
        }

        private Color _doneColor = Color.Blue;

        /// <summary>
        /// Gets or sets the color of the done.
        /// </summary>
        /// <value>The color of the done.</value>
        [DefaultValue(typeof(Color), "Blue")]
        public Color DoneColor
        {
            [DebuggerStepThrough]
            get
            {
                return _doneColor;
            }
            [DebuggerStepThrough]
            set
            {
                _doneColor = value;
                Invalidate();
            }
        }

        private float _percent;

        /// <summary>
        /// Gets or sets the percent.
        /// </summary>
        /// <value>The percent.</value>
        [DefaultValue(0f)]
        public float Percent
        {
            [DebuggerStepThrough]
            get
            {
                return _percent;
            }
            [DebuggerStepThrough]
            set
            {
                if ((value < 0)
                     || (value > 100))
                {
                    throw new ArgumentException();
                }
                _percent = value;
                Invalidate();
            }
        }

        private bool _square = true;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ProgressCircle"/> is square.
        /// </summary>
        /// <value><c>true</c> if square; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool Square
        {
            [DebuggerStepThrough]
            get
            {
                return _square;
            }
            [DebuggerStepThrough]
            set
            {
                _square = value;
            }
        }

        private bool _drawValue = true;

        /// <summary>
        /// Gets or sets a value indicating whether [draw value].
        /// </summary>
        /// <value><c>true</c> if [draw value]; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool DrawValue
        {
            [DebuggerStepThrough]
            get
            {
                return _drawValue;
            }
            [DebuggerStepThrough]
            set
            {
                _drawValue = value;
                Invalidate();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressCircle"/> class.
        /// </summary>
        public ProgressCircle()
        {
            ResizeRedraw = true;
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        #endregion

        #region Control members

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            Rectangle r = ClientRectangle;
            r.Width--;
            r.Height--;
            Rectangle r2 = r;
            r2.Inflate(-2, -2);
            string s = string.Format("{0:0}", _percent);
            SizeF size = g.MeasureString(s, Font);
            size.Width = size.Width * 5 / 3;
            size.Height = size.Height * 5 / 3;
            RectangleF inner = new RectangleF
                (
                r.Left + r.Width / 2 - size.Width / 2,
                r.Top + r.Height / 2 - size.Height / 2,
                size.Width,
                size.Height
                );
            using (Pen pen = new Pen(ForeColor, 0))
            {
                using (Brush fillBrush = new SolidBrush(FillColor))
                {
                    using (Brush doneBrush = new SolidBrush(DoneColor))
                    {
                        using (Brush textBrush = new SolidBrush(ForeColor))
                        {
                            g.FillEllipse(fillBrush, r);
                            g.FillPie(doneBrush, r2, 270, 360 * _percent / 100);
                            if (_drawValue)
                            {
                                g.FillEllipse(fillBrush, inner);
                            }
                            for (float i = 0; i < 360; i += 20)
                            {
                                g.FillPie(fillBrush, r2, i, 4);
                            }
                            if (_drawValue)
                            {
                                g.DrawString(s, Font, textBrush, inner, TextFormat.CenterCenter);
                            }
                            g.DrawEllipse(pen, r);
                        }
                    }
                }
            }
            base.OnPaint(e);
        }

        /// <summary>
        /// Performs the work of setting the specified bounds of this control.
        /// </summary>
        /// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left"></see> property value of the control.</param>
        /// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top"></see> property value of the control.</param>
        /// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width"></see> property value of the control.</param>
        /// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height"></see> property value of the control.</param>
        /// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified"></see> values.</param>
        protected override void SetBoundsCore
            (int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (_square)
            {
                int size = Math.Min(width, height);
                base.SetBoundsCore(x, y, size, size, specified);
            }
            else
            {
                base.SetBoundsCore(x, y, width, height, specified);
            }
        }

        #endregion
    }
}