/* MenuForm.cs --
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
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Menus;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class MenuForm
        : Form
    {
        #region Properties

        /// <summary>
        /// Current menu entry.
        /// </summary>
        [CanBeNull]
        public MenuEntry CurrentEntry
        {
            get
            {
                DataGridViewRow currentRow = _grid.CurrentRow;
                MenuEntry result = currentRow == null
                    ? null
                    : (MenuEntry)currentRow.DataBoundItem;

                return result;
            }
        }

        /// <summary>
        /// Entries
        /// </summary>
        [NotNull]
        public MenuEntry[] Entries { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuForm()
        {
            InitializeComponent();

            _grid.AutoGenerateColumns = false;

            Entries = new MenuEntry[0];
            _grid.DataSource = Entries;
            _grid.Focus();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Set entries.
        /// </summary>
        public void SetEntries
            (
                [NotNull] IEnumerable<MenuEntry> entries
            )
        {
            Code.NotNull(entries, "entries");

            Entries = entries.ToArray();
            _grid.DataSource = Entries;
        }

        #endregion

        private void _grid_DoubleClick
            (
                object sender,
                EventArgs e
            )
        {
            DialogResult = DialogResult.OK;
        }

        private void _grid_PreviewKeyDown
            (
                object sender,
                PreviewKeyDownEventArgs e
            )
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.IsInputKey = false;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
