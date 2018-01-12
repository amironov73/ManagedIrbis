// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforU.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AM;
using AM.Logging;
using AM.Text;
using AM.Text.Ranges;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Кумуляция номеров журналов – &uf('U')
    // Вид функции: U.
    // Назначение: Кумуляция номеров журналов.
    // Формат (передаваемая строка):
    // U<strbase>,<stradd>
    // где:
    // <strbase> – исходная кумулированная строка.
    // <stradd> – кумулируемые номера.
    //
    // Пример:
    //
    // &unifor("U"v909^h",12")
    //

    static class UniforU
    {
        #region Public methods

        /// <summary>
        /// Check whether the issue is present
        /// in cumulated collection.
        /// </summary>
        public static void Check
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = StringUtility.SplitString
                    (
                        expression,
                        CommonSeparators.Comma,
                        2
                    );
                if (parts.Length == 2)
                {
                    string issue = parts[0];
                    string cumulated = parts[1];

                    bool result = Check(issue, cumulated);
                    string output = result ? "1" : "0";
                    context.Write(node, output);
                }
            }
        }

        /// <summary>
        /// Check whether the issue is present
        /// in cumulated collection.
        /// </summary>
        public static bool Check
            (
                [NotNull] string issue,
                [NotNull] string cumulated
            )
        {
            NumberRangeCollection collection = NumberRangeCollection.Parse(cumulated);
            NumberText number = new NumberText(issue);
            bool result = collection.Contains(number);

            return result;
        }

        /// <summary>
        /// Cumulate the magazine issues.
        /// </summary>
        [NotNull]
        public static string Cumulate
            (
                [NotNull] string issues
            )
        {
            try
            {
                NumberRangeCollection collection = NumberRangeCollection.Parse(issues);
                List<NumberText> numbers = collection
                    .Distinct()
                    .ToList();
                NumberRangeCollection result = NumberRangeCollection.Cumulate(numbers);

                return result.ToString();
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "Unifor3::Cumulate",
                        exception
                    );

                return issues;
            }
        }

        /// <summary>
        /// Cumulate the magazine issues.
        /// </summary>
        public static void Cumulate
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = Cumulate(expression);
                context.WriteAndSetFlag(node, output);
            }
        }

        /// <summary>
        /// Decumulate the magazine issues.
        /// </summary>
        [NotNull]
        public static string Decumulate
            (
                [NotNull] string issues
            )
        {
            NumberRangeCollection collection = NumberRangeCollection.Parse(issues);
            StringBuilder result = new StringBuilder();
            bool first = true;
            foreach (NumberText number in collection)
            {
                if (!first)
                {
                    result.Append(',');
                }
                first = false;
                result.Append(number);
            }

            return result.ToString();
        }

        /// <summary>
        /// Decumulate the magazine issues.
        /// </summary>
        public static void Decumulate
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = Decumulate(expression);
                context.WriteAndSetFlag(node, output);
            }
        }

        #endregion
    }
}
