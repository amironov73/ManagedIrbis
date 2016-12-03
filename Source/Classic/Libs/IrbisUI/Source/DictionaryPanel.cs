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
                DataGridViewRow currentRow = _grid.CurrentRow;
                TermInfo result = currentRow == null
                    ? null
                    : (TermInfo) currentRow.DataBoundItem;

                return result;
            }
        }

        /// <summary>
        /// Terms.
        /// </summary>
        [NotNull]
        public TermInfo[] Terms { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DictionaryPanel()
        {
            InitializeComponent();

            _grid.AutoGenerateColumns = false;

            Terms = new TermInfo[0];
            _grid.DataSource = Terms;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Set terms.
        /// </summary>
        public void SetTerms
            (
                [NotNull] IEnumerable<TermInfo> terms
            )
        {
            Code.NotNull(terms, "terms");

            Terms = terms.ToArray();
            _grid.DataSource = Terms;
        }

        #endregion
    }
}
