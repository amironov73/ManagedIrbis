/* PftWhile.cs --
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
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// While loop.
    /// </summary>
    /// <example>
    /// $x=0;
    /// while $x &lt; 10
    /// do
    ///     $x, ') ',
    ///     'Прикольно же!'
    ///     #
    ///     $x=$x+1;
    /// end
    /// </example>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftWhile
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Condition.
        /// </summary>
        [CanBeNull]
        public PftCondition Condition { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftWhile()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftWhile
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.While);
        }

        #endregion

        #region Private members

        private bool EvaluateCondition
            (
                [NotNull] PftContext context
            )
        {
            if (ReferenceEquals(Condition, null))
            {
                return true;
            }

            Condition.Execute(context);

            return Condition.Value;
        }

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
            OnBeforeExecution(context);

            while (EvaluateCondition(context))
            {
                context.Execute(Children);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc/>
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Name = "While"
            };

            if (!ReferenceEquals(Condition, null))
            {
                PftNodeInfo condition = new PftNodeInfo
                {
                    Name = "Condition"
                };
                result.Children.Add(condition);
                condition.Children.Add(Condition.GetNodeInfo());
            }

            PftNodeInfo body = new PftNodeInfo
            {
                Name = "Body"
            };
            result.Children.Add(body);
            foreach (PftNode node in Children)
            {
                body.Children.Add(node.GetNodeInfo());
            }

            return result;
        }

        #endregion
    }
}
