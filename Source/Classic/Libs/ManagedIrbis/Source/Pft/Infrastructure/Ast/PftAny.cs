// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftAny.cs --
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
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftAny
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Condition
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
                    if (!ReferenceEquals(InnerCondition, null))
                    {
                        List<PftNode> nodes = new List<PftNode>
                        {
                            InnerCondition
                        };
                        _virtualChildren.SetChildren(nodes);
                    }
                }

                return _virtualChildren;
            }
            [ExcludeFromCodeCoverage]
            protected set
            {
                // Nothing to do here

                Log.Error
                    (
                        "PftAny::Children: "
                        + "set value="
                        + value.ToVisibleString()
                    );
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
        public PftAny()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftAny
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Any);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftAny
            (
                [NotNull] PftCondition condition
            )
        {
            Code.NotNull(condition, "condition");

            InnerCondition = condition;
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
            PftAny result = (PftAny)base.Clone();

            if (!ReferenceEquals(InnerCondition, null))
            {
                result.InnerCondition = (PftCondition)InnerCondition.Clone();
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
                    ((PftAny)otherNode).InnerCondition
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
            if (context.CurrentGroup != null)
            {
                Log.Error
                    (
                        "PftAny::Execute: "
                        + "nested group detected"
                    );

                throw new PftSemanticException("Nested group");
            }

            PftCondition condition = InnerCondition
                .ThrowIfNull("Condition");

            PftGroup group = new PftGroup();

            try
            {
                context.CurrentGroup = group;
                context.VMonitor = false;

                OnBeforeExecution(context);

                bool value = false;

                for (
                        context.Index = 0;
                        context.Index < PftConfig.MaxRepeat;
                        context.Index++
                    )
                {
                    context.VMonitor = false;

                    condition.Execute(context);

                    if (!context.VMonitor || context.BreakFlag)
                    {
                        break;
                    }

                    value = condition.Value;
                    if (value)
                    {
                        break;
                    }
                }

                Value = value;

                OnAfterExecution(context);
            }
            finally
            {
                context.CurrentGroup = null;
            }
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
            printer
                .SingleSpace()
                .Write("any(");
            if (!ReferenceEquals(InnerCondition, null))
            {
                InnerCondition.PrettyPrint(printer);
            }
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
            result.Append("any(");
            PftUtility.NodesToText(result, Children);
            result.Append(')');

            return StringBuilderCache.GetStringAndRelease(result);
        }

        #endregion
    }
}
