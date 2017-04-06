// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DatasetDriver.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC

#region Using directives

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class DatasetDriver
        : ReportDriver
    {
        #region Properties

        /// <summary>
        /// Dataset.
        /// </summary>
        [CanBeNull]
        public DataSet DataSet { get; internal set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private List<string> _currentLine;

        #endregion

        #region Public methods

        #endregion

        #region ReportDriver members

        /// <inheritdoc cref="ReportDriver.BeginDocument"/>
        public override void BeginDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(report, "report");

            DataSet = new DataSet();
            DataSet.Tables.Add(new DataTable());
        }

        /// <inheritdoc cref="ReportDriver.BeginRow"/>
        public override void BeginRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(band, "band");

            _currentLine = new List<string>();
        }

        /// <inheritdoc cref="ReportDriver.BeginCell"/>
        public override void BeginCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(cell, "cell");

            _currentLine.Add(null);
        }

        /// <inheritdoc cref="ReportDriver.EndRow"/>
        public override void EndRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(band, "band");

            DataTable table = DataSet.Tables[0];
            DataRow row = table.NewRow();
            row.ItemArray = _currentLine.ToArray();
            table.Rows.Add(row);
        }

        /// <inheritdoc cref="ReportDriver.Write"/>
        public override void Write
            (
                ReportContext context,
                string text
            )
        {
            Code.NotNull(context, "context");

            _currentLine[_currentLine.Count - 1] = text;
        }

        #endregion
    }
}

#endif
