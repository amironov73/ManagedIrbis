// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftParallelGroup.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

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
                        )
                        + 1;

                    PftIteration[] allIterations = new PftIteration[repeatCount];
                    for (int index = 0; index < repeatCount; index++)
                    {
                        PftIteration iteration = new PftIteration
                            (
                                context,
                                (NonNullCollection<PftNode>)Children,
                                index,
                                (iter,data) => iter.Context.Execute(iter.Nodes),
                                this
                            );
                        allIterations[index] = iteration;
                    }

                    Task[] tasks = allIterations
                        .Select(iter => iter.Task)
                        .ToArray();
                    Task.WaitAll(tasks);

                    foreach (PftIteration iteration in allIterations)
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
