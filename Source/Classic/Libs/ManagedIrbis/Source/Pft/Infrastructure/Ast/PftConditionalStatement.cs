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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

using AM;
using AM.Logging;

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
        public PftNodeCollection ElseBranch { get; private set; }

        /// <summary>
        /// Then branch.
        /// </summary>
        [NotNull]
        public PftNodeCollection ThenBranch { get; private set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(Condition, null))
                    {
                        nodes.Add(Condition);
                    }
                    nodes.AddRange(ThenBranch);
                    nodes.AddRange(ElseBranch);
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            [ExcludeFromCodeCoverage]
            protected set
            {
                // Nothing to do here
                Log.Error
                    (
                        "PftConditionalStatement::Children: "
                        + "set value="
                        + value.ToVisibleString()
                    );
            }
        }

        /// <inheritdoc cref="PftNode.ComplexExpression" />
        public override bool ComplexExpression
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftConditionalStatement()
        {
            ElseBranch = new PftNodeCollection(this);
            ThenBranch = new PftNodeCollection(this);
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

            ElseBranch = new PftNodeCollection(this);
            ThenBranch = new PftNodeCollection(this);
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            PftConditionalStatement result = (PftConditionalStatement)base.Clone();

            result._virtualChildren = null;

            if (!ReferenceEquals(Condition, null))
            {
                result.Condition = (PftCondition)Condition.Clone();
            }

            result.ElseBranch = ElseBranch.CloneNodes(result)
                .ThrowIfNull();
            result.ThenBranch = ThenBranch.CloneNodes(result)
                .ThrowIfNull();

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

            PftConditionalStatement otherStatement
                = (PftConditionalStatement)otherNode;
            PftSerializationUtility.CompareNodes
                (
                    Condition,
                    otherStatement.Condition
                );
            PftSerializationUtility.CompareLists
                (
                    ThenBranch,
                    otherStatement.ThenBranch
                );
            PftSerializationUtility.CompareLists
                (
                    ElseBranch,
                    otherStatement.ElseBranch
                );
        }

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (ReferenceEquals(Condition, null))
            {
                throw new PftCompilerException();
            }

            Condition.Compile(compiler);
            compiler.CompileNodes(ThenBranch);
            compiler.CompileNodes(ElseBranch);

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .Write("bool flag = ")
                .CallNodeMethod(Condition)
                .WriteIndent()
                .WriteLine("if (flag)")
                .WriteIndent()
                .WriteLine("{")
                .IncreaseIndent()
                .CallNodes(ThenBranch)
                .DecreaseIndent()
                .WriteIndent()
                .WriteLine("}");
            if (ElseBranch.Count != 0)
            {
                compiler
                    .WriteIndent()
                    .WriteLine("else")
                    .WriteIndent()
                    .WriteLine("{")
                    .IncreaseIndent()
                    .CallNodes(ElseBranch)
                    .DecreaseIndent()
                    .WriteIndent()
                    .WriteLine("}");
            }

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
            Condition = (PftCondition)PftSerializer.DeserializeNullable(reader);
            PftSerializer.Deserialize(reader, ThenBranch);
            PftSerializer.Deserialize(reader, ElseBranch);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (ReferenceEquals(Condition, null))
            {
                Log.Error
                    (
                        "PftConditionalStatement::Execute: "
                        + "Condition not set"
                    );

                throw new PftSyntaxException();
            }

            if (ThenBranch.Count == 0
                && ElseBranch.Count == 0)
            {
                Log.Warn
                    (
                        "PftConditionalStatement::Execute: "
                        + "Empty Then and Else branches"
                    );
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

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
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

        /// <inheritdoc cref="PftNode.Optimize"/>
        public override PftNode Optimize()
        {
            if (!ReferenceEquals(Condition, null))
            {
                Condition = (PftCondition) Condition.Optimize();
            }
            ThenBranch.Optimize();
            ElseBranch.Optimize();

            if (ThenBranch.Count == 0
                && ElseBranch.Count == 0)
            {
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
            //bool needComment = false;

            printer.EatWhitespace();
            printer.EatNewLine();

            printer
                .WriteLine()
                .WriteIndent()
                .Write("if ");
            if (!ReferenceEquals(Condition, null))
            {
                Condition.PrettyPrint(printer);
            }

            printer
                .WriteLine()
                .WriteIndent()
                .Write("then ");

            bool isComplex = PftUtility.IsComplexExpression(ThenBranch);
            if (isComplex)
            {
                //needComment = true;
                printer.IncreaseLevel();
                printer.EatWhitespace();
                printer.EatNewLine();
                printer.WriteLine();
                printer.WriteIndent();
            }
            printer.WriteNodes(ThenBranch);
            if (isComplex)
            {
                printer.DecreaseLevel();
                printer.WriteLine();
            }

            if (ElseBranch.Count != 0)
            {
                isComplex = PftUtility.IsComplexExpression(ElseBranch);
                printer.EatNewLine();
                if (ThenBranch.Count != 0)
                {
                    printer
                        .WriteLine()
                        .WriteIndent();
                }
                printer.Write("else ");
                if (isComplex)
                {
                    //needComment = true;
                    printer.IncreaseLevel();
                    printer.EatWhitespace();
                    printer.EatNewLine();
                    printer.WriteLine();
                    printer.WriteIndent();
                }
                printer.WriteNodes(ElseBranch);
                if (isComplex)
                {
                    printer.DecreaseLevel();
                    printer.WriteLine();
                }
            }

            printer.EatWhitespace();
            printer.EatNewLine();
            printer.WriteLine();

            printer
                .WriteIndentIfNeeded()
                .Write("fi,");

            //if (needComment
            //    && !ReferenceEquals(Condition, null))
            //{
            //    printer.Write(" /* ");
            //    Condition.PrettyPrint(printer);
            //}

            printer.WriteLine();
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);
            PftSerializer.SerializeNullable(writer, Condition);
            PftSerializer.Serialize(writer, ThenBranch);
            PftSerializer.Serialize(writer, ElseBranch);
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        [DebuggerStepThrough]
        protected internal override bool ShouldSerializeText()
        {
            return false;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("if ");
            if (!ReferenceEquals(Condition, null))
            {
                result.Append(Condition);
            }
            result.Append(" then");
            foreach (PftNode node in ThenBranch)
            {
                result.Append(' ');
                result.Append(node);
            }
            if (ElseBranch.Count != 0)
            {
                result.Append(" else");
                foreach (PftNode node in ElseBranch)
                {
                    result.Append(' ');
                    result.Append(node);
                }
            }
            result.Append(" fi");

            return result.ToString();
        }

        #endregion
    }
}
