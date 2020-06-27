// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DatabaseInfoCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Text;
using System.Linq;

using AM;
using AM.Text;

using ManagedIrbis;

using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace CatBoy.Commands
{
    /// <summary>
    /// Информация о базе данных.
    /// </summary>
    class DatabaseInfoCommand
        : BotCommand
    {
        /// <inheritdoc />
        public override string Name => "info";

        /// <inheritdoc />
        public override string Description => "Информация о базе данных";

        /// <summary>
        /// Describe the database.
        /// </summary>
        public static string Describe(DatabaseInfo db)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("БД: {0}", db.Name.ToVisibleString());
            result.AppendLine();

            if (!string.IsNullOrEmpty(db.Description))
            {
                result.AppendFormat("Описание: {0}",
                    db.Description.ToVisibleString());
                result.AppendLine();
            }

            if (!ReferenceEquals(db.LogicallyDeletedRecords, null)
                && db.LogicallyDeletedRecords.Length != 0)
            {
                result.Append("Логически удалённые записи: ");
                result.Append(db.LogicallyDeletedRecords.Length.ToString());
                result.AppendLine(" шт.");
            }

            if (!ReferenceEquals(db.PhysicallyDeletedRecords, null)
                && db.PhysicallyDeletedRecords.Length != 0)
            {
                result.Append("Физически удалённые записи: ");
                result.Append(db.PhysicallyDeletedRecords.Length.ToString());
                result.AppendLine(" шт.");
            }

            if (!ReferenceEquals(db.NonActualizedRecords, null)
                && db.NonActualizedRecords.Length != 0)
            {
                result.Append("Неактуализированные записи: ");
                result.AppendLine
                    (NumericUtility.CompressRange(db.NonActualizedRecords));
            }

            if (!ReferenceEquals(db.LockedRecords, null)
                && db.LockedRecords.Length != 0)
            {
                result.Append("Заблокированные записи: ");
                result.AppendLine
                    (NumericUtility.CompressRange(db.LockedRecords));
            }

            result.AppendFormat("Макс. MFN: {0}", db.MaxMfn);
            result.AppendLine();

            result.AppendFormat("Только для чтения: {0}", db.ReadOnly);
            result.AppendLine();

            result.AppendFormat("Блокировка БД в целом: {0}", db.DatabaseLocked);
            result.AppendLine();

            return result.ToString();
        }

        /// <inheritdoc />
        public override void Execute(Message message, TelegramBotClient client)
        {
            if (Filter(message, client))
            {
                return;
            }

            var chatId = message.Chat.Id;
            var text = GetCommandText(message);

            var navigator = new TextNavigator(text);
            var dbname = navigator.ReadWord();
            if (string.IsNullOrEmpty(dbname))
            {
                client.SendTextMessageAsync(chatId, "Не задано имя базы данных");
                return;
            }

            try
            {
                using (var irbis = GetConnection())
                {
                    var dbinfo = irbis.GetDatabaseInfo(dbname);
                    client.SendTextMessageAsync(chatId, Describe(dbinfo)).Wait();
                }
            }
            catch (Exception exception)
            {
                client.SendTextMessageAsync(chatId, exception.Message).Wait();
            }
        }
    }
}
