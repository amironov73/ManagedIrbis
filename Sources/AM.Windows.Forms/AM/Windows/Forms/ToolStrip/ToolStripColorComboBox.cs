/* ToolStripColorComboBox.cs -- ComboBox that appears in ToolStrip
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="T:System.Windows.Forms.ComboBox"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
          | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripColorComboBox
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Gets the ColorComboBox.
        /// </summary>
        [NotNull]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public ColorComboBox ColorComboBox
        {
            [DebuggerStepThrough]
            get
            {
                return (ColorComboBox) Control;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ToolStripColorComboBox"/> class.
        /// </summary>
        public ToolStripColorComboBox()
            : base(new ColorComboBox())
        {
        }

        #endregion
    }
}