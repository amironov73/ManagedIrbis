/* PftConditionAndOr.cs --
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
using System.Threading.Tasks;
using AM;
using CodeJam;

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

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (ReferenceEquals(LeftOperand, null))
            {
                throw new PftSyntaxException();
            }

            LeftOperand.Execute(context);
            bool left = LeftOperand.Value;

            if (!ReferenceEquals(RightOperand, null))
            {
                if (string.IsNullOrEmpty(Operation))
                {
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
                    throw new PftSyntaxException();
                }
            }

            Value = left;

            OnAfterExecution(context);
        }

        /// <inheritdoc/>
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
