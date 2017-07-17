// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftGroup.cs -- группа
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Группа.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftGroup
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Throw an exception when empty group detected?
        /// </summary>
        public static bool ThrowOnEmpty { get; set; }

        /// <inheritdoc cref="PftNode.ComplexExpression"/>
        public override bool ComplexExpression
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static PftGroup()
        {
            ThrowOnEmpty = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftGroup()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftGroup
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.LeftParenthesis);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                Log.Error
                    (
                        "PftGroup::Execute: "
                        + "nested group detected: "
                        + this
                    );

                throw new PftSemanticException
                    (
                        "Nested group: "
                        + this
                    );
            }

            if (Children.Count == 0)
            {
                Log.Error
                    (
                        "PftGroup::Execute: "
                        + "empty group detected: "
                        + this
                    );

                if (ThrowOnEmpty)
                {
                    throw new PftSemanticException
                        (
                            "Empty group: "
                            + this
                        );
                }
            }

            try
            {
                context.CurrentGroup = this;

                OnBeforeExecution(context);

                try
                {
                    context.DoRepeatableAction
                        (
                            ctx =>
                            {
                                foreach (PftNode child in Children)
                                {
                                    child.Execute(ctx);
                                }
                            }
                        );
                }
                catch (PftBreakException exception)
                {
                    // It was break operator

                    Log.TraceException
                        (
                            "PftGroup::Execute",
                            exception
                        );
                }

                OnAfterExecution(context);
            }
            finally
            {
                context.CurrentGroup = null;
            }
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            bool isComplex = PftUtility.IsComplexExpression(Children);
            if (isComplex)
            {
                printer.EatWhitespace();
                printer.EatNewLine();
                printer.WriteLine();
                printer
                    .WriteIndent()
                    .Write('(');
                printer.IncreaseLevel();
                printer.WriteLine();
                printer.WriteIndent();
            }
            else
            {
                printer
                    .WriteIndendIfNeeded()
                    .Write("( ");
            }
            base.PrettyPrint(printer);
            if (isComplex)
            {
                printer.EatWhitespace();
                printer.EatNewLine();
                printer.WriteLine()
                    .DecreaseLevel()
                    .WriteIndent()
                    .Write(')')
                    .WriteLine();
            }
            else
            {
                printer
                    .WriteIndendIfNeeded()
                    .Write(')');
            }
        }

        #endregion
    }
}
