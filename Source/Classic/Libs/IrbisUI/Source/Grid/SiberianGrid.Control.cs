/* SiberianGrid.Control.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Grid
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class SiberianGrid
    {
        #region Control members

        // ReSharper disable InconsistentNaming
        private const int WS_VSCROLL = 0x00200000;
        private const int WS_HSCROLL = 0x00100000;
        // ReSharper restore InconsistentNaming

        /// <inheritdoc />
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams result = base.CreateParams;

                result.Style |= WS_VSCROLL;

                return result;
            }
        }

        /// <inheritdoc />
        protected override Size DefaultSize
        {
            get { return new Size(640, 375); }
        }


        /// <inheritdoc />
        protected override bool IsInputKey
            (
                Keys keyData
            )
        {
            // Enable all the keys.
            return true;
        }

        /// <inheritdoc/>
        protected override void OnKeyDown
            (
                KeyEventArgs e
            )
        {
            base.OnKeyDown(e);

            if (e.Modifiers == 0)
            {
                e.Handled = true;

                switch (e.KeyCode)
                {
                    case Keys.Up:
                        MoveOneLineUp();
                        break;

                    case Keys.Down:
                        MoveOneLineDown();
                        break;

                    case Keys.Left:
                        MoveOneColumnLeft();
                        break;

                    case Keys.Right:
                        MoveOneColumnRight();
                        break;

                    case Keys.Enter:
                        CreateEditor(true, null);
                        break;
                }
            }
        }

        /// <inheritdoc />
        protected override void OnKeyPress
            (
                KeyPressEventArgs e
            )
        {
            base.OnKeyPress(e);

            if (!char.IsControl(e.KeyChar))
            {
                CreateEditor
                    (
                        false,
                        e.KeyChar.ToString()
                    );
                e.Handled = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnPaint
            (
                PaintEventArgs paintEvent
            )
        {
            Graphics graphics = paintEvent.Graphics;
            Rectangle clip = paintEvent.ClipRectangle;

            using (Brush brush = new SolidBrush(BackColor))
            {
                graphics.FillRectangle(brush, clip);
            }

            int x = 0;
            int y = ClientSize.Height;
            int index;
            PaintEventArgs args;

            using (Brush brush = new SolidBrush(ForeColor))
            using (Pen pen = new Pen(brush))
            {
                foreach (SiberianColumn column in Columns)
                {
                    x += column.Width;
                    graphics.DrawLine(pen, x, 0, x, y);
                }

                x = ClientSize.Width;
                y = 0;
                index = 0;
                foreach (SiberianRow row in Rows)
                {
                    args = new PaintEventArgs
                        (
                            graphics,
                            new Rectangle
                            (
                                0,
                                y,
                                x,
                                row.Height
                            )
                        );
                    row.Paint(args);

                    graphics.DrawLine(pen, 0, y, x, y);
                    y += row.Height;
                }

                x = 0;
                index = 0;
                foreach (SiberianColumn column in Columns)
                {
                    int dx = column.Width;

                    y = 0;
                    foreach (SiberianRow row in Rows)
                    {
                        int dy = row.Height;

                        args = new PaintEventArgs
                            (
                                graphics,
                                new Rectangle
                                    (
                                        x + 1,
                                        y + 1,
                                        dx - 2,
                                        dy - 2
                                    )
                            );
                        SiberianCell cell = row.Cells[index];
                        cell.Paint(args);
                        y += dy;
                    }
                    index++;
                    x += dx;
                }
            }
        }

        #endregion
    }
}
