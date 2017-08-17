// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DictionaryEntry.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using AM.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DictionaryEntry
    {
        #region Properties

        /// <summary>
        /// Title.
        /// </summary>
        [CanBeNull]
        public string Title { get; set; }

        /// <summary>
        /// List of references.
        /// </summary>
        [NotNull]
        public List<int> References { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DictionaryEntry()
        {
            References = new List<int>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = StringBuilderCache.Acquire();
            result.Append(Title);
            result.Append(' ');
            int[] refs = References.ToArray();
            Array.Sort(refs);
            bool first = true;
            foreach (int reference in refs)
            {
                if (!first)
                {
                    result.Append(", ");
                }
                result.Append(reference);
                first = false;
            }

            return StringBuilderCache.GetStringAndRelease(result);
        }

        #endregion
    }
}
