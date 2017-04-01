// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportRenderingEventArgs.cs -- 
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
    public sealed class ReportRenderingEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Context.
        /// </summary>
        [NotNull]
        public ReportContext Context { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportRenderingEventArgs
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
