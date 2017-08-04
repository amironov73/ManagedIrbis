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
using System.IO;
using System.Text;

using AM;
using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// <code>
    /// from $x in (v692^b/)
    /// where $x:'2008'
    /// select 'Item: ', $x,
    /// order $x,
    /// end
    /// </code>
    /// </example>
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
        public PftNodeCollection Source { get; private set; }

            /// <summary>
        /// Where clause.
        /// </summary>
        [CanBeNull]
        public PftCondition Where { get; set; }

        /// <summary>
        /// Select clause.
        /// </summary>
        [NotNull]
        public PftNodeCollection Select { get; private set; }

        /// <summary>
        /// Order clause.
        /// </summary>
        [NotNull]
        public PftNodeCollection Order { get; private set; }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

        /// <inheritdoc cref="PftNode.ComplexExpression" />
        public override bool ComplexExpression
        {
            get { return true; }
        }

        /// <inheritdoc cref="PftNode.Children" />
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

                Log.Error
                    (
                        "PftFrom::Children: "
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
        public PftFrom()
        {
            Source = new PftNodeCollection(this);
            Select = new PftNodeCollection(this);
            Order = new PftNodeCollection(this);
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

            Source = new PftNodeCollection(this);
            Select = new PftNodeCollection(this);
            Order = new PftNodeCollection(this);
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        #endregion

        #region Public methods

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            PftFrom result = (PftFrom)base.Clone();

            result._virtualChildren = null;

            if (!ReferenceEquals(Variable, null))
            {
                result.Variable = (PftVariableReference) Variable.Clone();
            }

            result.Source = Source.CloneNodes(result).ThrowIfNull();

            if (!ReferenceEquals(Where, null))
            {
                result.Where = (PftCondition) Where.Clone();
            }

            result.Select = Select.CloneNodes(result).ThrowIfNull();
            result.Order = Order.CloneNodes(result).ThrowIfNull();

            return result;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.CompareNode" />
        internal override void CompareNode
            (
                PftNode otherNode
            )
        {
            base.CompareNode(otherNode);

            PftFrom otherFrom = (PftFrom) otherNode;
            PftSerializationUtility.CompareNodes
                (
                    Variable,
                    otherFrom.Variable
                );
            PftSerializationUtility.CompareLists
                (
                    Source,
                    otherFrom.Source
                );
            PftSerializationUtility.CompareNodes
                (
                    Where,
                    otherFrom.Where
                );
            PftSerializationUtility.CompareLists
                (
                    Select,
                    otherFrom.Select
                );
            PftSerializationUtility.CompareLists
                (
                    Order,
                    otherFrom.Order
                );
        }

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (ReferenceEquals(Variable, null)
                || Source.Count == 0
                || Select.Count == 0)
            {
                throw new PftCompilerException();
            }

            // TODO implement

            compiler.CompileNodes(Source);
            if (!ReferenceEquals(Where, null))
            {
                Where.Compile(compiler);
            }
            compiler.CompileNodes(Select);
            compiler.CompileNodes(Order);

            compiler.StartMethod(this);

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        }

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Variable = (PftVariableReference) PftSerializer
                .DeserializeNullable(reader);
            PftSerializer.Deserialize(reader, Source);
            Where = (PftCondition) PftSerializer.DeserializeNullable(reader);
            PftSerializer.Deserialize(reader, Select);
            PftSerializer.Deserialize(reader, Order);
        }

        /// <inheritdoc cref="PftNode.Execute" />
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

#if WIN81 || PORTABLE

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

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
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

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
        {
            printer.EatWhitespace();
            printer.EatNewLine();

            printer
                .WriteLine()
                .WriteIndent()
                .Write("from ");

            if (!ReferenceEquals(Variable, null))
            {
                Variable.PrettyPrint(printer);
            }

            printer.Write(" in ");

            bool first = true;
            foreach (PftNode node in Source)
            {
                if (!first)
                {
                    printer.Write(", ");
                }
                node.PrettyPrint(printer);
                first = false;
            }
            printer.WriteLine();

            if (!ReferenceEquals(Where, null))
            {
                printer
                    .WriteIndent()
                    .Write("where ");
                Where.PrettyPrint(printer);
                printer.WriteLine();
            }

            printer
                .WriteIndentIfNeeded()
                .Write("select ");
            first = true;
            foreach (PftNode node in Select)
            {
                if (!first)
                {
                    printer.Write(", ");
                }
                node.PrettyPrint(printer);
                first = false;
            }
            printer.WriteLine();

            if (Order.Count != 0)
            {
                printer
                    .WriteIndent()
                    .Write("order ");
                first = true;
                foreach (PftNode node in Order)
                {
                    if (!first)
                    {
                        printer.Write(", ");
                    }
                    node.PrettyPrint(printer);
                    first = false;
                }
                printer.WriteLine();
            }

            printer.WriteLine();
            printer
                .WriteIndent()
                .WriteLine("end");
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.SerializeNullable(writer, Variable);
            PftSerializer.Serialize(writer, Source);
            PftSerializer.SerializeNullable(writer, Where);
            PftSerializer.Serialize(writer, Select);
            PftSerializer.Serialize(writer, Order);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = StringBuilderCache.Acquire();
            result.Append("from ");
            result.Append(Variable);
            result.Append(" in ");
            PftUtility.NodesToText(result, Source);
            if (!ReferenceEquals(Where, null))
            {
                result.Append(" where ");
                result.Append(Where);
            }
            result.Append(" select ");
            PftUtility.NodesToText(result, Select);
            if (Order.Count != 0)
            {
                result.Append(" order ");
                PftUtility.NodesToText(result, Order);
            }
            result.Append(" end");

            return StringBuilderCache.GetStringAndRelease(result);
        }

        #endregion
    }
}
