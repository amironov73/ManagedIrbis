// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus1.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using AM;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;
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

        #endregion

        #region Public methods

        // ================================================================

        //
        // Очистить(опустошить) все глобальные переменные – &uf('+1…
        // Вид функции: +1.
        // Назначение: Очистить (опустошить) все глобальные переменные.
        // Формат (передаваемая строка):
        // +1
        // Пример:
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
        // Пример:
        // &unifor(‘+1Omhr.mnu|’,(v910^m/))

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
            while (lines.Count != 0)
            {
                // Удаляем пустые строки в конце
                int offset = lines.Count - 1;
                if (string.IsNullOrEmpty(lines[offset]))
                {
                    lines.RemoveAt(offset);
                }
                else
                {
                    break;
                }
            }

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
        // Чтение глобальных переменных – &uf('+1R…
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
        // Примеры:
        // &unifor('+1R100,2')
        //

        public static void ReadGlobal
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = StringUtility.SplitString
                    (
                        expression,
                        _comma,
                        2
                    );

                string indexText = parts[0];
                int index;
                string countText = null;
                if (parts.Length == 2)
                {
                    countText = parts[1];
                }
                if (NumericUtility.TryParseInt32(indexText, out index))
                {
                    int count = 1;
                    if (!string.IsNullOrEmpty(countText))
                    {
                        NumericUtility.TryParseInt32(countText, out count);
                    }

                    bool flag = false;
                    while (count != 0)
                    {
                        if (flag)
                        {
                            context.WriteLine(node);
                        }

                        flag = context.Globals.HaveVariable(index);
                        RecordField[] fields = context.Globals.Get(index);
                        StringBuilder output = new StringBuilder();
                        bool first = true;
                        foreach (RecordField field in fields)
                        {
                            if (!first)
                            {
                                output.AppendLine();
                            }
                            first = false;
                            output.Append(field.ToText());
                        }
                        if (output.Length != 0)
                        {
                            context.Write(node, output.ToString());
                            context.OutputFlag = true;
                        }

                        index++;
                        count--;
                    }
                }
            }
        }

        // ================================================================

        //
        // Запись в глобальные переменные – &uf('+1W…
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
        // Примеры:
        // &unifor('+1W100,0#',(v910/))
        //

        public static void WriteGlobal
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
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
                    valueText = parts[1];
                }

                int index;
                if (NumericUtility.TryParseInt32(indexText, out index))
                {
                    context.Globals.Add(index, valueText);
                }
            }
        }

        // ================================================================



        #endregion
    }
}
