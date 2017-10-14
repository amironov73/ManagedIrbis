// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforC.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Identifiers;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Контроль ISSN/ISBN – &uf('C
    // Вид функции: C.
    // Назначение: Контроль ISSN/ISBN.Возвращаемое значение:
    // 0 - при положительном результате, 1 - при отрицательном.
    // Формат (передаваемая строка):
    // С<ISSN/ISBN>
    // Примеры:
    // &unifor("C"v10^a)
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    static class UniforC
    {
        #region Public methods

        /// <summary>
        /// Контроль ISSN/ISBN.
        /// Возвращаемое значение: 0 – при положительном
        /// результате, 1 – при отрицательном.
        /// </summary>
        public static void CheckIsbn
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expresion
            )
        {
            Code.NotNull(context, "context");

            string output = "1";

            if (!string.IsNullOrEmpty(expresion))
            {
                List<char> digits = new List<char>(expresion.Length);
                foreach (char c in expresion)
                {
                    if (PftUtility.DigitsX.Contains(c))
                    {
                        digits.Add(c);
                    }
                }
                if (digits.Count == 8)
                {
                    if (Issn.CheckControlDigit(expresion))
                    {
                        output = "0";
                    }
                }
                else if (digits.Count == 10)
                {
                    if (Isbn.Validate(expresion, false))
                    {
                        output = "0";
                    }
                }
            }

            context.Write(node, output);
            context.OutputFlag = true;
        }

        #endregion
    }
}
