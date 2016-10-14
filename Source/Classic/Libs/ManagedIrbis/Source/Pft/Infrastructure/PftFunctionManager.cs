/* PftFunctionManager.cs --
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

            RegisterStandardFunctions();
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

        private static void RegisterStandardFunctions()
        {
            var reg = BuiltinFunctions.Registry;

            reg.Add("error", Error);
            reg.Add("tolower", ToLower);
            reg.Add("toupper", ToUpper);
            reg.Add("warn", Warn);
        }

        //================================================================
        // STANDARD BUILTIN FUNCTIONS
        //================================================================

        private static void Error(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                context.Output.Error.WriteLine(expression);
            }
        }

        private static void ToLower(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, expression.ToLower());
            }
        }

        private static void ToUpper(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, expression.ToUpper());
            }
        }

        private static void Warn(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                context.Output.Warning.WriteLine(expression);
            }
        }

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

        #endregion
    }
}
