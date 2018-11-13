// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DictionaryPanel.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// Panel for dictionary entries.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class DictionaryPanel
        : UserControl
    {
        #region Events

        /// <summary>
        /// Raised when the term is choosed.
        /// </summary>
        public event EventHandler Choosed;

        #endregion

        #region Properties

        /// <summary>
        /// Adapter.
        /// </summary>
        public TermAdapter Adapter { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DictionaryPanel()
        {
            InitializeComponent();

            _grid.AutoGenerateColumns = false;
            _SetupEvents();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DictionaryPanel
            (
                [NotNull] TermAdapter adapter
            )
        {
            InitializeComponent();

            _grid.AutoGenerateColumns = false;
            Adapter = adapter;
            _grid.DataSource = Adapter.Source;
            _SetupEvents();
        }

        #endregion

        #region Private members

        private void _SetupEvents()
        {
            _grid.KeyDown += _grid_KeyDown;
            _grid.KeyPress += _grid_KeyPress;
            _grid.DoubleClick += _grid_DoubleClick;
            _grid.MouseWheel += _grid_MouseWheel;
            _keyBox.KeyDown += _keyBox_KeyDown;
            _keyBox.DelayedTextChanged += _keyBox_TextChanged;
            _keyBox.EnterPressed += _keyBox_TextChanged;
            _keyBox.MouseWheel += _grid_MouseWheel;
            _scrollControl.Scroll += _scrollControl_Scroll;
            _scrollControl.MouseWheel += _grid_MouseWheel;
        }

        private void _grid_KeyPress
        (
            object sender,
            KeyPressEventArgs e
        )
        {
            char keyChar = e.KeyChar;
            if (keyChar != '\0')
            {
                _keyBox.Focus();
                Application.DoEvents();
                SendKeys.Send(e.KeyChar.ToString());
            }
        }

        private void _grid_MouseWheel
            (
                object sender,
                MouseEventArgs e
            )
        {
            if (ReferenceEquals(Adapter, null))
            {
                return;
            }

            int delta = e.Delta;

            if (delta > 0)
            {
                Adapter.MovePrevious();
            }
            else if (delta < 0)
            {
                Adapter.MoveNext();
            }
        }

        private int _VisibleRowCount()
        {
            return _grid.DisplayedRowCount(true);
        }

        private void _grid_KeyDown
            (
                object sender,
                KeyEventArgs e
            )
        {
            if (ReferenceEquals(Adapter, null))
            {
                return;
            }

            switch (e.KeyData)
            {
                case Keys.Down:
                    Adapter.MoveNext();
                    e.Handled = true;
                    break;

                case Keys.Up:
                    Adapter.MovePrevious();
                    e.Handled = true;
                    break;

                case Keys.PageDown:
                    Adapter.MoveNext(_VisibleRowCount());
                    e.Handled = true;
                    break;

                case Keys.PageUp:
                    Adapter.MoveNext(_VisibleRowCount());
                    e.Handled = true;
                    break;

                case Keys.Enter:
                    _RaiseChoosed();
                    break;
            }
        }

        private void _keyBox_TextChanged
            (
                object sender,
                EventArgs e
            )
        {
            if (ReferenceEquals(Adapter, null))
            {
                return;
            }

            string startTerm = _keyBox.Text.Trim();
            Adapter.Fill(startTerm);
        }

        private void _keyBox_KeyDown
            (
                object sender,
                KeyEventArgs e
            )
        {
            if (ReferenceEquals(Adapter, null))
            {
                return;
            }

            switch (e.KeyData)
            {
                case Keys.Down:
                    Adapter.MoveNext();
                    e.Handled = true;
                    break;

                case Keys.Up:
                    Adapter.MovePrevious();
                    e.Handled = true;
                    break;

                case Keys.PageDown:
                    Adapter.MoveNext(_VisibleRowCount());
                    e.Handled = true;
                    break;

                case Keys.PageUp:
                    Adapter.MoveNext(_VisibleRowCount());
                    e.Handled = true;
                    break;

                case Keys.Enter:
                    _RaiseChoosed();
                    break;
            }
        }

        private void _RaiseChoosed()
        {
            Choosed.Raise(this);
        }

        private void _grid_DoubleClick
            (
                object sender,
                EventArgs e
            )
        {
            _RaiseChoosed();
        }

        private void _scrollControl_Scroll
            (
                object sender,
                ScrollEventArgs e
            )
        {
            if (ReferenceEquals(Adapter, null))
            {
                return;
            }

            switch (e.Type)
            {
                case ScrollEventType.SmallIncrement:
                    Adapter.MoveNext();
                    break;

                case ScrollEventType.SmallDecrement:
                    Adapter.MovePrevious();
                    break;

                case ScrollEventType.LargeIncrement:
                    Adapter.MoveNext(_VisibleRowCount());
                    break;

                case ScrollEventType.LargeDecrement:
                    Adapter.MoveNext(_VisibleRowCount());
                    break;
            }
        }

        #endregion
    }
}
