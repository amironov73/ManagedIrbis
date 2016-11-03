/* PftCodeBlock.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mx;

using Microsoft.CSharp;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftCodeBlock
        : PftNode
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCodeBlock()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCodeBlock
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.TripleCurly);

            if (string.IsNullOrEmpty(token.Text))
            {
                throw new PftSyntaxException(token);
            }

            Text = token.Text;
        }

        #endregion

        #region Private members

        private bool _compiled;

        private MethodInfo _method;

        private const string Prologue = @"
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

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using MoonSharp.Interpreter;

namespace ManagedIrbis.Pft.UserSpace
{
    static class <<<CLASSNAME>>>
    {
        static void UserCode (PftNode node, PftContext context)
        {
";

        private const string Epilogue = @"
        }
    }
}
";

        private static bool HandleErrors
            (
                PftNode node,
                PftContext context,
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
                context.WriteLine(node, builder.ToString());

                return true;
            }

            return false;
        }

        private MethodInfo Compile
            (
                PftNode node,
                PftContext context
            )
        {
            string className = "Class" + Guid.NewGuid().ToString("N");
            string code = Prologue.Replace("<<<CLASSNAME>>>", className)
                + Text
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
            if (!HandleErrors(node, context, results)
                && results.Errors.Count == 0)
            {
                Type type = results.CompiledAssembly.GetType
                    (
                        "ManagedIrbis.Pft.UserSpace."
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

            return null;
        }

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (!_compiled)
            {
                _compiled = true;
                _method = Compile(this, context);
            }

            if (!ReferenceEquals(_method, null))
            {
                _method.Invoke(null, new object[] {this, context});
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc />
        public override void Write
            (
                StreamWriter writer
            )
        {
            writer.Write("{{{");
            writer.Write(Text);
            writer.Write("}}}");
        }


        #endregion
    }
}
