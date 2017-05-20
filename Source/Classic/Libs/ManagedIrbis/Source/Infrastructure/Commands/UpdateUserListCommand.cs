// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UpdateUserListCommand.cs -- update user list on the server
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
    /// Update user list on the server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class UpdateUserListCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// User list to update.
        /// </summary>
        [CanBeNull]
        public UserInfo[] UserList { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public UpdateUserListCommand
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

        /// <inheritdoc cref="AbstractCommand.CreateQuery" />
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.SetUserList;

            if (ReferenceEquals(UserList, null))
            {
                Log.Error
                    (
                        "UpdateUserListCommand::CreateQuery: "
                        + "UserList not set"
                    );
                throw new IrbisException("UserList not set");
            }

            foreach (UserInfo userInfo in UserList)
            {
                string line = userInfo.Encode();
                result.AddAnsi(line);
            }

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

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public override bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<UpdateUserListCommand> verifier
                = new Verifier<UpdateUserListCommand>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .NotNull(UserList, "UserList");

            UserInfo[] userList = UserList.ThrowIfNull();

            verifier.Assert
                (
                    userList.Length != 0,
                    "UserList.Length == 0"
                );
            foreach (UserInfo userInfo in userList)
            {
                verifier.NotNull
                    (
                        userInfo,
                        "userInfo"
                    )
                    .VerifySubObject
                    (
                        userInfo,
                        "userInfo"
                    );
            }

            return verifier.Result;
        }

        #endregion
    }
}
