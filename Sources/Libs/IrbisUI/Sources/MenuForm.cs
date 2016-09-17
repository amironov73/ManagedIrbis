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
                MenuEntry result = (MenuEntry)_bindingSource.Current;

                return result;
            }
        }

        /// <summary>
        /// Entries
        /// </summary>
        [NotNull]
        public List<MenuEntry> Entries
        {
            get { return _entries; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuForm()
        {
            InitializeComponent();

            _entries = new List<MenuEntry>();
            _bindingSource = new BindingSource
            {
                DataSource = _entries
            };
            _grid.DataSource = _bindingSource;
        }

        #endregion

        #region Private members

        private readonly BindingSource _bindingSource;
        private readonly List<MenuEntry> _entries;

        #endregion
    }
}
