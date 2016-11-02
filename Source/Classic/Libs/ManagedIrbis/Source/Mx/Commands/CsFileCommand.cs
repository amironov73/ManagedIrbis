/* CsFileCommand.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Source.Mx;
using Microsoft.CSharp;
using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CsFileCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CsFileCommand()
            : base("CSFile")
        {
        }

        #endregion

        #region Private members

        private static bool HandleErrors
            (
                MxExecutive executive,
                CompilerResults results
            )
        {
            StringBuilder builder = new StringBuilder();

            foreach (var error in results.Errors)
            {
                builder.AppendLine(error.ToString());
            }

            if (builder.Length != 0)
            {
                executive.WriteLine(builder.ToString());
                return true;
            }

            return false;
        }

        static MethodInfo FindEntryPoint
            (
                Assembly assembly
            )
        {
            foreach (Type type in assembly.GetTypes())
            {
                MethodInfo main = type.GetMethod
                    (
                        "Main",
                        BindingFlags.Static
                        | BindingFlags.Public
                        | BindingFlags.NonPublic
                    );
                if (!ReferenceEquals(main, null))
                {
                    return main;
                }
            }

            return null;
        }

        #endregion

        #region Public methods

        #endregion

        #region MxCommand members

        /// <inheritdoc/>
        public override bool Execute
            (
                MxExecutive executive,
                MxArgument[] arguments
            )
        {
            OnBeforeExecute();

            if (arguments.Length != 0)
            {
                string argument = arguments[0].Text;
                if (!string.IsNullOrEmpty(argument))
                {
                    CSharpCodeProvider provider = new CSharpCodeProvider();

                    CompilerParameters parameters = new CompilerParameters
                        (
                            MxUtility.AssemblyReferences
                        )
                    {
                        GenerateExecutable = true,
                        GenerateInMemory = true,
                        CompilerOptions = "/d:DEBUG",
                        WarningLevel = 4,
                        IncludeDebugInformation = true
                    };
                    CompilerResults results
                        = provider.CompileAssemblyFromFile
                        (
                            parameters,
                            argument
                        );
                    if (!HandleErrors(executive, results)
                        && results.Errors.Count == 0)
                    {
                        MethodInfo main = FindEntryPoint(results.CompiledAssembly);
                        if (!ReferenceEquals(main, null))
                        {
                            main.Invoke(null, null);
                        }
                    }
                }
            }

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
