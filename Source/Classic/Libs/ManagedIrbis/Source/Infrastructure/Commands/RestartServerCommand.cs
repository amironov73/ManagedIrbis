// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RestartServerCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// Reload database dictionary.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RestartServerCommand
        : AbstractCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RestartServerCommand
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
            result.CommandCode = CommandCode.RestartServer;

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

        #endregion
    }
}
