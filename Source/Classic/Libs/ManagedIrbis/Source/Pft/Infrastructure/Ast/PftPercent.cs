// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftPercent.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Команда % подавляет все последовательно
    /// расположенные пустые строки(если они имеются)
    /// между текущей строкой и последней непустой строкой.
    /// Таким образом, формат
    /// 
    /// %##V10%##V20%##V30 ...
    /// 
    /// приведет к созданию одной и только одной пустой
    /// строки между каждым полем, независимо от их наличия
    /// или отсутствия в документе.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftPercent
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
        public PftPercent()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftPercent
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Percent);
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

            compiler
                .WriteIndent()
                .WriteLine("Context.Output.RemoveEmptyLines();")
                .WriteIndent()
                .WriteLine("if (!Context.Output.PrecededByEmptyLine())")
                .WriteIndent()
                .WriteLine("{")
                .IncreaseIndent()
                .WriteIndent()
                .WriteLine("Context.WriteLine(null);")
                .DecreaseIndent()
                .WriteIndent()
                .WriteLine("}")
                .WriteIndent()
                .WriteLine("Context.EatNextNewLine = true;");

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

            context.Output.RemoveEmptyLines();
            if (!context.Output.PrecededByEmptyLine())
            {
                context.WriteLine(this);
            }
            context.EatNextNewLine = true;

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
                .Write('%')
                .SingleSpace()
                .WriteLineIfNeeded();
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        [DebuggerStepThrough]
        protected internal override bool ShouldSerializeText()
        {
            return false;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return "%";
        }

        #endregion
    }
}
