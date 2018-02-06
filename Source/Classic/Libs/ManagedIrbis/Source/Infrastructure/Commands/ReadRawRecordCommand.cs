// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReadRawRecordCommand.cs -- read one record from the server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Logging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// Read one record from the server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReadRawRecordCommand
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
        public RawRecord RawRecord { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadRawRecordCommand
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

        /// <inheritdoc cref="AbstractCommand.Execute"/>
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            ServerResponse result = base.Execute(query);

            // Check whether no records read
            if (result.GetReturnCode() != -201)
            {
                string[] lines = result
                    .RemainingUtfStrings()
                    .ToArray();

                RawRecord = RawRecord.Parse(lines);
                RawRecord.Mfn = Mfn;
                RawRecord.Database = Database ?? Connection.Database;
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
                        "ReadRawRecordCommand::Verify: "
                        + "verification error"
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
