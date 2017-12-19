// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftF2.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;

using AM;

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
    /// Форматирование по принципам .NET Framework
    /// </summary>
    /// <example>
    /// <code>
    /// f2(3+0.14,'E4')
    /// </code>
    /// </example>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftFmt
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Format.
        /// </summary>
        [NotNull]
        public PftNodeCollection Format { get; private set; }

        /// <summary>
        /// Number.
        /// </summary>
        [CanBeNull]
        public PftNumeric Number { get; set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(Number, null))
                    {
                        nodes.Add(Number);
                    }
                    nodes.AddRange(Format);
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            [ExcludeFromCodeCoverage]
            protected set
            {
                // Nothing to do here
            }
        }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFmt()
        {
            Format = new PftNodeCollection(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFmt
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Fmt);

            Format = new PftNodeCollection(this);
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        #endregion

        #region Public methods

        /// <summary>
        /// Format the number according specified format.
        /// </summary>
        public static void FormatNumber
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                double number,
                [CanBeNull] string format
            )
        {
            Code.NotNull(context, "context");

            string output;

            if (string.IsNullOrEmpty(format))
            {
                output = number.ToString
                    (
                        CultureInfo.InvariantCulture
                    );
            }
            else
            {
                output = number.ToString
                    (
                        format,
                        CultureInfo.InvariantCulture
                    );
            }

            context.Write(node, output);
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            PftFmt result = (PftFmt) base.Clone();

            result._virtualChildren = null;

            result.Format = Format.CloneNodes(result).ThrowIfNull();

            if (!ReferenceEquals(Number, null))
            {
                result.Number = (PftNumeric) Number.Clone();
            }

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

            PftFmt otherF = (PftFmt)otherNode;
            PftSerializationUtility.CompareLists
                (
                    Format,
                    otherF.Format
                );
            PftSerializationUtility.CompareNodes
                (
                    Number,
                    otherF.Number
                );
        }

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (ReferenceEquals(Number, null)
                || Format.Count == 0)
            {
                throw new PftCompilerException();
            }

            Number.Compile(compiler);
            compiler.CompileNodes(Format);

            string actionName = compiler.CompileAction(Format);

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .Write("double value = ")
                .CallNodeMethod(Number);

            compiler
                .WriteIndent()
                .WriteLine("string format = Evaluate({0});", actionName);

            compiler
                .WriteIndent()
                .WriteLine("string text = value.ToString(format, CultureInfo.InvariantCulture);")
                .WriteIndent()
                .WriteLine("Context.Write(null, text);");

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

            PftSerializer.Deserialize(reader, Format);
            Number = (PftNumeric) PftSerializer.DeserializeNullable(reader);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (!ReferenceEquals(Number, null))
            {
                Number.Execute(context);
                double number = Number.Value;
                string format = context.Evaluate(Format);

                FormatNumber
                    (
                        context,
                        this,
                        number,
                        format
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
                Name = "Fmt"
            };

            if (!ReferenceEquals(Number, null))
            {
                result.Children.Add(Number.GetNodeInfo());
            }

            if (Format.Count != 0)
            {
                PftNodeInfo format = new PftNodeInfo
                {
                    Name = "Format"
                };
                result.Children.Add(format);
                foreach (PftNode node in Format)
                {
                    format.Children.Add(node.GetNodeInfo());
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
            printer
                .SingleSpace()
                .Write("fmt(");
            if (!ReferenceEquals(Number, null))
            {
                Number.PrettyPrint(printer);
            }
            printer.EatWhitespace();
            printer
                .Write(", ")
                .Write(Format)
                .Write(')');
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.Serialize(writer, Format);
            PftSerializer.SerializeNullable(writer, Number);
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
            StringBuilder result = new StringBuilder();
            result.Append("fmt(");
            if (!ReferenceEquals(Number, null))
            {
                result.Append(Number);
            }
            result.Append(',');
            bool first = true;
            foreach (PftNode node in Format)
            {
                if (!first)
                {
                    result.Append(' ');
                }
                result.Append(node);
                first = false;
            }
            result.Append(')');

            return result.ToString();
        }

        #endregion
    }
}
