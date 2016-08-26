/* ListUsersCommand.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ListUsersCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Result.
        /// </summary>
        [CanBeNull]
        public UserInfo[] Result { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ListUsersCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region AbstractCommand members

        /// <inheritdoc />
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.GetUserList;

            return result;
        }

        /// <inheritdoc />
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            ServerResponse response = base.Execute(query);
            response.GetReturnCode();

            Result = UserInfo.Parse(response);

            return response;
        }

        #endregion
    }
}
