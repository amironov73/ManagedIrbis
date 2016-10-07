/* Thermometer.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Простой "градусник".
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [System.ComponentModel.DesignerCategoryAttribute("Code")]
    public sealed class Thermometer
        : Control
    {
        #region Events

        /// <summary>
        /// Raised when current temperature changed.
        /// </summary>
        public event EventHandler CurrentTemperatureChanged;

        /// <summary>
        /// Raised when minimal temperature changed.
        /// </summary>
        public event EventHandler MinimalTemperatureChanged;

        /// <summary>
        /// Raised when maximal temperature changed.
        /// </summary>
        public event EventHandler MaximalTemperatureChanged;

        #endregion

        #region Properties

        private const double DefaultCurrentTemperature = 0.0;

        private double _currentTemperature = DefaultMinimalTemperature;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(DefaultCurrentTemperature)]
        public double CurrentTemperature
        {
            [DebuggerStepThrough]
            get
            {
                return _currentTemperature;
            }
            [DebuggerStepThrough]
            set
            {
                _currentTemperature = value;
                _SetTemperatures();
                if (CurrentTemperatureChanged != null)
                {
                    CurrentTemperatureChanged(this, EventArgs.Empty);
                }
            }
        }

        private const double DefaultMinimalTemperature = 0.0;

        private double _minimalTemperature = DefaultMinimalTemperature;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(DefaultMinimalTemperature)]
        public double MinimalTemperature
        {
            [DebuggerStepThrough]
            get
            {
                return _minimalTemperature;
            }
            [DebuggerStepThrough]
            set
            {
                _minimalTemperature = value;
                _SetTemperatures();
                if (MinimalTemperatureChanged != null)
                {
                    MinimalTemperatureChanged(this, EventArgs.Empty);
                }
            }
        }

        private const double DefaultMaximalTemperature = 100.0;

        private double _maximalTemperature = DefaultMaximalTemperature;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(DefaultMaximalTemperature)]
        public double MaximalTemperature
        {
            [DebuggerStepThrough]
            get
            {
                return _maximalTemperature;
            }
            [DebuggerStepThrough]
            set
            {
                _maximalTemperature = value;
                _SetTemperatures();
                if (MaximalTemperatureChanged != null)
                {
                    MaximalTemperatureChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Thermometer()
        {
            ResizeRedraw = true;
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        #endregion

        #region Private members

        private void _SetTemperatures()
        {
            _maximalTemperature = Math.Max(_maximalTemperature,
                _minimalTemperature);
            _currentTemperature = Math.Max(Math.Min(
                _currentTemperature, _maximalTemperature),
                _minimalTemperature);
            Invalidate();
        }

        #endregion

        #region Control members

        /// <inheritdoc />
        protected override void OnPaint
            (
                PaintEventArgs e
            )
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            int globuleDiameter = 16;
            int columnWidth = 8;
            int columnPosition = (globuleDiameter - columnWidth) / 2;
            using (Pen blackPen = new Pen(Color.DarkGray))
            using (Brush whiteBrush = new SolidBrush(Color.White))
            using (Brush redBrush = new SolidBrush(Color.Red))
            {
                Rectangle globule = new Rectangle(0,
                    Height - globuleDiameter, globuleDiameter,
                    globuleDiameter);
                Rectangle column = new Rectangle(columnPosition,
                    columnWidth / 2, columnWidth, Height - globuleDiameter);
                Rectangle innerColumn = column;
                Rectangle gap = new Rectangle(columnPosition, 0,
                    columnWidth, columnWidth);
                innerColumn.Inflate(-1, -1);
                innerColumn.Y -= 2;
                innerColumn.Height += 6;
                g.FillPie(whiteBrush, gap, 180f, 180f);
                g.DrawArc(blackPen, gap, 180f, 180f);
                g.FillEllipse(whiteBrush, globule);
                g.DrawEllipse(blackPen, globule);
                g.DrawRectangle(blackPen, column);
                using (LinearGradientBrush glassBrush
                    = new LinearGradientBrush(innerColumn, Color.White,
                    Color.Black, 0f))
                {
                    ColorBlend blend = new ColorBlend();
                    blend.Colors = new Color[]
                            {
                                Color.FromArgb ( 230, 230, 255 ),
                                Color.White,
                                Color.FromArgb ( 220, 220, 255 ),
                            };
                    blend.Positions = new float[]
                            {
                                0f,
                                0.3f,
                                1f
                            };
                    glassBrush.InterpolationColors = blend;
                    g.FillRectangle(glassBrush, innerColumn);
                }
                using (GraphicsPath globulePath = new GraphicsPath())
                {
                    globulePath.AddEllipse(globule);
                    using (PathGradientBrush globuleBrush
                        = new PathGradientBrush(globulePath))
                    {
                        globuleBrush.CenterPoint = new PointF(
                            (float)globule.Left + globuleDiameter / 4,
                            (float)globule.Top + globuleDiameter / 4);
                        ColorBlend blend = new ColorBlend();
                        blend.Colors = new Color[]
                            {
                                Color.Red,
                                Color.Red,
                                Color.White
                            };
                        blend.Positions = new float[]
                            {
                                0f,
                                0.5f,
                                1f
                            };
                        globuleBrush.InterpolationColors = blend;
                        g.FillEllipse(globuleBrush, globule);
                    }
                }
                int innerColumnHeight = (int)
                    ((_currentTemperature - _minimalTemperature)
                    / (_maximalTemperature - _minimalTemperature)
                    * column.Height);
                if (innerColumnHeight > 0)
                {
                    Rectangle redColumn = new Rectangle(columnPosition + 1,
                        Height - globuleDiameter - innerColumnHeight + 2,
                        columnWidth - 2, innerColumnHeight);
                    using (LinearGradientBrush columnBrush = new LinearGradientBrush
                        (redColumn, Color.White, Color.Red, 0f))
                    {
                        ColorBlend blend = new ColorBlend();
                        blend.Colors = new Color[]
                            {
                                Color.Red,
                                Color.FromArgb ( 255, 200, 200 ),
                                Color.Red
                            };
                        blend.Positions = new float[]
                            {
                                0f,
                                0.3f,
                                1f
                            };
                        columnBrush.InterpolationColors = blend;
                        g.FillRectangle(columnBrush, redColumn);
                    }
                    int half = columnWidth / 2 - 1;
                    Rectangle columnGap = new Rectangle(columnPosition + 1,
                        redColumn.Top - columnWidth / 2 + 1, columnWidth - 2,
                        columnWidth - 2);
                    g.FillPie(redBrush, columnGap, 180f, 180f);
                }
            }
            base.OnPaint(e);
        }

        #endregion
    }
}
