/* PictureViewForm.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using AM.Drawing;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class PictureViewForm
        : Form
    {
        #region Events

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public Image Image
        {
            [DebuggerStepThrough]
            get
            {
                return _pictureBox.Image;
            }
            set
            {
                _SetImage(value);
            }
        }

        private PictureViewMode _mode = PictureViewMode.Auto;

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        public PictureViewMode Mode
        {
            [DebuggerStepThrough]
            get
            {
                return _mode;
            }
            [DebuggerStepThrough]
            set
            {
                _mode = value;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PictureViewForm"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        public PictureViewForm
            (
                [CanBeNull] Image image
            )
        {
            InitializeComponent();
            _pictureBox.Image = image;
        }

        #endregion

        #region Private members

        private void _SetImage(Image image)
        {
            _pictureBox.Image = image;
        }

        private void _copyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(_pictureBox.Image);
        }

        private void _openButton_Click(object sender, EventArgs e)
        {
            if (_openFileDialog.ShowDialog(this)
                 == DialogResult.OK)
            {
                _pictureBox.Image
                    = ImageUtility.LoadFromFile(_openFileDialog.FileName);
            }
        }

        private void _saveButton_Click(object sender, EventArgs e)
        {
            if (_saveFileDialog.ShowDialog(this)
                 == DialogResult.OK)
            {
                _pictureBox.Image.Save(_saveFileDialog.FileName);
            }
        }

        private void _pasteButton_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                _pictureBox.Image = Clipboard.GetImage();
            }
        }

        private void _printSetupButton_Click(object sender, EventArgs e)
        {
            _pageSetupDialog.ShowDialog(this);
        }

        private void _printPreviewButton_Click(object sender, EventArgs e)
        {
            _picturePrinter.Image = _pictureBox.Image;
            _picturePrinter.Preview();
        }

        private void _printButton_Click(object sender, EventArgs e)
        {
            _picturePrinter.Image = _pictureBox.Image;
            _picturePrinter.Print();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Shows the specified parent.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static PictureViewForm Show
            (
                [CanBeNull] IWin32Window parent,
                [CanBeNull] Image image
            )
        {
            PictureViewForm result = new PictureViewForm(image);
            result.Show(parent);

            return result;
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="image">The image.</param>
        public static void ShowDialog
            (
                [CanBeNull] IWin32Window parent,
                [CanBeNull] Image image
            )
        {
            using (PictureViewForm pvf = new PictureViewForm(image))
            {
                pvf.ShowDialog(parent);
            }
        }

        #endregion
    }
}
