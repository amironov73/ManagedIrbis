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

        /// <summary>
        /// Gets the dialog.
        /// </summary>
        /// <value>The dialog.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public OpenFileDialog Dialog
        {
            [DebuggerStepThrough]
            get
            {
                return _dialog;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FileNameBox()
        {
            _dialog = new OpenFileDialog();
            Button.Click += _Button_Click;
        }

        #endregion

        #region Private members

        private OpenFileDialog _dialog;

        void _Button_Click(object sender, EventArgs e)
        {
            Dialog.FileName = Text;
            if (Dialog.ShowDialog(Parent) == DialogResult.OK)
            {
                Text = Dialog.FileName;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Releases the unmanaged resources used by the
        /// <see cref="T:System.Windows.Forms.TextBox"/>
        /// and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release
        /// both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        protected override void Dispose
            (
                bool disposing
            )
        {
            base.Dispose(disposing);
            if (_dialog != null)
            {
                _dialog.Dispose();
                _dialog = null;
            }
        }

        #endregion
    }
}
