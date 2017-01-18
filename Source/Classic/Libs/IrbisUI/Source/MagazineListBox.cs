// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MagazineListBox.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using ManagedIrbis.Magazines;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class MagazineListBox 
        : UserControl
    {
        #region Events

        /// <summary>
        /// Fired when selected magazine changed.
        /// </summary>
        public event EventHandler SelectedMagazineChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Selected magazine.
        /// </summary>
        [CanBeNull]
        public MagazineInfo SelectedMagazine
        {
            get
            {
                MagazineInfo result 
                    = _listBox.SelectedItem as MagazineInfo;

                return result;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MagazineListBox()
        {
            InitializeComponent();

            _textBox.DelayedTextChanged += _textBox_DelayedTextChanged;
            _textBox.PreviewKeyDown += _textBox_PreviewKeyDown;
            _listBox.SelectedIndexChanged += _listBox_SelectedIndexChanged;
        }

        #endregion

        #region Private members

        private MagazineInfo[] _magazines;

        private void _listBox_SelectedIndexChanged
            (
                object sender,
                EventArgs e
            )
        {
            SelectedMagazineChanged.Raise(this);
        }

        private void _textBox_DelayedTextChanged
            (
                object sender,
                EventArgs e
            )
        {
            string text = _textBox.Text.Trim();

            for (int i = 0; i < _magazines.Length; i++)
            {
                string candidate = _magazines[i].Title;
                if (string.Compare(candidate, text,
                    StringComparison.CurrentCultureIgnoreCase)
                    >= 0)
                {
                    _listBox.SelectedIndex = i;
                    break;
                }
            }
        }

        void _textBox_PreviewKeyDown
            (
                object sender,
                PreviewKeyDownEventArgs e
            )
        {
            if (e.KeyData == Keys.Up)
            {
                if (_listBox.SelectedIndex > 0)
                {
                    _listBox.SelectedIndex--;
                }
                e.IsInputKey = false;
            }
            else if (e.KeyData == Keys.Down)
            {
                if (_listBox.SelectedIndex < (_listBox.Items.Count - 1))
                {
                    _listBox.SelectedIndex++;
                }
                e.IsInputKey = false;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Load all magazines from database.
        /// </summary>
        [NotNull]
        public MagazineManager LoadMagazines
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            MagazineManager result = new MagazineManager(connection);

            LoadMagazines(result);

            return result;
        }

        /// <summary>
        /// Load all magazines from database.
        /// </summary>
        public void LoadMagazines
            (
                [NotNull] MagazineManager manager
            )
        {
            Code.NotNull(manager, "manager");

            MagazineInfo[] magazines = manager.GetAllMagazines();
            SetMagazines(magazines);
        }

        /// <summary>
        /// Set list of magazines.
        /// </summary>
        public void SetMagazines
            (
                [NotNull] IEnumerable<MagazineInfo> magazines
            )
        {
            Code.NotNull(magazines, "magazines");

            _listBox.DataSource = null;

            _magazines = magazines
                .OrderBy(magazine => magazine.ExtendedTitle)
                .ToArray();
            _listBox.DisplayMember = "ExtendedTitle";
            _listBox.DataSource = _magazines;
        }

        #endregion
    }
}
