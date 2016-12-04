// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* .cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

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
    public class SiberianSubFieldColumn
        : SiberianColumn
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianSubFieldColumn()
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
            SiberianCell result = new SiberianSubFieldCell();
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

            SiberianSubFieldCell subFieldCell = (SiberianSubFieldCell)cell;

            SiberianSubField subField = (SiberianSubField)subFieldCell.Row.Data;
            if (ReferenceEquals(subField, null))
            {
                return null;
            }

            string text = subField.Value;

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
