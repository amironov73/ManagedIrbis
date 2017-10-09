// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PingPlotter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using JetBrains.Annotations;

using ManagedIrbis.Statistics;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Source.Statistics
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]

    public sealed partial class PingPlotter
        : Control
    {
        #region Properties

        /// <summary>
        /// Statistics.
        /// </summary>
        [CanBeNull]
        public PingStatistics Statistics { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PingPlotter()
        {
            BackColor = Color.White;
            DoubleBuffered = true;
            TabStop = false;
            SetStyle
                (
                    ControlStyles.DoubleBuffer
                    | ControlStyles.OptimizedDoubleBuffer
                    | ControlStyles.ResizeRedraw,
                    true
                );
            SetStyle
                (
                    ControlStyles.Selectable,
                    false
                );

            InitializeComponent();
        }

        #endregion

        #region Control members

        /// <inheritdoc cref="Control.OnPaint" />
        protected override void OnPaint
            (
                PaintEventArgs e
            )
        {
            base.OnPaint(e);

            if (ReferenceEquals(Statistics, null))
            {
                return;
            }

            PingData[] data = Statistics.Data.ToArray();
            if (data.Length == 0)
            {
                return;
            }

            int maxTime = Statistics.MaxTime;
            if (maxTime == 0)
            {
                maxTime = 1;
            }
            Rectangle workArea = ClientRectangle;
            int height = workArea.Height;
            int top = workArea.Top;
            int bottom = workArea.Bottom;
            double scale = (double)height / maxTime;

            Graphics graphics = e.Graphics;

            using (Pen linePen = new Pen(Color.LightBlue))
            {
                int time = 50;
                while (time < maxTime)
                {
                    int y = (int)(bottom - time * scale);
                    graphics.DrawLine
                        (
                            linePen,
                            workArea.Left,
                            y,
                            workArea.Right,
                            y
                        );

                    time += 50;
                }
            }

            using (Pen linePen = new Pen(Color.Blue))
            using (Pen errorPen = new Pen(Color.Red))
            using (Pen endPen = new Pen(Color.LightGreen))
            {
                int i = 0;
                int x1 = workArea.Left;
                int x2 = x1 + 1;
                int y1 = (int)(bottom - data[i].RoundTripTime * scale);
                int right = workArea.Right;

                while (x2 < right && i < data.Length)
                {
                    if (data[i].Success)
                    {
                        int y2 = (int)(bottom - data[i].RoundTripTime * scale);
                        graphics.DrawLine(linePen, x1, y1, x2, y2);
                        y1 = y2;
                    }
                    else
                    {
                        graphics.DrawLine
                            (
                                errorPen,
                                x1, y1, x1, bottom
                            );
                    }

                    x1++;
                    x2++;
                    i++;
                }

                graphics.DrawLine
                    (
                        endPen,
                        x1, top, x1, bottom
                    );
            }
        }

        #endregion
    }
}
