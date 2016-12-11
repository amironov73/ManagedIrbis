// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CalendarEditingControl.cs -- in-place editor for DataGridViewCalendarCell
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
    /// In-place editor for <see cref="DataGridViewCalendarCell"/>.
    /// </summary>
    /// <remarks>Stolen from MSDN article
    /// "How to: Host Controls in Windows Forms DataGridView Cells"
    /// </remarks>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    [System.ComponentModel.ToolboxItem(false)]
    public class DataGridViewCalendarEditingControl
        : DateTimePicker,
          IDataGridViewEditingControl
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="DataGridViewCalendarEditingControl"/> class.
        /// </summary>
        public DataGridViewCalendarEditingControl()
        {
            Format = DateTimePickerFormat.Short;
        }

        #endregion

        #region Private members

        #endregion

        #region IDataGridViewEditingControl members

        /// <summary>
        /// Gets or sets the formatted value of the cell being 
        /// modified by the editor.
        /// </summary>
        public object EditingControlFormattedValue
        {
            get
            {
                return Value.ToShortDateString();
            }
            set
            {
                string newValue = value as string;
                if (newValue != null)
                {
                    Value = DateTime.Parse(newValue);
                }
            }
        }

        /// <summary>
        /// Retrieves the formatted value of the cell.
        /// </summary>
        public object GetEditingControlFormattedValue
            (
                DataGridViewDataErrorContexts context
            )
        {
            return EditingControlFormattedValue;
        }

        /// <summary>
        /// Changes the control's user interface (UI) to be consistent with 
        /// the specified cell style.
        /// </summary>
        public void ApplyCellStyleToEditingControl
            (
                DataGridViewCellStyle dataGridViewCellStyle
            )
        {
            Font = dataGridViewCellStyle.Font;
            CalendarForeColor = dataGridViewCellStyle.ForeColor;
            CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }

        /// <summary>
        /// Gets or sets the index of the hosting cell's parent row.
        /// </summary>
        /// <value></value>
        /// <returns>The index of the row that contains the cell, 
        /// or –1 if there is no parent row.</returns>
        public int EditingControlRowIndex { get; set; }

        /// <summary>
        /// The control wants input key?
        /// </summary>
        public bool EditingControlWantsInputKey
            (
                Keys key,
                bool dataGridViewWantsInputKey
            )
        {
            // Let the DateTimePicker handle the keys listed.
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Prepares the currently selected cell for editing.
        /// </summary>
        public void PrepareEditingControlForEdit
            (
                bool selectAll
            )
        {
            // No preparation needs to be done.
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cell contents need 
        /// to be repositioned whenever the value changes.
        /// </summary>
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DataGridView"/> that contains the cell.
        /// </summary>
        public DataGridView EditingControlDataGridView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value of the editing 
        /// control differs from the value of the hosting cell.
        /// </summary>
        public bool EditingControlValueChanged { get; set; }

        /// <summary>
        /// Gets the cursor used when the mouse pointer
        /// is over the <see cref="P:System.Windows.Forms.DataGridView.EditingPanel" /> but not over the editing control.
        /// </summary>
        public Cursor EditingPanelCursor
        {
            get { return Cursor; }
        }

        /// <summary>
        /// Raises the 
        /// <see cref="E:System.Windows.Forms.DateTimePicker.ValueChanged"/> 
        /// event.
        /// </summary>
        protected override void OnValueChanged
            (
                EventArgs eventArgs
            )
        {
            EditingControlValueChanged = true;
            EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventArgs);
        }

        #endregion
    }
}