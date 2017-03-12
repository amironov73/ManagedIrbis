// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftFrom.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

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
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftFrom
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Variable reference.
        /// </summary>
        [CanBeNull]
        public PftVariableReference Variable { get; set; }

        /// <summary>
        /// Source.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> Source { get; private set; }

            /// <summary>
        /// Where clause.
        /// </summary>
        [CanBeNull]
        public PftCondition Where { get; set; }

        /// <summary>
        /// Select clause.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> Select { get; private set; }

        /// <summary>
        /// Order clause.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> Order { get; private set; }

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
                    if (!ReferenceEquals(Variable, null))
                    {
                        nodes.Add(Variable);
                    }
                    nodes.AddRange(Source);
                    if (!ReferenceEquals(Where, null))
                    {
                        nodes.Add(Where);
                    }
                    nodes.AddRange(Select);
                    nodes.AddRange(Order);
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
        public PftFrom()
        {
            Source = new NonNullCollection<PftNode>();
            Select = new NonNullCollection<PftNode>();
            Order = new NonNullCollection<PftNode>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFrom
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.From);

            Source = new NonNullCollection<PftNode>();
            Select = new NonNullCollection<PftNode>();
            Order = new NonNullCollection<PftNode>();
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        #endregion

        #region Public methods

        #endregion

        #region ICloneable members

        /// <inheritdoc />
        public override object Clone()
        {
            PftFrom result = (PftFrom)base.Clone();

            result._virtualChildren = null;

            if (!ReferenceEquals(Variable, null))
            {
                result.Variable = (PftVariableReference) Variable.Clone();
            }

            result.Source = Source.CloneNodes().ThrowIfNull();

            if (!ReferenceEquals(Where, null))
            {
                result.Where = (PftCondition) Where.Clone();
            }

            result.Select = Select.CloneNodes().ThrowIfNull();
            result.Order = Order.CloneNodes().ThrowIfNull();

            return result;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            PftVariableManager manager = context.Variables;
            PftVariableReference variable = Variable;
            if (!ReferenceEquals(variable, null))
            {
                string name = variable.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    List<string> buffer = new List<string>();

                    // In clause
                    string sourceText = context.Evaluate(Source);
                    string[] lines = sourceText.SplitLines();

                    // Where clause
                    if (!ReferenceEquals(Where, null))
                    {
                        foreach (string line in lines)
                        {
                            manager.SetVariable(name, line);
                            Where.Execute(context);
                            if (Where.Value)
                            {
                                buffer.Add(line);
                            }
                        }

                        lines = buffer.ToArray();
                    }

                    // Select clause
                    buffer.Clear();
                    foreach (string line in lines)
                    {
                        manager.SetVariable(name, line);
                        string value = context.Evaluate(Select);
                        buffer.Add(value);
                    }

                    lines = buffer.ToArray();

                    // Order clause
                    buffer.Clear();
                    if (Order.Count != 0)
                    {
                        foreach (string line in lines)
                        {
                            manager.SetVariable(name, line);
                            string value = context.Evaluate(Order);
                            buffer.Add(value);
                        }

#if WIN81

                        // TODO: implement!

#else

                        Array.Sort
                            (
                                lines,
                                buffer.ToArray()
                            );

#endif
                    }

                    string output = string.Join
                        (
                            Environment.NewLine,
                            lines
                        );
                    if (!string.IsNullOrEmpty(output))
                    {
                        context.Write(this, output);
                    }
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
                Name = "From"
            };

            if (!ReferenceEquals(Variable, null))
            {
                PftNodeInfo variable = new PftNodeInfo
                {
                    Name = "Variable"
                };
                result.Children.Add(variable);
                variable.Children.Add(Variable.GetNodeInfo());
            }

            PftNodeInfo sourceClause = new PftNodeInfo
            {
                Name = "Source"
            };
            result.Children.Add(sourceClause);
            foreach (PftNode node in Source)
            {
                sourceClause.Children.Add(node.GetNodeInfo());
            }

            if (!ReferenceEquals(Where, null))
            {
                PftNodeInfo whereClause = new PftNodeInfo
                {
                    Name = "Where"
                };
                result.Children.Add(whereClause);
                whereClause.Children.Add(Where.GetNodeInfo());
            }

            PftNodeInfo selectClause = new PftNodeInfo
            {
                Name = "Select"
            };
            result.Children.Add(selectClause);
            foreach (PftNode node in Select)
            {
                selectClause.Children.Add(node.GetNodeInfo());
            }

            if (Order.Count != 0)
            {
                PftNodeInfo orderClause = new PftNodeInfo
                {
                    Name = "Order"
                };
                result.Children.Add(orderClause);
                foreach (PftNode node in Order)
                {
                    orderClause.Children.Add(node.GetNodeInfo());
                }
            }

            return result;
        }

        #endregion
    }
}
