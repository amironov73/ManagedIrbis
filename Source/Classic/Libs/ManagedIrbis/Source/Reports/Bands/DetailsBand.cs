// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DetailsBand.cs -- 
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
    /// Details (repeating) band.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class DetailsBand
        : ReportBand
    {
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
        public override void Render
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            if (context.Index >= 0)
            {
                RenderOnce(context, null);
            }
            else
            {
                RenderWithRecords(context, null);
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
