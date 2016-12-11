// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ToolStripOrdinaryButton.cs -- Button that appears in ToolStrip.
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
    /// <see cref="T:System.Windows.Forms.Button"/> that
    /// appears in <see cref="T:System.Windows.Forms.ToolStrip"/>.
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolStripItemDesignerAvailability
        (ToolStripItemDesignerAvailability.ToolStrip
        | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripOrdinaryButton
        : ToolStripControlHost
    {
        #region Properties

        /// <summary>
        /// Gets the Button.
        /// </summary>
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        [NotNull]
        public Button Button
        {
            [DebuggerStepThrough]
            get
            {
                return (Button) Control;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ToolStripOrdinaryButton()
            : base(new Button())
        {
        }

        #endregion
    }
}