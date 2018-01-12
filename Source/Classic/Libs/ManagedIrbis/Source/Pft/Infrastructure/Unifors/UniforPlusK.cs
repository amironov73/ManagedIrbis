// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforK.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Collections;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak неописанный unifor('+K')
    // Похож на unifor K, перебирает файл mnu.
    // В отличие от unifor K ищет по значениям элементов
    // максимально близкое значение и возвращает ключ.
    // В качестве разделителя может быть символ вертикальной черты | и обратный слэш \
    // Оба ведут себя одинаково.
    // Функция корректно работает только с упорядоченным списком.
    // В документе ftp://www.library.tomsk.ru/pub/IRBIS/unifor+.doc
    // утверждается, что unifor +K для авторского знака.
    // В принципе, возможно и так,
    // если подсунуть mnu с авторской таблицей, то будет работать.
    //

    static class UniforPlusK
    {
        #region Public methods

        /// <summary>
        /// Get Author Sign
        /// </summary>
        public static void GetAuthorSign
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
                string menuName = navigator.ReadUntil('\\', '!');
                if (string.IsNullOrEmpty(menuName))
                {
                    return;
                }

                char separator = navigator.ReadChar();
                if (separator != '\\'
                    && separator != '!')
                {
                    return;
                }

                string text = navigator.GetRemainingText();
                if (string.IsNullOrEmpty(text))
                {
                    return;
                }

                // Немного магии от разработчиков ИРБИС
                if (text.StartsWith("Ы") || text.StartsWith("ы"))
                {
                    context.WriteAndSetFlag(node, "Ы");

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
                    // Алгоритм подсмотрен в раскомпилированном коде
                    // Код гуляет по пунктам меню,
                    // как только результат сравнения строки меньше нуля,
                    // остановка цикла.
                    // Сравнение без учета регистра
                    // Точнее в irbis64.dll делается приведение
                    // к верхнему регистру, но в C# можно сравнивать и так

                    string output = null;
                    MenuEntry entry = null;
                    StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                    NonNullCollection<MenuEntry> entries = menu.Entries;
                    for (int i = 0; i < entries.Count; i++)
                    {
                        if (comparer.Compare(text, entries[i].Comment) < 0)
                        {
                            break;
                        }

                        entry = entries[i];
                    }

                    if (!ReferenceEquals(entry, null))
                    {
                        output = entry.Code;
                    }

                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        #endregion
    }
}
