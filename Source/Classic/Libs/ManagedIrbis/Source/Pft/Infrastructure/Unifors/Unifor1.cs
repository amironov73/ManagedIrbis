// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor1.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.PlatformSpecific;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class Unifor1
    {
        #region Private members

        #endregion

        #region Public methods

        //
        // Вернуть заданный подэлемент – &uf('1')
        //
        // Формат (передаваемая строка):
        // 1NCXY?V<tag>^<delim>*<offset>.<length>#<occur>
        //
        // где:
        // N – номер повторения подэлемента;
        // если указана * – номер подэлемента совпадает
        // со счетчиком повторяющейся группы.
        // ХY – разделители между подэлементами.
        // С – принимает значения: R – разделители
        // справа от каждого подэлемента, кроме последнего;
        // L – разделители слева от каждого подэлемента;
        // D – каждый подэлемент заключен слева
        // разделителем Х и справа – Y.
        // ? – символ-разделитель.
        //
        // Остальные параметры аналогичны параметрам для функции
        // 'Выдать заданное повторение поля' – &uf('A')
        //
        // Примеры:
        // (/&unifor('1*R; ?v910^h#1'))
        //

        public static void GetElement
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (string.IsNullOrEmpty(expression)
                || ReferenceEquals(context.Record, null))
            {
                return;
            }

            TextNavigator navigator = new TextNavigator(expression);

            int index = context.Index;
            if (navigator.PeekChar() == '*')
            {
                navigator.ReadChar();
            }
            else
            {
                string indexText = navigator.ReadInteger();
                if (string.IsNullOrEmpty(indexText))
                {
                    return;
                }
                if (!NumericUtility.TryParseInt32(indexText, out index))
                {
                    return;
                }
            }

            char mode = navigator.ReadChar();
            char left = navigator.ReadChar();
            char right = navigator.ReadChar();
            char question = navigator.ReadChar();
            if (question != '?')
            {
                return;
            }

            char command = navigator.ReadChar();
            if (command != 'v'
                && command != 'V')
            {
                return;
            }

            string tagText = navigator.ReadInteger();
            if (string.IsNullOrEmpty(tagText))
            {
                return;
            }
            int tag = NumericUtility.ParseInt32(tagText);

            char code = SubField.NoCode;
            if (navigator.PeekChar() == '^')
            {
                navigator.ReadChar();
                code = navigator.ReadChar();
                if (code == SubField.NoCode)
                {
                    return;
                }
            }

            int offset = 0, length = 0;
            if (navigator.PeekChar() == '*')
            {
                navigator.ReadChar();
                string offsetText = navigator.ReadInteger();
                if (!NumericUtility.TryParseInt32(offsetText, out offset))
                {
                    return;
                }
            }
            if (navigator.PeekChar() == '.')
            {
                navigator.ReadChar();
                string lengthText = navigator.ReadInteger();
                if (!NumericUtility.TryParseInt32(lengthText, out length))
                {
                    return;
                }
            }

            int repeat = 0;
            if (navigator.PeekChar() == '#')
            {
                navigator.ReadChar();
                if (navigator.PeekChar() == '*')
                {
                    repeat = context.Index;
                }
                else
                {
                    string repeatText = navigator.ReadInteger();
                    if (!NumericUtility.TryParseInt32(repeatText, out repeat))
                    {
                        return;
                    }
                }
            }
            repeat = repeat - 1;

            RecordField field = context.Record
                .Fields.GetField(tag, repeat);
            if (ReferenceEquals(field, null))
            {
                return;
            }

            string text = code == SubField.NoCode
                ? field.ToText()
                : field.GetFirstSubFieldValue(code);
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (offset != 0 || length != 0)
            {
                text = PftUtility.SafeSubString(text, offset, length);
            }
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            string output = null;

            string[] items = text.Split(new[] {left, right});
            if (mode == 'R' || mode == 'r')
            {
                output = items.GetOccurrence(index);
            }
            else if (mode == 'L' || mode == 'l')
            {
                output = items.GetOccurrence(index);
            }
            else
            {
                // not implemented
            }

            if (!string.IsNullOrEmpty(output))
            {
                context.Write(node, output);
                context.OutputFlag = true;

                if (!ReferenceEquals(context._vMonitor, null))
                {
                    context._vMonitor.Output = true;
                }
            }
        }

        #endregion
    }
}
