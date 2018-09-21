// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListProcessesCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;

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
    public class ListProcessesCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ListProcessesCommand
            (
                [NotNull] WorkData data
            )
            : base(data)
        {
        }

        #endregion

        #region Private members

        [NotNull]
        private static string _FormatTime
            (
                DateTime time
            )
        {
            return time.ToString("dd.MM.yyyy hh:mm:ss");
        }

        [CanBeNull]
        private static string _TranslateWorkstation
            (
                [CanBeNull] string code
            )
        {
            switch (code)
            {
                case "c":
                case "C":
                    return "Каталогизатор";

                case "r":
                case "R":
                    return "Читатель";

                case "b":
                case "B":
                    return "Книговыдача";

                case "p":
                case "P":
                    return "Комплектатор";

                case "k":
                case "K":
                    return "Книгообеспеченность";

                case "a":
                case "A":
                    return "Администратор";
            }

            return code;
        }

        [CanBeNull]
        private static string _TranslateCommand
            (
                [CanBeNull] string command
            )
        {
            switch (command)
            {
                case "+1":
                    return "IRBIS_SERVER_STAT";

                case "3":
                    return "IRBIS_FORMAT_ISO_GROUP";

                case "5":
                    return "IRBIS_GBL";

                case "a":
                case "A":
                    return "IRBIS_REG";

                case "b":
                case "B":
                    return "IRBIS_UNREG";

                case "c":
                case "C":
                    return "IRBIS_READ";

                case "d":
                case "D":
                    return "IRBIS_UPDATE";

                case "e":
                case "E":
                    return "IRBIS_RUNLOCK";

                case "f":
                case "F":
                    return "IRBIS_RECIFUPDATE";

                case "g":
                case "G":
                    return "IRBIS_SVR_FORMAT";

                case "h":
                case "H":
                    return "IRBIS_TRM_READ";

                case "i":
                case "I":
                    return "IRBIS_POSTING";

                case "j":
                case "J":
                    return "IRBIS_GBL_RECORD";

                case "k":
                case "K":
                    return "IRBIS_SEARCH";

                case "m":
                case "M":
                    return "IRBIS_BACKUP";

                case "n":
                case "N":
                    return "IRBIS_NOOP";

                case "o":
                case "O":
                    return "IRBIS_MAXMFN";

                case "r":
                case "R":
                    return "IRBIS_FULLTEXT_SEARCH";

                case "s":
                case "S":
                    return "IRBIS_DB_EMPTY";

                case "t":
                case "T":
                    return "IRBIS_DB_NEW";

                case "u":
                case "U":
                    return "IRBIS_DB_UNLOCK";

                case "v":
                case "V":
                    return "IRBIS_MFN_POSTINGS";

                case "w":
                case "W":
                    return "IRBIS_DB_DELETE";

                case "x":
                case "X":
                    return "IRBIS_RELOAD_MASTER";

                case "y":
                case "Y":
                    return "IRBIS_RELOAD_DICT";

                case "z":
                case "Z":
                    return "IRBIS_CREATE_DICT";
            }

            return command;
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
                // UpdateContext();

                // Типичный ответ сервера

                // 0                   // Общий код возврата
                // 1                   // Количество процессов (не считая сервера)
                // 9                   // Число строк на один процесс
                // *                   // Порядковый номер
                // Local IP address    // IP-адрес
                // Сервер ИРБИС        // Имя процесса
                // *****               // Идентификатор клиента
                // *****               // АРМ
                // 21.09.2018 13:25:39 // Подключение
                // *****               // Последняя команда
                // *****               // Номер команды
                // 3920                // Идентификатор процесса
                // Активный            // Состояние

                ServerContext[] contexts = engine.Contexts.ToArray();
                ServerResponse response = Data.Response.ThrowIfNull();
                response.WriteInt32(0).NewLine();
                // Общее число подключенных клиентов
                response.WriteInt32(contexts.Length + 1).NewLine();
                // Число строк на один процесс
                response.WriteInt32(9).NewLine();
                int index = 1;

                // Сначала идет сервер
                response.WriteAnsiString("*").NewLine();
                response.WriteAnsiString("Local IP address").NewLine();
                response.WriteAnsiString("Сервер ИРБИС").NewLine();
                response.WriteAnsiString("*****").NewLine();
                response.WriteAnsiString("*****").NewLine();
                response.WriteAnsiString(_FormatTime(engine.StartedAt)).NewLine();
                response.WriteAnsiString("*****").NewLine();
                response.WriteAnsiString("*****").NewLine();
                response.WriteInt32(Process.GetCurrentProcess().Id).NewLine();
                response.WriteAnsiString("Активный").NewLine();

                foreach (ServerContext ctx in contexts)
                {
                    response.WriteInt32(index++).NewLine();
                    response.WriteAnsiString(ctx.Address).NewLine();
                    response.WriteAnsiString(ctx.Username).NewLine();
                    response.WriteAnsiString(ctx.Id).NewLine();
                    response.WriteAnsiString(_TranslateWorkstation(ctx.Workstation)).NewLine();
                    response.WriteAnsiString(_FormatTime(ctx.Connected)).NewLine();
                    response.WriteAnsiString(_TranslateCommand(ctx.LastCommand)).NewLine();
                    response.WriteInt32(ctx.CommandCount).NewLine();
                    response.WriteInt32(Process.GetCurrentProcess().Id).NewLine();
                    response.WriteAnsiString("Активный").NewLine();
                }
                SendResponse();
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }
            catch (Exception exception)
            {
                Log.TraceException("ListProcessesCommand::Execute", exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
