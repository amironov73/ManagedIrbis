// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftRsum.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.IO;
using AM.Logging;
using AM.Text;

using CodeJam;

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
    public sealed class PftRsum
        : PftNumeric
    {
        #region Properties

        /// <summary>
        /// Name of the function.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRsum()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRsum
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRsum
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");

            Name = token.Text;
            if (string.IsNullOrEmpty(Name))
            {
                Log.Error
                    (
                        "PftRsum::Constructor: "
                        + "Name="
                        + Name.ToVisibleString()
                    );

                throw new PftSyntaxException("Name");
            }
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

            PftRsum otherRsum = (PftRsum)otherNode;
            bool result = Name == otherRsum.Name;
            if (!result)
            {
                throw new PftSerializationException();
            }
        }

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            compiler.CompileNodes(Children);

            string actionName = compiler.CompileAction(Children);

            compiler.StartMethod(this);

            string functionName;
            switch (Name)
            {
                case "rsum":
                    functionName = "Sum";
                    break;

                case "rmin":
                    functionName = "Min";
                    break;

                case "rmax":
                    functionName = "Max";
                    break;

                case "ravr":
                    functionName = "Average";
                    break;

                default:
                    throw new PftCompilerException();
            }

            compiler
                .WriteIndent()
                .WriteLine("double result = 0.0;");

            if (!string.IsNullOrEmpty(actionName))
            {
                compiler
                    .WriteIndent()
                    .Write("string text = Evaluate({0});", actionName);

                compiler
                    .WriteIndent()
                    .WriteLine("double[] values = PftUtility.ExtractNumericValues(text);")
                    .WriteIndent()
                    .WriteLine("if (values.Length != 0)")
                    .WriteIndent()
                    .WriteLine("{")
                    .IncreaseIndent()
                    .WriteIndent()
                    .WriteLine("result = values.{0}();", functionName)
                    .DecreaseIndent()
                    .WriteIndent()
                    .WriteLine("}");
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

            Name = reader.ReadNullableString();
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            Value = 0.0;

            if (!string.IsNullOrEmpty(Name))
            {
                string text = context.Evaluate(Children);
                double[] values = PftUtility.ExtractNumericValues(text);
                if (values.Length != 0)
                {
                    switch (Name)
                    {
                        case "rsum":
                            Value = values.Sum();
                            break;

                        case "rmin":
                            Value = values.Min();
                            break;

                        case "rmax":
                            Value = values.Max();
                            break;

                        case "ravr":
                            Value = values.Average();
                            break;

                        default:
                            Log.Error
                                (
                                    "PftRsum::Execute: "
                                    + "unexpected function name="
                                    + Name.ToVisibleString()
                                );

                            throw new PftSyntaxException(this);
                    }
                }
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.EatWhitespace();
            printer
                .SingleSpace()
                .Write(Name)
                .Write('(')
                .WriteNodes(Children)
                .Write(')');
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.WriteNullable(Name);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = StringBuilderCache.Acquire();
            result.Append(Name);
            result.Append('(');
            PftUtility.NodesToText(result, Children);
            result.Append(')');

            return StringBuilderCache.GetStringAndRelease(result);
        }

        #endregion
    }
}
