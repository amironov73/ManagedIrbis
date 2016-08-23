/* ToolStripTrackBar.cs -- TrackBar that appears in ToolStrip 
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

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="T:System.Windows.Forms.TrackBar"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
          | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripTrackBar
        : ToolStripControlHost
    {
        #region Nested classes

        class TrackBarWithTransparentBackground
            : TrackBar
        {
            public TrackBarWithTransparentBackground()
            {
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the track bar.
        /// </summary>
        /// <value>The track bar.</value>
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public TrackBar TrackBar
        {
            [DebuggerStepThrough]
            get
            {
                return (Control as TrackBar);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="ToolStripTrackBar"/> class.
        /// </summary>
        public ToolStripTrackBar()
            : base(new TrackBarWithTransparentBackground())
        {
            // TODO support transparent background
            TrackBar.BackColor = Color.Transparent;
            TrackBar.BackColor = SystemColors.InactiveCaption;
            TrackBar.AutoSize = false;
            AutoSize = false;
        }

        #endregion
    }
}