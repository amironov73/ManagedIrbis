/* ToolStripMaskedTextBox.cs -- MaskedTextBox that appears in ToolStrip.
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
    /// <see cref="T:System.Windows.Forms.MaskedTextBox"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
          | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripMaskedTextBox
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Gets the MaskedTextBox.
        /// </summary>
        [NotNull]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public MaskedTextBox MaskedTextBox
        {
            [DebuggerStepThrough]
            get
            {
                return (MaskedTextBox) Control;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ToolStripMaskedTextBox()
            : base(new MaskedTextBox())
        {
        }

        #endregion
    }
}