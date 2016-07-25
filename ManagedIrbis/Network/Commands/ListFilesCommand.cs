/* ListFilesCommand.cs -- list server files
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * State: poor
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
using AM.Collections;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Network.Commands
{
    /// <summary>
    /// List server files.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ListFilesCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// File specification (can contain wildcards).
        /// </summary>
        [NotNull]
        public NonNullCollection<FileSpecification> Specifications
        {
            get { return _specifications; }
        }

        /// <summary>
        /// List of found files.
        /// </summary>
        [CanBeNull]
        public string[] Files { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ListFilesCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            _specifications = new NonNullCollection<FileSpecification>();
        }

        #endregion

        #region Private members

        private readonly NonNullCollection<FileSpecification> _specifications;

        #endregion

        #region Public methods

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
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.ListFiles;


            if (Specifications.Count == 0)
            {
                throw new IrbisException("specification list is empty");
            }
            foreach (FileSpecification specification in Specifications)
            {
                specification.Verify(true);
                result.Add(specification);
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

            List<string> files = result.RemainingAnsiStrings();
            Files = files.SelectMany
                (
                    line => IrbisText.IrbisToWindows(line).SplitLines()
                )
                .ToArray();

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
            // ReSharper disable AssignNullToNotNullAttribute
            Verifier<ListFilesCommand> verifier
                = new Verifier<ListFilesCommand>
                    (
                        this,
                        throwOnError
                    );

            foreach (FileSpecification specification in Specifications)
            {
                verifier.VerifySubObject
                    (
                        specification,
                        "specification"
                    );
            }

            return verifier.Result;
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        #region Object members

        #endregion
    }
}
