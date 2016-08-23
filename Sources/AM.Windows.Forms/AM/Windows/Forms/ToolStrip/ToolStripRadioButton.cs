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
        /// Gets the radio button.
        /// </summary>
        /// <value>The radio button.</value>
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public RadioButton RadioButton
        {
            [DebuggerStepThrough]
            get
            {
                return (Control as RadioButton);
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