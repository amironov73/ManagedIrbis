// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftParallelForEach.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// parallel foreach $x in (v692^g,/)
    /// do
    ///     $x, #
    ///     if $x:'2010' then break fi
    /// end
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftParallelForEach
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Variable reference.
        /// </summary>
        [CanBeNull]
        public PftVariableReference Variable { get; set; }

        /// <summary>
        /// Sequence.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> Sequence { get; private set; }

        /// <summary>
        /// Body.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> Body { get; private set; }

        /// <inheritdoc/>
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    nodes.AddRange(Sequence);
                    nodes.AddRange(Body);
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
        public PftParallelForEach()
        {
            Sequence = new NonNullCollection<PftNode>();
            Body = new NonNullCollection<PftNode>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftParallelForEach
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Parallel);

            Sequence = new NonNullCollection<PftNode>();
            Body = new NonNullCollection<PftNode>();
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        private string[] GetSequence
            (
                [NotNull] PftContext context
            )
        {
            List<string> result = new List<string>();

            foreach (PftNode node in Sequence)
            {
                string text = context.Evaluate(node);
                if (!string.IsNullOrEmpty(text))
                {
                    string[] lines = text.SplitLines()
                        .NonEmptyLines()
                        .ToArray();
                    result.AddRange(lines);
                }
            }

            return result.ToArray();
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

            PftVariableReference variable = Variable
                .ThrowIfNull("variable");
            string name = variable.Name
                .ThrowIfNull("variable.Name");

            string[] items = GetSequence(context);
            try
            {
                foreach (string item in items)
                {
                    context.Variables.SetVariable(name, item);

                    context.Execute(Body);
                }
            }
            catch (PftBreakException)
            {
                // Nothing to do here
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc/>
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = "ParallelForEach"
            };

            if (!ReferenceEquals(Variable, null))
            {
                result.Children.Add(Variable.GetNodeInfo());
            }

            PftNodeInfo sequence = new PftNodeInfo
            {
                Name = "Sequence"
            };
            result.Children.Add(sequence);
            foreach (PftNode node in Sequence)
            {
                sequence.Children.Add(node.GetNodeInfo());
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

        #region ICloneable members

        /// <inheritdoc />
        public override object Clone()
        {
            PftParallelForEach result = (PftParallelForEach)base.Clone();

            result._virtualChildren = null;

            result.Sequence = Sequence.CloneNodes().ThrowIfNull();
            result.Body = Body.CloneNodes().ThrowIfNull();

            if (!ReferenceEquals(Variable, null))
            {
                result.Variable = (PftVariableReference)Variable.Clone();
            }

            return result;
        }

        #endregion
    }
}
