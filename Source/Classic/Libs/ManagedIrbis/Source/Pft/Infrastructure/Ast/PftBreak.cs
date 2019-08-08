// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftBreak.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /*
     * Как работает оператор break в ИРБИС64:
     *
     * Вне повторяющейся группы он немедленно прекращает
     * выполнение текущего контекста (управление переходит
     * родительскому контексту, если таковой существует).
     * При этом неважно, находится ли он внутри операторных
     * скобок IF/FI.
     *
     * Внутри повторяющейся группы он по-разному работает
     * в зависимости от того, находится ли он внутри
     * операторных скобок IF/FI.
     *
     * Внутри IF/FI он выставляет флаг "не надо следующего
     * повторения группы". Вне IF/FI он просто прекращает
     * выполнение текущего контекста (см. выше).
     *
     * Есть маленькая тонкость: break останавливает повторяющуюся
     * группу, только если находится на первом логическом уровне
     * после IF, иначе она срабатывает как вне IF/FI.
     *
     * Пример:
     *
     * 'first',/,
     * (
     * 'second',/,
     * if 1=1 then break fi,
     * v300,/,
     * 'third',/,
     * ),
     * 'fourth',
     *
     * Выведет:
     *
     * first
     * second
     * Указ.
     * third
     * second
     * third
     * fourth
     *
     */

    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftBreak
        : PftNode
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ConstantExpression" />
        public override bool ConstantExpression
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftBreak()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftBreak
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Break);
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
            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine("if (InGroup)")
                .WriteIndent()
                .WriteLine("{")
                .IncreaseIndent()
                .WriteIndent()

                .WriteLine("if (PftConfig.BreakImmediate)")
                .WriteIndent()
                .WriteLine("{")
                .IncreaseIndent()
                .WriteIndent()
                .WriteLine("throw new PftBreakException(null);")
                .DecreaseIndent()
                .WriteIndent()
                .WriteLine("}")
                .WriteIndent()
                .WriteLine("else")
                .WriteIndent()
                .WriteLine("{")
                .IncreaseIndent()
                .WriteIndent()
                .WriteLine("Context.BreakFlag = true;")
                .DecreaseIndent()
                .WriteIndent()
                .WriteLine("}")

                .WriteIndent()
                .WriteLine("}")
                .WriteIndent()
                .WriteLine("else")
                .WriteIndent()
                .WriteLine("{")
                .IncreaseIndent()
                .WriteIndent()
                .WriteLine("throw new PftBreakException(null);")
                .DecreaseIndent()
                .WriteIndent()
                .WriteLine("}"); //-V3010

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

            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                // Мы внутри группы
                bool handled = false;

                if (PftConfig.BreakImmediate)
                {
                    Log.Trace
                        (
                            "PftBreak::Execute: "
                            + "break inside the group"
                        );

                    throw new PftBreakException(this);
                }

                PftNode possibleCondition = Parent;
                if (possibleCondition is PftConditionalStatement)
                {
                    //PftNode possibleGroup = condition.Parent;
                    //if ((possibleGroup is PftGroup)
                    //    || (possibleGroup is PftParallelGroup))
                    //{
                        context.BreakFlag = true;
                        handled = true;
                    //}
                }

                if (!handled)
                {
                    throw new PftBreakException(this);
                }
            }
            else
            {
                // Это не группа, а оператор for
                // или что-нибудь в этом роде

                Log.Trace
                    (
                        "PftBreak::Execute: "
                        + "break outside the group"
                    );

                throw new PftBreakException(this);
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
                .SingleSpace()
                .Write("break")
                .SingleSpace();
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
            return "break";
        }

        #endregion
    }
}
