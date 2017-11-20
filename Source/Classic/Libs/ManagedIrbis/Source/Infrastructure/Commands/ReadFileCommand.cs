// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReadFileCommand.cs -- read text file(s) from the server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// Read text file(s) from the server
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReadFileCommand
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

        /// <summary>
        /// Retrieved text files.
        /// </summary>
        [CanBeNull]
        public string[] Result { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connection"></param>
        public ReadFileCommand
            (
                [NotNull] IIrbisConnection connection
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

        /// <summary>
        /// Get file text.
        /// </summary>
        [NotNull]
        public string[] GetFileText
            (
                [NotNull] ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            int count = Files.Count;
            string[] result = new string[count];

            for (int i = 0; i < count; i++)
            {
                string text = response.GetAnsiString();
                text = IrbisText.IrbisToWindows(text);
                result[i] = text;
            }

            return result;
        }

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Check the server response.
        /// </summary>
        public override void CheckResponse
            (
                ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            // Don't check: there's no return code
            response.RefuseAnReturnCode();
        }

        /// <summary>
        /// Create client query.
        /// </summary>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.ReadDocument;

            foreach (FileSpecification fileName in Files)
            {
                string item = fileName.ToString();
                result.AddAnsi(item);
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
            Result = GetFileText(result);

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
            Verifier<ReadFileCommand> verifier
                = new Verifier<ReadFileCommand>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .Assert(Files.Count != 0, "Files.Count")
                .Assert(base.Verify(throwOnError));

            return verifier.Result;
        }

        #endregion
    }
}
