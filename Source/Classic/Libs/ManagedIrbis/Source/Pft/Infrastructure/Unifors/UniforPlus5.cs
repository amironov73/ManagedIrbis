// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus5.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using AM;
using AM.Logging;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдача элемента списка/справочника в соответствии
    // с индексом (номером повторения) повторяющейся группы – &uf('+5')
    // Вид функции: +5.
    // Назначение: Выдача элемента списка/справочника в соответствии
    // с индексом (номером повторения) повторяющейся группы.
    // Присутствует в версиях ИРБИС с 2005.2.
    // Формат (передаваемая строка):
    // +5Х<имя_справочника/списка>
    // где Х принимает значения: Т – выдать значение;
    // F – выдать пояснение(имеет смысл, если задается справочник,
    // т. е. файл с расширением MNU).
    //
    // Пример:
    //
    // (&unifor('+5Tfield.mnu'),' – ',&unifor('+5Ffield.mnu'))
    //

    //
    // ibatrak
    // unifor +5 парсит *.MNU как меню,
    // а если встречается любой другой файл, выдает просто текстовую строку.
    // Пустая строка означает конец разбора.
    //

    static class UniforPlus5
    {
        #region Public methods

        public static void GetMenuEntry
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            // ibatrak
            // минимальная длина выражения команда + .mnu (имя файла - только расширение) = 5
            if (string.IsNullOrEmpty(expression) || expression.Length < 5)
            {
                return;
            }

            TextNavigator navigator = new TextNavigator(expression);
            char command = CharUtility.ToUpperInvariant(navigator.ReadChar());
            if (command != 'T' && command != 'F')
            {
                Log.Warn
                    (
                        "UniforPlus5::GetMenuEntry: "
                        + "unknown command="
                        + command.ToVisibleString()
                    );

                return;
            }

            string fileName = navigator.GetRemainingText();
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            FileSpecification specification;
            string output = null;
            int index = context.Index;

            string extension = StringUtility.ToUpperInvariant(Path.GetExtension(fileName));
            if (extension != ".MNU")
            {
                specification = new FileSpecification
                    (
                        IrbisPath.System,
                        fileName
                    );
                string text = context.Provider.ReadFile(specification);
                if (!string.IsNullOrEmpty(text))
                {
                    string[] lines = text.SplitLines();
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string value = lines[i];
                        if (string.IsNullOrEmpty(value))
                        {
                            break;
                        }

                        if (i == index)
                        {
                            output = value;
                            break;
                        }
                    }
                }
            }
            else
            {
                // .MNU
                specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        context.Provider.Database,
                        fileName
                    );
                MenuFile menu = context.Provider.ReadMenuFile(specification);
                if (ReferenceEquals(menu, null))
                {
                    return;
                }

                MenuEntry entry = menu.Entries.GetItem(index);
                if (ReferenceEquals(entry, null))
                {
                    return;
                }

                switch (command)
                {
                    case 'T':
                        output = entry.Code;
                        break;

                    case 'F':
                        output = entry.Comment;
                        break;
                }
            }

            context.WriteAndSetFlag(node, output);
        }

        #endregion
    }
}
