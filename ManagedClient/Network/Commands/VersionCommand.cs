/* VersionCommand.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Network.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class VersionCommand
        : AbstractCommand
    {
        #region Construction

        /// <summary>
        /// Constructor
        /// </summary>
        public VersionCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Execute the command.
        /// </summary>
        public override IrbisServerResponse Execute
            (
                IrbisClientQuery query
            )
        {
            query.CommandCode = CommandCode.ServerInfo;

            return base.Execute(query);
        }

        #endregion
    }
}
