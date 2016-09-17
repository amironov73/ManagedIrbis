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
using System.Threading.Tasks;
using System.Windows.Forms;

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
    /// Panel for dictionary entries.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class DictionaryPanel
        : UserControl
    {
        #region Properties

        /// <summary>
        /// Current term.
        /// </summary>
        [CanBeNull]
        public TermInfo CurrentTerm
        {
            get
            {
                TermInfo result = (TermInfo) _bindingSource.Current;

                return result;
            }
        }

        /// <summary>
        /// Terms.
        /// </summary>
        [NotNull]
        public List<TermInfo> Terms
        {
            get { return _terms; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DictionaryPanel()
        {
            InitializeComponent();

            _grid.AutoGenerateColumns = false;

            _terms = new List<TermInfo>();
            _bindingSource = new BindingSource
            {
                DataSource = _terms
            };
            _grid.DataSource = _bindingSource;
        }

        #endregion

        #region Private members

        private readonly BindingSource _bindingSource;
        private readonly List<TermInfo> _terms;

        #endregion
    }
}
