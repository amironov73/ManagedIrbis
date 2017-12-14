// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftMode.cs -- переключение режима вывода
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using AM;
using AM.IO;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Переключение режима вывода полей/подполей.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftMode
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Output mode.
        /// </summary>
        public PftFieldOutputMode OutputMode { get; set; }

        /// <summary>
        /// Upper-case mode.
        /// </summary>
        public bool UpperMode { get; set; }

        /// <inheritdoc cref="PftNode.ConstantExpression" />
        public override bool ConstantExpression
        {
            get { return true; }
        }

        /// <inheritdoc cref="PftNode.RequiresConnection" />
        public override bool RequiresConnection
        {
            get { return false; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMode()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMode
            (
                [NotNull] string text
            )
        {
            try
            {
                ParseText(text);
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "PftMode::Constructor",
                        exception
                    );

                throw new PftSyntaxException
                    (
                        "bad mode text",
                        exception
                    );
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMode
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Mpl);

            try
            {
                ParseText
                    (
                        token.Text.ThrowIfNull("token.Text")
                    );
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "PftMode::Constructor",
                        exception
                    );

                throw new PftSyntaxException(token, exception);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse specified text.
        /// </summary>
        public void ParseText
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            text = text.ToLower();
            if (text.Length != 3
                || text[0] != 'm')
            {
                Log.Error
                    (
                        "PftMode::ParseText: "
                        + "text.Length != 3: "
                        + text.ToVisibleString()
                    );

                throw new ArgumentException("mode");
            }

            switch (text[1])
            {
                case 'p':
                    OutputMode = PftFieldOutputMode.PreviewMode;
                    break;

                case 'h':
                    OutputMode = PftFieldOutputMode.HeaderMode;
                    break;

                case 'd':
                    OutputMode = PftFieldOutputMode.DataMode;
                    break;

                default:
                    Log.Error
                        (
                            "PftMode::ParseText: "
                            + "unexpected mode="
                            + text.ToVisibleString()
                        );

                    throw new ArgumentException();
            }
            switch (text[2])
            {
                case 'u':
                    UpperMode = true;
                    break;

                case 'l':
                    UpperMode = false;
                    break;

                default:
                    Log.Error
                        (
                            "PftMode::ParseText: "
                            + "unexpected mode="
                            + text.ToVisibleString()
                        );

                    throw new ArgumentException();
            }
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine
                    (
                        "Context.FieldOutputMode = PftFieldOutputMode.{0};",
                        OutputMode
                    )
                .WriteIndent()
                .WriteLine
                    (
                        "Context.UpperMode = {0};",
                        CompilerUtility.BooleanToText(UpperMode)
                    );

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

            OutputMode = (PftFieldOutputMode) reader.ReadPackedInt32();
            UpperMode = reader.ReadBoolean();
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            context.FieldOutputMode = OutputMode;
            context.UpperMode = UpperMode;

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.Write(ToString());
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.WritePackedInt32((int) OutputMode);
            writer.Write(UpperMode);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder("m",3);

            char c;
            switch (OutputMode)
            {
                case PftFieldOutputMode.PreviewMode:
                    c = 'p';
                    break;

                case PftFieldOutputMode.HeaderMode:
                    c = 'h';
                    break;

                case PftFieldOutputMode.DataMode:
                    c = 'd';
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            result.Append(c);

            result.Append
                (
                    UpperMode ? 'u' : 'l'
                );

            return result.ToString();
        }

        #endregion
    }
}
