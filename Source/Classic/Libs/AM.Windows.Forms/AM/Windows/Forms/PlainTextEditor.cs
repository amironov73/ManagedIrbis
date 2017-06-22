// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PlainTextEditor.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

using AM.Drawing.Printing;

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
    public partial class PlainTextEditor
        : UserControl
    {
        #region Properties

        [CanBeNull]
        private string FileName { get; set; }

        /// <inheritdoc />
        public override string Text
        {
            get { return _textBox.Text; }
            set { _textBox.Text = value; }
        }

        /// <summary>
        /// Text box.
        /// </summary>
        [NotNull]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TextBox TextBox
        {
            get { return _textBox; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlainTextEditor()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private void _newToolStripButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            Clear();
        }

        private void _openToolStripButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            LoadFromFile();
        }

        private void _saveToolStripButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            SaveToFile();
        }

        private void _printToolStripButton_Click
            (
                object sender, 
                EventArgs e
            )
        {
            Print();
        }

        private void _cutToolStripButton_Click
            (
                object sender, 
                EventArgs e
            )
        {
            Cut();
        }

        private void _copyToolStripButton_Click
            (
                object sender, 
                EventArgs e
            )
        {
            Copy();
        }

        private void _pasteToolStripButton_Click
            (
                object sender, 
                EventArgs e
            )
        {
            Paste();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add the button to the toolbox.
        /// </summary>
        [NotNull]
        public PlainTextEditor AddButton
            (
                [NotNull] ToolStripButton button
            )
        {
            Code.NotNull(button, "button");

            _toolStrip.Items.Add(button);

            return this;
        }

        /// <summary>
        /// Clear the text area.
        /// </summary>
        public void Clear()
        {
            _textBox.Clear();
        }

        /// <summary>
        /// Copy selected text to the clipboard.
        /// </summary>
        public void Copy()
        {
            _textBox.Copy();
        }

        /// <summary>
        /// Cut selected text to the clipboard.
        /// </summary>
        public void Cut()
        {
            _textBox.Cut();
        }

        /// <summary>
        /// Load text from file.
        /// </summary>
        public void LoadFromFile()
        {
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = _openFileDialog.FileName
                    .ThrowIfNull("FileName");

                LoadFromFile(FileName);
            }
        }

        /// <summary>
        /// Load text from the file.
        /// </summary>
        public void LoadFromFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            Text = File.ReadAllText(fileName, Encoding.UTF8);
        }

        /// <summary>
        /// Paste text from the clipboard.
        /// </summary>
        public void Paste()
        {
            _textBox.Paste();
        }

        /// <summary>
        /// Print the text.
        /// </summary>
        public void Print()
        {
            PlainTextPrinter printer = new PlainTextPrinter();
            printer.Print(Text);
        }

        /// <summary>
        /// Save the text to file.
        /// </summary>
        public void SaveToFile()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                if (_saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                FileName = _saveFileDialog.FileName
                    .ThrowIfNull("FileName");
            }
            SaveToFile(FileName);
        }

        /// <summary>
        /// Save the text to the file.
        /// </summary>
        public void SaveToFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            File.WriteAllText(fileName, Text, Encoding.UTF8);
        }

        #endregion
    }
}
