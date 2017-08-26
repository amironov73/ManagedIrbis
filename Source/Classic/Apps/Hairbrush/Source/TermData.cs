// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TermData.cs -- 
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

using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace Hairbrush
{
    /// <summary>
    /// Search term data.
    /// </summary>
    [PublicAPI]
    public sealed class TermData
    {
        #region Properties

        /// <summary>
        /// Count.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        [CanBeNull]
        public string Text { get; set; }

        /// <summary>
        /// Selected?
        /// </summary>
        public bool Selected { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Build array of <see cref="TermData"/>
        /// from sequence of <see cref="TermInfo"/>.
        /// </summary>
        [NotNull]
        public static TermData[] FromRawTerms
            (
                [NotNull] IEnumerable<TermInfo> rawTerms,
                [NotNull] string prefix
            )
        {
            Code.NotNull(rawTerms, "rawTerms");
            Code.NotNull(prefix, "prefix");

            int prefixLength = prefix.Length;
            List<TermData> result = new List<TermData>();
            foreach (TermInfo term in rawTerms)
            {
                TermData data = new TermData
                {
                    Count = term.Count,
                    Text = term.Text.Substring(prefixLength),
                    Selected = false
                };
                result.Add(data);
            }

            return result.ToArray();
        }

        #endregion

        #region Object members

        #endregion
    }
}
