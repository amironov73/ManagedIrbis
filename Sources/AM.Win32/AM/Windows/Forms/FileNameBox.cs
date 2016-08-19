/* FileNameBox.cs -- TextBox with button and OpenFileDialog
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
    /// TextBox with embedded Button and OpenFileDialog.
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    public class FileNameBox
        : ButtonedTextBox
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FileNameBox()
        {
            Button.Click += _Button_Click;
        }

        #endregion

        #region Private members

        void _Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                FileName = Text
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                Text = dialog.FileName;
            }
        }

        #endregion

        #region Public methods

        #endregion
    }
}
