// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor0.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using AM;
using AM.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдать содержимое документа полностью
    // во внутреннем представлении – &uf('+0…
    // Вид функции: +0.
    // Назначение: Выдать содержимое документа полностью (формат ALL).
    // Формат(передаваемая строка):
    // +0
    // Результат расформатирования:
    // 0
    // 2#0
    // 0#1
    // 910#^YДА^PНЮАУ - каф. кримінального права
    // 920#ATHRA
    // 210#^AТацій^BВ. Я.^GВасиль Якович^8ukr
    // 710#^AТаций^BВ. Я.^GВасилий Яковлевич^8rus
    // 907#^A20110301^B111
    // 907#^A20110419^BZhukovskaya
    // 710#^ATatsiy^BV.^8eng
    // 907#^A20110421^BZhukovskaya
    // 907#^A20111108^B111
    //

    static class Unifor0
    {
        #region Public methods

        public static void FormatAll
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            MarcRecord record = context.Record;
            if (!ReferenceEquals(record, null))
            {
                StringBuilder builder = new StringBuilder();
                int fieldCount = 0;
                int dataSize = 0;
                int recordSize = 32; // Размер заголовка записи
                Encoding encoding = IrbisEncoding.Utf8;
                foreach (RecordField field in record.Fields)
                {
                    int tag = field.Tag;
                    fieldCount++;
                    string text = field.ToText();
                    int byteCount = encoding.GetByteCount(text);
                    dataSize += byteCount;
                    recordSize += 12; // Размер заголовка поля
                    recordSize += byteCount; // Размер данных поля
                    builder.AppendFormat
                        (
                            "\\b #{0}/{1}:_\\b0 {2}\\par ",
                            tag.ToInvariantString(),
                            field.Repeat,
                            RichText.Encode(text)
                        );
                }

                builder.AppendFormat
                    (
                        "\\par fields: {0} data size: {1} record size: {2}",
                        fieldCount,
                        dataSize,
                        recordSize
                    );

                context.Write(node, builder.ToString());
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
