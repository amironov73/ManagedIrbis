// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianTermGrid.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Search;
using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianTermGrid
        : SiberianGrid
    {
        #region Properties

        /// <summary>
        /// Column for term count.
        /// </summary>
        [NotNull]
        public SiberianTermCountColumn CountColumn { get; private set; }

        /// <summary>
        /// Column for term text.
        /// </summary>
        [NotNull]
        public SiberianTermTextColumn TextColumn { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianTermGrid()
        {
            HeaderHeight = 26;

            CountColumn = (SiberianTermCountColumn)CreateColumn<SiberianTermCountColumn>();
            CountColumn.Title = "Count";
            CountColumn.MinWidth = 50;
            CountColumn.Width = 50;

            TextColumn = (SiberianTermTextColumn)CreateColumn<SiberianTermTextColumn>();
            TextColumn.Title = "Terms";
            TextColumn.FillWidth = 100;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Load given terms.
        /// </summary>
        public void Load
            (
                [NotNull] TermInfo[] terms
            )
        {
            Code.NotNull(terms, "terms");

            Rows.Clear();

            foreach (TermInfo term in terms)
            {
                CreateRow(term);
            }

            Goto(1, 0);
        }

        #endregion

        #region Object members

        #endregion
    }
}
