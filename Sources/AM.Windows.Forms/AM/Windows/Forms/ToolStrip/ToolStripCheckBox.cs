/* ToolStripCheckBox.cs -- CheckBox that appears in ToolStrip
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="T:System.Windows.Forms.CheckBox"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
          | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripCheckBox
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Gets inner <see cref="T:System.Windows.Forms.CheckBox"/> 
        /// control.
        /// </summary>
        /// <value>Inner CheckBox control.</value>
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public CheckBox CheckBox
        {
            [DebuggerStepThrough]
            get
            {
                return (Control as CheckBox);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="ToolStripCheckBox"/> class.
        /// </summary>
        public ToolStripCheckBox()
            : base(new CheckBox())
        {
            CheckBox.BackColor = Color.Transparent;
        }

        #endregion

        #region ToolStripControlHost members

        /// <summary>
        /// Gets or sets the text to be displayed on the hosted control.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.String"/> 
        /// representing the text.</returns>
        [Bindable(true)]
        [DefaultValue(null)]
        [Localizable(true)]
        public override string Text
        {
            [DebuggerStepThrough]
            get
            {
                return CheckBox.Text;
            }
            [DebuggerStepThrough]
            set
            {
                CheckBox.Text = value;
            }
        }

        #endregion
    }
}