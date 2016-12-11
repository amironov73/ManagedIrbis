// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ToolStripRadioButton.cs -- RadioButton that appears in ToolStrip
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="T:System.Windows.Forms.RadioButton"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
          | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripRadioButton
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Gets the RadioButton.
        /// </summary>
        [NotNull]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public RadioButton RadioButton
        {
            [DebuggerStepThrough]
            get
            {
                return (RadioButton) Control;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="ToolStripRadioButton"/> class.
        /// </summary>
        public ToolStripRadioButton()
            : base(new RadioButton())
        {
        }

        #endregion
    }
}