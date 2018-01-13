// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusU.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using AM;
using AM.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    //
    // Неописанная функция unifor('+U')
    // Повторяет строку указанное количество раз.
    // Параметры: КОЛИЧЕСТВО#СТРОКА
    //

    static class UniforPlusU
    {
        #region Public methods

        /// <summary>
        /// ibatrak Повторяет строку указанное количество раз.
        /// </summary>
        public static void RepeatString
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
                    CommonSeparators.NumberSign,
                    2
                );
            if (parts.Length != 2
                || string.IsNullOrEmpty(parts[0])
                || string.IsNullOrEmpty(parts[1]))
            {
                return;
            }

            int count = parts[0].SafeToInt32();
            if (count <= 0)
            {
                return;
            }

            string text = parts[1];
            StringBuilder output = new StringBuilder(count * text.Length);
            while (count > 0)
            {
                output.Append(text);
                count--;
            }
            context.WriteAndSetFlag(node, output.ToString());
        }

        #endregion
    }
}
