// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftL.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftL
        : PftNumeric
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftL()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftL
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.L);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            compiler.CompileNodes(Children);

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine("double result = 0.0;");

            compiler
                .WriteIndent()
                .WriteLine("Action action = () =>")
                .WriteIndent()
                .WriteLine("{")
                .IncreaseIndent()
                .CallNodes(Children)
                .DecreaseIndent()
                .WriteIndent()
                .WriteLine("};");

            compiler
                .WriteIndent()
                .WriteLine("string text = Evaluate(action);")
                .WriteIndent()
                .WriteLine("if (!string.IsNullOrEmpty(text))")
                .WriteIndent()
                .WriteLine("{")
                .IncreaseIndent()
                .WriteIndent()
                .WriteLine("int[] found = Context.Provider.Search(text);")
                .WriteIndent()
                .WriteLine("if (found.Length != 0)")
                .WriteIndent()
                .WriteLine("{")
                .IncreaseIndent()
                .WriteIndent()
                .WriteLine("result = found[0];")
                .DecreaseIndent()
                .WriteIndent()
                .WriteLine("}")
                .DecreaseIndent()
                .WriteIndent()
                .WriteLine("}");

            compiler
                .WriteIndent()
                .WriteLine("return result;");

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            Value = 0;
            string expression = context.Evaluate(Children);
            if (!string.IsNullOrEmpty(expression))
            {
                int[] found = context.Provider.Search(expression);
                if (found.Length != 0)
                {
                    Value = found[0];
                }
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.EatWhitespace();
            printer
                .SingleSpace()
                .Write("l(");
            base.PrettyPrint(printer);
            printer.Write(')');
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("l(");
            bool first = true;
            foreach (PftNode child in Children)
            {
                if (!first)
                {
                    result.Append(' ');
                }
                result.Append(child);
                first = false;
            }
            result.Append(')');

            return result.ToString();
        }

        #endregion
    }
}
