// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforR.cs -- 
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
    // Генерация случайного числа – &uf('R
    // Вид функции: R.
    // Назначение: Генерация случайного числа.
    // Формат (передаваемая строка):
    // RNN
    // где NN – кол-во знаков в случайном числе (по умолчанию – 6).
    //
    // Примеры:
    //
    // &unifor('R')
    // &unifor('R4')
    //

    static class UniforR
    {
        #region Public methods

        /// <summary>
        /// Generate random number.
        /// </summary>
        public static void RandomNumber
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            int length = 6;
            if (!string.IsNullOrEmpty(expression))
            {
                NumericUtility.TryParseInt32(expression, out length);
            }
            if (length <= 0 || length > 9)
            {
                return;
            }

            int maxValue = 1;
            for (int i = 0; i < length; i++)
            {
                maxValue = maxValue * 10;
            }

            int result = context.Provider
                .PlatformAbstraction
                .GetRandomGenerator()
                .Next(maxValue);
            string format = new string('0', length);
            string output = result.ToString(format);
            context.Write(node, output);
            context.OutputFlag = true;
        }

        #endregion
    }
}
