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
using AM.Mathematics;
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
    public partial class SiberianGrid
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

        /// <summary>
        /// Whether the whole grid itself is read-only.
        /// </summary>
        public bool ReadOnly { get; private set; }

        /// <summary>
        /// Current editor (if any).
        /// </summary>
        [CanBeNull]
        public Control Editor { get; internal set; }

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

            _horizontalScroll = new HScrollBar
            {
                AutoSize = false,
                Dock = DockStyle.Bottom,
                SmallChange = 1
            };
            Controls.Add(_horizontalScroll);
            _horizontalScroll.Scroll += _horizontalScroll_Scroll;

            _verticalScroll = new VScrollBar
            {
                AutoSize = false,
                Dock = DockStyle.Right,
                SmallChange = 1
            };
            Controls.Add(_verticalScroll);
            _verticalScroll.Scroll += _verticalScroll_Scroll;

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

        private readonly ScrollBar _horizontalScroll;
        private readonly ScrollBar _verticalScroll;

        private int _DoScroll
            (
                ScrollBar scrollBar,
                ScrollEventType scrollType,
                int value
            )
        {
            switch (scrollType)
            {
                case ScrollEventType.First:
                    value = scrollBar.Minimum;
                    break;

                case ScrollEventType.LargeDecrement:
                    value -= scrollBar.LargeChange;
                    break;

                case ScrollEventType.LargeIncrement:
                    value += scrollBar.LargeChange;
                    break;

                case ScrollEventType.Last:
                    value = scrollBar.Maximum;
                    break;

                case ScrollEventType.SmallDecrement:
                    value -= scrollBar.SmallChange;
                    break;

                case ScrollEventType.SmallIncrement:
                    value += scrollBar.SmallChange;
                    break;
            }

            value = Math.Max(value, scrollBar.Minimum);
            value = Math.Min(value, scrollBar.Maximum);

            return value;
        }

        private void _horizontalScroll_Scroll
            (
                object sender,
                ScrollEventArgs e
            )
        {
            if (ReferenceEquals(CurrentRow, null))
            {
                return;
            }

            int value = _DoScroll
                (
                    _horizontalScroll,
                    e.Type,
                    e.OldValue
                );

            e.NewValue = value;

            Goto
                (
                    value,
                    CurrentRow.Index
                );
        }

        private void _verticalScroll_Scroll
            (
                object sender,
                ScrollEventArgs e
            )
        {
            if (ReferenceEquals(CurrentColumn, null))
            {
                return;
            }

            int value = _DoScroll
                (
                    _verticalScroll,
                    e.Type,
                    e.OldValue
                );

            e.NewValue = value;

            Goto
                (
                    CurrentColumn.Index,
                    value
                );
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Close current editor.
        /// </summary>
        public void CloseEditor
            (
                bool accept
            )
        {
            if (ReferenceEquals(CurrentCell, null))
            {
                return;
            }

            CurrentCell.CloseEditor(accept);
            Invalidate();
        }

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

            _horizontalScroll.Maximum = Columns.Count;
            _horizontalScroll.Value = CurrentColumn.Index;

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
        /// Create editor for current cell.
        /// </summary>
        [CanBeNull]
        public Control CreateEditor
            (
                bool edit,
                object state
            )
        {
            if (!ReferenceEquals(Editor, null))
            {
                return Editor;
            }
            if (ReadOnly)
            {
                return null;
            }
            if (ReferenceEquals(CurrentCell, null))
            {
                return null;
            }
            if (ReferenceEquals(CurrentColumn, null)
                || CurrentColumn.ReadOnly)
            {
                return null;
            }

            Editor = CurrentColumn.CreateEditor
                (
                    CurrentCell,
                    edit,
                    state
                );

            return Editor;
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

            _verticalScroll.Maximum = Rows.Count;
            _verticalScroll.Value = CurrentRow.Index;

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
        /// Get cell rectangle.
        /// </summary>
        public Rectangle GetCellRectangle
            (
                [NotNull] SiberianCell cell
            )
        {
            Code.NotNull(cell, "cell");

            int column = cell.Column.Index;
            int x = 0;
            for (int i = 0; i < column; i++)
            {
                x += Columns[i].Width;
            }

            int row = cell.Row.Index;
            int y = 0;
            for (int i = 0; i < row; i++)
            {
                y += Rows[i].Height;
            }
            int width = cell.Column.Width;
            int height = cell.Row.Height;

            Rectangle result = new Rectangle
                (
                    x,
                    y,
                    width,
                    height
                );

            return result;
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
            CloseEditor(true);


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

                _horizontalScroll.Maximum = Columns.Count;
                _horizontalScroll.Value = CurrentColumn.Index;

                _verticalScroll.Maximum = Rows.Count;
                _verticalScroll.Value = CurrentRow.Index;

                Invalidate();
            }

            return result;
        }

        /// <summary>
        /// Move one column left.
        /// </summary>
        [CanBeNull]
        public SiberianCell MoveOneColumnLeft()
        {
            if (ReferenceEquals(CurrentColumn, null))
            {
                return CurrentCell;
            }

            int index = CurrentColumn.Index;
            int newIndex = index - 1;

            for (; newIndex >= 0; newIndex--)
            {
                if (!Columns[newIndex].ReadOnly)
                {
                    break;
                }
            }

            if (newIndex < 0)
            {
                return CurrentCell;
            }

            SiberianCell result = MoveRelative
                (
                    newIndex - index,
                    0
                );

            return result;
        }

        /// <summary>
        /// Move one column right.
        /// </summary>
        [CanBeNull]
        public SiberianCell MoveOneColumnRight()
        {
            SiberianCell result = MoveRelative(1, 0);

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
        /// Move one row down.
        /// </summary>
        [CanBeNull]
        public SiberianCell MoveOneLineUp()
        {
            SiberianCell result = MoveRelative(0, -1);

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

        #endregion

        #region Object members

        #endregion
    }
}
