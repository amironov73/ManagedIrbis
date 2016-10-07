/* LabeledComboBox.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class LabeledComboBox
        : UserControl
    {
        #region Events

        #endregion

        #region Properties

        /// <summary>
        /// Gets the combo box.
        /// </summary>
        /// <value>The combo box.</value>
        [NotNull]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ComboBox ComboBox
        {
            [DebuggerStepThrough]
            get
            {
                return _comboBox;
            }
        }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>The label.</value>
        [NotNull]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Label Label
        {
            [DebuggerStepThrough]
            get
            {
                return _label;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledComboBox"/> class.
        /// </summary>
        public LabeledComboBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Control members

        #endregion
    }
}
