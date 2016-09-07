/* ButtonedTextBox.cs -- TextBox with embedded button
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM.Win32;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="T:System.Windows.Forms.TextBox"/> 
    /// that contains embedded
    /// <see cref="T:System.Windows.Forms.Button"/>.
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    public class ButtonedTextBox
        : TextBoxControlHost
    {
        #region Properties

        /// <summary>
        /// Gets the embedded button.
        /// </summary>
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Content)]
        public Button Button
        {
            [DebuggerStepThrough]
            get
            {
                return Control as Button;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ButtonedTextBox()
            : base(new Button())
        {
            Button.Width = 16;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ButtonedTextBox
            (
                [NotNull] Button button
            )
            : base(button)
        {
        }

        #endregion

        #region Private members

        /// <summary>
        /// Gets the <see cref="T:System.Windows.Forms.TextBox"/>
        /// margins.
        /// </summary>
        /// <param name="parentSize">Size of the parent.</param>
        /// <param name="leftMargin">The left margin.</param>
        /// <param name="rightMargin">The right margin.</param>
        /// <returns>
        /// Calculated location and size of hosted control.
        /// </returns>
        protected override Rectangle GetMargins
            (
                Size parentSize,
                out int leftMargin,
                out int rightMargin
            )
        {
            leftMargin = 0;
            int buttonWidth = Button.Width;
            int buttonHeight = parentSize.Height - 3;
            int delta = 3;

            switch (Button.FlatStyle)
            {
                case FlatStyle.Flat:
                case FlatStyle.Popup:
                    buttonHeight--;
                    buttonWidth--;
                    delta++;
                    break;
            }

            rightMargin = buttonWidth + delta;

            Rectangle result =
                new Rectangle
                    (
                    Width - rightMargin,
                    0,
                    buttonWidth,
                    buttonHeight
                    );

            return result;
        }

        #endregion
    }
}
