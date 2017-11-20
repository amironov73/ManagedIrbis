// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListFilesCommand.cs -- list server files
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * State: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Collections;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// List server files.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ListFilesCommand
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
                [NotNull] IIrbisConnection connection
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

        /// <inheritdoc cref="AbstractCommand.CheckResponse"/>
        public override void CheckResponse
            (
                ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            // Don't check: there's no return code
            response.RefuseAnReturnCode();
        }

        /// <inheritdoc cref="AbstractCommand.CreateQuery"/>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.ListFiles;

            if (Specifications.Count == 0)
            {
                Log.Error
                    (
                        "ListFilesCommand::CreateQuery: "
                        + "specification list is empty"
                    );

                throw new IrbisException("specification list is empty");
            }

            foreach (FileSpecification specification in Specifications)
            {
                specification.Verify(true);
                result.Add(specification);
            }

            return result;
        }

        /// <inheritdoc cref="AbstractCommand.Execute"/>
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

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
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
