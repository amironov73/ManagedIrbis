// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianFieldColumn.cs -- 
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
using AM.Windows.Forms;

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
    public class SiberianFieldColumn
        : SiberianColumn
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianFieldColumn()
        {
            BackColor = Color.White;
        }

        #endregion

        #region Private members

        private void Editor_KeyDown
            (
                object sender,
                KeyEventArgs args
            )
        {
            if (ReferenceEquals(Grid, null))
            {
                return;
            }

            if (args.Modifiers == 0)
            {
                switch (args.KeyCode)
                {
                    case Keys.Escape:
                        Grid.CloseEditor(false);
                        break;

                    case Keys.Up:
                        Grid.MoveOneLineUp();
                        break;

                    case Keys.Down:
                    case Keys.Enter:
                        Grid.MoveOneLineDown();
                        break;

                    case Keys.PageUp:
                        Grid.MoveOnePageUp();
                        break;

                    case Keys.PageDown:
                        Grid.MoveOnePageDown();
                        break;
                }
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region SiberianColumn members

        /// <inheritdoc/>
        public override SiberianCell CreateCell()
        {
            SiberianCell result = new SiberianFieldCell();
            result.Column = this;

            return result;
        }

        /// <inheritdoc />
        public override Control CreateEditor
            (
                SiberianCell cell,
                bool edit,
                object state
            )
        {
            Code.NotNull(cell, "cell");

            SiberianFieldCell fieldCell = (SiberianFieldCell)cell;

            SiberianField field = (SiberianField) fieldCell.Row.Data;
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            string text = field.Value;

            Rectangle rectangle = Grid.GetCellRectangle(cell);
            rectangle.Inflate(-1, -1);

            TextBoxWithButton result = new TextBoxWithButton
            {
                AutoSize = false,
                Location = rectangle.Location,
                Size = rectangle.Size,
                Font = Grid.Font,
                BorderStyle = BorderStyle.FixedSingle
            };
            result.TextBox.KeyDown += Editor_KeyDown;

            if (edit)
            {
                result.Text = text;
            }
            else
            {
                if (!ReferenceEquals(state, null))
                {
                    result.Text = state.ToString();
                    result.SelectionStart = result.TextLength;
                }
            }

            result.Parent = Grid;
            result.Show();
            result.Focus();

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
