// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftCsEval.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#if CLASSIC ||NETCORE

using System.Reflection;

#endif

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Исполнение динамического C#-формата.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftCsEval
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
        public PftCsEval()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCsEval
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.CsEval);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCsEval
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

        #region Private members

#if CLASSIC || NETCORE

        private string _text;
        private MethodInfo _method;

#endif

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

#if CLASSIC || NETCORE

            string expression = context.Evaluate(Children);
            if (!string.IsNullOrEmpty(expression))
            {
                if (expression != _text)
                {
                    _text = expression;
                    _method = SharpRunner.CompileSnippet
                        (
                            expression,
                            "PftNode node, PftContext context",
                            err => context.WriteLine(this, err)
                        );
                }

                if (!ReferenceEquals(_method, null))
                {
                    _method.Invoke(null, new object[] { this, context });
                }
            }

#endif

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
                .Write("cseval(")
                .WriteNodes(Children)
                .Write(")");
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString()" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("cseval(");
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
