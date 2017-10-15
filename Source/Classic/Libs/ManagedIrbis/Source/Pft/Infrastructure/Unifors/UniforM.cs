// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforM.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Linq;

using AM;
using AM.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Отсортировать повторения заданного поля – &uf('M
    // Вид функции: M.
    // Назначение: Отсортировать повторения заданного поля
    // (имеется в виду строковая сортировка) – функция ничего
    // не возвращает. Можно применять только в глобальной корректировке.
    // Формат (передаваемая строка):
    // MX<tag>^<delims>
    // где:
    // X – вид сортировки: I – по возрастанию; D – по убыванию.
    // <tag> – метка поля.
    // <delims> – разделители подполей, определяющих ключ сортировки.
    //
    // Пример:
    //
    // &unifor('MI910^BD')
    //

    static class UniforM
    {
        #region Private members

        class FieldToSort
        {
            public RecordField Field { get; set; }

            public string Text { get; set; }
        }

        static void SortField
            (
                [NotNull] MarcRecord record,
                int tag,
                char code,
                bool descending
            )
        {
            RecordField[] found = record.Fields.GetField(tag);
            FieldToSort[] fields = new FieldToSort[found.Length];

            for (int i = 0; i < found.Length; i++)
            {
                fields[i] = new FieldToSort
                {
                    Field = found[i],
                    Text = code == '\0'
                        ? found[i].ToText()
                        : found[i].GetFirstSubFieldValue(code)
                          ?? string.Empty
                };
                record.Fields.Remove(found[i]);
            }

            fields =
                (
                    descending
                    ? fields.OrderByDescending(field => field.Text)
                    : fields.OrderBy(field => field.Text)
                )
                .ToArray();

            record.Fields.AddRange
                (
                    fields.Select
                        (
                            field => field.Field
                        )
                    .ToArray()
                );
        }

        #endregion

        #region Public methods

        public static void Sort
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression)
                && !ReferenceEquals(context.Record, null))
            {
                TextNavigator navigator = new TextNavigator(expression);
                char direction = char.ToLower(navigator.ReadChar());
                if (direction != 'i' && direction != 'd')
                {
                    return;
                }

                string tagText = navigator.ReadUntil('^');
                if (string.IsNullOrEmpty(tagText))
                {
                    return;
                }

                int tag = NumericUtility.ParseInt32(tagText);
                char code = '\0';
                if (!navigator.IsEOF)
                {
                    navigator.ReadChar();
                    code = navigator.ReadChar();
                }

                SortField
                    (
                        context.Record,
                        tag,
                        code,
                        direction != 'i'
                    );
            }
        }

        #endregion
    }
}
