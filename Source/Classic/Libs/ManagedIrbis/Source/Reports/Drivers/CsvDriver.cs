// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CsvDriver.cs -- 
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

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CsvDriver
        : ReportDriver
    {
        #region Properties

        /// <summary>
        /// Field separator.
        /// </summary>
        [CanBeNull]
        public string Separator = ";";

        /// <summary>
        /// Quotes.
        /// </summary>
        [CanBeNull]
        public string Quotes = "\"";

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

            if (!string.IsNullOrEmpty(Quotes))
            {
                context.Output.Write(Quotes);
            }
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

            ReportOutput output = context.Output;

            if (!string.IsNullOrEmpty(Quotes))
            {
                output.Write(Quotes);
            }

            if (!string.IsNullOrEmpty(Separator))
            {
                output.Write(Separator);
            }
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

            context.Output.Write(Environment.NewLine);
        }

        /// <inheritdoc />
        public override void Write
            (
                ReportContext context,
                string text
            )
        {
            Code.NotNull(context, "context");

            context.Output.Write(text);
        }

        #endregion

        #region Object members

        #endregion
    }
}
