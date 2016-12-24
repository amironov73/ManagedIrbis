// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AttachmentBox.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

// ReSharper disable CoVariantArrayConversion

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Shows list of attachments.
    /// </summary>
    public partial class AttachmentBox
        : Form
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AttachmentBox
            (
                [NotNull] IEnumerable<BinaryAttachment> attachments
            )
        {
            Code.NotNull(attachments, "attachments");

            InitializeComponent();

            _listBox.Items.AddRange(attachments.ToArray());
        }

        #endregion

        #region Private members

        private void _saveButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            BinaryAttachment attachment
                = _listBox.SelectedItem as BinaryAttachment;

            if (!ReferenceEquals(attachment, null))
            {
                DialogResult rc = _saveFileDialog.ShowDialog(this);
                if (rc == DialogResult.OK)
                {
                    string fileName = _saveFileDialog.FileName;
                    File.WriteAllBytes
                        (
                            fileName,
                            attachment.Content
                        );
                }
            }
        }

        #endregion
    }
}
