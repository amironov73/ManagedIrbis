// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftRound.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Linq;
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
    public sealed class PftRound
        : PftNumeric
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
        public PftRound()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRound
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Round);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRound
            (
                [NotNull] PftNumeric value
            )
        {
            Code.NotNull(value, "value");

            Children.Add(value);
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            PftNumeric child = Children.FirstOrDefault() as PftNumeric;
            if (ReferenceEquals(child, null))
            {
                throw new PftCompilerException();
            }

            child.Compile(compiler);

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine("double value = ")
                .CallNodeMethod(child);

            compiler
                .WriteIndent()
                .WriteLine("double result = Math.Round(value);");

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

            PftNumeric child = Children.FirstOrDefault() as PftNumeric;
            if (!ReferenceEquals(child, null))
            {
                child.Execute(context);
                Value = Math.Round(child.Value);
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
                .Write("round(");
            base.PrettyPrint(printer);
            printer.Write(")");
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
            result.Append("round(");
            PftNode child = Children.FirstOrDefault();
            if (!ReferenceEquals(child, null))
            {
                result.Append(child);
            }
            result.Append(')');

            return result.ToString();
        }

        #endregion
    }
}
