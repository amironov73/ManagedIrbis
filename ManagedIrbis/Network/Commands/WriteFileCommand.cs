/* WriteFileCommand.cs -- write text file(s) to the server
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Collections;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network.Commands
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
            get { return _files; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public WriteFileCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            _files = new NonNullCollection<FileSpecification>();
        }

        #endregion

        #region Private members

        private readonly NonNullCollection<FileSpecification> _files;

        #endregion

        #region Public methods

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Create client query.
        /// </summary>
        public override IrbisClientQuery CreateQuery()
        {
            IrbisClientQuery result = base.CreateQuery();
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
                IrbisClientQuery query
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
                        file.Contents,
                        "file.Contents"
                    );
            }

            return verifier.Result;
        }

        #endregion
    }
}
