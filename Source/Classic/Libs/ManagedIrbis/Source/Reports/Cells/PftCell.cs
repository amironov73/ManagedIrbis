// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftCell.cs -- 
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

using ManagedIrbis.Pft;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PftCell
        : ReportCell
    {
        #region Properties

        /// <summary>
        /// Script text.
        /// </summary>
        [CanBeNull]
        public string Text { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private PftFormatter _formatter;

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

            string text = Text;

            if (string.IsNullOrEmpty(text))
            {
                // TODO: Skip or not on empty format?

                return;
            }

            if (ReferenceEquals(_formatter, null))
            {
                _formatter = new PftFormatter();
                _formatter.SetEnvironment(context.Client);
                _formatter.ParseProgram(text);
            }

            ReportDriver driver = context.Driver;

            string formatted = _formatter.Format(context.CurrentRecord);
            driver.BeginCell(context);
            driver.Write(context, formatted);
            driver.EndCell(context);
        }

        #endregion

        #region Object members

        #endregion
    }
}
