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

        public static string Decumulate
            (
                string issues
            )
        {
            NumberRangeCollection collection
                = NumberRangeCollection.Parse(issues);
            string[] items = collection.Select(number => number.ToString()).ToArray();
            return string.Join(",", items);
        }

        public static string Cumulate
            (
                string issues
            )
        {
            try
            {
                NumberRangeCollection collection
                    = NumberRangeCollection.Parse(issues);
                //collection.To
            }
            catch
            {
                return issues;
            }

            return null;
        }

        public static void Cumulate
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
        }

        #endregion
    }
}
