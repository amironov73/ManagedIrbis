// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReadRecordCommand.cs -- read one record from the server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
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
        /// Throw <see cref="IrbisNetworkException"/>
        /// when empty record received/decoded.
        /// </summary>
        public static bool ThrowOnEmptyRecord { get; set; }

        /// <summary>
        /// Throw <see cref="VerificationException"/>
        /// when bad record received/decoded.
        /// </summary>
        public static bool ThrowOnVerify { get; set; }

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
        public MarcRecord Record { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ReadRecordCommand()
        {
            ThrowOnEmptyRecord = true;
            ThrowOnVerify = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadRecordCommand
            (
                [NotNull] IIrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region AbstractCommand members

        /// <inheritdoc cref="AbstractCommand.CreateQuery" />
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.ReadRecord;

            string database = Database ?? Connection.Database;

            if (string.IsNullOrEmpty(database))
            {
                Log.Error
                    (
                        "ReadRecordCommand::CreateQuery: "
                        + "database not specified"
                    );

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

        /// <inheritdoc cref="AbstractCommand.Execute" />
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
                if (ThrowOnVerify)
                {
                    record.Verify(ThrowOnVerify);
                }

                if (ThrowOnEmptyRecord)
                {
                    IrbisNetworkUtility.ThrowIfEmptyRecord
                        (
                            record,
                            result
                        );
                }

                Record = record;
            }

            return result;
        }

        /// <inheritdoc cref="AbstractCommand.GoodReturnCodes"/>
        public override int[] GoodReturnCodes
        {
            // Record can be logically deleted
            // or blocked. It's normal.
            get { return new[] { -201, -600, -602, -603 }; }
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public override bool Verify
            (
                bool throwOnError
            )
        {
            bool result = !string.IsNullOrEmpty(Database)
                && Mfn > 0;

            if (result)
            {
                result = base.Verify(throwOnError);
            }

            if (!result)
            {
                Log.Error
                    (
                        "ReadRecordCommand::Verify: "
                        + "verification failed"
                    );

                if (throwOnError)
                {
                    throw new VerificationException();
                }
            }

            return result;
        }

        #endregion
    }
}
