// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ExemplarUtility.cs -- утилиты для работы с экземплярами (поле 910).
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using AM;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Утилиты для работы с экземплярами (поле 910).
    /// </summary>
    public static class ExemplarUtility
    {
        #region Public methods

        /// <summary>
        /// Вычисляет количество свободных экземпляров данной книги/журнала.
        /// </summary>
        /// <param name="record">Библиографическая запись на книгу/журнал.
        /// </param>
        /// <returns>Количество свободных экземпляров.</returns>
        public static int GetFreeExemplarCount
            (
                [CanBeNull] this MarcRecord record
            )
        {
            var result = ReferenceEquals(record, null)
                ? 0
                : GetFreeExemplarCount(record.Fields.GetField(910));

            return result;
        } // method GetFreeExemplarCount

        /// <summary>
        /// Вычисляет суммарное количество экземпляров данной книги/журнала.
        /// </summary>
        /// <param name="record">Библиографическая запись на книгу/журнал.
        /// </param>
        /// <returns>Количество экземпляров.</returns>
        public static int GetTotalExemplarCount
            (
                [CanBeNull] this MarcRecord record
            )
        {
            var result = ReferenceEquals(record, null)
                ? 0
                : GetTotalExemplarCount(record.Fields.GetField(910));

            return result;
        } // method GetTotalExemplarCount

        /// <summary>
        /// Вычисляет количество свободных экземпляров,
        /// числящихся за указанными полями.
        /// </summary>
        /// <param name="fields">Поля, подлежащие подсчету.</param>
        /// <returns>Количество свободных экземпляров.</returns>
        public static int GetFreeExemplarCount
            (
                [CanBeNull] this IEnumerable<RecordField> fields
            )
        {
            var result = 0;

            if (ReferenceEquals(fields, null))
            {
                return result;
            }

            foreach (var field in fields)
            {
                var status = field.GetFirstSubFieldValue('a')
                    .FirstChar();

                switch (status)
                {
                    // экземпляры индивидуального учета
                    case '0': // свободный
                        ++result;
                        break;

                    // остальные статусы индивидуального учета
                    // считаем занятыми
                    // 1 - на руках у читателя
                    // 5 - временно недоступен
                    // 9 - забронирован

                    // экземпляры группового учета
                    case 'U': case 'u': // вуз
                    case 'C': case 'c': // библиотечная сеть
                        var total = field.GetFirstSubFieldValue('1')
                            .SafeToInt32(); // всего экземпляров по месту хранения
                        var nonFree = field.GetFirstSubFieldValue('2')
                            .SafeToInt32(); // из них числятся выданными
                        var free = total - nonFree;
                        result += free;
                        break;

                    // прочие статусы не считаем
                    // R - требуется размножение (ещё не отработал AUTOIN)
                    // 8 - поступил, но не дошел до места хранения
                    // 2 - экземпляр ещё не поступил
                    // 3 - в переплете
                    // 4 - утерян
                    // 6 - списан
                    // P - переплетен (входит в подшивку)
                }
            }

            return result;
        } // method GetFreeExemplarCount

        /// <summary>
        /// Вычисляет общее количество экземпляров,
        /// числящихся за указанными полями.
        /// </summary>
        /// <param name="fields">Поля, подлежащие подсчету.</param>
        /// <returns>Количество экземпляров.</returns>
        public static int GetTotalExemplarCount
            (
                [CanBeNull] this IEnumerable<RecordField> fields
            )
        {
            var result = 0;

            if (ReferenceEquals(fields, null))
            {
                return result;
            }

            foreach (var field in fields)
            {
                var status = field.GetFirstSubFieldValue('a')
                    .FirstChar();

                switch (status)
                {
                    // экземпляры индивидуального учета
                    case '0': // свободный
                    case '1': // на руках у читателя
                    case '5': // временно недоступен
                    case '9': // забронирован
                        ++result;
                        break;

                    // экземпляры группового учета:
                    case 'U': case 'u': // вуз
                    case 'C': case 'c': // библиотечная сеть
                        var count = field.GetFirstSubFieldValue('1')
                            .SafeToInt32();
                        result += count;
                        break;

                    // прочие статусы не считаем
                    // R - требуется размножение (ещё не отработал AUTOIN)
                    // 8 - поступил, но не дошел до места хранения
                    // 2 - экземпляр ещё не поступил
                    // 3 - в переплете
                    // 4 - утерян
                    // 6 - списан
                    // P - переплетен (входит в подшивку)
                }
            }

            return result;
        } // method GetTotalExemplarCount

        /// <summary>
        /// Вычисляет количество свободных экземпляров.
        /// </summary>
        /// <param name="exemplars">Сведения об экземплярах,
        /// подлежащие уточнению.</param>
        /// <returns>Количество свободных экхемпляров.</returns>
        public static int GetFreeExemplarCount
            (
                [CanBeNull] this IEnumerable<ExemplarInfo> exemplars
            )
        {
            var result = 0;

            if (!ReferenceEquals(exemplars, null))
            {
                foreach (var exemplar in exemplars)
                {
                    result += exemplar.GetFreeCount();
                }
            }

            return result;
        } // method GetFreeExemplarCount

        /// <summary>
        /// Вычисляет общее количество экземпляров.
        /// </summary>
        /// <param name="exemplars">Информация об экземплярах,
        /// подлежащих уточнению.</param>
        /// <returns>Количество экземпляров.</returns>
        public static int GetTotalExemplarCount
            (
                [CanBeNull] this IEnumerable<ExemplarInfo> exemplars
            )
        {
            var result = 0;

            if (!ReferenceEquals(exemplars, null))
            {
                foreach (var exemplar in exemplars)
                {
                    result += exemplar.GetTotalCount();
                }
            }

            return result;
        } // method GetTotalExemplarCount

        #endregion

    } // class ExemplarUtility

} // namespace ManagedIrbis.Fields
