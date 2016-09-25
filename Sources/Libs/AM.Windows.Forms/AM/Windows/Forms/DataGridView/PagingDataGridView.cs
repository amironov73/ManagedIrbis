/* PagingDataGridView.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [System.ComponentModel.DesignerCategory("Code")]
    public class PagingDataGridView
        : DataGridView
    {
        #region Events

        /// <summary>
        /// Raised on paging.
        /// </summary>
        public event EventHandler<PagingDataGridViewEventArgs> Paging;

        #endregion

        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PagingDataGridView()
        {
            ReadOnly = true;
            AutoGenerateColumns = false;
            RowHeadersVisible = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AllowUserToAddRows = false;
            AllowUserToResizeRows = false;
            AllowUserToDeleteRows = false;
            ScrollBars = ScrollBars.None;

            DataGridViewCellStyle mainStyle = new DataGridViewCellStyle();
            DefaultCellStyle = mainStyle;

            DataGridViewCellStyle alternateStyle
                = new DataGridViewCellStyle(mainStyle)
                {
                    BackColor = Color.LightGray
                };
            AlternatingRowsDefaultCellStyle = alternateStyle;
        }

        #endregion

        #region Private members

        private void _HandleScrolling(bool down)
        {
            DataGridViewRow row = CurrentRow;
            if (row == null)
            {
                return;
            }

            int rowIndex = row.Index;
            int rowCount = RowCount;

            if (down)
            {
                if (rowIndex >= rowCount - 1)
                {
                    PerformPaging(true, false);
                }
            }
            else
            {
                if (rowIndex == 0)
                {
                    PerformPaging(false, false);
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Fill the grid.
        /// </summary>
        public void FillGrid
            (
                [NotNull] IEnumerable<object> objects
            )
        {
            Code.NotNull(objects, "objects");

            DataSource = objects.ToArray();
        }

        /// <summary>
        /// Perform paging.
        /// </summary>
        public void PerformPaging
            (
                bool scrollDown,
                bool initialCall
            )
        {
            PagingDataGridViewEventArgs eventArgs
                = new PagingDataGridViewEventArgs
            {
                InitialCall = initialCall,
                ScrollDown = scrollDown,
                CurrentRow = CurrentRow
            };

            Paging.Raise(this, eventArgs);

            if (!eventArgs.Success)
            {
                return;
            }

            if (!scrollDown && RowCount > 0)
            {
                CurrentCell = Rows[RowCount - 1].Cells[0];
            }
        }

        #endregion

        #region DataGridView members

        /// <inheritdoc />
        protected override void OnKeyDown
        (
            KeyEventArgs e
        )
        {
            base.OnKeyDown(e);

            bool down = false;

            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.PageDown:
                    down = true;
                    break;

                case Keys.Up:
                case Keys.PageUp:
                    break;

                default:
                    return;
            }

            e.Handled = true;

            _HandleScrolling(down);
        }

        /// <inheritdoc />
        protected override void OnMouseWheel
            (
                MouseEventArgs e
            )
        {
            base.OnMouseWheel(e);

            _HandleScrolling(e.Delta < 0);
        }

        #endregion
    }
}
