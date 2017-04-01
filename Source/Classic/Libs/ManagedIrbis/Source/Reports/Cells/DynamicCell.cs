// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DynamicCell.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;

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
    public class DynamicCell
        : ReportCell
    {
        #region Events

        /// <summary>
        /// Raised on cell computation.
        /// </summary>
        public event EventHandler<ReportComputeEventArgs> Computation;

        /// <summary>
        /// Raised on cell rendering.
        /// </summary>
        public event EventHandler<ReportRenderingEventArgs> Rendering;

        #endregion

        #region ReportCell members

        /// <inheritdoc cref="ReportCell.Compute"/>
        public override string Compute
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeCompute(context);

            ReportComputeEventArgs eventArgs
                = new ReportComputeEventArgs(context);
            Computation.Raise(this, eventArgs);

            OnAfterCompute(context);

            return eventArgs.Result;
        }

        /// <inheritdoc cref="ReportCell.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            ReportRenderingEventArgs eventArgs
                = new ReportRenderingEventArgs(context);
            Rendering.Raise(this, eventArgs);
        }

        #endregion

        #region Object members

        #endregion
    }
}
