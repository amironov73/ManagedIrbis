// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DictionaryForm.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class DictionaryForm
        : Form
    {
        #region Properties

        /// <summary>
        /// Adapter.
        /// </summary>
        public TermAdapter Adapter { get; set; }

        /// <summary>
        /// Chosen term.
        /// </summary>
        [CanBeNull]
        public string ChosenTerm { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DictionaryForm()
        {
            InitializeComponent();

            _grid.AutoGenerateColumns = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DictionaryForm
            (
                [NotNull] TermAdapter adapter
            )
        {
            InitializeComponent();

            _grid.AutoGenerateColumns = false;
            Adapter = adapter;
            _grid.DataSource = Adapter.Source;
            _SetupEvents();
            _keyBox.Focus();
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
            _scroll.Scroll += _scroll_Scroll;
            _scroll.MouseWheel += _grid_MouseWheel;
            _bottomPanel.MouseWheel += _grid_MouseWheel;
            _okButton.MouseWheel += _grid_MouseWheel;
            _cancelButton.MouseWheel += _grid_MouseWheel;
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
            ChosenTerm = Adapter.CurrentValue;
            DialogResult = DialogResult.OK;
        }

        private void _grid_DoubleClick
            (
                object sender,
                EventArgs e
            )
        {
            _RaiseChoosed();
        }

        private void _scroll_Scroll
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

        private void _okButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            ChosenTerm = Adapter.CurrentValue;
            DialogResult = DialogResult.OK;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Goto the start term.
        /// </summary>
        public void Goto
            (
                [CanBeNull] string startTerm
            )
        {
            Adapter?.Fill(startTerm);
        }

        /// <summary>
        /// Choose one term from the dictionary.
        /// </summary>
        [CanBeNull]
        public static string ChooseTerm
            (
                [CanBeNull] IWin32Window owner,
                [NotNull] TermAdapter adapter,
                [CanBeNull] string startTerm
            )
        {
            Code.NotNull(adapter, nameof(adapter));

            using (DictionaryForm form = new DictionaryForm(adapter))
            {
                form.Goto(startTerm);
                if (form.ShowDialog(owner) == DialogResult.OK)
                {
                    return form.ChosenTerm;
                }
            }

            return null;
        }

        #endregion
    }
}
