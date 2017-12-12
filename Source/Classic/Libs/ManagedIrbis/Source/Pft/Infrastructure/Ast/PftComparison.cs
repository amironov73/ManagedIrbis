// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftComparison.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

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

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get
            {
                switch (Operation)
                {
                    case "::":
                    case "~":
                    case "~~":
                    case "!~":
                    case "!~~":
                    case "==":
                    case "!==":

                        return true;
                }

                return base.ExtendedSyntax;
            }
        }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    PftNode operationNode = new PftNode
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
            [ExcludeFromCodeCoverage]
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
        public static bool DoNumericOperation
            (
                [NotNull] PftContext context,
                double leftValue,
                [NotNull] string operation,
                double rightValue
            )
        {
            Code.NotNull(context, "context");
            Code.NotNullNorEmpty(operation, "operation");

#if PocketPC || WINMOBILE

            operation = operation.ToLower();

#else

            operation = operation.ToLowerInvariant();

#endif

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
                    // Original PFT behavior: exact number comparison
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    result = leftValue == rightValue; //-V3024
                    break;

                case "!=":
                case "<>":
                    // Original PFT behavior: exact number comparison
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    result = leftValue != rightValue; //-V3024
                    break;

                case ">":
                    result = leftValue > rightValue;
                    break;

                case ">=":
                    result = leftValue >= rightValue;
                    break;

                default:
                    Log.Error
                        (
                            "PftComparison::DoNumericOperation: "
                            + "unexpected operation: "
                            + operation
                        );

                    throw new PftSyntaxException();
            }

            Log.Trace
                (
                    "PftComparison::DoNumericOperation: left="
                    + leftValue
                    + ", operation="
                    + operation
                    + ", right="
                    + rightValue
                    + ", result="
                    + result
                );

            return result;
        }

        /// <summary>
        /// Do the operation.
        /// </summary>
        public static bool DoStringOperation
            (
                [NotNull] PftContext context,
                [NotNull] string leftValue,
                [NotNull] string operation,
                [NotNull] string rightValue
            )
        {
            Code.NotNull(context, "context");
            Code.NotNullNorEmpty(operation, "operation");

#if PocketPC || WINMOBILE

            operation = operation.ToLower();

#else

            operation = operation.ToLowerInvariant();

#endif

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
                    Log.Error
                        (
                            "PftComparison::DoStringOperation: "
                            + "unexpected operation: "
                            + operation
                        );

                    throw new PftSyntaxException();
            }

            Log.Trace
                (
                    "PftComparison::DoStringOperation: left="
                    + leftValue
                    + ", operation="
                    + operation
                    + ", right="
                    + rightValue
                    + ", result="
                    + result
                );

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
                NumericUtility.TryParseDouble(stringValue, out result);

                return result;
            }

            return numeric.Value;
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            PftComparison result = (PftComparison)base.Clone();

            result._virtualChildren = null;

            if (!ReferenceEquals(LeftOperand, null))
            {
                result.LeftOperand = (PftNode) LeftOperand.Clone();
            }

            if (!ReferenceEquals(RightOperand, null))
            {
                result.RightOperand = (PftNode) RightOperand.Clone();
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

            PftComparison otherComparison = (PftComparison) otherNode;
            PftSerializationUtility.CompareNodes
                (
                    LeftOperand,
                    otherComparison.LeftOperand
                );
            if (Operation != otherComparison.Operation)
            {
                throw new IrbisException();
            }
            PftSerializationUtility.CompareNodes
                (
                    RightOperand,
                    otherComparison.RightOperand
                );
        }

        /// <inheritdoc cref="PftNode.Compile" />
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

            PftContext context = new PftContext(null);
            if (PftUtility.IsNumeric(context, LeftOperand))
            {
                compiler
                    .WriteIndent()
                    .Write("double left = ")
                    .CallNodeMethod(LeftOperand);
                compiler
                    .WriteIndent()
                    .Write("double right = ")
                    .CallNodeMethod(RightOperand);
                compiler
                    .WriteIndent()
                    .WriteLine("string operation = \"{0}\";", Operation);

                // TODO implement properly
                compiler
                    .WriteIndent()
                    .WriteLine
                        (
                            "bool result = PftComparison" +
                            ".DoNumericOperation(Context, left, operation, right);"
                        );
            }
            else
            {
                compiler
                    .WriteIndent()
                    .Write("string left = Evaluate(")
                    .RefNodeMethod(LeftOperand)
                    .WriteLine(");");
                compiler
                    .WriteIndent()
                    .Write("string right = Evaluate(")
                    .RefNodeMethod(RightOperand)
                    .WriteLine(");");
                compiler
                    .WriteIndent()
                    .WriteLine("string operation = \"{0}\";", Operation);

                // TODO implement properly
                compiler
                    .WriteIndent()
                    .WriteLine
                        (
                            "bool result = PftComparison" +
                            ".DoStringOperation(Context, left, operation, right);"
                        );
            }

            compiler
                .WriteIndent()
                .WriteLine("return result;");

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

            LeftOperand = PftSerializer.DeserializeNullable(reader);
            Operation = reader.ReadNullableString();
            RightOperand = PftSerializer.DeserializeNullable(reader);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            string operation = Operation.ThrowIfNull();

            if (ReferenceEquals(LeftOperand, null))
            {
                Log.Error
                    (
                        "PftComparison::Execute: "
                        + "LeftOperand not set"
                    );

                throw new PftSyntaxException(this);
            }
            if (ReferenceEquals(RightOperand, null))
            {
                Log.Error
                    (
                        "PftComparison::Execute: "
                        + "RightOperand not set"
                    );

                throw new PftSyntaxException(this);
            }

            bool leftNumeric = PftUtility.IsNumeric
                (
                    context,
                    LeftOperand
                );
            bool rightNumeric = PftUtility.IsNumeric
                (
                    context,
                    RightOperand
                );

            if (leftNumeric || rightNumeric)
            {
                double leftValue = GetValue(context, LeftOperand);
                double rightValue = GetValue
                    (
                        context, 
                        RightOperand.ThrowIfNull("RightOperand")
                    );
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

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = SimplifyTypeName(GetType().Name)
            };

            if (!ReferenceEquals(LeftOperand, null))
            {
                PftNodeInfo leftNode = new PftNodeInfo
                {
                    Node = LeftOperand,
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
                    Node = RightOperand,
                    Name = "Right"
                };
                result.Children.Add(rightNode);
                rightNode.Children.Add(RightOperand.GetNodeInfo());
            }

            return result;
        }

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode Optimize()
        {
            if (ReferenceEquals(LeftOperand, null)
                || ReferenceEquals(RightOperand, null)
                || string.IsNullOrEmpty(Operation))
            {
                throw new PftCompilerException();
            }

            LeftOperand = LeftOperand.Optimize();
            RightOperand = RightOperand.Optimize();

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

            printer.Write(Operation);

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

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            if (!ReferenceEquals(LeftOperand, null))
            {
                result.Append(LeftOperand);
            }

            result.Append(Operation);

            if (!ReferenceEquals(RightOperand, null))
            {
                result.Append(RightOperand);
            }

            return result.ToString();
        }

        #endregion
    }
}
