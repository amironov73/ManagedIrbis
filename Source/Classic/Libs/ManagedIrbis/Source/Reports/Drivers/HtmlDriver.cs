// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HtmlDriver.cs -- 
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
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Reports;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class HtmlDriver
        : ReportDriver
    {
        #region Properties

        /// <summary>
        /// Table borders visible?
        /// </summary>
        public bool Borders { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportDriver members

        /// <inheritdoc />
        public override void BeginCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(cell, "cell");

            context.Output.Write("<td>");
        }

        /// <inheritdoc />
        public override void BeginDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(report, "report");

            string table = string.Format
                (
                    "<table border={0}>",
                    Borders ? "1" : "0"
                );

            context.Output.Write(table);
        }

        /// <inheritdoc />
        public override void BeginRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(band, "band");

            context.Output.Write("<tr>");
        }

        /// <inheritdoc />
        public override void EndCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(cell, "cell");

            context.Output.Write("</td>");
        }

        /// <inheritdoc />
        public override void EndDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(report, "report");

            ReportOutput output = context.Output;
            output.Write("</table>");
            output.Write(Environment.NewLine);
        }

        /// <inheritdoc />
        public override void EndRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(band, "band");

            ReportOutput output = context.Output;
            output.Write("</tr>");
            output.Write(Environment.NewLine);
        }

        /// <inheritdoc />
        public override void Write
            (
                ReportContext context,
                string text
            )
        {
            Code.NotNull(context, "context");

            context.Output.Write
                (
                    HtmlText.Encode(text)
                );
        }

        #endregion

        #region Object members

        #endregion
    }
}
