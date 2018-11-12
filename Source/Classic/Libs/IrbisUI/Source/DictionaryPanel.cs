// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DictionaryPanel.cs --
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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

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
            _grid.DoubleClick += _grid_DoubleClick;
            _keyBox.KeyDown += _keyBox_KeyDown;
            _keyBox.TextChanged += _keyBox_TextChanged;
            _scrollControl.Scroll += _scrollControl_Scroll;
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
            string startTerm = _keyBox.Text.Trim();
            Adapter.Fill(startTerm);
        }

        private void _keyBox_KeyDown
            (
                object sender,
                KeyEventArgs e
            )
        {
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
            switch (e.Type)
            {
                case ScrollEventType.SmallIncrement:
                    Adapter.MoveNext();
                    //Thread.Sleep(20);
                    break;

                case ScrollEventType.SmallDecrement:
                    Adapter.MovePrevious();
                    //Thread.Sleep(20);
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

        #region Public methods


        #endregion
    }
}
