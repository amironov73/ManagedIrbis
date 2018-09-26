// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConnectCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;
using AM.Logging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Commands
{

    //
    // Начиная с версии 2018.1
    // Для клиентских АРМов реализована авторизация по учетной
    // записи клиента в Windows.
    // Чтобы это работало, необходимо:
    // * в клиентском INI-файле (cirbisc.ini, cirbisb.ini и т.д.)
    // в секции [MAIN] установить параметр USERNAME=!
    // * для соответствующей учетной записи клиента на сервере
    // в качестве ПАРОЛЯ установить !
    // В результате вход в АРМы будет происходить БЕЗ АВТОРИЗАЦИИ.
    //
    // В текущей реализации сервера для поддержки данного
    // нововведения ничего делать не надо.
    //

    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ConnectCommand
        : ServerCommand
    {
        #region Properties

        /// <inheritdoc cref="ServerCommand.SendVersion" />
        public override bool SendVersion
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConnectCommand
            (
                [NotNull] WorkData data
            )
            : base(data)
        {
        }

        #endregion

        #region ServerCommand members

        /// <inheritdoc cref="ServerCommand.Execute" />
        public override void Execute()
        {
            IrbisServerEngine engine = Data.Engine.ThrowIfNull();
            engine.OnBeforeExecute(Data);

            try
            {
                ClientRequest request = Data.Request.ThrowIfNull();
                string clientId = request.ClientId.ThrowIfNull();
                ServerContext context = engine.FindContext(clientId);
                if (!ReferenceEquals(context, null))
                {
                    // Клиент с таким идентификатором уже зарегистрирован
                    throw new IrbisException(-3337);
                }

                string username = request.RequireAnsiString();
                string password = request.RequireAnsiString();

                UserInfo userInfo = engine.FindUser(username);
                if (ReferenceEquals(userInfo, null))
                {
                    throw new IrbisException(-3333);
                }

                if (password != userInfo.Password)
                {
                    throw new IrbisException(-4444);
                }

                Data.User = userInfo;
                string iniFile = engine.GetUserIniFile(userInfo, request.Workstation);

                context = Data.Engine.CreateContext(clientId);
                Data.Context = context;
                context.Address = Data.Socket.GetRemoteAddress();
                context.Username = username;
                context.Password = password;
                context.Workstation = request.Workstation;

                ServerResponse response = Data.Response.ThrowIfNull();
                // Код возврата
                response.WriteInt32(0).NewLine();
                // Интервал подтверждения
                response.WriteInt32(engine.IniFile.ClientTimeLive).NewLine();
                // INI-файл
                response.WriteAnsiString(iniFile).NewLine();

                SendResponse();
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }
            catch (Exception exception)
            {
                Log.TraceException("ConnectCommand::Execute", exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
