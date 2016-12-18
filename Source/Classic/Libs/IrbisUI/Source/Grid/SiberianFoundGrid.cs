// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianFoundGrid.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Search;
using ManagedIrbis.Worksheet;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianFoundGrid
        : SiberianGrid
    {
        #region Properties

        /// <summary>
        /// MFN.
        /// </summary>
        [NotNull]
        public SiberianFoundMfnColumn MfnColumn { get; private set; }

        /// <summary>
        /// Checkbox.
        /// </summary>
        [NotNull]
        public SiberianFoundCheckColumn CheckColumn { get; private set; }

        /// <summary>
        /// Icon.
        /// </summary>
        [NotNull]
        public SiberianFoundIconColumn IconColumn { get; private set; }

        /// <summary>
        /// Description.
        /// </summary>
        [NotNull]
        public SiberianFoundDescriptionColumn DescriptionColumn { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianFoundGrid()
        {
            HeaderHeight = 26;

            MfnColumn = CreateColumn<SiberianFoundMfnColumn>();
            MfnColumn.Title = "MFN";
            MfnColumn.MinWidth = 50;
            MfnColumn.Width = 50;

            CheckColumn = CreateColumn<SiberianFoundCheckColumn>();
            CheckColumn.MinWidth = 20;
            CheckColumn.Width = 20;

            IconColumn = CreateColumn<SiberianFoundIconColumn>();
            IconColumn.MinWidth = 20;
            IconColumn.Width = 20;

            DescriptionColumn = CreateColumn<SiberianFoundDescriptionColumn>();
            DescriptionColumn.FillWidth = 100;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Use given WSS.
        /// </summary>
        public void Load
            (
                [NotNull] FoundLine[] lines
            )
        {
            Code.NotNull(lines, "lines");

            Rows.Clear();

            foreach (FoundLine line in lines)
            {
                CreateRow(line);
            }

            Goto(Rows.Count - 1, 0);
        }

        #endregion

        #region SiberianGrid members

        #endregion

        #region Object members

        #endregion
    }
}
