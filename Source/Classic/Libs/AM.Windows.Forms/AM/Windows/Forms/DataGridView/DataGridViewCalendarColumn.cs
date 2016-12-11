// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CalendarColumn.cs -- column of DataGridViewCalendarCell's
   Ars Magna project, http://library.istu.edu/am */

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
    /// Column of <see cref="DataGridViewCalendarCell"/>'s.
    /// </summary>
    /// <remarks>Stolen from MSDN article
    /// "How to: Host Controls in Windows Forms DataGridView Cells"
    /// </remarks>
    [PublicAPI]
    public class DataGridViewCalendarColumn
        : DataGridViewColumn
    {
        #region Properties

        /// <summary>
        /// Gets or sets the format of date.
        /// </summary>
        /// <value>Format of date.</value>
        public string Format
        {
            [DebuggerStepThrough]
            get
            {
                return ((DataGridViewCalendarCell)CellTemplate).Format;
            }
            [DebuggerStepThrough]
            set
            {
                ((DataGridViewCalendarCell)CellTemplate).Format = value;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="DataGridViewCalendarColumn"/> class.
        /// </summary>
        public DataGridViewCalendarColumn()
            : base(new DataGridViewCalendarCell())
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region DataGridViewColumn members

        /// <summary>
        /// Gets or sets the template used to create new cells.
        /// </summary>
        /// <value></value>
        /// <returns>A 
        /// <see cref="T:System.Windows.Forms.DataGridViewCell"/>
        /// that all other cells in the column are modeled after. 
        /// The default is null.</returns>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (!(value is DataGridViewCalendarCell))
                {
                    throw new InvalidCastException("Must be a CalendarCell");
                }
                base.CellTemplate = value;
            }
        }

        /// <summary>
        /// Creates an exact copy of this column.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the cloned 
        /// <see cref="DataGridViewTextBoxColumn"></see>.
        /// </returns>
        public override object Clone()
        {
            object result = base.Clone();
            DataGridViewCalendarColumn clone
                = result as DataGridViewCalendarColumn;
            if (clone != null)
            {
                clone.Format = Format;
            }
            return result;
        }

        #endregion
    }
}