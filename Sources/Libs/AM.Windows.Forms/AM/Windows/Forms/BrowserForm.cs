/* BrowserForm.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BrowserForm
        : Form
    {
        #region Events

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the document text.
        /// </summary>
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DocumentText
        {
            [DebuggerStepThrough]
            get
            {
                return _webBrowser.DocumentText;
            }
            [DebuggerStepThrough]
            set
            {
                //_webBrowser.Navigate ( "about:blank" );
                //_webBrowser.Document.Write ( value );
                _webBrowser.DocumentText = value;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="BrowserForm"/> class.
        /// </summary>
        public BrowserForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private void _copyButton_Click(object sender, System.EventArgs e)
        {
            Clipboard.SetText(DocumentText, TextDataFormat.Html);
        }

        private void _openButton_Click(object sender, System.EventArgs e)
        {
            if (_openFileDialog.ShowDialog(this)
                 == DialogResult.OK)
            {
                _webBrowser.Navigate(_openFileDialog.FileName);
            }
        }

        private void _saveButton_Click(object sender, System.EventArgs e)
        {
            if (_saveFileDialog.ShowDialog(this)
                 == DialogResult.OK)
            {
                File.WriteAllText(_saveFileDialog.FileName, DocumentText);
            }
        }

        private void _pageSetupButton_Click(object sender, System.EventArgs e)
        {
            _webBrowser.ShowPageSetupDialog();
        }

        private void _pasteButton_Click(object sender, System.EventArgs e)
        {
            string text = Clipboard.GetText(TextDataFormat.Html)
                          ?? Clipboard.GetText(TextDataFormat.UnicodeText)
                          ?? Clipboard.GetText();

            if (!string.IsNullOrEmpty(text))
            {
                DocumentText = text;
            }
        }

        private void _printButton_Click(object sender, System.EventArgs e)
        {
            _webBrowser.ShowPrintDialog();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <returns></returns>
        public HtmlDocument GetDocument()
        {
            if (_webBrowser.Document == null)
            {
                _webBrowser.Navigate("about:blank");
            }
            return _webBrowser.Document;
        }

        #endregion
    }
}