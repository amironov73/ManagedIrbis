/* SuggestingTextBox.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Suggestions
{
    /// <summary>
    /// <see cref="TextBox"/> with suggesting facility.
    /// </summary>
    [PublicAPI]
    public partial class SuggestingTextBox
        : TextBox
    {
        #region Events

        /// <summary>
        /// Raised when suggestion needed.
        /// </summary>
        public event EventHandler SuggestionNeeded;

        #endregion

        #region Properties

        /// <summary>
        /// Is suggestion list visible.
        /// </summary>
        public bool Opened
        {
            get { return _listBox.Visible; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SuggestingTextBox()
        {
            _listBox = new ListBox
            {
                Visible = false
            };
            _listBox.PreviewKeyDown += _listBox_PreviewKeyDown;
            TextChanged += _TextChanged;
        }

        #endregion

        #region Private members

        private ListBox _listBox;
        private bool _busy;

        private void _listBox_PreviewKeyDown
            (
                object sender,
                PreviewKeyDownEventArgs e
            )
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;

                case Keys.Up:
                    if (_listBox.SelectedIndex == 0)
                    {
                        e.IsInputKey = false;
                        Close();
                    }
                    break;

                case Keys.Tab:
                case Keys.Enter:
                    if (_listBox.Focused)
                    {
                        e.IsInputKey = false;
                        Accept();
                    }
                    break;
            }
        }

        private void _TextChanged
            (
                object sender,
                EventArgs e
            )
        {
            if (_busy)
            {
                return;
            }
            try
            {
                _busy = true;

                SuggestionNeeded.Raise(this);
            }
            finally
            {
                _busy = false;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Accept user choice (if any).
        /// </summary>
        public void Accept()
        {
            if (_listBox.Focused)
            {
                Text = _listBox.Text;
                Close();
            }
        }

        /// <summary>
        /// Close suggestion list.
        /// </summary>
        public void Close()
        {
            if (Opened)
            {
                _listBox.Visible = false;
                Focus();
            }
        }

        /// <summary>
        /// Open suggestion list.
        /// </summary>
        public void Open()
        {
            if (!Opened)
            {
                _listBox.Top = Bottom + 1;
                _listBox.Left = Left;
                _listBox.Width = Width;
                _listBox.Height = 100;

                _listBox.Visible = true;
            }
        }

        /// <summary>
        /// Open suggestions list if needed.
        /// </summary>
        public void OpenIfNeeded()
        {
            if (_listBox.Items.Count != 0)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Set suggestions.
        /// </summary>
        public void SetItems
            (
                [NotNull] IEnumerable<object> items
            )
        {
            Code.NotNull(items, "items");

            _listBox.Items.Clear();
            _listBox.Items.AddRange(items.ToArray());
        }

        #endregion

        #region TextBox members

        /// <inheritdoc />
        protected override void OnPreviewKeyDown
            (
                PreviewKeyDownEventArgs e
            )
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    Open();
                    e.IsInputKey = false;
                    _listBox.Focus();
                    if (_listBox.Items.Count != 0)
                    {
                        _listBox.SelectedIndex = 0;
                    }
                    break;
            }
        }

        /// <inheritdoc />
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            _listBox.Parent = Parent;
        }

        /// <inheritdoc />
        protected override void Dispose
            (
                bool disposing
            )
        {
            if (disposing)
            {
                _listBox.Dispose();
            }
        }

        #endregion
    }
}
