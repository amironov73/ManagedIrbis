/* PftGroup.cs -- группа
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            if (context.CurrentGroup != null)
            {
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
                catch (PftBreakException)
                {
                    // Nothing to do here
                    // Just swallow the exception
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
