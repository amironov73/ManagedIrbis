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

        public static bool UpdateCard
            (
                [NotNull] JObject card,
                [NotNull] string label,
                [CanBeNull] string newValue
            )
        {
            var found = card.SelectToken($"$.values[?(@.label == '{label}')]");
            if (ReferenceEquals(found, null))
            {
                return false;
            }

            var oldValue = ((JValue)found["value"])?.Value?.ToString();
            if (oldValue == newValue)
            {
                return false;
            }

            found["value"] = new JValue(newValue);

            return true;
        }

        /// <summary>
        /// Обновляет данные на карте.
        /// </summary>
        public static void ProcessReader
            (
                ReaderInfo reader,
                DicardsConfiguration configuration,
                IrbisConnection connection,
                OsmiCardsClient client
            )
        {
            var ticket = OsmiUtility.GetReaderId(reader, configuration);
            var card = client.GetRawCard(ticket);
            if (card is null)
            {
                Log.Error($"CardUpdater: unable to fetch card for ticket{ticket}");
                return;
            }

            // Обновляем ФИО
            var fioField = configuration.FioField.ThrowIfNull("FioField");
            var fioText = reader.FullName;
            UpdateCard(card, fioField, fioText);

            // Обновляем штрих-код
            var barcodeField = configuration.BarcodeField;
            if (barcodeField != null)
            {
                UpdateCard(card, barcodeField, ticket);
            }

            // Массив задолженных книг
            var books = reader.Visits
                .Where(v => !v.IsVisit)
                .Where(v => !v.IsReturned)
                .ToArray();

            // Сюда пишем задолженные книги
            var totalBookList = new StringBuilder(4096);
            var totalBookCount = 0;
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
                    Log.Debug($"Reminder: нет описания книги {book.Index} у читателя {ticket}");
                }
                else
                {
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
                    }
                }
            }

            var totalCountField = configuration.TotalCountField.ThrowIfNull("totalCountField");
            var expiredCountField = configuration.TotalCountField.ThrowIfNull("expiredCountField");
            var totalListField = configuration.TotalCountField.ThrowIfNull("totalListField");
            var expiredListField = configuration.TotalCountField.ThrowIfNull("expiredListField");
            var needUpdate = UpdateCard(card, totalCountField, totalBookCount.ToInvariantString())
                || UpdateCard(card, expiredCountField, expiredBookCount.ToInvariantString())
                || UpdateCard(card, totalListField, totalBookList.ToString())
                || UpdateCard(card, expiredListField, expiredBookList.ToString());
            if (needUpdate)
            {
                client.UpdateCard(ticket, card.ToString(), false);
                Log.Debug($"Reminder: UpdateCard for reader {ticket}");
            }
        }

        #endregion
    }
}
