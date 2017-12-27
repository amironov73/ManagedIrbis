// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftBlank.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
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
    public sealed class PftBlank
        : PftCondition
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftBlank()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftBlank
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Blank);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftBlank
            (
                params PftNode[] children
            )
            : base(children)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Is the string blank?
        /// </summary>
        public static bool IsBlank
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            foreach (char c in text)
            {
                if (!char.IsWhiteSpace(c))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            compiler.CompileNodes(Children);

            string actionName = compiler.CompileAction(Children);

            compiler.StartMethod(this);

            if (string.IsNullOrEmpty(actionName))
            {
                compiler
                    .WriteIndent()
                    .WriteLine("bool result = true;");
            }
            else
            {
                compiler
                    .WriteIndent()
                    .WriteLine("string text = Evaluate({0});", actionName);

                compiler
                    .WriteIndent()
                    .WriteLine("bool result = PftBlank.IsBlank(text);");
            }

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

            string text = context.Evaluate(Children);

            Value = IsBlank(text);

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .SingleSpace()
                .Write("blank(");
            base.PrettyPrint(printer);
            printer.Write(')');
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        [DebuggerStepThrough]
        protected internal override bool ShouldSerializeText()
        {
            return false;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("blank(");
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
