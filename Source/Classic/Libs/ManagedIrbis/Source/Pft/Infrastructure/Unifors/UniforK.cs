// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforK.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Раскодировка через справочник – &uf('K')
    // Вид функции: K.
    // Назначение: Возвращает значение из справочника,
    // соответствующее переданному коду
    // (иными словами, осуществляется раскодировка).
    // Формат (передаваемая строка):
    // K<имя_справочника><разделитель><исх_значение>
    // <разделитель> между<имя_справочника>
    // и <исх_значение> может быть двух видов:
    // \ - раскодировка с учетом регистра,
    // ! - раскодировка без учета регистра.
    //
    // Примеры:
    //
    // &unifor("Kjz.mnu\"v101)
    // &uf('kFIO_SF.MNU!'&uf('av907^b#1'))
    //
    // Вместо разделителя \ может использоваться
    // недокументированный разделитель |, он означает то же самое
    //

    static class UniforK
    {
        #region Public methods

        /// <summary>
        /// Get MNU-file entry.
        /// </summary>
        public static void GetMenuEntry
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            Code.NotNull(context, "context");

            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                string menuName = navigator.ReadUntil('\\', '!', '|');
                if (string.IsNullOrEmpty(menuName))
                {
                    return;
                }

                char separator = navigator.ReadChar();
                if (separator != '\\'
                    && separator != '!'
                    && separator != '|')
                {
                    return;
                }

                string key = navigator.GetRemainingText();
                if (string.IsNullOrEmpty(key))
                {
                    return;
                }

                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        context.Provider.Database,
                        menuName
                    );
                MenuFile menu = context.Provider.ReadMenuFile
                    (
                        specification
                    );
                if (!ReferenceEquals(menu, null))
                {
                    string output = null;

                    switch (separator)
                    {
                        case '\\':
                        case '|': // nondocumented but used in scripts
                            output = menu.GetStringSensitive(key);
                            break;

                        case '!':
                            output = menu.GetString(key);
                            break;
                    }

                    if (!string.IsNullOrEmpty(output))
                    {
                        context.Write(node, output);
                        context.OutputFlag = true;
                    }
                }
            }
        }

        #endregion
    }
}
