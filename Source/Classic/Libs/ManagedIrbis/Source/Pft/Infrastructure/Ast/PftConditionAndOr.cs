// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftConditionAndOr.cs --
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
using AM.IO;
using AM.Logging;

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
    public sealed class PftConditionAndOr
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Left operand.
        /// </summary>
        [CanBeNull]
        public PftCondition LeftOperand { get; set; }

        /// <summary>
        /// Operation.
        /// </summary>
        [CanBeNull]
        public string Operation { get; set; }

        /// <summary>
        /// Right operand.
        /// </summary>
        [CanBeNull]
        public PftCondition RightOperand { get; set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(LeftOperand, null))
                    {
                        nodes.Add(LeftOperand);
                    }
                    if (!ReferenceEquals(RightOperand, null))
                    {
                        nodes.Add(RightOperand);
                    }
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            protected set
            {
                // Nothing to do here

                Log.Error
                    (
                        "PftConditionAndOr::Children: "
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
        public PftConditionAndOr()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftConditionAndOr
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
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
            PftConditionAndOr result = (PftConditionAndOr) base.Clone();

            result._virtualChildren = null;

            if (!ReferenceEquals(LeftOperand, null))
            {
                result.LeftOperand = (PftCondition) LeftOperand.Clone();
            }

            if (!ReferenceEquals(RightOperand, null))
            {
                result.RightOperand = (PftCondition) RightOperand.Clone();
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

            PftConditionAndOr otherCondition
                = (PftConditionAndOr) otherNode;
            PftSerializationUtility.CompareNodes
                (
                    LeftOperand,
                    otherCondition.LeftOperand
                );
            if (Operation != otherCondition.Operation)
            {
                throw new IrbisException();
            }
            PftSerializationUtility.CompareNodes
                (
                    RightOperand,
                    otherCondition.RightOperand
                );
        }

        /// <inheritdoc cref="PftNode.Compile"/>
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (ReferenceEquals(LeftOperand, null)
                || ReferenceEquals(RightOperand, null)
                || string.IsNullOrEmpty(Operation))
            {
                throw new PftCompilerException();
            }

            LeftOperand.Compile(compiler);
            RightOperand.Compile(compiler);

            compiler.StartMethod(this);

            compiler.Output.Write("\tbool result = ");
            compiler.RefNodeMethod(LeftOperand);
            compiler.Output.Write("() ");
            if (Operation.SameString("and"))
            {
                compiler.Output.Write("&&");
            }
            else if (Operation.SameString("or"))
            {
                compiler.Output.Write("||");
            }
            else
            {
                throw new PftCompilerException();
            }
            compiler.Output.Write(' ');
            compiler.RefNodeMethod(RightOperand);
            compiler.Output.WriteLine("();");
            compiler.Output.WriteLine("\treturn result;");

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

            LeftOperand 
                = (PftCondition) PftSerializer.DeserializeNullable(reader);
            Operation = reader.ReadNullableString();
            RightOperand
                = (PftCondition) PftSerializer.DeserializeNullable(reader);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (ReferenceEquals(LeftOperand, null))
            {
                Log.Error
                    (
                        "PftConditionAndOr::Execute: "
                        + "LeftOperand not set"
                    );

                throw new PftSyntaxException();
            }

            LeftOperand.Execute(context);
            bool left = LeftOperand.Value;

            if (!ReferenceEquals(RightOperand, null))
            {
                if (string.IsNullOrEmpty(Operation))
                {
                    Log.Error
                        (
                            "PftConditionAndOr::Execute: "
                            + "Operation not set"
                        );

                    throw new PftSyntaxException();
                }

                RightOperand.Execute(context);
                bool right = RightOperand.Value;

                if (Operation.SameString("and"))
                {
                    left = left && right;
                }
                else if (Operation.SameString("or"))
                {
                    left = left || right;
                }
                else
                {
                    Log.Error
                        (
                            "PftConditionAndOr::Execute: "
                            + "unexpected operation="
                            + Operation.NullableToVisibleString()
                        );

                    throw new PftSyntaxException();
                }
            }

            Value = left;

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = "ConditionAndOr"
            };

            if (!ReferenceEquals(LeftOperand, null))
            {
                PftNodeInfo left = new PftNodeInfo
                {
                    Node = LeftOperand,
                    Name = "LeftOperand"
                };
                result.Children.Add(left);
                left.Children.Add(LeftOperand.GetNodeInfo());
            }

            PftNodeInfo operation = new PftNodeInfo
            {
                Name = "Operation",
                Value = Operation
            };
            result.Children.Add(operation);

            if (!ReferenceEquals(RightOperand, null))
            {
                PftNodeInfo right = new PftNodeInfo
                {
                    Node = RightOperand,
                    Name = "RightOperand"
                };
                result.Children.Add(right);
                right.Children.Add(RightOperand.GetNodeInfo());
            }

            return result;
        }

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode Optimize()
        {
            if (!ReferenceEquals(LeftOperand, null))
            {
                LeftOperand = (PftCondition) LeftOperand.Optimize();
            }
            if (!ReferenceEquals(RightOperand, null))
            {
                RightOperand = (PftCondition) RightOperand.Optimize();
            }

            return this;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            if (!ReferenceEquals(LeftOperand, null))
            {
                LeftOperand.PrettyPrint(printer);
            }

            printer
                .SingleSpace()
                .Write(Operation)
                .SingleSpace();

            if (!ReferenceEquals(RightOperand, null))
            {
                RightOperand.PrettyPrint(printer);
            }
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.SerializeNullable(writer, LeftOperand);
            writer.WriteNullable(Operation);
            PftSerializer.SerializeNullable(writer, RightOperand);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            if (!ReferenceEquals(LeftOperand, null))
            {
                result.Append(LeftOperand);
            }
            result.Append(' ');
            result.Append(Operation);
            result.Append(' ');
            if (!ReferenceEquals(RightOperand, null))
            {
                result.Append(RightOperand);
            }

            return result.ToString();
        }

        #endregion
    }
}
