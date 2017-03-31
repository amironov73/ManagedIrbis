// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RawPftCell.cs -- 
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
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class RawPftCell
        : PftCell
    {
        #region PftCell members

        /// <inheritdoc cref="ReportCell.Evaluate" />
        public override void Evaluate
        (
            ReportContext context
        )
        {
            Code.NotNull(context, "context");

            string text = Text;

            if (string.IsNullOrEmpty(text))
            {
                // TODO: Skip or not on empty format?

                return;
            }

            ReportDriver driver = context.Driver;
            string formatted = Compute(context);

            driver.BeginCell(context, this);
            context.Output.Write(formatted);
            driver.EndCell(context, this);

            #endregion
        }
    }
}
