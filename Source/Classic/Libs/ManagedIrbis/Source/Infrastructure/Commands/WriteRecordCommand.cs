// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WriteRecordCommand.cs -- create or update record
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 * TODO read raw record
 */

#region Using directives

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// Create of update existing record in the database.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class WriteRecordCommand
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
        /// New max MFN (result of command execution).
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Don't parse server response.
        /// </summary>
        public bool DontParseResponse { get; set; }

        /// <summary>
        /// Record to write.
        /// </summary>
        [CanBeNull]
        public MarcRecord Record { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public WriteRecordCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region AbstractCommand members

        /// <inheritdoc cref="AbstractCommand.CreateQuery" />
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.UpdateRecord;

            if (ReferenceEquals(Record, null))
            {
                Log.Error
                    (
                        "WriteRecordCommand::CreateQuery: "
                        + "record is null"
                    );

                throw new IrbisNetworkException("record is null");
            }

            string database = Record.Database ?? Connection.Database;
            if (string.IsNullOrEmpty(database))
            {
                Log.Error
                    (
                        "WriteRecordCommand::CreateQuery: "
                        + "database not set"
                    );

                throw new IrbisNetworkException("database not set");
            }

            result
                .Add(database)
                .Add(Lock)
                .Add(Actualize)
                .Add(Record);

            return result;
        }

        /// <inheritdoc cref="AbstractCommand.Execute" />
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            Code.NotNull(query, "query");

            string database = Record.ThrowIfNull("Record").Database
                ?? Connection.Database;

            ServerResponse result = base.Execute(query);

            MaxMfn = result.GetReturnCode();

            MarcRecord record = Record.ThrowIfNull("Record");

            record.Database = database;
            record.HostName = Connection.Host;

            if (!DontParseResponse)
            {
                ProtocolText.ParseResponseForWriteRecord
                    (
                        result,
                        record
                    );
            }

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public override bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<WriteRecordCommand> verifier
                = new Verifier<WriteRecordCommand>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .NotNull(Record, "Record")
                .Assert(base.Verify(throwOnError));

            return verifier.Result;
        }

        #endregion
    }
}
