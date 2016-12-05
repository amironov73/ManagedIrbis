// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianMenuCommentCell.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using JetBrains.Annotations;

using ManagedIrbis.Menus;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianMenuCommentCell
        : SiberianCell
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region SiberianCell members

        /// <inheritdoc/>
        public override void Paint
            (
                PaintEventArgs args
            )
        {
            Graphics graphics = args.Graphics;
            Rectangle rectangle = args.ClipRectangle;

            Color foreColor = Color.Black;
            if (ReferenceEquals(Row, Grid.CurrentRow))
            {
                foreColor = Color.White;
            }

            if (ReferenceEquals(this, Grid.CurrentCell))
            {
                Color backColor = Color.Blue;
                using (Brush brush = new SolidBrush(backColor))
                {
                    graphics.FillRectangle(brush, rectangle);
                }
            }

            MenuEntry entry = (MenuEntry)Row.Data;

            if (!ReferenceEquals(entry, null))
            {
                TextFormatFlags flags
                    = TextFormatFlags.TextBoxControl
                      | TextFormatFlags.EndEllipsis
                      | TextFormatFlags.NoPrefix
                      | TextFormatFlags.VerticalCenter;

                TextRenderer.DrawText
                    (
                        graphics,
                        entry.Comment,
                        Grid.Font,
                        rectangle,
                        foreColor,
                        flags
                    );
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            int row = ReferenceEquals(Row, null) ? -1 : Row.Index,
                column = ReferenceEquals(Column, null) ? -1 : Column.Index;
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            MenuEntry entry = (MenuEntry)Row.Data;
            string text = string.Empty;
            if (!ReferenceEquals(entry, null))
            {
                text = entry.Comment;
            }

            return string.Format
                (
                    "MenuCommentCell [{0}, {1}]: {2}",
                    column,
                    row,
                    text
                );
        }

        #endregion
    }
}
