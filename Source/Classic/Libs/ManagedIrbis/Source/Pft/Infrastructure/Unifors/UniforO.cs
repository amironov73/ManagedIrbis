// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforO.cs --
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
using AM.Collections;
using AM.Text;
using AM.Text.Ranges;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Fields;
using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforO
    {
        #region Private members

        #endregion

        #region Public methods

        public static string AllExemplars
            (
                MarcRecord record
            )
        {
            ExemplarInfo[] exemplars = ExemplarInfo.Parse(record);
            DictionaryCounterInt32<string> counter
                = new DictionaryCounterInt32<string>();

            foreach (ExemplarInfo exemplar in exemplars)
            {
                string place = exemplar.Place;
                if (string.IsNullOrEmpty(place))
                {
                    continue;
                }

                switch (exemplar.Status)
                {
                    case ExemplarStatus.Summary:
                    case ExemplarStatus.BiblioNet:
                        string amountText = exemplar.Amount;
                        int amount;
                        if (int.TryParse(amountText, out amount))
                        {
                            counter.Augment(place, amount);
                        }
                        break;

                    default:
                        counter.Augment(place, 1);
                        break;
                }
            }

            StringBuilder result = new StringBuilder();

            bool first = true;
            foreach (var pair in counter)
            {
                if (!first)
                {
                    result.Append(", ");
                }
                first = false;
                result.AppendFormat
                    (
                        "{0}({1})",
                        pair.Key,
                        pair.Value
                    );
            }

            return result.ToString();
        }

        public static void AllExemplars
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!ReferenceEquals(context.Record, null))
            {
                string output = AllExemplars(context.Record);
                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        public static string FreeExemplars
            (
                MarcRecord record
            )
        {
            ExemplarInfo[] exemplars = ExemplarInfo.Parse(record);
            DictionaryCounterInt32<string> counter
                = new DictionaryCounterInt32<string>();

            foreach (ExemplarInfo exemplar in exemplars)
            {
                string place = exemplar.Place;
                if (string.IsNullOrEmpty(place))
                {
                    continue;
                }

                switch (exemplar.Status)
                {
                    case ExemplarStatus.Summary:
                    case ExemplarStatus.BiblioNet:
                        string amountText = exemplar.Amount;
                        int amount;
                        if (int.TryParse(amountText, out amount))
                        {
                            string onHandText = exemplar.OnHand;
                            int onHand;
                            if (int.TryParse(onHandText, out onHand))
                            {
                                amount -= onHand;
                            }
                        }
                        counter.Augment(place, amount);
                        break;

                    case ExemplarStatus.Free:
                        counter.Augment(place, 1);
                        break;
                }
            }

            StringBuilder result = new StringBuilder();

            bool first = true;
            foreach (var pair in counter)
            {
                if (!first)
                {
                    result.Append(", ");
                }
                first = false;
                result.AppendFormat
                    (
                        "{0}({1})",
                        pair.Key,
                        pair.Value
                    );
            }

            return result.ToString();
        }

        public static void FreeExemplars
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!ReferenceEquals(context.Record, null))
            {
                string output = FreeExemplars(context.Record);
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
