// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextBoxWriter.cs -- TextWriter that writes to TextBox.
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="T:System.IO.TextWriter"/> that writes to
    /// <see cref="T:System.Windows.Forms.TextBox"/>.
    /// </summary>
    [PublicAPI]
    public class TextBoxWriter
        : TextWriter
    {
        #region Events

        /// <summary>
        /// Fired when scrolling occurs.
        /// </summary>
        public EventHandler Scroll;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether auto scroll
        /// is enabled.
        /// </summary>
        [DefaultValue(false)]
        public bool AutoScroll { get; set; }

        /// <summary>
        /// Gets the text box.
        /// </summary>
        /// <value>The text box.</value>
        public TextBoxBase TextBox { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TextBoxWriter"/> class.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        public TextBoxWriter
            (
                [NotNull] TextBoxBase textBox
            )
        {
            Code.NotNull(textBox, "textBox");

            TextBox = textBox;
            TextBox.Disposed += _textBox_Disposed;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup 
        /// operations before the
        /// <see cref="TextBoxWriter"/> is reclaimed 
        /// by garbage collection.
        /// </summary>
        ~TextBoxWriter()
        {
            Dispose();
        }

        #endregion

        #region Private members

        private bool _disposed;
        private bool _textBoxDisposed;

        private void _CheckDisposed()
        {
            if (_disposed || _textBoxDisposed)
            {
                throw new ObjectDisposedException("TextBoxWriter");
            }
        }

        private void _textBox_Disposed(object sender, EventArgs e)
        {
            _textBoxDisposed = true;
        }

        /// <summary>
        /// Called when scrolling occurs.
        /// </summary>
        protected virtual void OnScroll()
        {
            EventHandler handler = Scroll;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
            TextBox.SelectionStart = TextBox.TextLength;
            TextBox.SelectionLength = 0;
        }

        #endregion

        #region TextWriter members

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.TextWriter"></see> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            TextBox.Disposed -= _textBox_Disposed;
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// When overridden in a derived class, returns the <see cref="T:System.Text.Encoding"></see> in which the output is written.
        /// </summary>
        public override Encoding Encoding
        {
            get
            {
                return Encoding.Unicode;
            }
        }

        /// <summary>
        /// Writes a character to the text stream.
        /// </summary>
        public override void Write
            (
                char value
            )
        {
            _CheckDisposed();
            TextBox.AppendText(new string(value, 1));
            OnScroll();
        }

        /// <summary>
        /// Writes a string to the text stream.
        /// </summary>
        /// <param name="value">The string to write.</param>
        public override void Write
            (
                string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                _CheckDisposed();
                TextBox.AppendText(value);
                OnScroll();
            }
        }

        /// <summary>
        /// Writes a line terminator to the text stream.
        /// </summary>
        public override void WriteLine()
        {
            _CheckDisposed();
            TextBox.AppendText(Environment.NewLine);
            OnScroll();
        }

        /// <summary>
        /// Writes a string followed by a line terminator to the text stream.
        /// </summary>
        public override void WriteLine
            (
                string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                _CheckDisposed();
                TextBox.AppendText(value + Environment.NewLine);
                OnScroll();
            }
        }

        #endregion
    }
}