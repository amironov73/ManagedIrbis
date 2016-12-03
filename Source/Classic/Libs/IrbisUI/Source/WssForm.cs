// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DictionaryForm.cs --
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
using IrbisUI.Sources;
using JetBrains.Annotations;

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
    public partial class WssForm 
        : Form
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
                TermInfo result = (TermInfo)_bindingSource.Current;

                return result;
            }
        }

        /// <summary>
        /// Terms.
        /// </summary>
        [NotNull]
        public List<WorksheetLine> Lines
        {
            get { return _lines; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public WssForm()
        {
            InitializeComponent();

            _grid.AutoGenerateColumns = false;

            _lines = new List<WorksheetLine>();
            _bindingSource = new BindingSource
            {
                DataSource = _lines
            };
            _grid.DataSource = _bindingSource;
        }

        #endregion

        #region Private members

        private readonly BindingSource _bindingSource;
        private readonly List<WorksheetLine> _lines;

        #endregion
    }
}
