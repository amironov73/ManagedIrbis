// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusV.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    //
    // Неописанная функция unifor('+V')
    //
    // Выводит подстроку указанной длины.
    // Параметры: ДЛИНА#СТРОКА
    //

    static class UniforPlusV
    {
        #region Public methods

        /// <summary>
        /// ibatrak Выводит подстроку указанной длины
        /// </summary>
        public static void Substring
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

            char[] separators = {'#'};
            string[] parts = StringUtility.SplitString(expression, separators, 2);
            if (parts.Length != 2
               || string.IsNullOrEmpty(parts[1]))
            {
                return;
            }

            int length = parts[0].SafeToInt32();
            if (length <= 0)
            {
                return;
            }

            string output = parts[1].SafeSubstring(0, length);
            context.Write(node, output);
            context.OutputFlag = true;
        }

        #endregion
    }
}
