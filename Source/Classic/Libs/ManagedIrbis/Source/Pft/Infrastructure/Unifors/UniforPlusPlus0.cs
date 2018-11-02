// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusPlus0.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Text;

using AM;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдать содержимое документа полностью - только
    // содержимое полей – &uf('++0')
    // Вид функции: ++0.
    // Назначение: Выдать содержимое документа полностью (формат ALLl).
    // Формат(передаваемая строка):
    // ++0
    //

    //
    // Начиная с версии 2016.1, форматный выход &uf('++0')
    // модифицирован следующим образом
    // &uf('++0,nnn,mmm,...')
    // nnn, mmm - метки полей, которые ИСКЛЮЧАЮТСЯ из форматирования.
    //

    static class UniforPlusPlus0
    {
        #region Public methods

        public static void FormatAll
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            List<int> tagsToSkip = new List<int> { 953 };
            if (!string.IsNullOrEmpty(expression))
            {
                foreach (string item in StringUtility.SplitString(expression, ","))
                {
                    int tag;
                    if (NumericUtility.TryParseInt32(item, out tag))
                    {
                        tagsToSkip.Add(tag);
                    }
                }
            }

            MarcRecord record = context.Record;
            if (!ReferenceEquals(record, null))
            {
                StringBuilder output = new StringBuilder();
                foreach (RecordField field in record.Fields)
                {
                    if (field.Tag == IrbisGuid.Tag)
                    {
                        // Поле GUID не выводится
                        continue;
                    }

                    if (tagsToSkip.Contains(field.Tag))
                    {
                        continue;
                    }

                    if (field.IsEmpty)
                    {
                        continue;
                    }

                    string fieldValue = field.Value;
                    if (!string.IsNullOrEmpty(fieldValue))
                    {
                        output.Append(fieldValue);
                    }
                    foreach (SubField subField in field.SubFields)
                    {
                        output.Append(" ");
                        output.Append(subField.Value);
                    }
                    output.AppendLine();
                }
                context.WriteAndSetFlag(node, output.ToString());
            }
        }

        #endregion
    }
}
