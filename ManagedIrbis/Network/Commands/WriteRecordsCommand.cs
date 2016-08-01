/* WriteRecordCommand.cs -- create or update many records
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 * 
 * TODO determine max MFN
 */

#region Using directives

using AM;
using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network.Commands
{
    /// <summary>
    /// Create or update many records simultaneously.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class WriteRecordsCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Need actualize?
        /// </summary>
        public bool Actualize { get; set; }

        /// <summary>
        /// Need lock?
        /// </summary>
        public bool Lock { get; set; }

        /// <summary>
        /// Records to write.
        /// </summary>
        [NotNull]
        public NonNullCollection<RecordReference> References
        {
            get { return _references; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public WriteRecordsCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            _references = new NonNullCollection<RecordReference>();
        }

        #endregion

        #region Private members

        private readonly NonNullCollection<RecordReference> _references;

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
            result.CommandCode = CommandCode.SaveRecordGroup;

            if (References.Count == 0)
            {
                throw new IrbisNetworkException("no records given");
            }
            if (References.Count >= IrbisConstants.MaxPostings)
            {
                throw new IrbisNetworkException("too many records");
            }

            result
                .Add(Lock)
                .Add(Actualize);

            foreach (RecordReference reference in References)
            {
                if (ReferenceEquals(reference.Record, null))
                {
                    throw new IrbisException("record is null");
                }
                if (ReferenceEquals(reference.Database, null))
                {
                    reference.Database = reference.Record.Database;
                }
                if (ReferenceEquals(reference.Database, null))
                {
                    throw new IrbisException("database not set");
                }
                if (string.IsNullOrEmpty(reference.Record.Database))
                {
                    reference.Record.Database = reference.Database;
                }
                result.Add(reference);
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

            result.GetReturnCode();

            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable PossibleNullReferenceException
            for (int i = 0; i < References.Count; i++)
            {
                ProtocolText.ParseResponseForWriteRecords
                    (
                        result,
                        References[i].Record
                    );

                References[i].Mfn = References[i].Record.Mfn;
            }
            // ReSharper restore PossibleNullReferenceException
            // ReSharper restore AssignNullToNotNullAttribute

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
            Verifier<WriteRecordsCommand> verifier
                = new Verifier<WriteRecordsCommand>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .Assert
                (
                    References.Count < IrbisConstants.MaxPostings,
                    "References.Count"
                );

            foreach (RecordReference reference in References)
            {
                reference.Verify(throwOnError);
                verifier.NotNull(reference.Record, "record");
            }

            return verifier.Result;
        }

        #endregion
    }
}
