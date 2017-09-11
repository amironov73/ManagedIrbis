// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConnectCommand.cs -- connect to the IRBIS64 server
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
    public class ConnectCommand
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
        [CanBeNull]
        public string Username { get; set; }

        /// <summary>
        /// Server version.
        /// </summary>
        [CanBeNull]
        public string ServerVersion { get; set; }

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
            Log.Trace("ConnectCommand::Constructor");

            Connection.GenerateClientID();
            Connection.ResetCommandNumber();
        }

        #endregion

        #region AbstractCommand members

        /// <inheritdoc cref="AbstractCommand.CreateQuery" />
        public override ClientQuery CreateQuery()
        {
            Log.Trace("ConnectCommand::CreateQuery");

            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.RegisterClient;

            string username = Username ?? Connection.Username;
            if (string.IsNullOrEmpty(username))
            {
                Log.Error
                    (
                        "ConnectCommand::CreateQuery: "
                        + "username not specified"
                    );

                throw new IrbisException("username not specified");
            }

            string password = Password ?? Connection.Password;
            if (string.IsNullOrEmpty(password))
            {
                Log.Error
                    (
                        "ConnectCommand::CreateQuery: "
                        + "password not specified"
                    );

                throw new IrbisException("password not specified");
            }

            result.UserLogin = username;
            result.UserPassword = password;

            result.Arguments.Add(username);
            result.Arguments.Add(password);

            return result;
        }

        /// <inheritdoc cref="AbstractCommand.Execute" />
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            Code.NotNull(query, "query");

            Log.Trace("ConnectCommand::Execute");

            if (Connection.Connected)
            {
                Log.Error
                    (
                        "ConnectCommand::Execute: "
                        + "already connected"
                    );

                throw new IrbisException("Already connected");
            }

            ServerResponse result;
            
            while (true)
            {
                result = base.Execute(query);

                Log.Trace
                    (
                        "ConnectCommand::Execute: returnCode="
                        + result.ReturnCode
                    );

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

            ServerVersion = result.ServerVersion;

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="AbstractCommand.Verify" />
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
