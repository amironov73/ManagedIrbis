// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportVariable.cs -- 
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
    [DebuggerDisplay("{Name}: {Value}")]
    public sealed class ReportVariable
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Name of the variable.
        /// </summary>
        [NotNull]
        public string Name { get; set; }

        /// <summary>
        /// Value of the variable.
        /// </summary>
        [CanBeNull]
        public object Value { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportVariable
            (
                [NotNull] string name,
                [CanBeNull] object value
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
            Value = value;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ReportVariable> verifier
                = new Verifier<ReportVariable>(this, throwOnError);

            // TODO Add some verification here

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format
                (
                    "{0}: {1}",
                    Name.ToVisibleString(),
                    Value.ToVisibleString()
                );
        }

        #endregion
    }
}
