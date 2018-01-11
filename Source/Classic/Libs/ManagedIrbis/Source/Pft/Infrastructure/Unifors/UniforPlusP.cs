// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusP.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Search;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    // Неописанная функция unifor('+P')
    // Ищет термин, выводит результат функции IrbisPosting
    // Параметры: команда,терм
    // где команда:
    // 0 - MFN
    // 1 - Tag
    // 2 - Occ
    //

    static class UniforPlusP
    {
        #region Public methods

        /// <summary>
        /// Информация о первой ссылке на указанный терм.
        /// </summary>
        public static void GetPosting
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

            string[] parts = StringUtility.SplitString
                (
                    expression,
                    CommonSeparators.Comma,
                    2
                );
            if (parts.Length != 2)
            {
                return;
            }
            char command = parts[0].FirstChar();
            string startTerm = parts[1];
            if (string.IsNullOrEmpty(startTerm))
            {
                return;
            }

            startTerm = IrbisText.ToUpper(startTerm).ThrowIfNull();
            IrbisProvider provider = context.Provider;
            TermParameters parameters = new TermParameters
            {
                Database = provider.Database,
                StartTerm = startTerm,
                NumberOfTerms = 1
            };
            TermInfo[] terms = provider.ReadTerms(parameters);
            if (terms.Length < 1)
            {
                return;
            }

            string termText = terms[0].Text.ThrowIfNull();
            if (termText != startTerm)
            {
                return;
            }
            TermLink[] links = provider.ExactSearchLinks(termText);
            if (links.Length < 1)
            {
                return;
            }

            TermLink link = links[0];
            string output = null;
            switch (command)
            {
                //mfn
                case '0':
                    output = link.Mfn.ToInvariantString();
                    break;

                //tag
                case '1':
                    output = link.Tag.ToInvariantString();
                    break;

                //occ
                case '2':
                    output = link.Occurrence.ToInvariantString();
                    break;

                default:
                    Log.Warn
                        (
                            "UniforPlusP::GetPosting: "
                          + "unknown command=" + command
                        );
                    break;
            }

            if (!string.IsNullOrEmpty(output))
            {
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
