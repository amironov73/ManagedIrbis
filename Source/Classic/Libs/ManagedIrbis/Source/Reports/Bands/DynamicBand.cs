// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DynamicBand.cs -- 
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
    public class DynamicBand
        : ReportBand
    {
        #region Events

        /// <summary>
        /// Raised on band evaluation.
        /// </summary>
        public event EventHandler<ReportEvaluationEventArgs> Evaluation;

        #endregion

        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportBand members

        /// <inheritdoc />
        public override void Evaluate
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            ReportEvaluationEventArgs eventArgs
                = new ReportEvaluationEventArgs(context);
            Evaluation.Raise(this, eventArgs);
        }

        #endregion

        #region Object members

        #endregion
    }
}
