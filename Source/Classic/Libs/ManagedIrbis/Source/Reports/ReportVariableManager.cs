// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportVariableManager.cs -- 
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
    public sealed class ReportVariableManager
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Variable registry.
        /// </summary>
        [NotNull]
        public CaseInsensitiveDictionary<ReportVariable> Registry
        {
            get;
            private set;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportVariableManager()
        {
            Registry = new CaseInsensitiveDictionary<ReportVariable>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Dump all the variables.
        /// </summary>
        public void DumpVariables
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            var keys = Registry.Keys.OrderBy(key => key);
            foreach (string key in keys)
            {
                ReportVariable variable = Registry[key];
                writer.WriteLine(variable.ToString());
            }
            writer.WriteLine(new string('=', 60));
        }

        /// <summary>
        /// Get all variables.
        /// </summary>
        [NotNull]
        public ReportVariable[] GetAllVariables()
        {
            List<ReportVariable> result = new List<ReportVariable>();

            var keys = Registry.Keys.OrderBy(key => key);
            foreach (string key in keys)
            {
                ReportVariable variable = Registry[key];
                result.Add(variable);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get existing variable with the specified name.
        /// </summary>
        [CanBeNull]
        public ReportVariable GetExistingVariable
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            ReportVariable result;
            Registry.TryGetValue(name, out result);

            return result;
        }

        /// <summary>
        /// Get existing or create new variable with given name.
        /// </summary>
        [NotNull]
        public ReportVariable GetOrCreateVariable
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            ReportVariable result = GetExistingVariable(name);
            if (ReferenceEquals(result, null))
            {
                result = new ReportVariable(name, null);
                Registry.Add(name, result);
            }

            return result;
        }

        /// <summary>
        /// Set the variable value.
        /// </summary>
        [NotNull]
        public ReportVariable SetVariable
            (
                [NotNull] string name,
                [CanBeNull] object value
            )
        {
            Code.NotNullNorEmpty(name, "name");

            ReportVariable result = GetOrCreateVariable(name);
            result.Value = value;

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
            Verifier<ReportVariableManager> verifier
                = new Verifier<ReportVariableManager>(this, throwOnError);

            foreach (ReportVariable variable in this.GetAllVariables())
            {
                verifier.VerifySubObject(variable, "variable");
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
