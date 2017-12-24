// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReloadMasterFileCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// Reload database master file.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReloadMasterFileCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReloadMasterFileCommand
            (
                [NotNull] IIrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region AbstractCommand members

        /// <inheritdoc cref="AbstractCommand.CreateQuery"/>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.ReloadMasterFile;

            string database = Database ?? Connection.Database;
            if (string.IsNullOrEmpty(database))
            {
                Log.Error
                    (
                        "ReloadMasteFileCommand::CreateQuery: "
                        + "database not specified"
                    );

                throw new IrbisException("database not specified");
            }
            result.AddAnsi(database);

            return result;
        }

        /// <inheritdoc cref="AbstractCommand.Execute" />
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

        /// <inheritdoc cref="AbstractCommand.Verify" />
        public override bool Verify(bool throwOnError)
        {
            Verifier<ReloadMasterFileCommand> verifier
                = new Verifier<ReloadMasterFileCommand>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Database, "Database");

            return verifier.Result;
        }

        #endregion
    }
}
