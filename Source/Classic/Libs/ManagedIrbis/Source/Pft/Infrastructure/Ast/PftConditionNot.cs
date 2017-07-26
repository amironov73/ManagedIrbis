// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftConditionNot.cs --
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

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
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
    public sealed class PftConditionNot
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Inner condition
        /// </summary>
        [CanBeNull]
        public PftCondition InnerCondition { get; set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(InnerCondition, null))
                    {
                        nodes.Add(InnerCondition);
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
                        "PftConditionNot::Children: "
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
        public PftConditionNot()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftConditionNot
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
            PftConditionNot result = (PftConditionNot) base.Clone();

            result._virtualChildren = null;

            if (!ReferenceEquals(InnerCondition, null))
            {
                result.InnerCondition 
                    = (PftCondition) InnerCondition.Clone();
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

            PftSerializationUtility.CompareNodes
                (
                    InnerCondition,
                    ((PftConditionNot)otherNode).InnerCondition
                );
        }

        /// <inheritdoc cref="PftNode.Compile"/>
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (ReferenceEquals(InnerCondition, null))
            {
                throw new PftCompilerException();
            }

            InnerCondition.Compile(compiler);

            compiler.StartMethod(this);

            compiler.Output.Write("\tbool result = ");
            compiler.CallNodeMethod(InnerCondition);
            compiler.Output.WriteLine(";");
            compiler.Output.WriteLine("\treturn !result;");

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

            InnerCondition
                = (PftCondition) PftSerializer.DeserializeNullable(reader);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (ReferenceEquals(InnerCondition, null))
            {
                Log.Error
                    (
                        "PftConditionNot::Execute: "
                        + "InnerCondition not set"
                    );

                throw new PftSyntaxException();
            }

            InnerCondition.Execute(context);
            Value = !InnerCondition.Value;

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode Optimize()
        {
            if (!ReferenceEquals(InnerCondition, null))
            {
                InnerCondition = (PftCondition) InnerCondition.Optimize();
            }

            return this;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .SingleSpace()
                .Write("not")
                .SingleSpace();
            if (!ReferenceEquals(InnerCondition, null))
            {
                InnerCondition.PrettyPrint(printer);
            }
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.SerializeNullable(writer, InnerCondition);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(" not ");
            if (!ReferenceEquals(InnerCondition, null))
            {
                result.Append(InnerCondition);
            }

            return result.ToString();
        }

        #endregion
    }
}
