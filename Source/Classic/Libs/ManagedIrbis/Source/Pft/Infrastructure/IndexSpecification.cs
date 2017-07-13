// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IndexSpecification.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Index specification (for fields).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public struct IndexSpecification
        : ICloneable
    {
        #region Properties

        /// <summary>
        /// Index kind.
        /// </summary>
        public IndexKind Kind { get; set; }

        /// <summary>
        /// Index specified by literal.
        /// </summary>
        public int Literal { get; set; }

        /// <summary>
        /// Index specified by expression.
        /// </summary>
        [CanBeNull]
        public string Expression { get; set; }

        /// <summary>
        /// Compiled <see cref="Expression" />.
        /// </summary>
        [CanBeNull]
        public PftNumeric Program { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private PftNumeric CompileProgram()
        {
            if (!ReferenceEquals(Program, null))
            {
                return Program;
            }

            string expression = Expression
                .ThrowIfNull("Expression");

            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(expression);
            PftParser parser = new PftParser(tokens);
            Program = parser.ParseArithmetic();

            return Program;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compute value of the index.
        /// </summary>
        public int ComputeValue<T>
            (
                [NotNull] PftContext context,
                [NotNull] T[] array
            )
        {
            int result = 0;

            switch (Kind)
            {
                case IndexKind.None:
                    result = 0;
                    break;

                case IndexKind.Literal:
                    result = Literal - 1;
                    break;

                case IndexKind.LastRepeat:
                    result = array.Length - 1;
                    break;

                case IndexKind.NewRepeat:
                    result = array.Length;
                    break;

                case IndexKind.CurrentRepeat:
                    result = context.Index;
                    break;

                case IndexKind.AllRepeats:
                    result = 0;
                    break;

                case IndexKind.Expression:

                    PftNumeric program = CompileProgram();
                    context.Evaluate(program);
                    result = ((int)program.Value) - 1;

                    break;
            }

            return result;
        }

        /// <summary>
        /// Deserialize the specification.
        /// </summary>
        public void Deserialize
            (
                [NotNull] BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Kind = (IndexKind) reader.ReadPackedInt32();
            Literal = reader.ReadPackedInt32();
            Expression = reader.ReadNullableString();
            Program = null;
        }

        /// <summary>
        /// Get node info for debugger visualization.
        /// </summary>
        [NotNull]
        public PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Name = "Index"
            };
            PftNodeInfo kind = new PftNodeInfo
            {
                Name = "Kind",
                Value = Kind.ToString()
            };
            result.Children.Add(kind);
            PftNodeInfo expression = new PftNodeInfo
            {
                Name = "Expression",
                Value = Expression
            };
            result.Children.Add(expression);

            return result;
        }

        /// <summary>
        /// Serialize the specification.
        /// </summary>
        public void Serialize
            (
                [NotNull] BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WritePackedInt32((int) Kind)
                .WritePackedInt32(Literal)
                .WriteNullable(Expression);
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public object Clone()
        {
            IndexSpecification result
                = (IndexSpecification) MemberwiseClone();

#if CLASSIC || NETCORE

            if (!ReferenceEquals(Program, null))
            {
                result.Program = (PftNumeric) Program.Clone();
            }
#endif

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "{0}: {1}",
                    Kind,
                    Expression
                );
        }

        #endregion
    }
}
