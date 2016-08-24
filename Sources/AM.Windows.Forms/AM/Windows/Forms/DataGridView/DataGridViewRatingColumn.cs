/* DataGridViewRatingColumn.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
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
    public class DataGridViewRatingColumn
        : DataGridViewColumn
    {
        #region Properties

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        public int Maximum
        {
            get
            {
                return ((DataGridViewRatingCell)CellTemplate).Maximum;
            }
            set
            {
                ((DataGridViewRatingCell)CellTemplate).Maximum = value;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DataGridViewRatingColumn()
            : base(new DataGridViewRatingCell())
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods
        
        #endregion

        #region DataGridViewColumn members

        #endregion
    }
}
