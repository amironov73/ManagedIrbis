// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor2.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть последний номер MFN в базе + 1 – &uf('2…
    // Вид функции: 2.
    // Назначение: Возвращает последний номер MFN
    // в текущей базе данных, увеличенный на единицу (MAX_MFN) + 1).
    // В общем случае параметр имеет вид 2N,
    // где N - выводимое количество символов, обрезанное
    // до необходимой длинны справа.
    // Формат(передаваемая строка):
    // Пример:
    // &unifor('2'),  &unifor('27')
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class Unifor2
    {
        #region Public methods

        /// <summary>
        /// Вернуть последний номер MFN в базе + 1.
        /// </summary>
        public static void GetMaxMfn
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            Code.NotNull(context, "context");

            int width = 10;
            if (!string.IsNullOrEmpty(expression))
            {
                width = expression.SafeToInt32(0, 0, int.MaxValue);
            }
            if (width == 0 && !expression.ConsistOf('0'))
            {
                width = 10;
            }

            string result = (context.Provider.GetMaxMfn() + 1)
                .ToInvariantString();
            int length = result.Length;
            if (width < length)
            {
                if (width == 0)
                {
                    result = string.Empty;
                }
                else
                {
                    result = result.Substring(0, width);
                }
            }
            else if (width > length)
            {
                result = result.PadLeft(width, '0');
            }

            if (!string.IsNullOrEmpty(result))
            {
                context.Write(node, result);
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
