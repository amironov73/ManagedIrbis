// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftComment.cs -- comment
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Comment.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftComment
        : PftNode
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ConstantExpression" />
        public override bool ConstantExpression
        {
            get { return true; }
        }

        /// <inheritdoc cref="PftNode.RequiresConnection" />
        public override bool RequiresConnection
        {
            get { return false; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftComment()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftComment
            (
                [NotNull] PftToken token
            )
            : base (token)
        {
            Text = token.Text;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftComment
            (
                [CanBeNull] string text
            )
        {
            Text = text;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            compiler.StartMethod(this);

            // Nothing to do actually

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

            // Nothing to do actually

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode Optimize()
        {
            // Take the node away from the AST

            return null;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            // Добавляем пробел для читабельности
            printer
                .WriteIndentIfNeeded()
                .Write("/* ");
            printer.WriteLine(Text);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="PftNode.ToString" />
        public override string ToString()
        {
            return
                "/* "
                + Text
                + Environment.NewLine;
        }

        #endregion
    }
}
