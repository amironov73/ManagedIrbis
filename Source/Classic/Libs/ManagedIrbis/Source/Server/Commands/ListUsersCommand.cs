// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListUsersCommand.cs --
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
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ListUsersCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ListUsersCommand
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
                ServerContext context = engine.RequireAdministratorContext(Data);
                Data.Context = context;
                UpdateContext();

                // Типичный ответ сервера

                // 0              // Общий код возврата
                // 2              // Количество известных системе пользователей
                // 8              // Строк на одного пользователя
                // 1              // Номер по порядку
                // librarian      // Логин
                // secret         // Пароль
                // INI\MIRONC.INI // INI для Каталогизатора
                // irbisr.ini     // INI для Читателя
                // irbisb.ini     // INI для Книговыдачи
                // irbisp.ini     // INI для Комплектатора
                // irbisk.ini     // INI для Книгообеспеченности
                // irbisa.ini     // INI для Администратора
                // 2              // Номер по порядку
                // rdr            // Логин
                // rdr            // Пароль
                //                // Каталогизатор запрещен
                // INI\RDR_R.INI  // INI для Читателя
                //                // Книговыдача запрещена
                //                // Комплектатор запрещен
                //                // Книгообеспеченность запрещена
                //                // Администратор запрещен

                UserInfo[] users = engine.Users;
                ServerResponse response = Data.Response.ThrowIfNull();
                response.WriteInt32(0).NewLine();
                // Количество известных системе пользователей
                response.WriteInt32(users.Length).NewLine();
                response.WriteInt32(8).NewLine(); // Строк на одного пользователя
                int index = 1;
                foreach (UserInfo user in users)
                {
                    response.WriteInt32(index++).NewLine();
                    response.WriteAnsiString(user.Name).NewLine();
                    response.WriteAnsiString(user.Password).NewLine();
                    response.WriteAnsiString(user.Cataloger).NewLine();
                    response.WriteAnsiString(user.Reader).NewLine();
                    response.WriteAnsiString(user.Circulation).NewLine();
                    response.WriteAnsiString(user.Acquisitions).NewLine();
                    response.WriteAnsiString(user.Provision).NewLine();
                    response.WriteAnsiString(user.Administrator).NewLine();
                }
                SendResponse();
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }
            catch (Exception exception)
            {
                Log.TraceException("ListUsersCommand::Execute", exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
