/* ReadRecordCommand.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

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
            string database = Database ?? Connection.Database;

            if (string.IsNullOrEmpty(database))
            {
                throw new IrbisNetworkException("database not specified");
            }

            query.Arguments.Add(database);
            query.Arguments.Add(Mfn);
            if (VersionNumber != 0)
            {
                query.Arguments.Add(VersionNumber);
            }
            else
            {
                query.Arguments.Add(Lock);
            }
            if (!string.IsNullOrEmpty(Format))
            {
                query.Arguments.Add(Format);
            }

            ServerResponse result = base.Execute(query);

            return result;
        }

        /// <summary>
        /// Good return codes.
        /// </summary>
        public override int[] GoodReturnCodes
        {
            // Запись может быть логически удалена.
            get { return new[] { -201, -603 }; }
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
