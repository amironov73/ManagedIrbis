// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus4.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using AM;
using AM.Logging;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдача метки, порядкового номера и значения поля
    // в соответствии с индексом (номером повторения)
    // повторяющейся группы – &uf('+4…
    // Вид функции: +4.
    // Назначение: Выдача метки, порядкового номера и значения поля
    // в соответствии с индексом (номером повторения)
    // повторяющейся группы.
    // Присутствует в версиях ИРБИС с 2005.2.
    // Формат (передаваемая строка):
    // +4XY
    // где:
    // Х принимает три значения: T – выдать метку;
    // F – выдать значение поле; N – выдать порядковый номер поля
    // в записи (отличается от индекса повторения,
    // если Y принимает значение 1);
    // Y принимает значения: 0 – поля выдаются в порядке
    // расположения в записи; 1 – поля выдаются в порядке
    // возрастания меток (по умолчанию 0).
    // Примеры:
    // (…&unifor('+4T1'),'_' &unifor('+4N1'),': ', &unifor('+4F1'),)
    //

    static class UniforPlus4
    {
        #region Private members

        #endregion

        #region Public methods

        public static void GetField
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            MarcRecord record = context.Record;
            if (ReferenceEquals(record, null))
            {
                return;
            }

            TextNavigator navigator = new TextNavigator(expression);
            char command = navigator.ReadChar();
            char order = navigator.ReadChar();
            if (command == TextNavigator.EOF
                || order == TextNavigator.EOF)
            {
                return;
            }

            command = CharUtility.ToUpperInvariant(command);
            order = CharUtility.ToUpperInvariant(order);

            RecordField[] workingFields = record.Fields.ToArray();
            if (order != '0')
            {
                Array.Sort(workingFields, FieldComparer.ByTag());
            }

            int index = context.Index;
            RecordField currentField = workingFields.GetOccurrence(index);
            if (ReferenceEquals(currentField, null))
            {
                return;
            }

            string output = null;

            switch (command)
            {
                case 'T':
                    output = currentField.Tag;
                    break;

                case 'F':
                    output = currentField.ToText();
                    break;

                case 'N':
                    int fieldIndex = record.Fields.IndexOf(currentField) + 1;
                    output = fieldIndex.ToInvariantString();
                    break;

                default:
                    Log.Warn
                        (
                            "UniforPlus4::GetField: "
                            + "unknown command="
                            + command.ToVisibleString()
                        );
                    break;
            }

            if (!string.IsNullOrEmpty(output))
            {
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
