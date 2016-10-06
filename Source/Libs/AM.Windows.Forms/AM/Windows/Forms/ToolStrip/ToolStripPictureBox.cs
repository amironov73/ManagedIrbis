/* ToolStripPictureBox.cs -- PictureBox that appears in ToolStrip
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
    /// <see cref="T:System.Windows.Forms.PictureBox"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
        | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripPictureBox
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Gets the PictureBox.
        /// </summary>
        [NotNull]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public PictureBox PictureBox
        {
            [DebuggerStepThrough]
            get
            {
                return (PictureBox) Control;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="ToolStripPictureBox"/> class.
        /// </summary>
        public ToolStripPictureBox()
            : base(new PictureBox())
        {
            PictureBox.BackColor = Color.Transparent;
        }

        #endregion

        #region Control members

        /// <summary>
        /// Gets or sets the size of the 
        /// <see cref="T:System.Windows.Forms.ToolStripItem"/>.
        /// </summary>
        /// <returns>
        /// An ordered pair of type 
        /// <see cref="T:System.Drawing.Size"/> representing 
        /// the width and height of a rectangle.
        ///</returns>
        public override Size Size
        {
            [DebuggerStepThrough]
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
                if (PictureBox != null)
                {
                    PictureBox.Location = new Point
                    (
                        (Size.Width - PictureBox.Size.Width) / 2,
                        (Size.Height - PictureBox.Size.Height) / 2
                    );
                }
            }
        }

        #endregion
    }
}