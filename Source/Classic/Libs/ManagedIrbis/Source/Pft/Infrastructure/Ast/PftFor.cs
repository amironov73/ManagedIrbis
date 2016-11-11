/* PftFor.cs --
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
using AM.Collections;
using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftFor
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Initialization.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> Initialization { get; private set; }

        /// <summary>
        /// Condition.
        /// </summary>
        [CanBeNull]
        public PftCondition Condition { get; set; }

        /// <summary>
        /// Loop statements.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> Loop { get; private set; }

        /// <summary>
        /// Body.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> Body { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFor()
        {
            Initialization = new NonNullCollection<PftNode>();
            Loop = new NonNullCollection<PftNode>();
            Body = new NonNullCollection<PftNode>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFor
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Initialization = new NonNullCollection<PftNode>();
            Loop = new NonNullCollection<PftNode>();
            Body = new NonNullCollection<PftNode>();
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

            context.Execute(Initialization);

            while (EvaluateCondition(context))
            {
                context.Execute(Body);
                context.Execute(Loop);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc/>
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Name = "For"
            };

            if (Initialization.Count != 0)
            {
                PftNodeInfo init = new PftNodeInfo
                {
                    Name = "Init"
                };
                result.Children.Add(init);
                foreach (PftNode node in Initialization)
                {
                    init.Children.Add(node.GetNodeInfo());
                }
            }

            if (!ReferenceEquals(Condition, null))
            {
                PftNodeInfo condition = new PftNodeInfo
                {
                    Name = "Condition"
                };
                result.Children.Add(condition);
                condition.Children.Add(Condition.GetNodeInfo());
            }

            if (Loop.Count != 0)
            {
                PftNodeInfo loop = new PftNodeInfo
                {
                    Name="Loop"
                };
                result.Children.Add(loop);
                foreach (PftNode node in Loop)
                {
                    loop.Children.Add(node.GetNodeInfo());
                }
            }

            PftNodeInfo body = new PftNodeInfo
            {
                Name = "Body"
            };
            result.Children.Add(body);
            foreach (PftNode node in Body)
            {
                body.Children.Add(node.GetNodeInfo());
            }

            return result;
        }

        #endregion
    }
}
