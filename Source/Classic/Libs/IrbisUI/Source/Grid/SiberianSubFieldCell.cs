// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianSubFieldCell.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    public class SiberianSubFieldCell
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

        /// <inheritdoc />
        public override void CloseEditor
            (
                bool accept
            )
        {
            if (!ReferenceEquals(Grid.Editor, null))
            {
                if (accept)
                {
                    SiberianSubField subField 
                        = (SiberianSubField)Row.Data;
                    if (!ReferenceEquals(subField, null))
                    {
                        subField.Value = Grid.Editor.Text;
                    }
                }
            }

            base.CloseEditor(accept);
        }

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

            SiberianSubField subField = (SiberianSubField)Row.Data;

            if (!ReferenceEquals(subField, null))
            {
                string text = subField.Value;

                if (!string.IsNullOrEmpty(text))
                {

                    TextFormatFlags flags
                        = TextFormatFlags.TextBoxControl
                          | TextFormatFlags.EndEllipsis
                          | TextFormatFlags.NoPrefix
                          | TextFormatFlags.VerticalCenter;

                    TextRenderer.DrawText
                        (
                            graphics,
                            text,
                            Grid.Font,
                            rectangle,
                            foreColor,
                            flags
                        );
                }
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

            SiberianSubField subField = (SiberianSubField)Row.Data;
            string text = string.Empty;
            if (!ReferenceEquals(subField, null))
            {
                text = string.Format
                    (
                        "{0}: {1} ({2})",
                        subField.Code,
                        subField.Value,
                        subField.OriginalValue
                    );
            }

            return string.Format
                (
                    "SubFieldCell [{0}, {1}]: {2}",
                    column,
                    row,
                    text
                );
        }

        #endregion
    }
}
