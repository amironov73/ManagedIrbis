/* SiberianGrid.cs -- 
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
        [Browsable(false)]
        public SiberianColumn CurrentColumn { get; private set; }

        /// <summary>
        /// Current row.
        /// </summary>
        [CanBeNull]
        [Browsable(false)]
        public SiberianRow CurrentRow { get; private set; }

        /// <summary>
        /// Current cell.
        /// </summary>
        [CanBeNull]
        [Browsable(false)]
        public SiberianCell CurrentCell { get; private set; }

        /// <summary>
        /// Whether the whole grid itself is read-only.
        /// </summary>
        public bool ReadOnly { get; private set; }

        /// <summary>
        /// Current editor (if any).
        /// </summary>
        [CanBeNull]
        [Browsable(false)]
        public Control Editor { get; internal set; }

        /// <summary>
        /// Header height.
        /// </summary>
        public int HeaderHeight { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianGrid()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor

            Columns = new NonNullCollection<SiberianColumn>();
            Rows = new NonNullCollection<SiberianRow>();

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

            HeaderHeight = SiberianRow.DefaultHeight;
        }

        #endregion

        #region Private members

        private readonly ScrollBar _horizontalScroll;
        private readonly ScrollBar _verticalScroll;

        private int _leftColumn;
        private int _topRow;

        private bool _autoSizeWatch;

        internal void AutoSizeColumns()
        {
            if (_autoSizeWatch)
            {
                return;
            }

            try
            {
                _autoSizeWatch = true;

                SiberianColumn[] fixedColumns = Columns
                    .Where(column => column.FillWidth <= 0)
                    .ToArray();
                SiberianColumn[] needResize = Columns
                    .Where(column => column.FillWidth > 0)
                    .ToArray();

                if (needResize.Length != 0)
                {
                    int fixedSum = fixedColumns
                        .Sum(column => column.Width);
                    int fillSum = needResize
                        .Sum(column => column.FillWidth);

                    int remaining = ClientSize.Width - 1 - fixedSum;

                    foreach (SiberianColumn column in needResize)
                    {
                        column.Width = Math.Max
                            (
                                100,
                                remaining*column.FillWidth/fillSum
                            );
                    }
                }
            }
            finally
            {
                _autoSizeWatch = false;
            }
        }

        private int _DoScroll
            (
                ScrollBar scrollBar,
                ScrollEventArgs args
            )
        {
            int result = args.OldValue;

            switch (args.Type)
            {
                case ScrollEventType.First:
                    result = scrollBar.Minimum;
                    break;

                case ScrollEventType.LargeDecrement:
                    result -= scrollBar.LargeChange;
                    break;

                case ScrollEventType.LargeIncrement:
                    result += scrollBar.LargeChange;
                    break;

                case ScrollEventType.Last:
                    result = scrollBar.Maximum;
                    break;

                case ScrollEventType.SmallDecrement:
                    result -= scrollBar.SmallChange;
                    break;

                case ScrollEventType.SmallIncrement:
                    result += scrollBar.SmallChange;
                    break;

                case ScrollEventType.EndScroll:
                    result = args.NewValue;
                    break;

                case ScrollEventType.ThumbTrack:
                    result = args.NewValue;
                    break;
            }

            result = Math.Max(result, scrollBar.Minimum);
            result = Math.Min(result, scrollBar.Maximum);

            return result;
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
                    e
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
                    e
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

            AutoSizeColumns();
            Invalidate();

            return result;
        }

        /// <summary>
        /// Open editor for current cell.
        /// </summary>
        [CanBeNull]
        public Control OpenEditor
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
        /// Find cell under given mouse position.
        /// </summary>
        [CanBeNull]
        public SiberianCell FindCell
            (
                int x,
                int y
            )
        {
            SiberianColumn column = FindColumn(x, y);
            SiberianRow row = FindRow(x, y);

            if (!ReferenceEquals(column, null)
                && !ReferenceEquals(row, null))
            {
                SiberianCell result = GetCell
                    (
                        column.Index,
                        row.Index
                    );

                return result;
            }

            return null;
        }

        /// <summary>
        /// Find column under given mouse position.
        /// </summary>
        [CanBeNull]
        public SiberianColumn FindColumn
            (
                int x,
                int y
            )
        {
            int left = 0;
            
            for (int columnIndex = _leftColumn; columnIndex < Columns.Count; columnIndex++)
            {
                SiberianColumn column = Columns[columnIndex];
                int right = left + column.Width;
                if (x >= left && x <= right)
                {
                    return column;
                }
                left = right;
            }

            return null;
        }

        /// <summary>
        /// Find row under given mouse position.
        /// </summary>
        [CanBeNull]
        public SiberianRow FindRow
            (
                int x,
                int y
            )
        {
            int top = HeaderHeight;
            for (int rowIndex = _topRow; rowIndex < Rows.Count; rowIndex++)
            {
                SiberianRow row = Rows[rowIndex];
                int bottom = top + row.Height;
                if (y >= top && y <= bottom)
                {
                    return row;
                }
                top = bottom;
            }

            return null;
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
            for (int i = _leftColumn; i < column; i++)
            {
                x += Columns[i].Width;
            }

            int row = cell.Row.Index;
            int y = HeaderHeight;
            for (int i = _topRow; i < row; i++)
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

                if (CurrentColumn.Index < _leftColumn)
                {
                    _leftColumn = CurrentColumn.Index;
                }

                if (CurrentRow.Index < _topRow)
                {
                    _topRow = CurrentRow.Index;
                }

                // Adjust left column
                while (Columns.Count != 0)
                {
                    int x = 0;

                    for (int i = _leftColumn; i < CurrentColumn.Index; i++)
                    {
                        x += Columns[i].Width;
                    }
                    x += Columns[CurrentColumn.Index].Width;

                    if (x < ClientSize.Width)
                    {
                        break;
                    }

                    _leftColumn++;
                    if (_leftColumn >= Columns.Count)
                    {
                        _leftColumn = Math.Max(0, Columns.Count - 1);
                        break;
                    }
                }

                // Adjust top row
                while (Rows.Count != 0)
                {
                    int y = HeaderHeight;

                    for (int i = _topRow; i < CurrentRow.Index; i++)
                    {
                        y += Rows[i].Height;
                    }
                    y += Rows[CurrentRow.Index].Height;

                    if (y < ClientSize.Height)
                    {
                        break;
                    }

                    _topRow++;
                    if (_topRow >= Rows.Count)
                    {
                        _topRow = Math.Max(0, Rows.Count - 1);
                        break;
                    }
                }

                int visibleRows = (ClientSize.Height - HeaderHeight) 
                    / SiberianRow.DefaultHeight;
                visibleRows = Math.Max(visibleRows, 1);
                _verticalScroll.LargeChange = visibleRows;

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
        /// Move one page down.
        /// </summary>
        [CanBeNull]
        public SiberianCell MoveOnePageDown()
        {
            SiberianCell result = MoveRelative(0, _verticalScroll.LargeChange);

            return result;
        }

        /// <summary>
        /// Move one page up.
        /// </summary>
        [CanBeNull]
        public SiberianCell MoveOnePageUp()
        {
            SiberianCell result = MoveRelative(0, -_verticalScroll.LargeChange);

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
