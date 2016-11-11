/* PftFunctionManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Function manager.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftFunctionManager
    {
        #region Properties

        /// <summary>
        /// Function registry.
        /// </summary>
        [NotNull]
        public Dictionary<string, PftFunction> Registry { get; private set; }

        /// <summary>
        /// Builtin functions.
        /// </summary>
        [NotNull]
        public static PftFunctionManager BuiltinFunctions { get; private set; }

        /// <summary>
        /// User defined functions.
        /// </summary>
        [NotNull]
        public static PftFunctionManager UserFunctions { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static PftFunctionManager()
        {
            BuiltinFunctions = new PftFunctionManager();
            UserFunctions = new PftFunctionManager();

            StandardFunctions.Register();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFunctionManager()
        {
            Registry = new Dictionary<string, PftFunction>
                (
                    StringComparer.CurrentCultureIgnoreCase
                );
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Execute specified function.
        /// </summary>
        public static void ExecuteFunction
            (
                [NotNull] string name,
                [NotNull] PftContext context,
                [NotNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            Code.NotNullNorEmpty(name, "name");
            Code.NotNull(context, "context");
            Code.NotNull(node, "node");

            PftFunction function;
            if (!UserFunctions.Registry.TryGetValue
                (
                    name,
                    out function
                ))
            {
                if (!BuiltinFunctions.Registry.TryGetValue
                    (
                        name,
                        out function
                    ))
                {
                    throw new PftSemanticException
                        (
                            "unknown function: "
                            + name
                        );
                }
            }

            function(context, node, expression);
        }

        /// <summary>
        /// Find specified function.
        /// </summary>
        [CanBeNull]
        public PftFunction FindFunction
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            PftFunction result;
            Registry.TryGetValue(name, out result);

            return result;
        }

        /// <summary>
        /// Have specified function?
        /// </summary>
        public bool HaveFunction
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            return Registry.ContainsKey(name);
        }

        /// <summary>
        /// Register the function.
        /// </summary>
        public void RegisterFunction
            (
                [NotNull] string name,
                [NotNull] PftFunction function
            )
        {
            Code.NotNullNorEmpty(name,"name");
            Code.NotNull(function, "function");

            if (name.OneOf(PftUtility.GetReservedWords()))
            {
                throw new PftException("Reserved word: " + name);
            }
            if (HaveFunction(name))
            {
                throw new PftException("Function already registered: " + name);
            }

            Registry.Add(name, function);
        }

        #endregion
    }
}
