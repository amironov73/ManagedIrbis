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

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;

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
        public NonNullCollection<PftNode> Arguments { get; private set; }

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
                        + value.NullableToVisibleString()
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
            Arguments = new NonNullCollection<PftNode>();
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
            Arguments = new NonNullCollection<PftNode>();
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
            Arguments = new NonNullCollection<PftNode>();
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
            PftFunctionCall result = (PftFunctionCall) base.Clone();

            result._virtualChildren = null;

            result.Arguments = Arguments.CloneNodes().ThrowIfNull();

            return result;
        }

        #endregion

        #region PftNode members

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
    }
}
