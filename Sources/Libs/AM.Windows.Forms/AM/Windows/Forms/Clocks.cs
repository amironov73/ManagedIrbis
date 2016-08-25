/* Clocks.cs -- simple analog clock control
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Simple analog clock control.
    /// </summary>
    [PublicAPI]
    //[ToolboxBitmap("AM.Windows.Forms.Clocks.bmp")]
    [System.ComponentModel.DesignerCategory("Code")]
    public sealed class Clocks
        : Control
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="Clocks"/> class.
        /// </summary>
        public Clocks()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            Enabled = false;

            _timer = new Timer
            {
                Interval = 500,
                Enabled = true
            };
            _timer.Tick += _timer_Tick;
        }

        #endregion

        #region Private members

        private Timer _timer;

        void _timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        #endregion

        #region Control members

        /// <summary>
        /// Gets the default size of the control.
        /// </summary>
        /// <value></value>
        /// <returns>The default
        /// <see cref="T:System.Drawing.Size"/>
        /// of the control.</returns>
        protected override Size DefaultSize
        {
            get
            {
                return new Size(90, 90);
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the
        /// <see cref="T:System.Windows.Forms.Control"/>
        /// and its child controls and optionally releases
        /// the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both
        /// managed and unmanaged resources; false to release
        /// only unmanaged resources.</param>
        protected override void Dispose
            (
                bool disposing
            )
        {
            base.Dispose(disposing);
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        /// <summary>
        /// Performs the work of setting the specified
        /// bounds of this control.
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
            int min = (width < height) ? width : height;
            base.SetBoundsCore(x, y, min, min, specified);
        }

        /// <summary>
        /// Raises the
        /// <see cref="E:System.Windows.Forms.Control.Paint"/>
        /// event.
        /// </summary>
        protected override void OnPaint
            (
                PaintEventArgs e
            )
        {
            Graphics g = e.Graphics;
            Rectangle r = ClientRectangle;
            r.Width--;
            r.Height--;
            int xx0 = r.Width / 2;
            int xy0 = r.Height / 2;
            double cX = xx0;
            double cY = xy0;
            g.TranslateTransform(xx0, xy0);
            double radius = r.Height / 2 - 4;
            r = new Rectangle(-xx0, -xy0, r.Width, r.Height);
            Rectangle r2 = r;
            int dr = -r.Width / 30;
            if (dr > -3)
            {
                dr = -3;
            }
            r2.Inflate(dr, dr);
            using (Brush brush = new LinearGradientBrush(r,
                Color.LightGray, Color.White, 45))
            {
                g.FillEllipse(brush, r);
            }
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(r2);
                using (PathGradientBrush pgb = new PathGradientBrush(path))
                {
                    pgb.CenterPoint = new PointF(r.Left + r.Width / 4,
                        r.Top + r.Height / 4);
                    pgb.CenterColor = Color.White;
                    pgb.SurroundColors = new Color[] {
                        Color.LightSkyBlue
                    };
                    g.FillEllipse(pgb, r2);
                }
            }
            g.DrawEllipse(Pens.Gray, r);
            if (r.Height > 100)
            {
                g.DrawEllipse(Pens.Gray, r2);
            }

            double fontHeight = r.Height / 14;
            if (fontHeight >= 5)
            {
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                FontFamily ff = new FontFamily(GenericFontFamilies.SansSerif);
                FontStyle fs = (fontHeight >= 12) ?
                    FontStyle.Bold
                    : FontStyle.Regular;
                using (Font f = new Font(ff, (float)fontHeight, fs))
                {
                    for (double h = 1; h < 13; h++)
                    {
                        double hAngle = h * 30 * Math.PI / 180;
                        double dx = System.Math.Sin(hAngle) * radius;
                        double dy = System.Math.Cos(hAngle) * radius;
                        double x0 = +dx;
                        double y0 = -dy;
                        double x1 = +dx * 0.80;
                        double y1 = -dy * 0.80;
                        double x2 = +dx * 0.95;
                        double y2 = -dy * 0.95;
                        g.DrawLine(Pens.Black, (float)x0, (float)y0,
                            (float)x2, (float)y2);
                        g.DrawString(h.ToString("#"), f, Brushes.Blue,
                            (float)x1, (float)y1, format);
                    }
                }
            }

            // Кружок в центре
            int dotRadius = r.Width / 8;
            Rectangle centerDot = new Rectangle
                (
                    -dotRadius / 2,
                    -dotRadius / 2,
                    dotRadius,
                    dotRadius
                );
            Rectangle grayDot = centerDot;
            grayDot.X += 2;
            grayDot.Y += 2;
            g.FillEllipse(Brushes.LightGray, grayDot);

            DateTime t = DateTime.Now;

            // Минуты
            {
                double minAngle = t.Minute * 6 * System.Math.PI / 180;
                double dx = System.Math.Sin(minAngle) * radius;
                double dy = System.Math.Cos(minAngle) * radius;
                double x1 = +dx * 0.7;
                double y1 = -dy * 0.7;
                double x2 = -dx * 0.05;
                double y2 = +dy * 0.05;
                using (Pen p = new Pen(Brushes.LightGray, 2f))
                {
                    g.DrawLine(p, (float)x1 + 2, (float)y1 + 2,
                        (float)x2 + 2, (float)y2 + 2);
                }
                using (Pen p = new Pen(Brushes.Black, 2f))
                {
                    g.DrawLine(p, (float)x1, (float)y1,
                        (float)x2, (float)y2);
                }
            }

            // Секунды
            {
                double secAngle = t.Second * 6 * System.Math.PI / 180;
                double dx = System.Math.Sin(secAngle) * radius;
                double dy = System.Math.Cos(secAngle) * radius;
                double x1 = +dx * 0.85;
                double y1 = -dy * 0.85;
                double x2 = -dx * 0.10;
                double y2 = +dy * 0.10;
                g.DrawLine(Pens.LightGray, (float)x1 + 2, (float)y1 + 2,
                    (float)x2 + 2, (float)y2 + 2);
                g.DrawLine(Pens.Black, (float)x1, (float)y1,
                    (float)x2, (float)y2);
            }

            // Часы
            {
                double hourAngle = ((double)t.Hour + t.Minute / 60.0) * 30 *
                    System.Math.PI / 180;
                double dx = System.Math.Sin(hourAngle) * radius;
                double dy = System.Math.Cos(hourAngle) * radius;
                double x1 = +dx * 0.6;
                double y1 = -dy * 0.6;
                double x2 = -dx * 0.10;
                double y2 = +dy * 0.10;
                double x3 = +dy * 0.06;
                double y3 = +dx * 0.06;
                double x4 = -dy * 0.06;
                double y4 = -dx * 0.06;
                double x5 = +dx * 0.50;
                double y5 = -dy * 0.50;
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(r2);
                    using (PathGradientBrush pgb
                        = new PathGradientBrush(path))
                    {
                        pgb.CenterPoint = new PointF(r2.Left, r2.Top);
                        pgb.CenterColor = Color.LightBlue;
                        pgb.SurroundColors = new Color[] {
                            Color.Blue
                            };
                        PointF[] points = new PointF[5];
                        points[0] = new PointF((float)x1, (float)y1);
                        points[1] = new PointF((float)x3, (float)y3);
                        points[2] = new PointF((float)x2, (float)y2);
                        points[3] = new PointF((float)x4, (float)y4);
                        points[4] = new PointF((float)x1, (float)y1);
                        PointF[] grayPoints = new PointF[points.Length];
                        for (int i = 0; i < points.Length; i++)
                        {
                            grayPoints[i].X = points[i].X + 2;
                            grayPoints[i].Y = points[i].Y + 2;
                        }
                        g.FillPolygon(Brushes.LightGray, grayPoints,
                            FillMode.Winding);
                        g.FillPolygon(pgb, points, FillMode.Winding);
                    }
                }
            }

            // Снова кружок в центре
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(centerDot);
                using (PathGradientBrush pgb = new PathGradientBrush(path))
                {
                    pgb.CenterPoint = new PointF(-dotRadius / 4,
                        -dotRadius / 4);
                    pgb.CenterColor = Color.White;
                    pgb.SurroundColors = new Color[] {
                        Color.Blue
                    };
                    g.FillEllipse(pgb, centerDot);
                }
            }
        }

        #endregion
    }
}
