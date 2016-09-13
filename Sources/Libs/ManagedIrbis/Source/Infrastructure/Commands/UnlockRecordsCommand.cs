/* UnlockRecordsCommand.cs -- truncate the database
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// Truncate the database on the server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class UnlockRecordsCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// Record list.
        /// </summary>
        [NotNull]
        public List<int> Records { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnlockRecordsCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            Records = new List<int>();
        }

        #endregion

        #region AbstractCommand members

        /// <inheritdoc />
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.UnlockRecords;

            string database = Database ?? Connection.Database;
            if (string.IsNullOrEmpty(database))
            {
                throw new IrbisException("database not specified");
            }
            result.AddAnsi(database);

            if (Records.Count == 0)
            {
                throw new IrbisException("record list is empty");
            }
            result.Arguments.AddRange(Records.Cast<object>());

            return result;
        }

        /// <inheritdoc />
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            Code.NotNull(query, "query");

            ServerResponse result = base.Execute(query);
            result.GetReturnCode();

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
            Verifier<UnlockRecordsCommand> verifier
                = new Verifier<UnlockRecordsCommand>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(Database, "Database")
                .Assert(Records.Count != 0, "Records.Count");

            return verifier.Result;
        }

        #endregion
    }
}
