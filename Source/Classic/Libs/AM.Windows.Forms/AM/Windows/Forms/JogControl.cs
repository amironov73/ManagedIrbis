// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* JogControl.cs --
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

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategoryAttribute("Code")]
    public sealed class JogControl
        : Control
    {
        #region Events

        /// <summary>
        /// Raised when <see cref="Angle"/> changed.
        /// </summary>
        public event EventHandler AngleChanged;

        #endregion

        #region Properties

        private float _angle;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(0f)]
        public float Angle
        {
            [DebuggerStepThrough]
            get
            {
                return _angle;
            }
            [DebuggerStepThrough]
            set
            {
                _angle = value;
                while (_angle < 0)
                {
                    _angle += 360;
                }
                while (_angle > 360)
                {
                    _angle -= 360;
                }

                AngleChanged.Raise(this, EventArgs.Empty);

                Invalidate();
            }
        }

        private const string CoolColorName = "Black";
        private Color _coolColor;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(typeof(Color), CoolColorName)]
        public Color CoolColor
        {
            [DebuggerStepThrough]
            get
            {
                return _coolColor;
            }
            [DebuggerStepThrough]
            set
            {
                _coolColor = value;
                Invalidate();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public JogControl()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            ColorConverter cc = new ColorConverter();
            _coolColor = (Color)cc.ConvertFromString(CoolColorName);
        }

        #endregion

        #region Control members

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        protected override void OnPaint
            (
                PaintEventArgs e
            )
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            Rectangle r = ClientRectangle;
            r.Width--;
            r.Height--;
            //Color coolColor = Color.FromArgb ( 60, 20, 20 );
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(r);
                using (PathGradientBrush brush =
                    new PathGradientBrush(path))
                {
                    brush.CenterColor = Color.LightGray;
                    brush.CenterPoint = new PointF(r.Width / 4,
                                                     r.Height / 4);
                    brush.SurroundColors = new Color[]
                        {
                            _coolColor
                        };
                    g.FillEllipse(brush, r);
                }
                g.DrawPath(Pens.Black, path);
            }
            r.Width /= 2;
            r.Height /= 2;
            g.TranslateTransform(r.Width, r.Height);
            GraphicsState state = g.Save();
            g.RotateTransform(Angle);
            for (float a = 0f; a < 360f; a += 10f)
            {
                g.RotateTransform(10f);
                g.DrawLine(Pens.Gray,
                             0,
                             r.Height,
                             0,
                             r.Height * 9 / 10);
            }
            g.Restore(state);
            int x = (int)(r.Width
                            * Math.Sin(Math.PI * _angle / 180f) * 0.8);
            int y = (int)(-(double)r.Height
                            * Math.Cos(Math.PI * _angle / 180f) * 0.8);
            int x2 = (int)(r.Width
                             * Math.Sin(Math.PI * _angle / 180f) * 0.5);
            int y2 = (int)(-(double)r.Height
                             * Math.Cos(Math.PI * _angle / 180f) * 0.5);
            int dx = r.Width / 3;
            int dy = r.Height / 3;
            Rectangle r2 = new Rectangle(x - dx / 2, y - dy / 2, dx, dy);
            Rectangle r3 = new Rectangle(-r.Width, -r.Height, Width, Height);
            using (LinearGradientBrush brush = new LinearGradientBrush(
                r2, _coolColor, Color.LightGray, 45f))
            {
                using (LinearGradientBrush brush2 = new LinearGradientBrush(
                    r3, _coolColor, Color.LightGray, 45f))
                {
                    using (Pen pen = new Pen(brush2, dx / 3))
                    {
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;
                        g.DrawLine(pen, x2, y2, 0, 0);
                        g.FillEllipse(brush, r2);
                    }
                }
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Capture = true;
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            Capture = false;
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Capture)
            {
                float x = e.X - (float)Width / 2;
                float y = e.Y - (float)Height / 2;
                if (x == 0) //-V3024
                {
                }
                else
                {
                    Angle = (float)(90f + 180f * Math.Atan2(y, x) / Math.PI);
                }
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event.
        /// </summary>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            float delta = e.Delta / 30f;
            Angle -= delta;
            base.OnMouseWheel(e);
        }

        /// <summary>
        /// Performs the work of setting the specified bounds
        /// of this control.
        /// </summary>
        protected override void SetBoundsCore
            (
                int x,
                int y,
                int width,
                int height,
                BoundsSpecified specified
            )
        {
            int max = Math.Max(width, height);
            base.SetBoundsCore(x, y, max, max, specified);
        }

        #endregion
    }
}
