/* ReadFileCommand.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Network.Commands
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
        public List<IrbisFileSpecification> Files
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
            _files = new List<IrbisFileSpecification>();
        }

        #endregion

        #region Private members

        private readonly List<IrbisFileSpecification> _files;

        #endregion

        #region Public methods

        /// <summary>
        /// Get file text.
        /// </summary>
        [NotNull]
        public string[] GetFileText
            (
                [NotNull] IrbisServerResponse response
            )
        {
            Code.NotNull(response, "response");

            int count = Files.Count;
            string[] result = new string[count];

            for (int i = 0; i < count; i++)
            {
                string text = response.GetUtfString();
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
                IrbisServerResponse response
            )
        {
            // Don't check: there's no return code
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
        public override IrbisServerResponse Execute
            (
                IrbisClientQuery query
            )
        {
            foreach (IrbisFileSpecification fileName in Files)
            {
                string item = fileName.ToString();
                query.Arguments.Add(item);
            }

            IrbisServerResponse result = base.Execute(query);

            return result;
        }

        #endregion
    }
}
