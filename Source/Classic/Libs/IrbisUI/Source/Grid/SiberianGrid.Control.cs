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

                    case Keys.PageUp:
                        MoveOnePageUp();
                        break;

                    case Keys.PageDown:
                        MoveOnePageDown();
                        break;

                    case Keys.Enter:
                        OpenEditor(true, null);
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
                OpenEditor
                    (
                        false,
                        e.KeyChar.ToString()
                    );
                e.Handled = true;
            }
        }

        /// <inheritdoc />
        protected override void OnMouseClick
            (
                MouseEventArgs e
            )
        {
            CloseEditor(false);
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Left)
            {
                SiberianCell cell = FindCell(e.X, e.Y);

                if (!ReferenceEquals(cell, null))
                {
                    SiberianClickEventArgs eventArgs = new SiberianClickEventArgs
                    {
                        Grid = this,
                        Row = cell.Row,
                        Column = cell.Column,
                        Cell = cell
                    };

                    OnClick(eventArgs);
               }
                else
                {
                    SiberianRow row = FindRow(e.X, e.Y);
                    if (!ReferenceEquals(row, null)
                        && !ReferenceEquals(CurrentColumn, null))
                    {
                        Goto
                            (
                                CurrentColumn.Index,
                                row.Index
                            );
                    }
                }
            }
        }

        ///// <inheritdoc />
        //protected override void OnMouseDoubleClick
        //    (
        //        MouseEventArgs e
        //    )
        //{
        //    CloseEditor(false);
        //    base.OnMouseDoubleClick(e);

        //    if (e.Button == MouseButtons.Left)
        //    {
        //        SiberianCell cell = FindCell(e.X, e.Y);
        //        if (!ReferenceEquals(cell, null))
        //        {
        //            if (ReferenceEquals(cell, CurrentCell))
        //            {
        //                OpenEditor(true, null);
        //            }
        //        }
        //    }
        //}

        /// <inheritdoc/>
        protected override void OnMouseWheel
            (
                MouseEventArgs e
            )
        {
            base.OnMouseWheel(e);

            MoveRelative
                (
                    0,
                    - e.Delta / 10
                );
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

            Size usableSize = UsableSize;

            int x = 0;
            int y = usableSize.Height;
            PaintEventArgs args;

            // Рисуем заголовки колонок
            for (int columnIndex = _leftColumn; columnIndex < Columns.Count; columnIndex++)
            {
                SiberianColumn column = Columns[columnIndex];
                int height = HeaderHeight;

                clip = new Rectangle
                    (
                        x,
                        0,
                        column.Width,
                        height
                    );
                args = new PaintEventArgs
                    (
                        graphics,
                        clip
                    );
                column.PaintHeader(args);

                clip = new Rectangle
                    (
                        x,
                        height,
                        column.Width,
                        y - height
                    );
                args = new PaintEventArgs
                    (
                        graphics,
                        clip
                    );
                column.Paint(args);

                x += column.Width;
            }

            x = 0;
            using (Brush lineBrush = new SolidBrush(LineColor))
            using (Pen pen = new Pen(lineBrush))
            {
                // Рисуем линию, отделяющую заголовки от содержимого колонок
                int x2 = usableSize.Width;
                graphics.DrawLine(pen, 0, HeaderHeight, x2, HeaderHeight);

                // Рисуем вертикальные разделители между колонками
                for (int columnIndex = _leftColumn; columnIndex < Columns.Count; columnIndex++)
                {
                    SiberianColumn column = Columns[columnIndex];
                    x += column.Width;
                    graphics.DrawLine(pen, x, 0, x, y);
                }

                // Рисуем горизонтальные разделители между строками
                x = usableSize.Width;
                y = HeaderHeight;
                for (int rowIndex = _topRow; rowIndex < Rows.Count; rowIndex++)
                {
                    SiberianRow row = Rows[rowIndex];
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

                    if (y >= usableSize.Height)
                    {
                        break;
                    }
                }

                // Рисуем ячейки
                x = 0;
                for (int columnIndex = _leftColumn; columnIndex < Columns.Count; columnIndex++)
                {
                    SiberianColumn column = Columns[columnIndex];
                    int dx = column.Width;

                    y = HeaderHeight;
                    for (int rowIndex = _topRow; rowIndex < Rows.Count; rowIndex++)
                    {
                        SiberianRow row = Rows[rowIndex];
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
                        SiberianCell cell = row.Cells[columnIndex];
                        cell.Paint(args);
                        y += dy;
                    }
                    x += dx;
                }
            }
        }

        /// <inheritdoc />
        protected override void OnResize
            (
                EventArgs e
            )
        {
            base.OnResize(e);

            AutoSizeColumns();
            MoveRelative(0, 0);
        }

        #endregion
    }
}
