// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforL.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;
using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Search;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть окончание термина – &uf('L
    // Вид функции: L.
    // Назначение: Вернуть окончание термина.
    // Формат (передаваемая строка):
    // L<начало_термина>
    //
    // Пример:
    //
    // &unifor('L', 'JAZ=', 'рус')
    //
    // выдаст 'СКИЙ'
    //

    static class UniforL
    {
        #region Public methods

        public static void ContinueTerm
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

            expression = StringUtility.ToUpperInvariant(expression);

            IrbisProvider provider = context.Provider;
            TermParameters parameters = new TermParameters
            {
                Database = provider.Database,
                StartTerm = expression,
                NumberOfTerms = 10
            };

            TermInfo[] terms;
            try
            {
                terms = provider.ReadTerms(parameters);
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "UniforL::ContinueTerm",
                        exception
                    );

                return;
            }

            if (terms.Length == 0)
            {
                return;
            }

            string firstTerm = terms[0].Text;
            if (string.IsNullOrEmpty(firstTerm))
            {
                return;
            }

            if (!firstTerm.StartsWith(expression))
            {
                return;
            }

            string result = firstTerm.Substring(expression.Length);
            if (!string.IsNullOrEmpty(result))
            {
                context.Write(node, result);
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
