// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftParallelGroup.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Text;

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

        /// <summary>
        /// Throw an exception when an empty group is detected?
        /// </summary>
        public static bool ThrowOnEmpty { get; set; }

        /// <inheritdoc cref="PftNode.ComplexExpression" />
        public override bool ComplexExpression
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static PftParallelGroup()
        {
            ThrowOnEmpty = false;
        }

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

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftParallelGroup
            (
                params PftNode[] children
            )
            : base(children)
        {
        }

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
                        "PftParallelGroup::Execute: "
                        + "nested group detected: "
                        + this
                    );

                throw new PftSemanticException
                    (
                        "Nested group: "
                        + this
                    );
            }

            if (Children.Count == 0)
            {
                Log.Error
                    (
                        "PftParalllelGroup::Execute: "
                        + "empty group: "
                        + this
                    );

                if (ThrowOnEmpty)
                {
                    throw new PftSemanticException
                        (
                            "Empty group: "
                            + this
                        );
                }
            }

            try
            {
                PftGroup group = new PftGroup();

                context.CurrentGroup = group;

                OnBeforeExecution(context);

                try
                {
                    int[] affectedFields = GetAffectedFields();
                    int repeatCount = PftUtility.GetFieldCount
                        (
                            context,
                            affectedFields
                        )
                        + 1;

                    PftIteration[] allIterations
                        = new PftIteration[repeatCount];
                    for (int index = 0; index < repeatCount; index++)
                    {
                        PftIteration iteration = new PftIteration
                            (
                                context,
                                (PftNodeCollection)Children,
                                index,
                                (iter,data) => iter.Context.Execute(iter.Nodes),
                                this,
                                true
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
                            Log.TraceException
                                (
                                    "PftParallelGroup::Execute",
                                    iteration.Exception
                                );

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
                catch (PftBreakException exception)
                {
                    // It was break operator

                    Log.TraceException
                        (
                            "PftParalleGroup::Execute",
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

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode Optimize()
        {
            PftNodeCollection children = (PftNodeCollection) Children;
            children.Optimize();

            if (children.Count == 0)
            {
                // Take the node away from the AST

                return null;
            }

            return this;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            bool isComplex = PftUtility.IsComplexExpression(Children);
            if (isComplex)
            {
                printer.EatWhitespace();
                printer.EatNewLine();
                printer.WriteLine();
                printer
                    .WriteIndent()
                    .Write("parallel(");
                printer.IncreaseLevel();
                printer.WriteLine();
                printer.WriteIndent();
            }
            else
            {
                printer
                    .WriteIndentIfNeeded()
                    .Write("parallel( ");
            }
            base.PrettyPrint(printer);
            if (isComplex)
            {
                printer.EatWhitespace();
                printer.EatNewLine();
                printer.WriteLine()
                    .DecreaseLevel()
                    .WriteIndent()
                    .Write(')')
                    .WriteLine();
            }
            else
            {
                printer
                    .WriteIndentIfNeeded()
                    .Write(')');
            }
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        [DebuggerStepThrough]
        protected internal override bool ShouldSerializeText()
        {
            return false;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = StringBuilderCache.Acquire();
            result.Append("parallel(");
            PftUtility.NodesToText(result, Children);
            result.Append(')');

            return StringBuilderCache.GetStringAndRelease(result);
        }

        #endregion
    }
}
