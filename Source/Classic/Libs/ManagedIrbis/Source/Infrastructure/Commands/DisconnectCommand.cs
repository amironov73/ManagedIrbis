// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DisconnectCommand.cs -- disconnect from the server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Logging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// Disconnect from the server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class DisconnectCommand
        : AbstractCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DisconnectCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            Log.Trace("DisconnectCommand::Constructor");
        }

        #endregion

        #region AbstractCommand members

        /// <inheritdoc cref="AbstractCommand.CreateQuery" />
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.UnregisterClient;

            result.AddAnsi(Connection.Username);

            return result;
        }

        /// <inheritdoc cref="AbstractCommand.Execute" />
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            Log.Trace("DisconnectCommand::Execute");

            ServerResponse result = base.Execute(query);

            Log.Trace
                (
                    "DisconnectCommand::Execute: returnCode="
                    + result.ReturnCode
                );

            Connection._connected = false;

            return result;
        }

        /// <inheritdoc cref="AbstractCommand.CheckResponse" />
        public override void CheckResponse
            (
                ServerResponse response
            )
        {
            // Ignore the result
        }

        #endregion
    }
}
