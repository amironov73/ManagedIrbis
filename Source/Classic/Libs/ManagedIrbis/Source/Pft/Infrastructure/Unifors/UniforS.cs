/* UniforS.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforS
    {
        #region Private members

        /// <summary>
        /// Borrowed from: http://stackoverflow.com/questions/7040289/converting-integers-to-roman-numerals
        /// </summary>
        internal static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999))
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
            if (number >= 1) return "I" + ToRoman(number - 1);

            throw new ArgumentOutOfRangeException("something bad happened");
        }
        #endregion

        #region Public methods

        public static void Add
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                string text = navigator.ReadInteger();
                int delta;
                if (int.TryParse(text, out delta))
                {
                    context.UniversalCounter += delta;
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

        public static void Arabic
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            string output = context.UniversalCounter.ToInvariantString();
            context.Write(node, output);
            context.OutputFlag = true;
        }

        public static void Clear
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            context.UniversalCounter = 0;
        }

        public static void Roman
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            //
            // Attention: original IRBIS ignores &uf('SX')
            //

            string output = ToRoman(context.UniversalCounter);
            if (!string.IsNullOrEmpty(output))
            {
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
