/* WriteRecordCommand.cs -- 
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
        /// New max MFN.
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Record to write.
        /// </summary>
        [CanBeNull]
        public IrbisRecord Record { get; set; }

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
        public override IrbisClientQuery CreateQuery()
        {
            IrbisClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.UpdateRecord;

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
            Code.NotNull(query, "query");

            if (ReferenceEquals(Record, null))
            {
                throw new IrbisNetworkException("record is null");
            }

            string database = Record.Database ?? Connection.Database;
            if (string.IsNullOrEmpty(database))
            {
                throw new IrbisNetworkException("database not set");
            }

            query
                .Add(database)
                .Add(Lock)
                .Add(Actualize)
                .Add(Record);

            IrbisServerResponse result = base.Execute(query);

            MaxMfn = result.GetReturnCode();

            Record.Database = database;

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
            bool result = !ReferenceEquals(Record, null);

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
