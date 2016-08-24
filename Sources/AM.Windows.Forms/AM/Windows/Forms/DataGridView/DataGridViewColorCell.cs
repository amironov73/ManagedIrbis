/* DataGridViewColorCell.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public class DataGridViewColorCell
        : DataGridViewCell,
          IDataGridViewEditingCell
    {
        #region Private members

        private bool _valueChanged;

        #endregion

        #region Public methods

        /// <summary>
        /// Shows the editor.
        /// </summary>
        public void ShowEditor()
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.Color = (Color)Value;
                if (dialog.ShowDialog(DataGridView.FindForm())
                     == DialogResult.OK)
                {
                    _valueChanged = true;
                    DataGridView.NotifyCurrentCellDirty(true);
                    SetValue(RowIndex, dialog.Color);
                }
            }
        }

        #endregion

        #region DataGridViewCell members

        /// <summary>
        /// Paints the current <see cref="T:System.Windows.Forms.DataGridViewCell"/>.
        /// </summary>
        /// <param name="graphics">The <see cref="T:System.Drawing.Graphics"/> used to paint the <see cref="T:System.Windows.Forms.DataGridViewCell"/>.</param>
        /// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle"/> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView"/> that needs to be repainted.</param>
        /// <param name="cellBounds">A <see cref="T:System.Drawing.Rectangle"/> that contains the bounds of the <see cref="T:System.Windows.Forms.DataGridViewCell"/> that is being painted.</param>
        /// <param name="rowIndex">The row index of the cell that is being painted.</param>
        /// <param name="cellState">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates"/> values that specifies the state of the cell.</param>
        /// <param name="value">The data of the <see cref="T:System.Windows.Forms.DataGridViewCell"/> that is being painted.</param>
        /// <param name="formattedValue">The formatted data of the <see cref="T:System.Windows.Forms.DataGridViewCell"/> that is being painted.</param>
        /// <param name="errorText">An error message that is associated with the cell.</param>
        /// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle"/> that contains formatting and style information about the cell.</param>
        /// <param name="advancedBorderStyle">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle"/> that contains border styles for the cell that is being painted.</param>
        /// <param name="paintParts">A bitwise combination of the <see cref="T:System.Windows.Forms.DataGridViewPaintParts"/> values that specifies which parts of the cell need to be painted.</param>
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
            using (Brush brush = new SolidBrush(backColor))
            {
                graphics.FillRectangle(brush, cellBounds);
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
            Rectangle colorRectangle = cellBounds;
            colorRectangle.Inflate(-2, -2);
            if ((cellState & DataGridViewElementStates.ReadOnly)
                == DataGridViewElementStates.None)
            {
                Rectangle buttonRectangle = new Rectangle
                    (
                    colorRectangle.Left + colorRectangle.Width - colorRectangle.Height,
                    colorRectangle.Top,
                    colorRectangle.Height,
                    colorRectangle.Height
                    );
                colorRectangle.Width = buttonRectangle.Left - 2 - colorRectangle.Left;
                ComboBoxState state = ComboBoxState.Normal;
                if ((cellState & DataGridViewElementStates.Selected)
                     != DataGridViewElementStates.None)
                {
                    state = ComboBoxState.Hot;
                }
                ComboBoxRenderer.DrawDropDownButton
                    (
                    graphics,
                    buttonRectangle,
                    state
                    );
            }
            if (Value is Color)
            {
                Color color = (Color)Value;
                using (Brush brush = new SolidBrush(color))
                {
                    graphics.FillRectangle(brush, colorRectangle);
                }
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

        /// <summary>
        /// Gets the default value for a cell in the row for new records.
        /// </summary>
        /// <value></value>
        /// <returns>An <see cref="T:System.Object"></see> representing the default value.</returns>
        public override object DefaultNewRowValue
        {
            get
            {
                return Color.Black;
            }
        }

        /// <summary>
        /// Gets or sets the data type of the values in the cell.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"></see> representing the data type of the value in the cell.</returns>
        public override Type ValueType
        {
            get
            {
                return base.ValueType ?? typeof(Color);
            }
            set
            {
                base.ValueType = value;
            }
        }

        #region IDataGridViewEditingCell members

        ///<summary>
        ///Retrieves the formatted value of the cell.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Object"/> that represents the formatted version of the cell contents.
        ///</returns>
        ///
        ///<param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts"/> values that specifies the context in which the data is needed.</param>
        public object GetEditingCellFormattedValue
            (
                DataGridViewDataErrorContexts context
            )
        {
            return Value;
        }

        ///<summary>
        ///Prepares the currently selected cell for editing
        ///</summary>
        ///
        ///<param name="selectAll">true to select the cell contents; otherwise, false.</param>
        public void PrepareEditingCellForEdit(bool selectAll)
        {
            // throw new NotImplementedException ();
        }

        ///<summary>
        ///Gets or sets the formatted value of the cell.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Object"/> that contains the cell's value.
        ///</returns>
        ///
        public object EditingCellFormattedValue
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
            }
        }

        ///<summary>
        ///Gets or sets a value indicating whether the value of the cell has changed.
        ///</summary>
        ///
        ///<returns>
        ///true if the value of the cell has changed; otherwise, false.
        ///</returns>
        ///
        public bool EditingCellValueChanged
        {
            get
            {
                return _valueChanged;
            }
            set
            {
                _valueChanged = value;
            }
        }

        #endregion

        /// <summary>
        /// Called when the cell is clicked.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs"></see> that contains the event data.</param>
        protected override void OnClick
            (
                DataGridViewCellEventArgs e
            )
        {
            base.OnClick(e);
            if (!ReadOnly)
            {
                ShowEditor();
            }
        }

        #endregion
    }
}