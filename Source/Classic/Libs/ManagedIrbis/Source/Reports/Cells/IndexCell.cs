// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IndexCell.cs -- 
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
    public sealed class IndexCell
        : ReportCell
    {
        #region Properties

        /// <summary>
        /// Format.
        /// </summary>
        [CanBeNull]
        public string Format { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IndexCell()
        {
            Format = "{Index})";
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IndexCell
            (
                [CanBeNull] string format
            )
        {
            Format = format;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportCell members

        /// <inheritdoc />
        public override void Evaluate
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            ReportDriver driver = context.Driver;

            driver.BeginCell(context);

            string format = Format;
            if (!string.IsNullOrEmpty(format))
            {
                string index = (context.Index + 1)
                    .ToInvariantString();
                string total = context.Records.Count
                    .ToInvariantString();
                string text = format
                    .Replace("{Index}", index)
                    .Replace("{Total}", total);
                driver.Write(context, text);
            }

            driver.EndCell(context);
        }

        #endregion

        #region Object members

        #endregion
    }
}
