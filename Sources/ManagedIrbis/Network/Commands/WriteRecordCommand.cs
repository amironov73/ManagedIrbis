/* WriteRecordCommand.cs -- create or update record
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 * TODO read raw record
 */

#region Using directives

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network.Commands
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

        /// <summary>
        /// Create client query.
        /// </summary>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.UpdateRecord;

            if (ReferenceEquals(Record, null))
            {
                throw new IrbisNetworkException("record is null");
            }

            string database = Record.Database ?? Connection.Database;
            if (string.IsNullOrEmpty(database))
            {
                throw new IrbisNetworkException("database not set");
            }

            result
                .Add(database)
                .Add(Lock)
                .Add(Actualize)
                .Add(Record);

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

            // ReSharper disable PossibleNullReferenceException
            string database = Record.Database ?? Connection.Database;
            // ReSharper restore PossibleNullReferenceException

            ServerResponse result = base.Execute(query);

            MaxMfn = result.GetReturnCode();

            Record.Database = database;
            Record.HostName = Connection.Host;

            ProtocolText.ParseResponseForWriteRecord
                            (
                                result,
                                Record
                            );

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
