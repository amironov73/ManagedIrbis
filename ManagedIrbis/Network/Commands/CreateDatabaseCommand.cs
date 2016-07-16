/* CreateDatabaseCommand.cs -- 
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
    public sealed class CreateDatabaseCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Name for new database.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// Will the database be visible to reader?
        /// </summary>
        public bool ReaderAccess { get; set; }

        /// <summary>
        /// Template database name.
        /// </summary>
        [CanBeNull]
        public string Template { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateDatabaseCommand
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
            result.CommandCode = CommandCode.CreateDatabase;

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
            query
                .Add(Database)
                .Add(ReaderAccess);

            IrbisServerResponse result = base.Execute(query);

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
            Verifier<CreateDatabaseCommand> verifier
                = new Verifier<CreateDatabaseCommand>(this, throwOnError);
            verifier
                .NotNullNorEmpty(Database, "Database");

            return verifier.Result;
        }

        #endregion
    }
}
