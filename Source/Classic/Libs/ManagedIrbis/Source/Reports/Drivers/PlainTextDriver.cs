// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PlainTextDriver.cs -- 
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
    public sealed class PlainTextDriver
        : ReportDriver
    {
        #region Properties

        /// <summary>
        /// Cell delimiter.
        /// </summary>
        [CanBeNull]
        public string CellDelimiter { get; set; }

        /// <summary>
        /// Row delimiter.
        /// </summary>
        [CanBeNull]
        public string RowDelimiter { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlainTextDriver()
        {
            CellDelimiter = "\t";
            RowDelimiter = Environment.NewLine;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportDriver members

        /// <inheritdoc />
        public override void EndRow
            (
                ReportContext context
            )
        {
            ReportOutput output = context.Output;
            output.TrimEnd();
            output.Write(RowDelimiter);
        }

        /// <inheritdoc />
        public override void EndCell
            (
                ReportContext context
            )
        {
            context.Output.Write(CellDelimiter);
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
