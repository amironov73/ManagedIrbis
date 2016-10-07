/* CalendarCell.cs -- DateTimePicker hosted in DataGridViewCell
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="DateTimePicker"/> hosted in
    /// <see cref="DataGridViewCell"/>.
    /// </summary>
    /// <remarks>Stolen from MSDN article
    /// "How to: Host Controls in Windows Forms DataGridView Cells"
    /// </remarks>
    [PublicAPI]
    public class DataGridViewCalendarCell
        : DataGridViewTextBoxCell
    {
        #region Properties

        /// <summary>
        /// Gets or sets the format of the date.
        /// </summary>
        /// <value>Format of the date.</value>
        public string Format
        {
            [DebuggerStepThrough]
            get
            {
                return Style.Format;
            }
            set
            {
                Style.Format = value;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="DataGridViewCalendarCell"/> class.
        /// </summary>
        public DataGridViewCalendarCell()
        {
            // Use the short date format.
            Style.Format = "d";
        }

        #endregion

        #region DataGridViewCell members

        /// <summary>
        /// Initializes the control used to edit the cell.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the 
        /// cell's location.</param>
        /// <param name="initialFormattedValue">An 
        /// <see cref="T:System.Object"/> that represents the value 
        /// displayed by the cell when editing is started.</param>
        /// <param name="dataGridViewCellStyle">A 
        /// <see cref="T:System.Windows.Forms.DataGridViewCellStyle"/> 
        /// that represents the style of the cell.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// There is no associated 
        /// <see cref="T:System.Windows.Forms.DataGridView"/> 
        /// or if one is present, it does not have an associated editing control.
        /// </exception>
        public override void InitializeEditingControl
            (
                int rowIndex,
                object initialFormattedValue,
                DataGridViewCellStyle dataGridViewCellStyle
            )
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl
                (
                    rowIndex,
                    initialFormattedValue,
                    dataGridViewCellStyle
                );
            DataGridViewCalendarEditingControl dataGridViewCalendarControl
                = DataGridView.EditingControl as DataGridViewCalendarEditingControl;
            if (dataGridViewCalendarControl != null)
            {
                dataGridViewCalendarControl.Value = (DateTime)Value;
            }
        }

        /// <summary>
        /// Gets the type of the cell's hosted editing control.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"/> 
        /// representing the <see cref="DataGridViewCalendarEditingControl"/> 
        /// type.</returns>
        public override Type EditType
        {
            get
            {
                // Return the type of the editing contol that CalendarCell uses.
                return typeof(DataGridViewCalendarEditingControl);
            }
        }

        /// <summary>
        /// Gets or sets the data type of the values in the cell.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"/> 
        /// representing the data type of the value in the cell.</returns>
        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains.
                return typeof(DateTime);
            }
        }

        /// <summary>
        /// Gets the default value for a cell in the row for new records.
        /// </summary>
        /// <value></value>
        /// <returns>An <see cref="T:System.Object"/> representing 
        /// the default value.</returns>
        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value.
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Creates an exact copy of this cell.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the cloned 
        /// <see cref="DataGridViewTextBoxCell"></see>.
        /// </returns>
        public override object Clone()
        {
            object result = base.Clone();
            DataGridViewCalendarCell clone = result as DataGridViewCalendarCell;
            if (clone != null)
            {
                clone.Format = Format;
            }
            return result;
        }

        #endregion
    }
}