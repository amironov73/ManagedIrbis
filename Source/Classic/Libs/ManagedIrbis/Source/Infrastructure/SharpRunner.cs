// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SharpRunner.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#if CLASSIC

using System.CodeDom.Compiler;

using Microsoft.CSharp;

#endif

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SharpRunner
    {
        #region Properties

        /// <summary>
        /// Assembly references.
        /// </summary>
        public static List<string> AssemblyReferences = new List<string>
        {
            "System.dll",
            "System.Core.dll",
            "System.Data.dll",
            "System.Data.DataSetExtensions.dll",
            "System.Drawing.dll",
            "System.Windows.Forms.dll",
            "System.Xml.dll",
            "System.Xml.Linq.dll",

            "AM.Core.dll",
            "JetBrains.Annotations.dll",
            "ManagedIrbis.dll",
            "Microsoft.CSharp.dll",
            "MoonSharp.Interpreter.dll",
            "Newtonsoft.Json.dll"
        };

        /// <summary>
        /// Standard class prologue.
        /// </summary>
        public static string ClassPrologue = @"
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mx;

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using MoonSharp.Interpreter;

namespace ManagedIrbis.UserSpace
{
    static class <<<CLASSNAME>>>
    {
        static void UserCode (<<<ARGUMENTS>>>)
        {
";

        /// <summary>
        /// Standard class epilogue.
        /// </summary>
        public static string ClassEpilogue = @"
        }
    }
}
";

#if CLASSIC

        private static string ExtractErrors
            (
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
                return builder.ToString();
            }

            return null;
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

#endif

        #endregion

        #region Public methods

        /// <summary>
        /// Compile C# snippet.
        /// </summary>
        [CanBeNull]
        public static MethodInfo CompileSnippet
            (
                [NotNull] string text,
                [NotNull] string arguments,
                [NotNull] Action<string> errorAction
            )
        {
#if CLASSIC

            string className = "Class" + Guid.NewGuid().ToString("N");
            string code = ClassPrologue
                .Replace("<<<CLASSNAME>>>", className)
                .Replace("<<<ARGUMENTS>>>", arguments)
                + text
                + ClassEpilogue;

            CSharpCodeProvider provider = new CSharpCodeProvider();

            CompilerParameters parameters = new CompilerParameters
                (
                    AssemblyReferences.ToArray()
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
            string errors = ExtractErrors(results);
            if (!ReferenceEquals(errors, null))
            {
                errorAction(errors);
            }
            else
            {
                Type type = results.CompiledAssembly.GetType
                    (
                        "ManagedIrbis.UserSpace."
                        + className
                    );
                MethodInfo result = type.GetMethod
                    (
                        "UserCode",
                        BindingFlags.Static
                        | BindingFlags.NonPublic
                    );

                return result;
            }

#endif

            return null;
        }

        /// <summary>
        /// Compile C# file.
        /// </summary>
        [CanBeNull]
        public static MethodInfo CompileFile
            (
                [NotNull] string fileName,
                [NotNull] Action<string> errorAction
            )
        {
#if CLASSIC

            CSharpCodeProvider provider = new CSharpCodeProvider();

            CompilerParameters parameters = new CompilerParameters
                (
                    AssemblyReferences.ToArray()
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
                    fileName
                );
            string errors = ExtractErrors(results);
            if (!ReferenceEquals(errors, null))
            {
                errorAction(errors);
            }
            else
            {
                MethodInfo result = FindEntryPoint(results.CompiledAssembly);

                return result;
            }

#endif

            return null;
        }

        #endregion

    }
}
