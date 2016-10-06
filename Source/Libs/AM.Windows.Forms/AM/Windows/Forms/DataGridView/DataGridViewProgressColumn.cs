/* DataGridViewProgressColumn.cs -- 
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
    public class DataGridViewProgressColumn
        : DataGridViewColumn
    {
        #region Properties

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        [DefaultValue(100)]
        public int Maximum
        {
            get
            {
                return ((DataGridViewProgressCell)CellTemplate).Maximum;
            }
            set
            {
                ((DataGridViewProgressCell)CellTemplate).Maximum = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        [DefaultValue(0)]
        public int Minimum
        {
            get
            {
                return ((DataGridViewProgressCell)CellTemplate).Minimum;
            }
            set
            {
                ((DataGridViewProgressCell)CellTemplate).Minimum = value;
            }
        }


        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DataGridViewProgressColumn()
            : base(new DataGridViewProgressCell())
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
