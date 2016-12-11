// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DataGridViewRatingCell.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using RC=AM.Windows.Forms.Properties.Resources;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public class DataGridViewRatingCell
        : DataGridViewCell
    {
        #region Properties

        private int _maximum = 5;

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public int Maximum
        {
            [DebuggerStepThrough]
            get
            {
                return _maximum;
            }
            [DebuggerStepThrough]
            set
            {
                _maximum = value;
            }
        }

        #endregion

        #region Public methods

        ///<summary>
        ///Paints the current <see cref="T:System.Windows.Forms.DataGridViewCell"></see>.
        ///</summary>
        ///
        ///<param name="formattedValue">The formatted data of the <see cref="T:System.Windows.Forms.DataGridViewCell"></see> that is being painted.</param>
        ///<param name="paintParts">A bitwise combination of the <see cref="T:System.Windows.Forms.DataGridViewPaintParts"></see> values that specifies which parts of the cell need to be painted.</param>
        ///<param name="advancedBorderStyle">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle"></see> that contains border styles for the cell that is being painted.</param>
        ///<param name="graphics">The <see cref="T:System.Drawing.Graphics"></see> used to paint the <see cref="T:System.Windows.Forms.DataGridViewCell"></see>.</param>
        ///<param name="errorText">An error message that is associated with the cell.</param>
        ///<param name="rowIndex">The row index of the cell that is being painted.</param>
        ///<param name="clipBounds">A <see cref="T:System.Drawing.Rectangle"></see> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView"></see> that needs to be repainted.</param>
        ///<param name="cellState">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates"></see> values that specifies the state of the cell.</param>
        ///<param name="value">The data of the <see cref="T:System.Windows.Forms.DataGridViewCell"></see> that is being painted.</param>
        ///<param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle"></see> that contains formatting and style information about the cell.</param>
        ///<param name="cellBounds">A <see cref="T:System.Drawing.Rectangle"></see> that contains the bounds of the <see cref="T:System.Windows.Forms.DataGridViewCell"></see> that is being painted.</param>
        protected override void Paint
            (
                Graphics graphics,
                Rectangle clipBounds,
                Rectangle cellBounds,
                int rowIndex,
                DataGridViewElementStates cellState,
                object value,
                object formattedValue,
                string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts
            )
        {
            Color backColor = cellStyle.BackColor;
            if ((cellState & DataGridViewElementStates.Selected)
                 != DataGridViewElementStates.None)
            {
                backColor = cellStyle.SelectionBackColor;
            }
            using (Brush backBrush = new SolidBrush(backColor))
            {
                graphics.FillRectangle(backBrush, cellBounds);
            }
            if ((paintParts & DataGridViewPaintParts.Border)
                 != DataGridViewPaintParts.None)
            {
                PaintBorder(graphics,
                              clipBounds,
                              cellBounds,
                              cellStyle,
                              advancedBorderStyle);
            }
            if (value is float)
            {
                float current = (float)value;
                int curInt = (int)current;
                float curFloat = current - curInt;
                for (int i = 0; i < Maximum; i++)
                {
                    Image image = RC.Star0;
                    if (curInt > i)
                    {
                        image = RC.Star2;
                    }
                    else if (curInt == i)
                    {
                        if (curFloat >= 0.49f)
                        {
                            image = RC.Star1;
                        }
                    }
                    graphics.DrawImageUnscaled
                        (
                        image,
                        cellBounds.Left + 2 + image.Width * i,
                        cellBounds.Top + (cellBounds.Height - image.Height) / 2
                        );
                }
                base.Paint
                    (
                    graphics,
                    clipBounds,
                    cellBounds,
                    rowIndex,
                    cellState,
                    value,
                    formattedValue,
                    errorText,
                    cellStyle,
                    advancedBorderStyle,
                    paintParts
                    );
            }
        }

        ///<summary>
        ///Gets the type of the formatted value associated with the cell. 
        ///</summary>
        public override Type FormattedValueType
        {
            get
            {
                return typeof(float);
            }
        }

        ///<summary>
        ///Gets or sets the data type of the values in the cell. 
        ///</summary>
        public override Type ValueType
        {
            get
            {
                return typeof(float);
            }
            set
            {
                base.ValueType = value;
            }
        }

        ///<summary>
        ///Gets the default value for a cell in the row for new records.
        ///</summary>
        public override object DefaultNewRowValue
        {
            get
            {
                return 0f;
            }
        }

        #endregion
    }
}
