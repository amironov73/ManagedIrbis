// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftParallelGroup.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Linq;
using System.Threading.Tasks;

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
    public sealed class PftParallelGroup
        : PftNode
    {
        #region NestedClasses

        class Iteration
            : IDisposable
        {
            #region Properties

            private NonNullCollection<PftNode> Nodes { get; set; }

            private PftContext Context { get; set; }

            public int Index { get; private set; }

            public Task Task { get; private set; }

            public Exception Exception { get; private set; }

            public string Result { get { return Context.Text; } }

            #endregion

            #region Construction

            public Iteration
                (
                    [NotNull] PftContext context,
                    [NotNull] NonNullCollection<PftNode> nodes,
                    int index
                )
            {
                Context = context.Push();
                Nodes = nodes.CloneNodes();
                Index = index;
                Exception = null;

                Task = new Task(_Run);
                Task.Start();
            }

            #endregion

            #region Private members

            private void _Run()
            {
                try
                {
                    Context.Index = Index;

                    Context.Execute(Nodes);
                }
                catch (Exception exception)
                {
                    Exception = exception;
                }
            }

            #endregion

            #region Public methods

            #endregion

            #region IDisposable members

            public void Dispose()
            {
                Context.Pop();
            }

            #endregion

            #region Object members

            #endregion
        }

        #endregion

        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftParallelGroup()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftParallelGroup
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Parallel);
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
                PftGroup group = new PftGroup();

                context.CurrentGroup = group;

                OnBeforeExecution(context);

                try
                {
                    string[] affectedFields = GetAffectedFields();
                    int repeatCount = PftUtility.GetFieldCount
                        (
                            context,
                            affectedFields
                        );

                    Iteration[] iterations = new Iteration[repeatCount];
                    for (int i = 0; i < repeatCount; i++)
                    {
                        Iteration iteration = new Iteration
                            (
                                context,
                                (NonNullCollection<PftNode>)Children,
                                i
                            );
                        iterations[i] = iteration;
                    }

                    Task[] tasks = iterations
                        .Select(iter => iter.Task)
                        .ToArray();
                    Task.WaitAll(tasks);

                    foreach (Iteration iteration in iterations)
                    {
                        if (!ReferenceEquals(iteration.Exception, null))
                        {
                            throw new IrbisException
                                (
                                    "Exception in parallel group, iteration: "
                                    + iteration.Index,
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
