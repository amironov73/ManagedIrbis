// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusI.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Формирование ссылки (гиперссылки) – &uf('+I…
    // Вид функции: +I.
    // Назначение: Формирование ссылки(гиперссылки).
    // Присутствует в версиях ИРБИС с 2004.1.
    // Формат(передаваемая строка):
    // +I/содержание_ссылки/название_ссылки
    // /содержание_ссылки/ – внутренние данные ссылки,
    // ограниченные уникальным разделителем. Имеют структуру
    // NNN,HHH,<параметры>
    // где:
    // NNN – тип ссылки, возможные значения:
    // 0 – ссылки на внешние объекты;
    // 1 – ссылка на связанный документ(возможно в другой БД)
    // – "от одного к одному";
    // 2 – ссылка на связанные документы(возможно в другой БД)
    // – "от одного к многим";
    // HHH – экранная подсказка, может иметь вид:
    // @iii – где iii – номер текста в файле IRBISMSG.TXT,
    // \text\ – собственно текст подсказки в уникальных ограничителях.
    // <параметры>:
    // Для ссылок типа 0 представляет собой в общем виде
    // URL внешнего объекта (в том числе – полный путь на файл).
    // Для ссылок типа 1 и 2 <параметры> имеют следующую структуру:
    // имя_БД,имя_формата,termin
    // где:
    // имя_БД – имя базы данных, из которой будут браться связанные
    // документы; по умолчанию используется текущая БД.
    // имя_формата – имя формата, в соответствии с которым будут
    // расформатироваться связанные документы.
    // (по умолчанию используется оптимизированный формат).
    // termin – ключевой термин, на основе которого отбираются (ищутся)
    // связанные документы.
    //
    // Примеры:
    // &unifor('+I?0,,'v951^i'?', v951^t,|INTERNET|n951^t)
    // &unifor(|+I?1,,,, I=| v421 ^ w |?|, v421^a)
    //

    static class UniforPlusI
    {
        #region Public methods

        public static void BuildLink
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            // TODO implement properly

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            TextNavigator navigator = new TextNavigator(expression);
            char delimiter = navigator.ReadChar();
            string firstPart = navigator.ReadUntil(delimiter);
            if (string.IsNullOrEmpty(firstPart)
                || navigator.ReadChar() != delimiter)
            {
                return;
            }

            string title = navigator.GetRemainingText();

            string output = string.Format
                (
                    "</>{{\v {0}}}{1}{{\v}}",
                    firstPart,
                    title
                );
            context.WriteAndSetFlag(node, output);
        }

        #endregion
    }
}
