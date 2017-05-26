// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftParallelFor.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Parallel for loop.
    /// </summary>
    /// <example>
    /// parallel for $x=0; $x &lt; 10; $x = $x+1;
    /// do
    ///     $x, ') ',
    ///     'Прикольно же!'
    ///     #
    /// end
    /// </example>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftParallelFor
        : PftNode
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

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

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    nodes.AddRange(Initialization);
                    if (!ReferenceEquals(Condition, null))
                    {
                        nodes.Add(Condition);
                    }
                    nodes.AddRange(Loop);
                    nodes.AddRange(Body);
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            protected set
            {
                // Nothing to do here

                Log.Error
                    (
                        "PftParallelFor::Children: "
                        + "set value="
                        + value.NullableToVisibleString()
                    );
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftParallelFor()
        {
            Initialization = new NonNullCollection<PftNode>();
            Loop = new NonNullCollection<PftNode>();
            Body = new NonNullCollection<PftNode>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftParallelFor
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Parallel);

            Initialization = new NonNullCollection<PftNode>();
            Loop = new NonNullCollection<PftNode>();
            Body = new NonNullCollection<PftNode>();
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        private bool _EvaluateCondition
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

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            context.Execute(Initialization);

            try
            {
                List<PftIteration> allIterations 
                    = new List<PftIteration>();

                while (_EvaluateCondition(context))
                {
                    PftIteration iteration = new PftIteration
                        (
                            context,
                            Body,
                            0,
                            (iter, data) => iter.Context.Execute(iter.Nodes),
                            this,
                            false
                        );
                    allIterations.Add(iteration);

                    // TODO : fix this!
                    context.Execute(Loop);
                }

                Task[] tasks = allIterations
                    .Select(iter => iter.Task)
                    .ToArray();
                Task.WaitAll(tasks);

                foreach (PftIteration iteration in allIterations)
                {
                    if (!ReferenceEquals(iteration.Exception, null))
                    {
                        Log.TraceException
                            (
                                "PftParallelFor::Execute",
                                iteration.Exception
                            );

                        throw new IrbisException
                            (
                                "Exception in parallel for",
                                iteration.Exception
                            );
                    }

                    context.Write
                        (
                            this,
                            iteration.Result
                        );
                }

            }
            catch (PftBreakException exception)
            {
                // It was break operator

                Log.TraceException
                    (
                        "PftParallelFor::Execute",
                        exception
                    );
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = "ParallelFor"
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
                    Node = Condition,
                    Name = "Condition"
                };
                result.Children.Add(condition);
                condition.Children.Add(Condition.GetNodeInfo());
            }

            if (Loop.Count != 0)
            {
                PftNodeInfo loop = new PftNodeInfo
                {
                    Name = "Loop"
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

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            PftParallelFor result = (PftParallelFor) base.Clone();

            result._virtualChildren = null;

            result.Initialization 
                = Initialization.CloneNodes().ThrowIfNull();
            result.Loop = Loop.CloneNodes().ThrowIfNull();
            result.Body = Body.CloneNodes().ThrowIfNull();

            if (!ReferenceEquals(Condition, null))
            {
                result.Condition = (PftCondition) Condition.Clone();
            }

            return result;
        }

        #endregion
    }
}
