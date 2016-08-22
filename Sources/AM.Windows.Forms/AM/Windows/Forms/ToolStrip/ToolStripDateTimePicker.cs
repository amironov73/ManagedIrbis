/* ToolStripDateTimePicker.cs -- DateTimePicker that appears in ToolStrip.
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
    /// <see cref="T:System.Windows.Forms.DateTimePicker"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
          | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripDateTimePicker
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Gets the date time picker.
        /// </summary>
        /// <value>The date time picker.</value>
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public DateTimePicker DateTimePicker
        {
            [DebuggerStepThrough]
            get
            {
                return (Control as DateTimePicker);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ToolStripDateTimePicker"/> class.
        /// </summary>
        public ToolStripDateTimePicker()
            : base(new DateTimePicker())
        {
        }

        #endregion
    }
}