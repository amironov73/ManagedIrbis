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
        #region Properties

        /// <summary>
        /// Current term.
        /// </summary>
        [CanBeNull]
        public FoundLine Current
        {
            get
            {
                DataGridViewRow currentRow = _grid.CurrentRow;
                FoundLine result = currentRow == null
                    ? null
                    : (FoundLine) currentRow.DataBoundItem;

                return result;
            }
        }

        /// <summary>
        /// Terms.
        /// </summary>
        [NotNull]
        public FoundLine[] Lines { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FoundPanel()
        {
            InitializeComponent();

            _grid.AutoGenerateColumns = false;

            Lines = new FoundLine[0];
            _grid.DataSource = Lines;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Set found lines.
        /// </summary>
        public void SetFound
            (
                [NotNull] IEnumerable<FoundLine> found
            )
        {
            Code.NotNull(found, "found");

            Lines = found.ToArray();
            _grid.DataSource = Lines;
        }

        #endregion
    }
}
