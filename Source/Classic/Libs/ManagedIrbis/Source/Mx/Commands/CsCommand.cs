/* CsCommand.cs -- 
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
    public sealed class CsCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CsCommand()
            : base("CS")
        {
        }

        #endregion

        #region Private members

        private const string Prologue = @"
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

using ManagedIrbis.Source.Mx;

using MoonSharp.Interpreter;

namespace ManagedIrbis.Mx.UserSpace
{
    static class <<<CLASSNAME>>>
    {
        static void UserCode ()
        {
";

        private const string Epilogue = @"
        }
    }
}
";

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
                    string className = "Class" + Guid.NewGuid().ToString("N");
                    string code = Prologue.Replace("<<<CLASSNAME>>>", className)
                        + argument
                        + Epilogue;

                    CSharpCodeProvider provider = new CSharpCodeProvider();

                    CompilerParameters parameters = new CompilerParameters
                        (
                            MxUtility.AssemblyReferences
                        )
                    {
                        GenerateExecutable = false,
                        GenerateInMemory = true,
                        CompilerOptions = "/d:DEBUG",
                        WarningLevel = 4,
                        IncludeDebugInformation = true
                    };
                    CompilerResults results
                        = provider.CompileAssemblyFromSource
                        (
                            parameters,
                            code
                        );
                    if (!HandleErrors(executive, results)
                        && results.Errors.Count == 0)
                    {
                        Type type = results.CompiledAssembly.GetType
                            (
                                "ManagedIrbis.Mx.UserSpace."
                                + className
                            );
                        MethodInfo method = type.GetMethod
                            (
                                "UserCode",
                                BindingFlags.Static
                                | BindingFlags.NonPublic
                            );
                        method.Invoke(null, null);
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
