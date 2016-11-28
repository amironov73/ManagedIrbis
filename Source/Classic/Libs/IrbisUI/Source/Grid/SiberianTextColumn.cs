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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.IO;
using AM.Reflection;
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
            //BackColor = Color.White;
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
            SiberianCell result = new SiberianTextCell();
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

            SiberianTextCell textCell = (SiberianTextCell)cell;

            Rectangle rectangle = Grid.GetCellRectangle(cell);
            rectangle.Inflate(-1,-1);

            TextBox result = new TextBox
            {
                AutoSize = false,
                Location = rectangle.Location,
                Size = rectangle.Size,
                Font = Grid.Font,
                BorderStyle = BorderStyle.FixedSingle
            };
            result.KeyDown += Editor_KeyDown;

            if (edit)
            {
                result.Text = textCell.Text;
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

        /// <inheritdoc />
        public override void GetData
            (
                object theObject,
                SiberianCell cell
            )
        {
            SiberianTextCell textCell = (SiberianTextCell) cell;

            if (!string.IsNullOrEmpty(Member)
                && !ReferenceEquals(theObject, null))
            {
                Type type = theObject.GetType();
                MemberInfo memberInfo = type.GetMember(Member)
                    .First();
                PropertyOrField property = new PropertyOrField
                    (
                        memberInfo
                    );

                object value = property.GetValue(theObject);
                textCell.Text = ReferenceEquals(value, null)
                    ? null
                    : value.ToString();
            }
        }

        /// <inheritdoc />
        public override void PutData
            (
                object theObject,
                SiberianCell cell
            )
        {
            SiberianTextCell textCell = (SiberianTextCell)cell;

            if (!string.IsNullOrEmpty(Member)
                && !ReferenceEquals(theObject, null))
            {
                Type type = theObject.GetType();
                MemberInfo memberInfo = type.GetMember(Member)
                    .First();
                PropertyOrField property = new PropertyOrField
                    (
                        memberInfo
                    );

                property.SetValue(theObject, textCell.Text);
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
