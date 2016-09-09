/* PlainTextEditor.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
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

        private string FileName { get; set; }

        /// <inheritdoc />
        public override string Text
        {
            get { return _textBox.Text; }
            set { _textBox.Text = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlainTextEditor ()
        {
            InitializeComponent ();
        }

        #endregion

        #region Private members

        private void _newToolStripButton_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void _openToolStripButton_Click(object sender, EventArgs e)
        {
            LoadFromFile();
        }

        private void _saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveToFile();
        }

        private void _printToolStripButton_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void _cutToolStripButton_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void _copyToolStripButton_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void _pasteToolStripButton_Click(object sender, EventArgs e)
        {
            Paste();
        }

        #endregion

        #region Public methods

        public void Clear()
        {
            _textBox.Clear();
        }

        public void Copy()
        {
            _textBox.Copy();
        }

        public void Cut()
        {
            _textBox.Cut();
        }

        public void LoadFromFile()
        {
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = _openFileDialog.FileName;
                LoadFromFile(FileName);
            }
        }

        public void LoadFromFile(string fileName)
        {
            Text = File.ReadAllText(fileName, Encoding.UTF8);
        }

        public void Paste()
        {
            _textBox.Paste();
        }

        public void Print()
        {
            PlainTextPrinter printer = new PlainTextPrinter();
            printer.Print(Text);
        }

        public void SaveToFile()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                if (_saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                FileName = _saveFileDialog.FileName;
            }
            SaveToFile(FileName);
        }

        public void SaveToFile(string fileName)
        {
            File.WriteAllText(fileName, Text, Encoding.UTF8);
        }

        #endregion
    }
}
