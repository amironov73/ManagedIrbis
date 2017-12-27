// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftForEach.cs --
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
using System.Linq;
using System.Text;

using AM;
using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// foreach $x in (v692^g,/)
    /// do
    ///     $x, #
    ///     if $x:'2010' then break fi
    /// end
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftForEach
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
        public PftNodeCollection Sequence { get; private set; }

        /// <summary>
        /// Body.
        /// </summary>
        [NotNull]
        public PftNodeCollection Body { get; private set; }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

        /// <inheritdoc cref="PftNode.ComplexExpression"/>
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
                    nodes.AddRange(Sequence);
                    nodes.AddRange(Body);
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
                        "PftForEach::Children: "
                        + "set value="
                        + value.ToVisibleString()
                    );
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftForEach()
        {
            Sequence = new PftNodeCollection(this);
            Body = new PftNodeCollection(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftForEach
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.ForEach);

            Sequence = new PftNodeCollection(this);
            Body = new PftNodeCollection(this);
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

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            PftForEach result = (PftForEach)base.Clone();

            result._virtualChildren = null;

            result.Sequence = Sequence.CloneNodes(result)
                .ThrowIfNull();
            result.Body = Body.CloneNodes(result).ThrowIfNull();

            if (!ReferenceEquals(Variable, null))
            {
                result.Variable = (PftVariableReference)Variable.Clone();
            }

            return result;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.CompareNode"/>
        internal override void CompareNode
            (
                PftNode otherNode
            )
        {
            base.CompareNode(otherNode);

            PftForEach otherForEach = (PftForEach) otherNode;
            PftSerializationUtility.CompareNodes
                (
                    Variable,
                    otherForEach.Variable
                );
            PftSerializationUtility.CompareLists
                (
                    Sequence,
                    otherForEach.Sequence
                );
            PftSerializationUtility.CompareLists
                (
                    Body,
                    otherForEach.Body
                );
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
            PftSerializer.Deserialize(reader, Sequence);
            PftSerializer.Deserialize(reader, Body);
        }

        /// <inheritdoc cref="PftNode.Execute" />
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
            catch (PftBreakException exception)
            {
                // It was break operator

                Log.TraceException
                    (
                        "PftForEach::Execute",
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
                Name = "ForEach"
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
                .Write("foreach ");

            if (!ReferenceEquals(Variable, null))
            {
                Variable.PrettyPrint(printer);
            }
            printer.Write(" in ");

            bool first = true;
            foreach (PftNode node in Sequence)
            {
                if (!first)
                {
                    printer.Write(", ");
                }
                node.PrettyPrint(printer);
                first = false;
            }

            printer
                .WriteIndent()
                .WriteLine("do");

            printer.IncreaseLevel();
            printer.WriteNodes(Body);
            printer.DecreaseLevel();
            printer.EatWhitespace();
            printer.EatNewLine();
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
            PftSerializer.Serialize(writer, Sequence);
            PftSerializer.Serialize(writer, Body);
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
            result.Append("foreach ");
            result.Append(Variable);
            result.Append(" in ");
            PftUtility.NodesToText(result, Sequence);
            result.Append(" do ");
            PftUtility.NodesToText(result, Body);
            result.Append(" end");

            return StringBuilderCache.GetStringAndRelease(result);
        }

        #endregion
    }
}
