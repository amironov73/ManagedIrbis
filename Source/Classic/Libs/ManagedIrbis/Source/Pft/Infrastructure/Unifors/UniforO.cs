﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforO.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using AM;
using AM.Collections;

using JetBrains.Annotations;

using ManagedIrbis.Fields;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вывод сведений обо всех экземплярах по всем местам хранения – &uf('O…
    // Вид функции: O.
    // Назначение: Вывод сведений обо всех экземплярах по всем местам хранения.
    // Формат(передаваемая строка):
    // нет
    // Пример:
    // &unifor('O')
    //

    static class UniforO
    {
        #region Public methods

        /// <summary>
        /// Вспомогательный метод.
        /// </summary>
        [NotNull]
        public static string AllExemplars
            (
                [NotNull] MarcRecord record
            )
        {
            ExemplarInfo[] exemplars = ExemplarInfo.Parse(record);
            DictionaryCounterInt32<string> counter
                = new DictionaryCounterInt32<string>();

            foreach (ExemplarInfo exemplar in exemplars)
            {
                string place = exemplar.Place;
                if (string.IsNullOrEmpty(place))
                {
                    continue;
                }

                switch (exemplar.Status)
                {
                    case ExemplarStatus.Summary:
                    case ExemplarStatus.BiblioNet:
                        string amountText = exemplar.Amount;
                        int amount;
                        if (NumericUtility.TryParseInt32(amountText, out amount))
                        {
                            counter.Augment(place, amount);
                        }
                        break;

                    default:
                        counter.Augment(place, 1);
                        break;
                }
            }

            StringBuilder result = new StringBuilder();

            bool first = true;
            foreach (var pair in counter)
            {
                if (!first)
                {
                    result.Append(", ");
                }
                first = false;
                result.AppendFormat
                    (
                        "{0}({1})",
                        pair.Key,
                        pair.Value
                    );
            }

            return result.ToString();
        }

        /// <summary>
        /// Реализация &amp;uf('O').
        /// </summary>
        public static void AllExemplars
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!ReferenceEquals(context.Record, null))
            {
                string output = AllExemplars(context.Record);
                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        #endregion
    }
}
