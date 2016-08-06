/* UpdateUserListCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Network.Commands
{
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

        /// <summary>
        /// Create the client query.
        /// </summary>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.SetUserList;

            if (ReferenceEquals(UserList, null))
            {
                throw new IrbisException("UserList not set");
            }

            foreach (UserInfo userInfo in UserList)
            {
                string line = userInfo.Encode();
                result.AddAnsi(line);
            }

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
            Code.NotNull(query, "query");

            ServerResponse result = base.Execute(query);

            return result;
        }

        /// <summary>
        /// Verify the object state.
        /// </summary>
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
