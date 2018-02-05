// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WriteFileCommand.cs -- write text file(s) to the server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 */

#region Using directives

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
    /// Write text file(s) to the server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class WriteFileCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// File list.
        /// </summary>
        [NotNull]
        public NonNullCollection<FileSpecification> Files
        {
            get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public WriteFileCommand
            (
                [NotNull] IIrbisConnection connection
            )
            : base(connection)
        {
            Files = new NonNullCollection<FileSpecification>();
        }

        #endregion

        #region Private members

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
            result.CommandCode = CommandCode.ReadDocument;

            foreach (FileSpecification fileName in Files)
            {
                TextWithEncoding text = new TextWithEncoding
                    (
                        fileName.ToString(),
                        IrbisEncoding.Ansi
                    );
                result.Arguments.Add(text);
            }

            return result;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            Code.NotNull(query, "query");

            ServerResponse result = base.Execute(query);

            return result;
        }

        /// <summary>
        /// Verify object state.
        /// </summary>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<WriteFileCommand> verifier
                = new Verifier<WriteFileCommand>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .Assert(Files.Count != 0, "Files.Count")
                .Assert(base.Verify(throwOnError));

            foreach (FileSpecification file in Files)
            {
                verifier.NotNull
                    (
                        file.Content,
                        "file.Contents"
                    );
            }

            return verifier.Result;
        }

        #endregion
    }
}
