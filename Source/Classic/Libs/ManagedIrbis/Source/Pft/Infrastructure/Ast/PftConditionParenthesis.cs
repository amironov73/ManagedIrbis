// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftConditionParenthesis.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM;
using AM.Logging;

using JetBrains.Annotations;

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
    public sealed class PftConditionParenthesis
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Inner condition.
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
                        "PftConditionParenthesis::Children: "
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
        public PftConditionParenthesis()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftConditionParenthesis
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
            PftConditionParenthesis result
                = (PftConditionParenthesis)base.Clone();

            result._virtualChildren = null;

            if (!ReferenceEquals(InnerCondition, null))
            {
                result.InnerCondition
                    = (PftCondition)InnerCondition.Clone();
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
                    ((PftConditionParenthesis)otherNode).InnerCondition
                );
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
                        "PftConditionParenthesis::Execute: "
                        + "InnerCondition not set"
                    );

                throw new PftSyntaxException();
            }

            InnerCondition.Execute(context);
            Value = InnerCondition.Value;

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

            if (!ReferenceEquals(InnerCondition, null))
            {
                result.Children.Add(InnerCondition.GetNodeInfo());
            }

            return result;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.Write('(');
            if (!ReferenceEquals(InnerCondition, null))
            {
                InnerCondition.PrettyPrint(printer);
            }
            printer.EatWhitespace();
            printer.Write(')');
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
    }
}
