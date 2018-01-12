// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusPlus0.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Text;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;

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

    static class UniforPlusPlus0
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
                StringBuilder output = new StringBuilder();
                foreach (RecordField field in record.Fields)
                {
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
