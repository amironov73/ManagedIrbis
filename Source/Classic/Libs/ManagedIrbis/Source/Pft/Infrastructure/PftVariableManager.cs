/* PftVariableManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CodeJam;
using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftVariableManager
    {
        #region Properties

        /// <summary>
        /// Parent variable manager.
        /// </summary>
        [CanBeNull]
        public PftVariableManager Parent { get; private set; }

        /// <summary>
        /// Registry.
        /// </summary>
        [NotNull]
        public Dictionary<string, PftVariable> Registry { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariableManager
            (
                [CanBeNull] PftVariableManager parent
            )
        {
            Parent = parent;

            Registry = new Dictionary<string, PftVariable>
                (
#if NETCORE || UAP || WIN81

                    StringComparer.OrdinalIgnoreCase

#else

                    StringComparer.InvariantCultureIgnoreCase

#endif
                );
        }

        #endregion

        #region Private members

        /// <summary>
        /// Dump all the variables.
        /// </summary>
        public void DumpVariables
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            for (
                    PftVariableManager manager = this;
                    manager != null;
                    manager = manager.Parent
                )
            {
                var keys = manager.Registry.Keys.OrderBy(key=>key);
                foreach (string key in keys)
                {
                    PftVariable variable = manager.Registry[key];
                    writer.WriteLine(variable.ToString());
                }
                writer.WriteLine(new string('=', 60));
            }
        }

        /// <summary>
        /// Get existing variable with the specified name.
        /// </summary>
        [CanBeNull]
        public PftVariable GetExistingVariable
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            PftVariable result = null;
            for (
                    PftVariableManager manager = this;
                    manager != null;
                    manager = manager.Parent
                )
            {
                if (manager.Registry.TryGetValue(name, out result))
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Get existing or create new variable with given name.
        /// </summary>
        [NotNull]
        public PftVariable GetOrCreateVariable
            (
                [NotNull] string name,
                bool isNumeric
            )
        {
            Code.NotNullNorEmpty(name, "name");

            PftVariable result = GetExistingVariable(name);
            if (ReferenceEquals(result, null))
            {
                result = new PftVariable(name, isNumeric);
                Registry.Add(name, result);
            }

            return result;
        }

        /// <summary>
        /// Set the variable value.
        /// </summary>
        [NotNull]
        public PftVariable SetVariable
            (
                [NotNull] string name,
                [CanBeNull] string value
            )
        {
            Code.NotNullNorEmpty(name, "name");

            PftVariable result = GetOrCreateVariable(name, false);
            result.IsNumeric = false;
            result.StringValue = value;

            return result;
        }

        /// <summary>
        /// Set the variable value.
        /// </summary>
        [NotNull]
        public PftVariable SetVariable
            (
                [NotNull] string name,
                [CanBeNull] double value
            )
        {
            Code.NotNullNorEmpty(name, "name");

            PftVariable result = GetOrCreateVariable(name, true);
            result.IsNumeric = true;
            result.NumericValue = value;

            return result;
        }


        #endregion
    }
}
