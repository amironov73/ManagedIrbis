// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftConditionalStatement.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

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
    public sealed class PftConditionalStatement
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Condition
        /// </summary>
        [CanBeNull]
        public PftCondition Condition { get; set; }

        /// <summary>
        /// Else branch.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> ElseBranch { get; private set; }

            /// <summary>
        /// Then branch.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> ThenBranch { get; private set; }

        /// <inheritdoc />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>
                    {
                        Condition
                    };
                    nodes.AddRange(ThenBranch);
                    nodes.AddRange(ElseBranch);
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            protected set
            {
                // Nothing to do here
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftConditionalStatement()
        {
            ElseBranch = new NonNullCollection<PftNode>();
            ThenBranch = new NonNullCollection<PftNode>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftConditionalStatement
            (
                PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.If);

            ElseBranch = new NonNullCollection<PftNode>();
            ThenBranch = new NonNullCollection<PftNode>();
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

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

            if (ReferenceEquals(Condition, null))
            {
                throw new PftSyntaxException();
            }

            Condition.Execute(context);

            if (Condition.Value)
            {
                foreach (PftNode child in ThenBranch)
                {
                    child.Execute(context);
                }
            }
            else
            {
                foreach (PftNode child in ElseBranch)
                {
                    child.Execute(context);
                }
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc/>
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = SimplifyTypeName(GetType().Name)
            };

            if (!ReferenceEquals(Condition, null))
            {
                PftNodeInfo conditionNode = new PftNodeInfo
                {
                    Node = Condition,
                    Name = "Condition"
                };
                result.Children.Add(conditionNode);
                conditionNode.Children.Add(Condition.GetNodeInfo());
            }

            PftNodeInfo thenNode = new PftNodeInfo
            {
                Name = "Then"
            };
            foreach (PftNode node in ThenBranch)
            {
                thenNode.Children.Add(node.GetNodeInfo());
            }
            result.Children.Add(thenNode);

            if (ElseBranch.Count != 0)
            {
                PftNodeInfo elseNode = new PftNodeInfo
                {
                    Name = "Else"
                };
                foreach (PftNode node in ElseBranch)
                {
                    elseNode.Children.Add(node.GetNodeInfo());
                }
                result.Children.Add(elseNode);
            }

            return result;
        }

        #endregion
    }
}
