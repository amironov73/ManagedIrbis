// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CaseInsensitiveDictionary.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class CaseInsensitiveDictionary<T>
        : Dictionary<string, T>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CaseInsensitiveDictionary()
            : base (_GetComparer())
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CaseInsensitiveDictionary
            (
                int capacity
            ) 
            : base(capacity, _GetComparer())
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CaseInsensitiveDictionary
            (
                [NotNull] IDictionary<string, T> dictionary
            )
            : base(dictionary, _GetComparer())
        {
        }

        #endregion

        #region Private members

        /// <summary>
        /// Get comparer for the dictionary.
        /// </summary>
        private static IEqualityComparer<string> _GetComparer()
        {
            return StringUtility.GetCaseInsensitiveComparer();
        }

        #endregion
    }
}
