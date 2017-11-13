// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftFunctionCall.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.IO;
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
    public sealed class PftFunctionCall
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Function name.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Array of arguments.
        /// </summary>
        [NotNull]
        public PftNodeCollection Arguments { get; private set; }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
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
                    nodes.AddRange(Arguments);
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            protected set
            {
                // Nothing to do here

                Log.Error
                    (
                        "PftFunctionCall::Children: "
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
        public PftFunctionCall()
        {
            Arguments = new PftNodeCollection(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFunctionCall
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
            Arguments = new PftNodeCollection(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFunctionCall
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Identifier);

            Name = token.Text;
            Arguments = new PftNodeCollection(this);
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="PftNode.Clone" />
        public override object Clone()
        {
            PftFunctionCall result = (PftFunctionCall) base.Clone();

            result._virtualChildren = null;

            result.Arguments = Arguments.CloneNodes(result)
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

            PftFunctionCall otherCall = (PftFunctionCall) otherNode;
            if (Name != otherCall.Name)
            {
                throw new PftSerializationException();
            }
            PftSerializationUtility.CompareLists
                (
                    Arguments,
                    otherCall.Arguments
                );
        }

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new PftCompilerException();
            }

            compiler.CompileNodes(Arguments);

            string actionName = compiler.CompileAction(Arguments);

            compiler.StartMethod(this);

            // TODO implement properly

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

            Name = reader.ReadNullableString();
            PftSerializer.Deserialize(reader, Arguments);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            string name = Name;
            if (string.IsNullOrEmpty(name))
            {
                Log.Error
                    (
                        "PftFunctionCall::Execute: "
                        + "name not specified"
                    );

                throw new PftSyntaxException(this);
            }

            PftNode[] arguments = Arguments.ToArray();

            FunctionDescriptor descriptor = context.Functions
                .FindFunction(name);
            if (!ReferenceEquals(descriptor, null))
            {
                descriptor.Function
                    (
                        context,
                        this,
                        arguments
                    );
            }
            else
            {
                PftProcedure procedure = context.Procedures
                    .FindProcedure(name);

                if (!ReferenceEquals(procedure, null))
                {
                    string expression 
                        = context.GetStringArgument(arguments, 0);
                    procedure.Execute
                        (
                            context,
                            expression
                        );
                }
                else
                {
                    PftFunctionManager.ExecuteFunction
                        (
                            name,
                            context,
                            this,
                            arguments
                        );
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

            PftNodeInfo name = new PftNodeInfo
            {
                Name = "Name",
                Value = Name
            };
            result.Children.Add(name);

            PftNodeInfo arguments = new PftNodeInfo
            {
                Name = "Arguments"
            };
            arguments.Children.AddRange
                (
                    Arguments.Select(node => node.GetNodeInfo())
                );
            result.Children.Add(arguments);

            return result;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.EatWhitespace();
            printer
                .SingleSpace()
                .Write(Name)
                .Write('(')
                .WriteNodes(Arguments)
                .Write(')');
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.WriteNullable(Name);
            PftSerializer.Serialize(writer, Arguments);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="PftNode.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(Name);
            result.Append('(');
            PftUtility.NodesToText(result, Arguments);
            result.Append(')');

            return result.ToString();
        }

        #endregion
    }
}
