/* DataGridViewNumericEditingControl.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.ToolboxItem(false)]
    [System.ComponentModel.DesignerCategory("Code")]
    public class DataGridViewNumericEditingControl
        : NumericUpDown,
          IDataGridViewEditingControl
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DataGridViewNumericEditingControl()
        {
            TabStop = false;
        }

        #endregion

        #region Private members

        private DataGridView _editingControlDataGridView;
        private int _editingControlRowIndex;
        private bool _editingControlValueChanged;

        #endregion

        #region Public methods

        #endregion

        #region NumericUpDown members

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.NumericUpDown.ValueChanged"></see> event.
        ///</summary>
        ///
        ///<param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data. </param>
        protected override void OnValueChanged
            (
                EventArgs e
            )
        {
            base.OnValueChanged(e);

            _editingControlValueChanged = true;
            EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }

        #endregion

        #region IDataGridViewEditingControl members

        ///<summary>
        ///Changes the control's user interface (UI) to be consistent with the specified cell style.
        ///</summary>
        ///
        ///<param name="dataGridViewCellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle"></see> to use as the model for the UI.</param>
        public void ApplyCellStyleToEditingControl
            (
                DataGridViewCellStyle dataGridViewCellStyle
            )
        {
            Font = dataGridViewCellStyle.Font;
            ForeColor = dataGridViewCellStyle.ForeColor;
            BackColor = dataGridViewCellStyle.BackColor;
        }

        ///<summary>
        ///Determines whether the specified key is a regular input key that the editing control should process or a special key that the <see cref="T:System.Windows.Forms.DataGridView"></see> should process.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified key is a regular input key that should be handled by the editing control; otherwise, false.
        ///</returns>
        ///
        ///<param name="keyData">A <see cref="T:System.Windows.Forms.Keys"></see> that represents the key that was pressed.</param>
        ///<param name="dataGridViewWantsInputKey">true when the <see cref="T:System.Windows.Forms.DataGridView"></see> wants to process the <see cref="T:System.Windows.Forms.Keys"></see> in keyData; otherwise, false.</param>
        public bool EditingControlWantsInputKey
            (
                Keys keyData,
                bool dataGridViewWantsInputKey
            )
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    //			case Keys.D0:
                    //			case Keys.D1:
                    //			case Keys.D2:
                    //			case Keys.D3:
                    //			case Keys.D4:
                    //			case Keys.D5:
                    //			case Keys.D6:
                    //			case Keys.D7:
                    //			case Keys.D8:
                    //			case Keys.D9:
                    return true;
            }
            return !dataGridViewWantsInputKey;
        }

        ///<summary>
        ///Retrieves the formatted value of the cell.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Object"></see> that represents the formatted version of the cell contents.
        ///</returns>
        ///
        ///<param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts"></see> values that specifies the context in which the data is needed.</param>
        public object GetEditingControlFormattedValue
            (
                DataGridViewDataErrorContexts context
            )
        {
            return EditingControlFormattedValue;
        }

        ///<summary>
        ///Prepares the currently selected cell for editing.
        ///</summary>
        ///
        ///<param name="selectAll">true to select all of the cell's content; otherwise, false.</param>
        public void PrepareEditingControlForEdit
            (
                bool selectAll
            )
        {
            if (selectAll)
            {
                Select(0, Text.Length);
            }
            else
            {
                Select(Text.Length, 0);
            }
        }

        ///<summary>
        /// Gets or sets the <see cref="T:System.Windows.Forms.DataGridView"></see> that contains the cell.
        ///</summary>
        ///
        ///<returns>
        /// The <see cref="T:System.Windows.Forms.DataGridView"></see> that contains the <see cref="T:System.Windows.Forms.DataGridViewCell"></see> that is being edited; null if there is no associated <see cref="T:System.Windows.Forms.DataGridView"></see>.
        ///</returns>
        ///
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return _editingControlDataGridView;
            }
            set
            {
                _editingControlDataGridView = value;
            }
        }

        ///<summary>
        ///Gets or sets the formatted value of the cell being modified by the editor.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Object"></see> that represents the formatted value of the cell.
        ///</returns>
        ///
        public object EditingControlFormattedValue
        {
            get
            {
                return Value.ToString();
            }
            set
            {
                if (value is decimal)
                {
                    Value = (decimal)value;
                }
                else if (value is string)
                {
                    Value = decimal.Parse((string)value);
                }
                else
                {
                    throw new InvalidCastException();
                }
            }
        }

        ///<summary>
        ///Gets or sets the index of the hosting cell's parent row.
        ///</summary>
        ///
        ///<returns>
        ///The index of the row that contains the cell, or –1 if there is no parent row.
        ///</returns>
        ///
        public int EditingControlRowIndex
        {
            get
            {
                return _editingControlRowIndex;
            }
            set
            {
                _editingControlRowIndex = value;
            }
        }

        ///<summary>
        ///Gets or sets a value indicating whether the value of the editing control differs from the value of the hosting cell.
        ///</summary>
        ///
        ///<returns>
        ///true if the value of the control differs from the cell value; otherwise, false.
        ///</returns>
        ///
        public bool EditingControlValueChanged
        {
            get
            {
                return _editingControlValueChanged;
            }
            set
            {
                _editingControlValueChanged = value;
            }
        }

        ///<summary>
        ///Gets the cursor used when the mouse pointer is over the <see cref="P:System.Windows.Forms.DataGridView.EditingPanel"></see> but not over the editing control.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Windows.Forms.Cursor"></see> that represents the mouse pointer used for the editing panel. 
        ///</returns>
        ///
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        ///<summary>
        ///Gets or sets a value indicating whether the cell contents need to be repositioned whenever the value changes.
        ///</summary>
        ///
        ///<returns>
        ///true if the contents need to be repositioned; otherwise, false.
        ///</returns>
        ///
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        #endregion
    }
}
