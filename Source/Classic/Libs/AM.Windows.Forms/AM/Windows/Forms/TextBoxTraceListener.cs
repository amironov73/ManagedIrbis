// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextBoxTraceListener.cs -- trace listener that uses TextBox
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="TraceListener"/> that uses <see cref="TextBox"/>
    /// to write trace messages.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class TextBoxTraceListener
        : TraceListener
    {
        #region Properties

        private TextBox _textBox;

        /// <summary>
        /// Gets the text box used to write trace messages.
        /// </summary>
        /// <value>The text box used.</value>
        public TextBox TextBox
        {
            [DebuggerStepThrough]
            get
            {
                return _textBox;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="TextBoxTraceListener"/> class.
        /// </summary>
        public TextBoxTraceListener
            (
                [NotNull] TextBox textBox
            )
        {
            Code.NotNull(textBox, "textBox");

            _textBox = textBox;
            _textBox.Disposed += _textBox_Disposed;
        }

        #endregion

        #region Private members

        private bool _disposed;

        /// <summary>
        /// Handles the Disposed event of the _textBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> 
        /// instance containing the event data.</param>
        private void _textBox_Disposed(object sender, EventArgs e)
        {
            _disposed = true;
            Trace.Listeners.Remove(this);
        }

        #endregion

        #region TraceListener members

        /// <summary>
        /// When overridden in a derived class, writes the specified 
        /// message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write. 
        /// </param>
        public override void Write(string message)
        {
            if (!_disposed
                 && (TextBox != null))
            {
                if (TextBox.InvokeRequired)
                {
                    TextBox.Invoke((MethodInvoker)delegate { Write(message); });
                }
                else
                {
                    TextBox.AppendText(message);
                    TextBox.SelectionStart = TextBox.TextLength;
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to 
        /// the listener you create in the derived class, followed by 
        /// a line terminator.
        /// </summary>
        /// <param name="message">A message to write.
        /// </param>
        public override void WriteLine(string message)
        {
            if (!_disposed
                 && (TextBox != null))
            {
                if (TextBox.InvokeRequired)
                {
                    TextBox.Invoke((MethodInvoker)delegate { WriteLine(message); });
                }
                else
                {
                    TextBox.AppendText(message + Environment.NewLine);
                    TextBox.SelectionStart = TextBox.TextLength;
                }
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the 
        /// <see cref="TraceListener"/> and optionally releases 
        /// the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both 
        /// managed and unmanaged resources; false to release 
        /// only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && !_disposed)
            {
                _disposed = true;
                TextBox.Dispose();
            }
        }

        #endregion
    }
}
