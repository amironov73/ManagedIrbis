// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportComputeEventArgs.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

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
    public sealed class ReportComputeEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Context.
        /// </summary>
        [NotNull]
        public ReportContext Context { get; private set; }

        /// <summary>
        /// Result.
        /// </summary>
        [CanBeNull]
        public string Result { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportComputeEventArgs
            (
                [NotNull] ReportContext context
            )
        {
            Code.NotNull(context, "context");

            Context = context;
        }

        #endregion
    }
}
