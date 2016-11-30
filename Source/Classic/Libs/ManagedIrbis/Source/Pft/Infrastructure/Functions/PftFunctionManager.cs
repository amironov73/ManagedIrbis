// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftFunctionManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

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
        public Dictionary<string, FunctionDescriptor> Registry { get; private set; }

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
            Registry = new Dictionary<string, FunctionDescriptor>
                (
                    StringComparer.CurrentCultureIgnoreCase
                );
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Quick add the function.
        /// </summary>
        [NotNull]
        public PftFunctionManager Add
            (
                [NotNull] string name,
                [NotNull] PftFunction function
            )
        {
            Code.NotNullNorEmpty(name, "name");

            FunctionDescriptor descriptor = new FunctionDescriptor
            {
                Name = name,
                Function = function
            };

            Registry.Add(name, descriptor);

            return this;
        }

        /// <summary>
        /// Execute specified function.
        /// </summary>
        public static void ExecuteFunction
            (
                [NotNull] string name,
                [NotNull] PftContext context,
                [NotNull] PftNode node,
                [NotNull] PftNode[] arguments
            )
        {
            Code.NotNullNorEmpty(name, "name");
            Code.NotNull(context, "context");
            Code.NotNull(node, "node");
            Code.NotNull(arguments, "arguments");

            FunctionDescriptor descriptor;
            if (!UserFunctions.Registry.TryGetValue
                (
                    name,
                    out descriptor
                ))
            {
                if (!BuiltinFunctions.Registry.TryGetValue
                    (
                        name,
                        out descriptor
                    ))
                {
                    throw new PftSemanticException
                        (
                            "unknown function: "
                            + name
                        );
                }
            }

            descriptor.Function
                (
                    context,
                    node,
                    arguments
                );
        }

        /// <summary>
        /// Find specified function.
        /// </summary>
        [CanBeNull]
        public FunctionDescriptor FindFunction
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            FunctionDescriptor result;
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

            Add(name, function);
        }

        #endregion
    }
}
