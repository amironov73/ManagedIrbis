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

namespace ManagedIrbis.Source.Reports.Drivers
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

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportDriver members

        /// <inheritdoc />
        public override void BeginDocument
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            context.Output.Write("<table>");
        }

        /// <inheritdoc />
        public override void BeginRow
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            context.Output.Write("<tr>");
        }

        /// <inheritdoc />
        public override void EndCell
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            context.Output.Write("</td>");
        }

        /// <inheritdoc />
        public override void EndDocument
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            context.Output.Write("</table>");
        }

        /// <inheritdoc />
        public override void EndRow
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            context.Output.Write("</tr>");
        }

        /// <inheritdoc />
        public override void Write
            (
                ReportContext context,
                string text
            )
        {
            Code.NotNull(context, "context");

            // TODO: encode entities

            context.Output.Write(text);
        }

        /// <inheritdoc />
        public override void BeginCell
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            context.Output.Write("<td>");
        }

        #endregion

        #region Object members

        #endregion
    }
}
