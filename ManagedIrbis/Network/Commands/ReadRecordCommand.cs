/* ReadRecordCommand.cs -- read one record from the server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 * TODO implement raw reading?
 */

#region Using directives

using AM;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network.Commands
{
    /// <summary>
    /// Read one record from the server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReadRecordCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// Format.
        /// </summary>
        [CanBeNull]
        public string Format { get; set; }

        /// <summary>
        /// Need lock?
        /// </summary>
        public bool Lock { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Version.
        /// </summary>
        public int VersionNumber { get; set; }

        /// <summary>
        /// Readed record.
        /// </summary>
        [CanBeNull]
        public MarcRecord ReadRecord { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadRecordCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Create client query.
        /// </summary>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.ReadRecord;

            string database = Database ?? Connection.Database;

            if (string.IsNullOrEmpty(database))
            {
                throw new IrbisNetworkException("database not specified");
            }

            result.Arguments.Add(database);
            result.Arguments.Add(Mfn);
            if (VersionNumber != 0)
            {
                result.Arguments.Add(VersionNumber);
            }
            else
            {
                result.Arguments.Add(Lock);
            }
            if (!string.IsNullOrEmpty(Format))
            {
                result.Arguments.Add(Format);
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
            ServerResponse result = base.Execute(query);

            // Check whether no records read
            if (result.GetReturnCode() != -201)
            {

                MarcRecord record = new MarcRecord
                {
                    HostName = Connection.Host,
                    Database = Database
                };

                record = ProtocolText.ParseResponseForReadRecord
                    (
                        result,
                        record
                    );
                record.Verify(true);
                ReadRecord = record;
            }

            return result;
        }

        /// <summary>
        /// Good return codes.
        /// </summary>
        public override int[] GoodReturnCodes
        {
            // Record can be logically deleted
            // or blocked. It's normal.
            get { return new[] { -201, -602, -603 }; }
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
            bool result = !string.IsNullOrEmpty(Database)
                && (Mfn > 0);

            if (result)
            {
                result = base.Verify(throwOnError);
            }

            if (!result && throwOnError)
            {
                throw new VerificationException();
            }

            return result;
        }

        #endregion
    }
}
