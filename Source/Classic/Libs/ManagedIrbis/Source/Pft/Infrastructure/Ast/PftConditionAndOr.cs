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

using AM;
using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;

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

        #endregion
    }
}
