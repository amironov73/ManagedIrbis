// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportAttributes.cs -- attributes for cell, band, report
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Attributes for cell, band, report.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReportAttributes
        : Dictionary<string, object>,
        IVerifiable
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get the attribute value by name.
        /// </summary>
        [CanBeNull]
        public object GetAttribute
            (
                [NotNull] string name
            )
        {
            Code.NotNull(name, "name");

            object result;
            TryGetValue(name, out result);

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ReportAttributes> verifier
                = new Verifier<ReportAttributes>(this, throwOnError);

            // TODO Add some verification

            return verifier.Result;
        }

        #endregion
    }
}
