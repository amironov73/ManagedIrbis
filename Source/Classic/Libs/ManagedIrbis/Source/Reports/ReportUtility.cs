// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportUtility.cs -- 
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
    public static class ReportUtility
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get PFT formatter for the report context.
        /// </summary>
        [NotNull]
        public static PftFormatter GetFormatter
            (
                [NotNull] this ReportContext context,
                [CanBeNull] string expression
            )
        {
            Code.NotNull(context, "context");

            PftFormatter result = new PftFormatter();
            result.SetEnvironment(context.Client);
            if (!string.IsNullOrEmpty(expression))
            {
                result.ParseProgram(expression);
            }

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
