/* SiberianGrid.cs -- 
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
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianGrid
        : Control
    {
        #region Properties

        /// <summary>
        /// Columns.
        /// </summary>
        [NotNull]
        public NonNullCollection<SiberianColumn> Columns { get; private set; }

        /// <summary>
        /// Rows.
        /// </summary>
        [NotNull]
        public NonNullCollection<SiberianRow> Rows { get; private set; }

        /// <summary>
        /// Current column.
        /// </summary>
        [CanBeNull]
        public SiberianColumn CurrentColumn { get; private set; }

        /// <summary>
        /// Current row.
        /// </summary>
        [CanBeNull]
        public SiberianRow CurrentRow { get; private set; }

        /// <summary>
        /// Current cell.
        /// </summary>
        [CanBeNull]
        public SiberianCell CurrentCell { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianGrid()
        {
            Columns = new NonNullCollection<SiberianColumn>();
            Rows = new NonNullCollection<SiberianRow>();

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            DoubleBuffered = true;

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.StandardClick, true);
            SetStyle(ControlStyles.StandardDoubleClick, true);
            SetStyle(ControlStyles.UserMouse, true);

            BackColor = Color.DarkGray;
            ForeColor = Color.Black;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Create column of specified type.
        /// </summary>
        [NotNull]
        public SiberianColumn CreateColumn<T>()
            where T : SiberianColumn, new()
        {
            T result = new T
            {
                Grid = this,
                Index = Columns.Count
            };

            if (ReferenceEquals(CurrentColumn, null))
            {
                CurrentColumn = result;
            }

            foreach (SiberianRow row in Rows)
            {
                SiberianCell cell = result.CreateCell();
                cell.Row = row;
                row.Cells.Add(cell);
            }

            Columns.Add(result);

            if (ReferenceEquals(CurrentCell, null))
            {
                if (!ReferenceEquals(CurrentRow, null))
                {
                    CurrentCell = GetCell
                    (
                        CurrentColumn.Index,
                        CurrentRow.Index
                    );
                }
            }

            Invalidate();

            return result;
        }

        /// <summary>
        /// Create row.
        /// </summary>
        [NotNull]
        public SiberianRow CreateRow
            (
                object data
            )
        {
            SiberianRow result = new SiberianRow
            {
                Data = data,
                Index = Rows.Count,
                Grid = this
            };

            if (ReferenceEquals(CurrentRow, null))
            {
                CurrentRow = result;
            }

            foreach (SiberianColumn column in Columns)
            {
                SiberianCell cell = column.CreateCell();
                cell.Row = result;
                result.Cells.Add(cell);
            }

            Rows.Add(result);

            if (ReferenceEquals(CurrentCell, null))
            {
                if (!ReferenceEquals(CurrentColumn, null))
                {
                    CurrentCell = GetCell
                    (
                        CurrentColumn.Index,
                        CurrentRow.Index
                    );
                }
            }

            Invalidate();

            return result;
        }

        /// <summary>
        /// Get cell for given column and row.
        /// </summary>
        [CanBeNull]
        public SiberianCell GetCell
            (
                int column,
                int row
            )
        {
            if (column >= 0 && column < Columns.Count
                && row >= 0 && row < Rows.Count)
            {
                return Rows[row].Cells[column];
            }

            return null;
        }

        /// <summary>
        /// Go to specified cell.
        /// </summary>
        [CanBeNull]
        public SiberianCell Goto
            (
                int column,
                int row
            )
        {
            if (column >= Columns.Count)
            {
                column = Columns.Count - 1;
            }
            if (column < 0)
            {
                column = 0;
            }
            if (row >= Rows.Count)
            {
                row = Rows.Count - 1;
            }
            if (row < 0)
            {
                row = 0;
            }

            SiberianCell result = GetCell(column, row);

            if (!ReferenceEquals(result, null))
            {
                CurrentRow = result.Row;
                CurrentColumn = result.Column;
                CurrentCell = result;
                Invalidate();
            }

            return result;
        }

        /// <summary>
        /// Move one row down.
        /// </summary>
        [CanBeNull]
        public SiberianCell MoveOneLineDown()
        {
            SiberianCell result = MoveRelative(0, 1);

            return result;
        }

        /// <summary>
        /// Relative movement.
        /// </summary>
        [CanBeNull]
        public SiberianCell MoveRelative
            (
                int columnDelta,
                int rowDelta
            )
        {
            SiberianCell current = CurrentCell;
            if (ReferenceEquals(current, null))
            {
                return null;
            }

            int column = current.Column.Index + columnDelta;
            int row = current.Row.Index + rowDelta;

            SiberianCell result = Goto(column, row);

            return result;
        }

        /// <summary>
        /// Move one row down.
        /// </summary>
        [CanBeNull]
        public SiberianCell MoveOneLineUp()
        {
            SiberianCell result = MoveRelative(0, -1);

            return result;
        }

        #endregion

        #region Control members

        private const int WS_VSCROLL = 0x00200000;
        private const int WS_HSCROLL = 0x00100000;

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
                }
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

        #region Object members

        #endregion
    }
}
