/* SiberianTextColumn.cs -- 
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
    public class SiberianTextColumn
        : SiberianColumn
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianTextColumn()
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
            SiberianCell result = new SiberianTextCell();
            result.Column = this;

            return result;
        }

        /// <inheritdoc />
        public override Control CreateEditor
            (
                SiberianCell cell,
                bool edit
            )
        {
            Code.NotNull(cell, "cell");

            SiberianTextCell textCell = (SiberianTextCell)cell;

            Rectangle rectangle = Grid.GetCellRectangle(cell);
            rectangle.Inflate(-1,-1);

            TextBox result = new TextBox
            {
                AutoSize = false,
                Location = rectangle.Location,
                Size = rectangle.Size,
                Font = Grid.Font,
                //BorderStyle = BorderStyle.None
            };
            result.KeyDown += Editor_KeyDown;

            if (edit)
            {
                result.Text = textCell.Text;
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
