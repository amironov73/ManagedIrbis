/* UniforU.cs --
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
using AM.Text.Ranges;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforU
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Check whether the issue is present
        /// in cumulated collection.
        /// </summary>
        public static bool Check
            (
                string issue,
                string cumulated
            )
        {
            NumberRangeCollection collection
                = NumberRangeCollection.Parse(cumulated);
            NumberText number = new NumberText(issue);
            bool result = collection.Contains(number);

            return result;
        }

        /// <summary>
        /// Cumulate the magazine issues.
        /// </summary>
        public static string Cumulate
            (
                string issues
            )
        {
            try
            {
                NumberRangeCollection collection
                    = NumberRangeCollection.Parse(issues);
                List<NumberText> numbers = collection
                    .Distinct()
                    .ToList();
                NumberRangeCollection result
                    = NumberRangeCollection.Cumulate(numbers);

                return result.ToString();
            }
            catch
            {
                return issues;
            }
        }

        /// <summary>
        /// Decumulate the magazine issues.
        /// </summary>
        public static string Decumulate
            (
                string issues
            )
        {
            NumberRangeCollection collection
                = NumberRangeCollection.Parse(issues);
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

        public static void Check
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = expression.Split(new []{','}, 2);
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

        public static void Cumulate
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = Cumulate(expression);
                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        public static void Decumulate
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = Decumulate(expression);
                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        #endregion
    }
}
