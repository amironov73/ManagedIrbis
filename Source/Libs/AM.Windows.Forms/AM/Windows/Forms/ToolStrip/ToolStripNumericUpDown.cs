/* ToolStripNumericUpDown.cs -- NumericUpDown that appears in ToolStrip
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
    /// <see cref="T:System.Windows.Forms.NumericUpDown"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
          | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripNumericUpDown
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Gets the NumericUpDown.
        /// </summary>
        [NotNull]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public NumericUpDown NumericUpDown
        {
            [DebuggerStepThrough]
            get
            {
                return (NumericUpDown) Control;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ToolStripNumericUpDown"/> class.
        /// </summary>
        public ToolStripNumericUpDown()
            : base(new NumericUpDown())
        {
        }

        #endregion
    }
}