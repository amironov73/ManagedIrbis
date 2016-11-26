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

        #endregion

        #region Control members

        /// <inheritdoc />
        protected override Size DefaultSize
        {
            get { return new Size(640, 375); }
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
                                y + row.Height
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
