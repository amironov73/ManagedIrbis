// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus5.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

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
    // т.е.файл с расширением MNU).
    //
    // Пример:
    //
    // (&unifor('+5Tfield.mnu'),' – ',&unifor('+5Ffield.mnu'))
    //

    static class UniforPlus5
    {
        #region Public methods

        public static void GetEntry
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

            TextNavigator navigator = new TextNavigator(expression);
            char command = navigator.ReadChar();
            string menuName = navigator.GetRemainingText();
            if (command == TextNavigator.EOF
                || string.IsNullOrEmpty(menuName))
            {
                return;
            }

            command = CharUtility.ToUpperInvariant(command);
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

            int index = context.Index;
            MenuEntry entry = menu.Entries.GetItem(index);
            if (ReferenceEquals(entry, null))
            {
                return;
            }

            string output = null;

            switch (command)
            {
                case 'T':
                    output = entry.Code;
                    break;

                case 'F':
                    output = entry.Comment;
                    break;

                default:
                    Log.Warn
                        (
                            "UniforPlus5::GetEntry: "
                            + "unknown command="
                            + command.ToVisibleString()
                        );
                    break;
            }

            context.WriteAndSetFlag(node, output);
        }

        #endregion
    }
}
