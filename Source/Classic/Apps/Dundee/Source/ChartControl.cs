using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dundee
{
    public partial class ChartControl
        : Control
    {
        public int[]Values { get; set; }


        public ChartControl()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            Enabled = false;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            if (ReferenceEquals(Values, null) || Values.Length == 0)
            {
                return;
            }

            int length = Values.Length;
            int minValue = Values.Min();
            int maxValue = Values.Max();
            if (minValue == maxValue)
            {
                return;
            }

            int fullWidth = ClientSize.Width;
            int fullHeight = ClientSize.Height;
            int padding = 4;
            int orgX = padding;
            int orgY = fullHeight - padding;
            int workWidth = fullWidth - padding - padding;
            int workHeight = fullHeight - padding - padding;
            if (workWidth <= 0 || workHeight <= 0)
            {
                return;
            }

            float scaleX = ((float) workWidth) / length;
            float scaleY = ((float) workHeight) / (maxValue - minValue);

            g.SmoothingMode = SmoothingMode.HighQuality;
            using (Pen pen = new Pen(ForeColor, 0))
            using (Brush brush = new SolidBrush(ForeColor))
            {
                float x1 = orgX;
                float y1 = orgY;
                for (int i = 0; i < length; i++)
                {
                    float x2 = orgX + scaleX * i;
                    float y2 = orgY - (Values[i] - minValue) * scaleY;
                    g.DrawLine(pen, x1, y1, x2, y2);
                    g.FillEllipse(brush, x2 - 2, y2 - 2, 4, 4);
                    x1 = x2;
                    y1 = y2;
                }
            }
        }
    }
}
