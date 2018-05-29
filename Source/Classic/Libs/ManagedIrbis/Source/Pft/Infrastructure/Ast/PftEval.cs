// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftEval.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Исполнение динамического PFT-формата.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftEval
        : PftNode
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
        public PftEval()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftEval
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Eval);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftEval
            (
                params PftNode[] children
            )
        {
            foreach (PftNode child in children)
            {
                Children.Add(child);
            }
        }

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            string expression = context.Evaluate(Children);
            if (!string.IsNullOrEmpty(expression))
            {
                PftProgram program = PftUtility.CompileProgram(expression);
                program.Execute(context);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .WriteIndentIfNeeded()
                .Write("eval(")
                .WriteNodes(Children)
                .Write(")");
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString()" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("eval(");
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
