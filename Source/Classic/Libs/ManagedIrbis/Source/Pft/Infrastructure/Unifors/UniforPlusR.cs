// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusR.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

 #region Using directives

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Усекает строку справа до точки.
    // Используется для отсечения одного уровня в рубриках ГРНТИ – &uf('+R')
    // Вид функции: +R.
    // Назначение: Усечение кода рубрики в рубрикаторе ГРНТИ до вышестоящего.
    // Формат (передаваемая строка):
    // +R<строка>
    //
    // Пример:
    // формат
    // &unifor("+R"v3)
    // исходная строка
    // 02.61.45
    // результирующая строка
    // 02.61
    //

    static class UniforPlusR
    {
        #region Public methods

        /// <summary>
        /// Trim text at last dot.
        /// </summary>
        public static void TrimAtLastDot
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                int position = expression.LastIndexOf('.');
                if (position >= 0)
                {
                    string output = expression.Substring(0, position);
                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        #endregion
    }
}
