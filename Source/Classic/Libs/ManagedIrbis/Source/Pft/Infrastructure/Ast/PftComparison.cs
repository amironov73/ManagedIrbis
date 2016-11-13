/* PftComparison.cs --
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
using System.Text.RegularExpressions;
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
    public sealed class PftComparison
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Left operand.
        /// </summary>
        [CanBeNull]
        public PftNode LeftOperand { get; set; }

        /// <summary>
        /// Operation.
        /// </summary>
        [CanBeNull]
        public string Operation { get; set; }

        /// <summary>
        /// Right operand.
        /// </summary>
        [CanBeNull]
        public PftNode RightOperand { get; set; }

        /// <inheritdoc />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    PftNode operationNode = new PftNode()
                    {
                        Text = Operation
                    };
                    List<PftNode> nodes = new List<PftNode>
                    {
                        LeftOperand,
                        operationNode,
                        RightOperand
                    };
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            protected set
            {
                // Nothing to do here
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftComparison()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftComparison
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

        /// <summary>
        /// Do the operation.
        /// </summary>
        public bool DoNumericOperation
            (
                [NotNull] PftContext context,
                double leftValue,
                [NotNull] string operation,
                double rightValue
            )
        {
            Code.NotNull(context, "context");
            Code.NotNullNorEmpty(operation, "operation");

            operation = operation.ToLowerInvariant();
            bool result;
            switch (operation)
            {
                case "<":
                    result = leftValue < rightValue;
                    break;

                case "<=":
                    result = leftValue <= rightValue;
                    break;

                case "=":
                case "==":
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    result = leftValue == rightValue;
                    break;

                case "!=":
                case "<>":
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    result = leftValue != rightValue;
                    break;

                case ">":
                    result = leftValue > rightValue;
                    break;

                case ">=":
                    result = leftValue >= rightValue;
                    break;

                default:
                    throw new PftSyntaxException(this);
            }

            return result;
        }

        /// <summary>
        /// Do the operation.
        /// </summary>
        public bool DoStringOperation
            (
                [NotNull] PftContext context,
                [NotNull] string leftValue,
                [NotNull] string operation,
                [NotNull] string rightValue
            )
        {
            Code.NotNull(context, "context");
            Code.NotNullNorEmpty(operation, "operation");

            operation = operation.ToLowerInvariant();
            bool result;
            switch (operation)
            {
                case ":":
                    result = PftUtility.ContainsSubString
                        (
                            leftValue,
                            rightValue
                        );
                    break;

                case "::":
                    result = PftUtility.ContainsSubStringSensitive
                        (
                            leftValue,
                            rightValue
                        );
                    break;

                case "=":
                    result = leftValue.SameString(rightValue);
                    break;

                case "==":
                    result = leftValue.SameStringSensitive
                        (
                            rightValue
                        );
                    break;

                case "!=":
                case "<>":
                    result = !leftValue.SameString
                        (
                            rightValue
                        );
                    break;

                case "!==":
                    result = !leftValue.SameStringSensitive
                        (
                            rightValue
                        );
                    break;

                case "<":
                    result = PftUtility.CompareStrings
                        (
                            leftValue,
                            rightValue
                        )
                        < 0;
                    break;

                case "<=":
                    result = PftUtility.CompareStrings
                        (
                            leftValue,
                            rightValue
                        )
                        <= 0;
                    break;

                case ">":
                    result = PftUtility.CompareStrings
                        (
                            leftValue,
                            rightValue
                        )
                        > 0;
                    break;

                case ">=":
                    result = PftUtility.CompareStrings
                        (
                            leftValue,
                            rightValue
                        )
                        >= 0;
                    break;

                case "~":
                    result = Regex.IsMatch
                        (
                            leftValue,
                            rightValue,
                            RegexOptions.IgnoreCase
                        );
                    break;

                case "~~":
                    result = Regex.IsMatch
                        (
                            leftValue,
                            rightValue
                        );
                    break;

                case "!~":
                    result = !Regex.IsMatch
                        (
                            leftValue,
                            rightValue,
                            RegexOptions.IgnoreCase
                        );
                    break;

                case "!~~":
                    result = !Regex.IsMatch
                        (
                            leftValue,
                            rightValue
                        );
                    break;

                default:
                    throw new PftSyntaxException(this);
            }

            return result;
        }

        private double GetValue
            (
                [NotNull] PftContext context,
                [NotNull] PftNode node
            )
        {
            string stringValue = context.Evaluate(node);

            PftNumeric numeric = node as PftNumeric;
            if (ReferenceEquals(numeric, null))
            {
                double result;
                double.TryParse(stringValue, out result);

                return result;
            }

            return numeric.Value;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            string operation = Operation.ThrowIfNull();

            if (ReferenceEquals(LeftOperand, null))
            {
                throw new PftSyntaxException(this);
            }
            if (string.IsNullOrEmpty(Operation))
            {
                throw new PftSyntaxException(this);
            }
            if (ReferenceEquals(RightOperand, null))
            {
                throw new PftSyntaxException(this);
            }

            if (LeftOperand is PftNumeric
                || RightOperand is PftNumeric)
            {
                double leftValue = GetValue(context, LeftOperand);
                double rightValue = GetValue(context, RightOperand);
                Value = DoNumericOperation
                    (
                        context,
                        leftValue,
                        operation,
                        rightValue
                    );
            }
            else
            {
                string leftValue = context.Evaluate(LeftOperand);
                string rightValue = context.Evaluate(RightOperand);
                Value = DoStringOperation
                    (
                        context,
                        leftValue,
                        operation,
                        rightValue
                    );
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc/>
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Name = SimplifyTypeName(GetType().Name)
            };

            if (!ReferenceEquals(LeftOperand, null))
            {
                PftNodeInfo leftNode = new PftNodeInfo
                {
                    Name = "Left"
                };
                result.Children.Add(leftNode);
                leftNode.Children.Add(LeftOperand.GetNodeInfo());
            }

            if (!string.IsNullOrEmpty(Operation))
            {
                PftNodeInfo operationNode = new PftNodeInfo
                {
                    Name="Operation",
                    Value = Operation
                };
                result.Children.Add(operationNode);
            }

            if (!ReferenceEquals(RightOperand, null))
            {
                PftNodeInfo rightNode = new PftNodeInfo
                {
                    Name = "Right"
                };
                result.Children.Add(rightNode);
                rightNode.Children.Add(RightOperand.GetNodeInfo());
            }

            return result;
        }

        /// <inheritdoc/>
        public override void PrintDebug
            (
                TextWriter writer,
                int level
            )
        {
            for (int i = 0; i < level; i++)
            {
                writer.Write("| ");
            }
            writer.WriteLine("Left:");
            if (!ReferenceEquals(LeftOperand, null))
            {
                LeftOperand.PrintDebug(writer, level + 1);
            }

            for (int i = 0; i < level; i++)
            {
                writer.Write("| ");
            }
            writer.WriteLine("Operation:");
            for (int i = 0; i <= level; i++)
            {
                writer.Write("| ");
            }
            writer.WriteLine(Operation);

            for (int i = 0; i < level; i++)
            {
                writer.Write("| ");
            }
            writer.WriteLine("Right:");
            if (!ReferenceEquals(RightOperand, null))
            {
                RightOperand.PrintDebug(writer, level + 1);
            }
        }

        #endregion
    }
}
