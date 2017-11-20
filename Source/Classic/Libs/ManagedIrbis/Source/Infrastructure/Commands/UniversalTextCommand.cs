// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniversalTextCommand.cs -- universal command with text lines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using AM;
using AM.Collections;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// Universal command with text lines.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class UniversalTextCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Command code.
        /// </summary>
        [NotNull]
        public string CommandCode
        {
            get { return _commandCode; }
        }

        /// <summary>
        /// Lines.
        /// </summary>
        [NotNull]
        public NonNullCollection<TextWithEncoding> TextLines
        {
            get { return _textLines; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public UniversalTextCommand
            (
                [NotNull] IIrbisConnection connection,
                [NotNull] string commandCode
            )
            : base(connection)
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(commandCode, "commandCode");

            _commandCode = commandCode;
            _textLines = new NonNullCollection<TextWithEncoding>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UniversalTextCommand
            (
                [NotNull] IIrbisConnection connection,
                [NotNull] string commandCode,
                [NotNull] string[] lines,
                [NotNull] Encoding encoding
            )
            : this (connection, commandCode)
        {
            Code.NotNull(lines, "lines");
            Code.NotNull(encoding, "encoding");

            foreach (string line in lines)
            {
                TextLines.Add
                    (
                        new TextWithEncoding
                            (
                                line,
                                encoding
                            )
                    );
            }
        }

        #endregion

        #region Private members

        private readonly string _commandCode;

        private readonly NonNullCollection<TextWithEncoding> _textLines;

        #endregion

        #region Public methods

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Create client query.
        /// </summary>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode;

            foreach (TextWithEncoding line in TextLines)
            {
                result.Add(line);
            }

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify object state.
        /// </summary>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<UniversalTextCommand> verifier
                = new Verifier<UniversalTextCommand>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .NotNullNorEmpty(CommandCode, "CommandCode")
                .NotNull(TextLines, "TextLines")
                .Assert(TextLines.Count > 0, "TextLines.Count")
                .Assert(base.Verify(throwOnError));

            return verifier.Result;
        }

        #endregion
    }
}
