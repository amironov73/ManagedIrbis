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
            : this()
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.If);
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
        public override void PrintDebug
            (
                TextWriter writer,
                int level
            )
        {
            for (int i = 0; i < level; i++)
            {
                writer.Write("| ");
            }
            writer.WriteLine("ConditionalStatement");

            for (int i = 0; i <= level; i++)
            {
                writer.Write("| ");
            }
            writer.WriteLine("Condition:");

            if (!ReferenceEquals(Condition, null))
            {
                Condition.PrintDebug(writer, level + 2);
            }

            for (int i = 0; i <= level; i++)
            {
                writer.Write("| ");
            }
            writer.WriteLine("Then:");
            foreach (PftNode node in ThenBranch)
            {
                node.PrintDebug(writer, level + 2);
            }

            for (int i = 0; i <= level; i++)
            {
                writer.Write("| ");
            }
            writer.WriteLine("Else:");
            foreach (PftNode node in ElseBranch)
            {
                node.PrintDebug(writer, level + 2);
            }
        }

        #endregion
    }
}
