// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor0.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Text;

using AM;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдать содержимое документа полностью в формате RTF – &uf('0…
    // Вид функции: 0.
    // Назначение: Выдать содержимое документа полностью(формат ALL).
    // Формат(передаваемая строка):
    // 0
    // Примеры:
    // &unifor('0')
    // Результат расформатирования:
    // \b #910/1:_\b0 ^YДА^PНЮАУ - каф. кримінального права\par
    // \b #920/1:_\b0 ATHRA\par \b #210/1:_\b0 ^AТацій^BВ. Я.^GВасиль
    // Якович^8ukr\par \b #710/1:_\b0 ^AТаций^BВ. Я.^GВасилий Яковлевич
    // ^8rus\par \b #907/1:_\b0 ^A20110301^B111\par \b #907/2:_\b0
    // ^A20110419^BZhukovskaya\par \b #710/2:_\b0 ^ATatsiy^BV.^8eng
    // \par \b #907/3:_\b0 ^A20110421^BZhukovskaya\par \b #907/4:
    // _\b0 ^A20111108^B111\par 
    //

    static class Unifor0
    {
        #region Private members

        #endregion

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
                    string tag = field.Tag.ThrowIfNull("Tag");
                    fieldCount++;
                    string text = field.ToText();
                    int byteCount = encoding.GetByteCount(text);
                    dataSize += byteCount;
                    recordSize += 12; // Размер заголовка поля
                    recordSize += byteCount; // Размер данных поля
                    builder.AppendFormat
                        (
                            "\\b #{0}/{1}:_\\b0 {2}\\par ",
                            tag,
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
