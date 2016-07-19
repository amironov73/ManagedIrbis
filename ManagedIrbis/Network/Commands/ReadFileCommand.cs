/* ReadFileCommand.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network.Commands
{
    /// <summary>
    /// 
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
        public List<FileSpecification> Files
        {
            get { return _files; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connection"></param>
        public ReadFileCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            _files = new List<FileSpecification>();
        }

        #endregion

        #region Private members

        private readonly List<FileSpecification> _files;

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
            response._returnCodeRetrieved = true;
        }

        /// <summary>
        /// Create client query.
        /// </summary>
        public override IrbisClientQuery CreateQuery()
        {
            IrbisClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.ReadDocument;

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

            foreach (FileSpecification fileName in Files)
            {
                string item = fileName.ToString();
                query.Arguments.Add(item);
            }

            ServerResponse result = base.Execute(query);

            return result;
        }

        #endregion
    }
}
