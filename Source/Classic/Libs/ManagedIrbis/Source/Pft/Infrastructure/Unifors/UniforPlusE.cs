// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusE.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Возвращает порядковый номер заданного поля в записи – &uf('+E
    // Вид функции: +E.
    // Назначение: Возвращает порядковый номер заданного поля в записи.
    // При отсутствии в записи заданного поля возвращается пустота.
    // Присутствует в версиях ИРБИС с 2009.1.
    // Формат (передаваемая строка):
    // +Etag#occ
    // где:
    // tag – метка поля;
    // occ – номер повторения поля(по умолчанию – 1).
    // Может принимать значение* – это означает номер текущего
    // повторения в повторяющейся группе.
    //

    static class UniforPlusE
    {
        #region Public methods

        /// <summary>
        /// Get field index.
        /// </summary>
        public static void GetFieldIndex
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            MarcRecord record = context.Record;
            if (!ReferenceEquals(record, null)
                && !string.IsNullOrEmpty(expression))
            {
                string[] parts = expression.Split('#');
                int tag = parts[0].SafeToInt32();
                string occText = parts.Length > 1
                    ? parts[1]
                    : "1";
                int occ;
                if (occText == "*")
                {
                    occ = context.Index;
                }
                else if (occText == string.Empty)
                {
                    occ = 0;
                }
                else
                {
                    if (!NumericUtility.TryParseInt32(occText, out occ))
                    {
                        return;
                    }
                    occ--;
                }
                RecordField field = record.Fields.GetField(tag, occ);
                if (!ReferenceEquals(field, null))
                {
                    int index = record.Fields.IndexOf(field) + 1;
                    string output = index.ToInvariantString();
                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        #endregion
    }
}
