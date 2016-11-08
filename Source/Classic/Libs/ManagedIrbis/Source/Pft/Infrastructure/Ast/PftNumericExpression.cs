/* PftNumericExpression.cs --
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
    public sealed class PftNumericExpression
        : PftNumeric
    {
        #region Properties

        /// <summary>
        /// Left operand.
        /// </summary>
        [CanBeNull]
        public PftNumeric LeftOperand { get; set; }

        /// <summary>
        /// Operation.
        /// </summary>
        [CanBeNull]
        public string Operation { get; set; }

        /// <summary>
        /// Right operand.
        /// </summary>
        [CanBeNull]
        public PftNumeric RightOperand { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Do the operation.
        /// </summary>
        public double DoOperation
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
            double result;
            switch (operation)
            {
                case "+":
                    result = leftValue + rightValue;
                    break;

                case "-":
                    result = leftValue - rightValue;
                    break;

                case "*":
                    result = leftValue * rightValue;
                    break;

                case "/":
                case "div":
                    result = leftValue / rightValue;
                    break;

                default:
                    throw new PftSyntaxException(this);
            }

            return result;
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

            PftContext clone = context.Push();
            clone.Evaluate(LeftOperand);
            double leftValue = LeftOperand.Value;
            clone.Evaluate(RightOperand);
            context.Pop();
            double rightValue = RightOperand.Value;
            Value = DoOperation
                (
                    context,
                    leftValue,
                    Operation.ThrowIfNull(),
                    rightValue
                );

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
                    Name = "Operation",
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
