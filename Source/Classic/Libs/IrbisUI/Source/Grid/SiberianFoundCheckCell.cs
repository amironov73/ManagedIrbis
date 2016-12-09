// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianFoundCheckCell.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

/* SiberianFoundMfnCell.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using JetBrains.Annotations;

using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianFoundCheckCell
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

            if (ReferenceEquals(this, Grid.CurrentCell))
            {
                Color backColor = Color.Blue;
                using (Brush brush = new SolidBrush(backColor))
                {
                    graphics.FillRectangle(brush, rectangle);
                }
            }

            FoundLine found = (FoundLine)Row.Data;

            if (!ReferenceEquals(found, null))
            {
                CheckBoxState state = found.Selected
                    ? CheckBoxState.CheckedNormal
                    : CheckBoxState.UncheckedNormal;

                Point point = new Point
                    (
                        rectangle.X + 2,
                        rectangle.Y + 2
                    );

                CheckBoxRenderer.DrawCheckBox
                    (
                        graphics,
                        point,
                        state
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

            FoundLine found = (FoundLine)Row.Data;
            string text = string.Empty;
            if (!ReferenceEquals(found, null))
            {
                text = string.Format
                    (
                        "{0}: {1}",
                        found.Mfn,
                        found.Selected
                    );
            }

            return string.Format
                (
                    "FoundCheckCell [{0}, {1}]: {2}",
                    column,
                    row,
                    text
                );
        }

        #endregion
    }
}
