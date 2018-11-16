// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FoundPanel.cs -- список найденных документов
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// Grid with found documents list.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class FoundPanel
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
        public RecordAdapter Adapter { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FoundPanel()
        {
            InitializeComponent();

            _grid.AutoGenerateColumns = false;
            _SetupEvents();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FoundPanel
            (
                [NotNull] RecordAdapter adapter
            )
        {
            Code.NotNull(adapter, "adapter");

            InitializeComponent();

            _grid.AutoGenerateColumns = false;
            Adapter = adapter;
            _grid.DataSource = adapter.Source;
            _SetupEvents();
        }

        #endregion

        #region Private members

        private void _SetupEvents()
        {
            _grid.KeyDown += _grid_KeyDown;
            //_grid.KeyPress += _grid_KeyPress;
            _grid.DoubleClick += _grid_DoubleClick;
            _grid.MouseWheel += _grid_MouseWheel;
            //_keyBox.KeyDown += _keyBox_KeyDown;
            //_keyBox.DelayedTextChanged += _keyBox_TextChanged;
            //_keyBox.EnterPressed += _keyBox_TextChanged;
            //_keyBox.MouseWheel += _grid_MouseWheel;
            //_scrollControl.Scroll += _scrollControl_Scroll;
            //_scrollControl.MouseWheel += _grid_MouseWheel;
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

        #endregion

        #region Public methods

        #endregion
    }
}
