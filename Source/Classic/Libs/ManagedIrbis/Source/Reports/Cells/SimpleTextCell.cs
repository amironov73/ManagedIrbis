// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SimpleTextCell.cs -- 
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
    public sealed class SimpleTextCell
        : TextCell
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimpleTextCell()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimpleTextCell
            (
                string text
            )
            : base(text)
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportCell

        /// <inheritdoc cref="ReportCell.Compute"/>
        public override string Compute
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeCompute(context);

            string result = Text;

            OnAfterCompute(context);

            return result;
        }

        /// <inheritdoc cref="ReportCell.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            string text = Compute(context);

            context.Output.Write(text);
        }

        #endregion

        #region Object members

        #endregion
    }
}
