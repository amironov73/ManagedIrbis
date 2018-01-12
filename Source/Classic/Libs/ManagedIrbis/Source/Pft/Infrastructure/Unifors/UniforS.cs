// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforS.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Универсальный счетчик – &uf('S')
    // Вид функции: S.
    //
    // Назначение: Универсальный счетчик.
    //
    // Формат (передаваемая строка):
    //
    // SN
    // где:
    //
    // N=0 - обнулить счетчик.
    // N=1..9 – увеличить значение счетчика на со-отв. значение.
    // N=A – вернуть значение счетчика – арабскими цифрами.
    // N=X – вернуть значение счетчика – римскими цифрами.
    //
    // Примеры:
    //
    // &unifor('S0')
    // &unifor('S1')
    // &unifor('SA')
    //

    static class UniforS
    {
        #region Private members

        /// <summary>
        /// Borrowed from:
        /// http://stackoverflow.com/questions/7040289/converting-integers-to-roman-numerals
        /// </summary>
        internal static string ToRoman(int number)
        {
            if (number < 0 || number > 3999)
            {
                return string.Empty;
            }

            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);

            return "I" + ToRoman(number - 1);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// &amp;uf('S1') -увеличить значение счётчика
        /// на соответствующее значение.
        /// </summary>
        public static void Add
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                string text = navigator.ReadInteger();
                int delta;
                if (NumericUtility.TryParseInt32(text, out delta))
                {
                    //ibatrak при значении 0 - сбросить счетчик
                    if (delta == 0)
                    {
                        context.UniversalCounter = 0;
                    }
                    else
                    {
                        context.UniversalCounter += delta;
                    }
                }
                char c = navigator.ReadChar();
                if (c == 'A' || c == 'a')
                {
                    Arabic(context, node, expression);
                }
                if (c == 'X' || c == 'x')
                {
                    Roman(context, node, expression);
                }
            }
        }

        /// <summary>
        /// &amp;uf('SA') - напечатать значение счётчика арабскими цифрами.
        /// </summary>
        public static void Arabic
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            string output = context.UniversalCounter.ToInvariantString();
            context.WriteAndSetFlag(node, output);
        }

        /// <summary>
        /// &amp;uf('S0') - обнулить счётчик.
        /// </summary>
        public static void Clear
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            context.UniversalCounter = 0;
        }

        /// <summary>
        /// &amp;uf('SX') - напечатать значение счётчика римскими цифрами.
        /// </summary>
        public static void Roman
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            //
            // Attention: original IRBIS ignores &uf('SX')
            //

            string output = ToRoman(context.UniversalCounter);
            context.WriteAndSetFlag(node, output);
        }

        #endregion
    }
}
