// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforY.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Linq;
using System.Text;

using AM;
using AM.Collections;

using JetBrains.Annotations;

using ManagedIrbis.Fields;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Возвращает данные обо всех свободных (не выданных) экземплярах по всем местах хранения – &uf('Y')
    // Вид функции: Y.
    // Назначение: Возвращает данные обо всех свободных(не выданных) экземплярах по всем местах хранения.
    // Формат(передаваемая строка):
    // нет
    //
    // Пример:
    //
    // &unifor('Y')
    //

    static class UniforY
    {
        #region Public methods

        /// <summary>
        /// Вспомогательный метод.
        /// </summary>
        [NotNull]
        public static string FreeExemplars
            (
                [NotNull] MarcRecord record
            )
        {
            ExemplarInfo[] exemplars = ExemplarInfo.Parse(record);
            DictionaryCounterInt32<string> counter
                = new DictionaryCounterInt32<string>();

            foreach (ExemplarInfo exemplar in exemplars)
            {
                string place = exemplar.Place ?? string.Empty;

                switch (exemplar.Status)
                {
                    case ExemplarStatus.Summary:
                    case ExemplarStatus.BiblioNet:
                        string amountText = exemplar.Amount;
                        int amount;
                        if (NumericUtility.TryParseInt32(amountText, out amount))
                        {
                            string onHandText = exemplar.OnHand;
                            int onHand;
                            if (NumericUtility.TryParseInt32(onHandText, out onHand))
                            {
                                amount -= onHand;
                            }
                        }
                        counter.Augment(place, amount);
                        break;

                    case ExemplarStatus.Free:
                        counter.Augment(place, 1);
                        break;
                }
            }

            StringBuilder result = new StringBuilder();

            bool first = true;
            foreach (string key in counter.Keys.OrderBy(_ => _))
            {
                int value = counter[key];
                if (!first)
                {
                    result.Append(", ");
                }
                first = false;
                result.AppendFormat
                    (
                        "{0}({1})",
                        key,
                        value
                    );
            }

            return result.ToString();
        }

        /// <summary>
        /// Реализация &amp;uf('y').
        /// </summary>
        public static void FreeExemplars
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!ReferenceEquals(context.Record, null))
            {
                string output = FreeExemplars(context.Record);
                context.WriteAndSetFlag(node, output);
            }
        }

        #endregion
    }
}
