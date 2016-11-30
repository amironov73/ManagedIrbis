// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IndexSpecification.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

#if CLASSIC || NETCORE

using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;

#endif

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

#if CLASSIC || NETCORE

        /// <summary>
        /// Compiled <see cref="Expression"/>.
        /// </summary>
        [CanBeNull]
        public PftNumeric Program { get; set; }

#endif

        #endregion

        #region Construction

        #endregion

        #region Private members

#if CLASSIC || NETCORE

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

#endif

        #endregion

        #region Public methods

#if CLASSIC || NETCORE

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

#endif

        #endregion

        #region Object members

        /// <inheritdoc/>
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
