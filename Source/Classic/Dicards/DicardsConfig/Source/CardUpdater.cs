// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* CardUpdate.cs -- обновлятор сведений на карте
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System.Linq;
using System.Text;

using AM;
using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Readers;

using Newtonsoft.Json.Linq;

using RestfulIrbis.OsmiCards;

#endregion

namespace DicardsConfig
{
    /// <summary>
    /// Обновлятор сведений на карте.
    /// </summary>
    public static class CardUpdater
    {
        #region Public methods

        /// <summary>
        /// Обновление поля в карте (локальное).
        /// </summary>
        /// <param name="card">Карта в виде XML.</param>
        /// <param name="label">Метка поля.</param>
        /// <param name="newValue">Новое значение поля.</param>
        /// <returns><c>false</c>, если в карте нет такого поля
        /// или если предыдущее значение совпадает с указанным.</returns>
        /// <remarks>
        /// Таким образом, если метод вернул <c>true</c>, значит, запись
        /// надо отослать на сервер.
        /// </remarks>
        public static bool UpdateField
            (
                [NotNull] JObject card,
                [NotNull] string label,
                [CanBeNull] string newValue
            )
        {
            var found = card.SelectToken($"$.values[?(@.label == '{label}')]");
            if (found is null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(newValue))
            {
                newValue = "-empty-";
            }

            var oldValue = ((JValue)found["value"])?.Value?.ToString();
            if (oldValue == newValue)
            {
                return false;
            }

            Log.Trace($"CardUpdater::UpdateField: {label}={newValue}");
            found["value"] = new JValue(newValue);

            return true;
        }

        /// <summary>
        /// Обновляет данные на карте.
        /// </summary>
        public static void UpdateReaderCard
            (
                [NotNull] ReaderInfo reader,
                [NotNull] DicardsConfiguration configuration,
                [NotNull] IrbisConnection connection,
                [NotNull] OsmiCardsClient client
            )
        {
            var ticket = OsmiUtility.GetReaderId(reader, configuration);
            var card = client.GetRawCard(ticket);
            if (card is null)
            {
                Log.Error($"CardUpdater: unable to fetch card for ticket {ticket}");
                return;
            }

            // Обновляем ФИО
            var fioField = configuration.FioField.ThrowIfNull("FioField");
            var fioText = reader.FullName;
            var needUpdate = UpdateField(card, fioField, fioText);

            // Ссылка на электронный каталог
            var catalogField = configuration.CatalogField;
            var catalogUrl = configuration.CatalogUrl;
            if (!string.IsNullOrEmpty(catalogField))
            {
                needUpdate = needUpdate
                             || UpdateField(card, catalogField, catalogUrl);
            }

            // Ссылка на личный кабинет
            var cabinetField = configuration.CabinetField;
            var cabinetUrl = configuration.CabinetUrl;
            if (!string.IsNullOrEmpty(cabinetField))
            {
                needUpdate = needUpdate
                             || UpdateField(card, cabinetField, cabinetUrl);
            }

            // Обновляем штрих-код
            var barcodeField = configuration.BarcodeField;
            if (barcodeField != null)
            {
                needUpdate = needUpdate
                    && UpdateField(card, barcodeField, ticket);
            }

            // Массив задолженных книг
            var books = new VisitInfo[0];
            if (reader.Visits != null)
            {
                books = reader.Visits
                    .Where(v => !v.IsVisit)
                    .Where(v => !v.IsReturned)
                    .ToArray();
            }

            // Сюда пишем задолженные книги
            var totalBookList = new StringBuilder(4096);
            var totalBookCount = 0;

            // Сюда пишем просроченные книги
            var expiredBookList = new StringBuilder(4096);
            var expiredBookCount = 0;

            foreach (var book in books)
            {
                var description = book.Description;
                if (string.IsNullOrEmpty(description))
                {
                    description = connection.SearchFormat
                        (
                            $"\"I={book.Index}\"",
                            "@brief"
                        )
                        .FirstOrDefault()
                        ?.Text;
                }

                if (string.IsNullOrEmpty(description))
                {
                    Log.Debug("UpdateReaderCard: "
                        + $"нет описания книги {book.Index} у читателя {ticket}");
                }
                else
                {
                    Log.Debug
                        (
                            $"UpdateReaderCard: {ticket}: '{book.DateReturnedString}' "
                            + $"'{book.Index}' '{book.DateGivenString}' "
                            + $"'{book.DateExpectedString}' '{book.Inventory}' '{book.Barcode}' "
                            + $"{description}"
                        );

                    if (totalBookList.Length != 0)
                    {
                        totalBookList.Append("\n");
                    }

                    totalBookList.Append($"{totalBookCount + 1}. {description}");
                    ++totalBookCount;

                    if (book.Expired)
                    {
                        if (expiredBookList.Length != 0)
                        {
                            expiredBookList.Append("\n");
                        }

                        expiredBookList.Append($"{expiredBookCount + 1}. {description}");
                        ++expiredBookCount;
                        Log.Debug($"UpdateReaderCard: {ticket} {book.Inventory}: loan expired");
                    }
                }
            }

            // Общее количество книг на руках
            var totalCountField = configuration.TotalCountField;
            if (!string.IsNullOrEmpty(totalCountField))
            {
                needUpdate = needUpdate
                    || UpdateField(card, totalCountField, totalBookCount.ToInvariantString());
            }

            // Количество просроченных книг
            var expiredCountField = configuration.ExpiredCountField;
            if (!string.IsNullOrEmpty(expiredCountField))
            {
                needUpdate = needUpdate
                    || UpdateField(card, expiredCountField, expiredBookCount.ToInvariantString());
            }

            // Список книг на руках
            var totalListField = configuration.TotalListField;
            if (!string.IsNullOrEmpty(totalListField))
            {
                needUpdate = needUpdate
                    || UpdateField(card, totalListField, totalBookList.ToString());
            }

            // Список просроченных книг
            var expiredListField = configuration.ExpiredListField;
            if (!string.IsNullOrEmpty(expiredListField))
            {
                needUpdate = needUpdate
                    || UpdateField(card, expiredListField, expiredBookList.ToString());
            }

            // Напоминание о необходимости сдать книги в библиотеку
            var messageField = configuration.ReminderField;
            var messageText = expiredBookCount == 0 ? string.Empty : configuration.ReminderMessage;
            if (!string.IsNullOrEmpty(messageField))
            {
                needUpdate = needUpdate
                             || UpdateField(card, messageField, messageText);
            }

            Log.Debug("$UpdateReaderCard: {ticket} totalBookCount={totalBookCount}");
            Log.Debug("$UpdateReaderCard: {ticket} expiredBookCount={expiredBookCount}");
            Log.Info($"UpdateReaderCard: {ticket} needUpdate={needUpdate}");

            // Если в карте что-то изменилось, отправляем её на сервер
            if (needUpdate)
            {
                client.UpdateCard(ticket, card.ToString(), false);
                Log.Info($"UpdateReaderCard: {ticket} OK");
            }
        }

        #endregion

    } // class CardUpdater

} // namespace DicardsConfig
