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
                bool edit
            )
        {
            if (!ReferenceEquals(Editor, null))
            {
                return Editor;
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
                    edit
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
            SiberianCell result = MoveRelative(-1, 0);

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
