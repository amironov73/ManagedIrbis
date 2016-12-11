// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListBoxTextWriter.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
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
    /// <see cref="TextWriter"/> that writes to
    /// <see cref="ListBox"/>.
    /// </summary>
    [PublicAPI]
    public sealed class ListBoxTextWriter
        : TextWriter
    {
        #region Properties

        private int _maxLineCount = 1000;

        ///<summary>
        /// Максимальное количество строк в списке.
        /// По мере появления "лишних" строк, самые верхние
        /// строки удаляются из списка.
        ///</summary>
        public int MaxLineCount
        {
            [DebuggerStepThrough]
            get
            {
                return _maxLineCount;
            }
            [DebuggerStepThrough]
            set
            {
                _maxLineCount = value;
            }
        }

        ///<summary>
        /// Скроллировать ли список к нижнему элементу 
        /// по мере заполнения списка.
        ///</summary>
        public bool ScrollToBottom { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ListBoxTextWriter
            (
                [NotNull] ListBox listBox
            )
        {
            Code.NotNull(listBox, "listBox");

            _listBox = listBox;
            _builder = new StringBuilder();
            _lines = new List<string>();
            _sync = new object();
        }

        #endregion

        #region Private members

        private object _sync;

        private ListBox _listBox;

        private StringBuilder _builder;

        private List<string> _lines;

        private void _NewLine()
        {
            if (_builder.Length > 0)
            {
                _lines.Add(_builder.ToString());
                _builder.Length = 0;
            }
        }
        private void _RemoveExcessLines()
        {
            while (_listBox.Items.Count > MaxLineCount)
            {
                _listBox.Items.RemoveAt(0);
            }
        }

        #endregion

        #region TextWriter members

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
        /// Clears all buffers for the current writer and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            if (_listBox == null)
            {
                return;
            }
            lock (_sync)
            {
                try
                {
                    _listBox.BeginUpdate();
                    foreach (string line in _lines)
                    {
                        _listBox.Items.Add(line);
                        _RemoveExcessLines();
                    }
                    _lines.Clear();
                    if (_builder.Length > 0)
                    {
                        _listBox.Items.Add(_builder.ToString());
                        _builder.Length = 0;
                    }
                    _RemoveExcessLines();
                    if ((ScrollToBottom)
                         && (_listBox.Items.Count > 0))
                    {
                        _listBox.SelectedIndex = _listBox.Items.Count - 1;
                    }
                }
                finally
                {
                    _listBox.EndUpdate();
                }
            }
        }

        /// <summary>
        /// Writes a line terminator to the text stream.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter"></see> is closed. </exception>
        public override void WriteLine()
        {
            lock (_sync)
            {
                if (_builder.Length > 0)
                {
                    _NewLine();
                }
                else
                {
                    _lines.Add(string.Empty);
                }
            }
        }

        /// <summary>
        /// Writes a string followed by a line terminator to the text stream.
        /// </summary>
        /// <param name="value">The string to write. If value is null, only the line termination characters are written.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter"></see> is closed. </exception>
        public override void WriteLine(string value)
        {
            lock (_sync)
            {
                _builder.Append(value);
                _NewLine();
            }
        }

        /// <summary>
        /// Writes out a formatted string and a new line, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)"></see>.
        /// </summary>
        /// <param name="format">The formatting string.</param>
        /// <param name="arg">The object array to write into format string.</param>
        /// <exception cref="T:System.FormatException">The format specification in format is invalid.-or- The number indicating an argument to be formatted is less than zero, or larger than or equal to arg.Length. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter"></see> is closed. </exception>
        /// <exception cref="T:System.ArgumentNullException">A string or object is passed in as null. </exception>
        public override void WriteLine(string format, params object[] arg)
        {
            lock (_sync)
            {
                _builder.AppendFormat(format, arg);
                _NewLine();
            }
        }

        /// <summary>
        /// Writes a string to the text stream.
        /// </summary>
        /// <param name="value">The string to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter"></see> is closed. </exception>
        public override void Write(string value)
        {
            lock (_sync)
            {
                _builder.Append(value);
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.TextWriter"></see> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            Flush();
            base.Dispose(disposing);
        }

        #endregion
    }
}
