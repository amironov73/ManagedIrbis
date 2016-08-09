/* ConnectCommand.cs -- connect to the IRBIS64 server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
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

namespace ManagedIrbis.Network.Commands
{
    //
    // EXTRACT FROM OFFICIAL DOCUMENTATION
    //
    // Если код возврата равен ZERO,
    // то следующие строки - это ini-файл определенный
    // на сервере для данного пользователя.
    // 
    // Если код возврата не равен ZERO - только одна строка.
    // 
    // Коды возврата:
    // ZERO
    // CLIENT_ALREADY_EXISTS  - пользователь
    // уже зарегистрирован.
    // WRONG_PASSWORD - неверный пароль.
    //

    /// <summary>
    /// Connect to the IRBIS64 server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ConnectCommand
        : AbstractCommand
    {
        #region Constants

        /// <summary>
        /// Response specification.
        /// </summary>
        public const string ResponseSpecification = "AIIIAAAAAAT";

        #endregion

        #region Properties

        /// <summary>
        /// Server configuration file content
        /// (on successful connection).
        /// </summary>
        [CanBeNull]
        public string Configuration { get; set; }

        /// <summary>
        /// User password. If not specified,
        /// connection password used.
        /// </summary>
        [CanBeNull]
        public string Password { get; set; }

        /// <summary>
        /// User name. If not specified,
        /// connection name used.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Doesn't require connection.
        /// </summary>
        public override bool RequireConnection
        {
            get { return false; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConnectCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            Connection.GenerateClientID();
            Connection.ResetCommandNumber();
        }

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Create the query.
        /// </summary>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.RegisterClient;

            string username = Username ?? Connection.Username;
            if (string.IsNullOrEmpty(username))
            {
                throw new IrbisException("username not specified");
            }
            string password = Password ?? Connection.Password;
            if (string.IsNullOrEmpty(password))
            {
                throw new IrbisException("password not specified");
            }

            result.UserLogin = username;
            result.UserPassword = password;

            result.Arguments.Add(username);
            result.Arguments.Add(password);

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

            if (Connection.Connected)
            {
                throw new IrbisException("Already connected");
            }

            ServerResponse result;
            
            while (true)
            {
                result = base.Execute(query);

                // CLIENT_ALREADY_EXISTS
                if (result.ReturnCode == -3337)
                {
                    query.ClientID = Connection
                        .GenerateClientID();
                }
                else
                {
                    break;
                }
            }

            if (result.ReturnCode == 0)
            {
                Configuration = result.RemainingAnsiText();
            }

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
            Verifier<ConnectCommand> verifier
                = new Verifier<ConnectCommand>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Connection.Username, "Username")
                .NotNullNorEmpty(Connection.Password, "Password");

            return verifier.Result;
        }

        #endregion
    }
}
