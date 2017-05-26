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

        #endregion

        #region Construction

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
                        + "nested group detected"
                    );

                throw new PftSemanticException("Nested group");
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

        #endregion
    }
}
