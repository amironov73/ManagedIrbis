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
                FoundLine result = (FoundLine)_bindingSource.Current;

                return result;
            }
        }

        /// <summary>
        /// Terms.
        /// </summary>
        [NotNull]
        public List<FoundLine> Lines
        {
            get { return _lines; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FoundPanel()
        {
            InitializeComponent();

            _grid.AutoGenerateColumns = false;

            _lines = new List<FoundLine>();
            _bindingSource = new BindingSource
            {
                DataSource = _lines
            };
            _grid.DataSource = _bindingSource;
        }

        #endregion

        #region Private members

        private readonly BindingSource _bindingSource;
        private readonly List<FoundLine> _lines;

        #endregion
    }
}
