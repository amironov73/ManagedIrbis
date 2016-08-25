/* DataGridViewNumericColumn.cs -- 
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
    public class DataGridViewNumericColumn
        : DataGridViewColumn
    {
        #region Properties

        /// <summary>
        /// Gets or sets the decimal points.
        /// </summary>
        /// <value>The decimal points.</value>
        public int DecimalPoints
        {
            get
            {
                return ((DataGridViewNumericCell)CellTemplate).DecimalPoints;
            }
            set
            {
                ((DataGridViewNumericCell)CellTemplate).DecimalPoints = value;
            }
        }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public decimal DefaultValue
        {
            get
            {
                return ((DataGridViewNumericCell)CellTemplate).DefaultValue;
            }
            set
            {
                ((DataGridViewNumericCell)CellTemplate).DefaultValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public decimal Maximum
        {
            get
            {
                return ((DataGridViewNumericCell)CellTemplate).Maximum;
            }
            set
            {
                ((DataGridViewNumericCell)CellTemplate).Maximum = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public decimal Minimum
        {
            get
            {
                return ((DataGridViewNumericCell)CellTemplate).Minimum;
            }
            set
            {
                ((DataGridViewNumericCell)CellTemplate).Minimum = value;
            }
        }


        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DataGridViewNumericColumn()
            : base(new DataGridViewNumericCell())
        {
            // CellTemplate = new DataGridViewNumericCell ();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region DataGridViewColumn members

        ///<summary>
        ///Gets or sets the template used to create new cells.
        ///</summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                // return _cellTemplate;
                return base.CellTemplate;
            }
            set
            {
                if ((value != null)
                     && !(value is DataGridViewNumericCell))
                {
                    throw new InvalidCastException();
                }
                base.CellTemplate = value;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public override object Clone()
        {
            DataGridViewColumn result = (DataGridViewColumn)base.Clone();
            DataGridViewNumericColumn clone
                = result as DataGridViewNumericColumn;
            // result.CellTemplate = (DataGridViewCell) CellTemplate.Clone ();
            if (clone != null)
            {
                clone.DecimalPoints = DecimalPoints;
                clone.Maximum = Maximum;
                clone.DefaultValue = DefaultValue;
                clone.Minimum = Minimum;
            }
            return result;
        }

        #endregion

    }
}