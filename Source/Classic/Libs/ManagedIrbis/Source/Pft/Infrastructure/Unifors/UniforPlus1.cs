// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus1.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AM;
using AM.Collections;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Устаревшие функции для работы с глобальными переменными
    //

    static class UniforPlus1
    {
        #region Private members

        private static readonly char[] _comma = { ',' };
        private static readonly char[] _numberSign = { '#' };
        private static readonly char[] _verticalLine = { '|' };

        // ================================================================

        private static string _GetGlobal
            (
                [NotNull] PftContext context,
                int index
            )
        {
            RecordField[] fields = context.Globals.Get(index);
            StringBuilder result = new StringBuilder();
            bool first = true;
            foreach (RecordField field in fields)
            {
                if (!first)
                {
                    result.AppendLine();
                }
                first = false;
                result.Append(field.ToText());
            }

            return result.ToString();
        }

        // ================================================================

        [NotNull]
        public static List<string> _GetGlobals
            (
                [NotNull] PftContext context,
                int index,
                int count
            )
        {
            if (count <= 0)
            {
                return new List<string>();
            }

            List<string> result = new List<string>(count);
            while (count > 0)
            {
                if (context.Globals.HaveVariable(index))
                {
                    string item = _GetGlobal(context, index)
                        ?? string.Empty;
                    string[] lines = item.SplitLines();
                    result.AddRange(lines);
                }
                count--;
                index++;
            }

            return result;
        }

        // ================================================================

        /// <summary>
        /// Parse NNN,nnn
        /// </summary>
        private static int[] _ParseOne
            (
                [NotNull] PftContext context,
                [CanBeNull] string expression
            )
        {
            int[] result = null;
            if (string.IsNullOrEmpty(expression))
            {
                goto DONE;
            }

            string[] parts = StringUtility.SplitString
                (
                    expression,
                    _comma,
                    2
                );

            int index;
            string specification = parts[0];
            if (specification.StartsWith("*"))
            {
                specification = specification.Substring(1);
                if (specification.StartsWith("+"))
                {
                    specification = specification.Substring(1);
                }
                if (NumericUtility.TryParseInt32(specification, out index))
                {
                    index += context.Index + 1;
                }
                else
                {
                    index = context.Index + 1;
                }
            }
            else
            {
                if (!NumericUtility.TryParseInt32(specification, out index))
                {
                    goto DONE;
                }
            }

            int count = 1;
            if (parts.Length == 2)
            {
                count = parts[1].SafeToInt32(count);
            }

            result = new[]
            {
                index,
                count
            };

            DONE:
            return result;
        }

        // ================================================================

        /// <summary>
        /// Parse NNN,nnn#MMM,mmm
        /// </summary>
        private static int[] _ParsePair
            (
                [CanBeNull] string expression
            )
        {
            int[] result = null;
            if (string.IsNullOrEmpty(expression))
            {
                goto DONE;
            }

            string[] parts = StringUtility.SplitString
                (
                    expression,
                    _numberSign,
                    2
                );
            if (parts.Length != 2)
            {
                goto DONE;
            }
            string[] subs = StringUtility.SplitString
                (
                    parts[0],
                    _comma,
                    2
                );
            int firstIndex;
            if (!NumericUtility.TryParseInt32(subs[0], out firstIndex))
            {
                goto DONE;
            }
            int firstCount = 1;
            if (subs.Length == 2)
            {
                firstCount = subs[1].SafeToInt32(firstCount);
            }
            subs = StringUtility.SplitString
                (
                    parts[1],
                    _comma,
                    2
                );
            int secondIndex;
            if (!NumericUtility.TryParseInt32(subs[0], out secondIndex))
            {
                goto DONE;
            }
            int secondCount = 1;
            if (subs.Length == 2)
            {
                secondCount = subs[1].SafeToInt32(secondCount);
            }

            result = new[]
            {
                firstIndex,
                firstCount,
                secondIndex,
                secondCount
            };

            DONE:
            return result;
        }

        // ================================================================

        /// <summary>
        /// Удаляем пустые строки в конце списка.
        /// </summary>
        private static void _RemoveEmptyTailLines
            (
                [NotNull] List<string> list
            )
        {
            while (list.Count != 0)
            {
                // Удаляем пустые строки в конце
                int offset = list.Count - 1;
                if (string.IsNullOrEmpty(list[offset]))
                {
                    list.RemoveAt(offset);
                }
                else
                {
                    break;
                }
            }
        }

        #endregion

        #region Public methods

        // ================================================================

        //
        // Сложение двух списков (групп переменных) – &uf('+1A
        // Вид функции: +1A.
        // Назначение: Сложение двух списков(групп переменных).
        // Формат(передаваемая строка):
        // +1ANNN,nnn#MMM,mmm
        // где:
        // NNN,MMM – номер первой или единственной переменной.
        // nnn,mmm – кол-во переменных(по умолчанию 1).
        //

        public static void AddGlobals
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            int[] pair = _ParsePair(expression);
            if (ReferenceEquals(pair, null))
            {
                return;
            }

            List<string> first = _GetGlobals(context, pair[0], pair[1]);
            List<string> second = _GetGlobals(context, pair[2], pair[3]);
            if (first.Count == 0
                && second.Count == 0)
            {
                return;
            }

            bool flag = true;
            IEqualityComparer<string> comparer
                = StringUtility.GetCaseInsensitiveComparer();
            foreach (string item in second)
            {
                if (!flag)
                {
                    context.WriteLine(node);
                }
                context.Write(node, item);
                context.OutputFlag = true;
                flag = false;
            }
            foreach (string item in first)
            {
                if (!second.Contains(item, comparer))
                {
                    if (!flag)
                    {
                        context.WriteLine(node);
                    }
                    context.Write(node, item);
                    context.OutputFlag = true;
                    flag = false;
                }
            }
        }

        // ================================================================

        //
        // Очистить(опустошить) все глобальные переменные – &uf('+1
        // Вид функции: +1.
        // Назначение: Очистить (опустошить) все глобальные переменные.
        // Формат (передаваемая строка):
        // +1
        //
        // Пример:
        //
        // &unifor('+1')
        //

        public static void ClearGlobals
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            context.Globals.Clear();
        }


        // ================================================================

        //
        // Групповая мультираскодировка списка
        // Формат:
        // +1O<MNU>|SSSS
        // где:
        // <MNU> - имя справочника(с расширением);
        // SSSS - список строк(результат расформатирования
        //
        // Пример:
        //
        // &unifor(‘+1Omhr.mnu|’,(v910^m/))
        //

        public static void DecodeList
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

            string[] parts = StringUtility.SplitString
                (
                    expression,
                    _verticalLine,
                    2
                );
            if (parts.Length != 2)
            {
                return;
            }

            string menuName = parts[0];
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    context.Provider.Database,
                    menuName
                );
            MenuFile menu = context.Provider.ReadMenuFile(specification);
            if (ReferenceEquals(menu, null))
            {
                return;
            }
            if (menu.Entries.Count == 0)
            {
                return;
            }

            List<string> lines = new List<string>(parts[1].SplitLines());
            _RemoveEmptyTailLines(lines);

            if (lines.Count == 0)
            {
                return;
            }

            bool first = true;
            foreach (string line in lines)
            {
                if (!first)
                {
                    context.WriteLine(node);
                }

                var value = menu.GetStringSensitive(line);
                context.Write(node, value);
                context.OutputFlag = true;

                first = false;
            }
        }

        // ================================================================

        //
        // Групповая мультираскодировка переменных
        // Формат:
        // +1K<MNU>|NNN,nnn
        // где:
        // <MNU> - имя справочника(с расширением);
        // NNN – номер первой или единственной переменной,
        // возможна конструкция *+-<число>. * – номер текущего повторения
        // в повторяющейся группе.
        // nnn – кол-во переменных (по умолчанию 1).
        //
        // Пример:
        //
        // &unifor(‘+1Kmhr.mnu|100,10’)
        //

        public static void DecodeGlobals
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

            string[] parts = StringUtility.SplitString
                (
                    expression,
                    _verticalLine,
                    2
                );
            if (parts.Length != 2)
            {
                return;
            }

            string menuName = parts[0];
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    context.Provider.Database,
                    menuName
                );
            MenuFile menu = context.Provider.ReadMenuFile(specification);
            if (ReferenceEquals(menu, null))
            {
                return;
            }
            if (menu.Entries.Count == 0)
            {
                return;
            }

            int[] one = _ParseOne(context, parts[1]);
            if (ReferenceEquals(one, null))
            {
                return;
            }
            int index = one[0];
            int count = one[1];

            List<string> lines = _GetGlobals(context, index, count);

            _RemoveEmptyTailLines(lines);
            if (lines.Count == 0)
            {
                return;
            }

            var first = true;
            foreach (string line in lines)
            {
                if (!first)
                {
                    context.WriteLine(node);
                }

                var value = menu.GetStringSensitive(line);
                context.Write(node, value);
                context.OutputFlag = true;

                first = false;
            }
        }

        // ================================================================

        //
        // Исключение неоригинальных значений из группы переменных – &uf('+1G
        // Вид функции: +1G.
        // Назначение: Исключение неоригинальных значений из группы переменных.
        // Формат(передаваемая строка):
        // +1GNNN,nnn
        // где:
        // NNN – номер первой или единственной переменной.
        // nnn – кол-во переменных(по умолчанию 1).
        //

        public static void DistinctGlobals
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            int[] one = _ParseOne(context, expression);
            if (ReferenceEquals(one, null))
            {
                return;
            }
            int index = one[0];
            int count = one[1];

            List<string> list = new List<string>(count);
            CaseInsensitiveDictionary<object> dictionary
                = new CaseInsensitiveDictionary<object>();
            while (count > 0)
            {
                if (context.Globals.HaveVariable(index))
                {
                    string item = _GetGlobal(context, index)
                        ?? string.Empty;
                    if (!dictionary.ContainsKey(item))
                    {
                        list.Add(item);
                        dictionary.Add(item, null);
                    }
                }
                count--;
                index++;
            }

            _RemoveEmptyTailLines(list);
            if (list.Count == 0)
            {
                return;
            }

            foreach (string line in list)
            {
                if (string.IsNullOrEmpty(line))
                {
                    context.WriteLine(node);
                }
                else
                {
                    context.WriteLine(node, line);
                }
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // +1I
        // Исключение неоригинальных значений из списка
        // Формат:
        // +1ISSSS
        //

        public static void DistinctList
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

            string[] original = expression.SplitLines();
            if (original.Length == 0)
            {
                return;
            }

            CaseInsensitiveDictionary<object> dictionary
                = new CaseInsensitiveDictionary<object>();

            foreach (string line in original)
            {
                string copy = line ?? string.Empty;
                if (!dictionary.ContainsKey(copy))
                {
                    dictionary.Add(copy, null);
                    if (string.IsNullOrEmpty(line))
                    {
                        context.WriteLine(node);
                    }
                    else
                    {
                        context.WriteLine(node, line);
                    }
                    context.OutputFlag = true;
                }
            }
        }

        // ================================================================

        //
        // Перемножение двух списков (групп переменных) – &uf('+1M
        // Вид функции: +1M.
        // Назначение: Перемножение двух списков(групп переменных).
        // Формат(передаваемая строка):
        // +1MNNN,nnn#MMM,mmm
        // где:
        // NNN,MMM – номер первой или единственной переменной.
        // nnn,mmm – кол-во переменных(по умолчанию 1).
        //

        public static void MultiplyGlobals
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            int[] pair = _ParsePair(expression);
            if (ReferenceEquals(pair, null))
            {
                return;
            }

            List<string> first = _GetGlobals(context, pair[0], pair[1]);
            List<string> second = _GetGlobals(context, pair[2], pair[3]);
            if (first.Count == 0
                || second.Count == 0)
            {
                return;
            }

            bool flag = true;
            IEqualityComparer<string> comparer
                = StringUtility.GetCaseInsensitiveComparer();
            foreach (string item in first)
            {
                if (second.Contains(item, comparer))
                {
                    if (!flag)
                    {
                        context.WriteLine(node);
                    }
                    context.Write(node, item);
                    context.OutputFlag = true;
                    flag = false;
                }
            }
        }

        // ================================================================

        //
        // Чтение глобальных переменных – &uf('+1R
        // Вид функции: +1R.
        // Назначение: Чтение глобальных переменных.
        // Присутствует в версиях ИРБИС с 2004.1.
        // Формат (передаваемая строка):
        // +1RNNN,nnn
        // где:
        // NNN – номер первой или единственной переменной,
        // возможна конструкция *+-<число>. * – номер текущего повторения
        // в повторяющейся группе.
        // nnn – кол-во переменных (по умолчанию 1).
        //
        // Пример:
        //
        // &unifor('+1R100,2')
        //

        public static void ReadGlobal
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            int[] one = _ParseOne(context, expression);
            if (ReferenceEquals(one, null))
            {
                return;
            }

            int index = one[0];
            int count = one[1];
            List<string> lines = _GetGlobals(context, index, count);
            _RemoveEmptyTailLines(lines);

            bool flag = false;
            foreach (string line in lines)
            {
                if (flag)
                {
                    context.WriteLine(node);
                }
                context.Write(node, line);
                context.OutputFlag = true;
                flag = true;
            }
        }

        // ================================================================

        //
        // Сортировка группы переменных – &uf('+1T
        // Вид функции: +1T.
        // Назначение: Сортировка группы переменных.
        // Формат (передаваемая строка):
        // +1TNNN,nnn
        // где:
        // NNN – номер первой или единственной переменной,
        // nnn – кол-во переменных (по умолчанию 1).
        //
        // Пример:
        //
        // &unifor('+1T100,4')
        //

        public static void SortGlobals
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            int[] one = _ParseOne(context, expression);
            if (ReferenceEquals(one, null))
            {
                return;
            }

            int index = one[0];
            int count = one[1];
            List<string> lines = _GetGlobals(context, index, count);
            _RemoveEmptyTailLines(lines);
            if (lines.Count == 0)
            {
                return;
            }

            lines.Sort(StringComparer.OrdinalIgnoreCase);

            foreach (string line in lines)
            {
                context.WriteLine(node, line);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // +1V
        // Сортировка списка
        // Формат:
        // +1VSSSS
        //

        public static void SortList
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

            string[] lines = expression.SplitLines();
            Array.Sort
                (
                    lines,
                    StringComparer.OrdinalIgnoreCase
                );

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    context.WriteLine(node);
                }
                else
                {
                    context.WriteLine(node, line);
                }
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // Вычитание списков (групп переменных) – &uf('+1S
        // Вид функции: +1S.
        // Назначение: Вычитание списков(групп переменных).
        // Формат(передаваемая строка):
        // +1SNNN,nnn#MMM,mmm
        // где:
        // NNN,MMM – номер первой или единственной переменной.
        // nnn,mmm – кол-во переменных(по умолчанию 1).
        //

        public static void SubstractGlobals
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            int[] pair = _ParsePair(expression);
            if (ReferenceEquals(pair, null))
            {
                return;
            }

            List<string> first = _GetGlobals(context, pair[0], pair[1]);
            List<string> second = _GetGlobals(context, pair[2], pair[3]);
            if (first.Count == 0)
            {
                return;
            }

            bool flag = true;
            IEqualityComparer<string> comparer
                = StringUtility.GetCaseInsensitiveComparer();
            foreach (string item in first)
            {
                if (!second.Contains(item, comparer))
                {
                    if (!flag)
                    {
                        context.WriteLine(node);
                    }
                    context.Write(node, item);
                    context.OutputFlag = true;
                    flag = false;
                }
            }
        }

        // ================================================================

        //
        // Запись в глобальные переменные – &uf('+1W
        // Вид функции: +1W.
        // Назначение: Запись в глобальные переменные.
        // Присутствует в версиях ИРБИС с 2004.1.
        // Формат (передаваемая строка):
        // +1WNNN,MMM#SSSS
        // где:
        // NNN – номер первой или единственной переменной,
        // возможна конструкция *+-<число>. * – номер текущего повторения
        // в повторяющейся группе.
        // MMM – номер переменной для сохранения кол-ва записанных переменных
        // (по умолчанию не используется).
        // SSSS – список строк(результат расформатирования).
        // Если задан MMM – каждая строка пишется в отдельную переменную,
        // в противном случае все пишется в одну переменную.
        //
        // Пример:
        //
        // &unifor('+1W100,0#',(v910/))
        //

        public static void WriteGlobal
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

            string[] parts = StringUtility.SplitString
                    (
                        expression,
                        _numberSign,
                        2
                    );

            string indexText = parts[0];
            string valueText = string.Empty;
            if (parts.Length == 2)
            {
                valueText = parts[1] ?? string.Empty;
            }

            parts = StringUtility.SplitString
                (
                    indexText,
                    _comma,
                    2
                );
            indexText = parts[0];

            bool useCount = false;
            int countVariable = 0;
            if (parts.Length == 2)
            {
                if (!NumericUtility.TryParseInt32
                    (
                        parts[1],
                        out countVariable
                    ))
                {
                    return;
                }
                useCount = true;
            }

            int index;
            if (!NumericUtility.TryParseInt32(indexText, out index))
            {
                return;
            }

            if (useCount)
            {
                string[] lines = valueText.SplitLines();
                foreach (string line in lines)
                {
                    context.Globals.Add(index, line);
                    index++;
                }

                context.Globals.Add
                    (
                        countVariable,
                        lines.Length.ToInvariantString()
                    );
            }
            else
            {
                context.Globals.Add(index, valueText);
            }
        }

        // ================================================================

        #endregion
    }
}
