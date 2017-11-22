// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchResult.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// Search result.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SearchResult
    {
        #region Properties

        /// <summary>
        /// Count of records found.
        /// </summary>
        public int FoundCount { get; set; }

        /// <summary>
        /// Search query text.
        /// </summary>
        [CanBeNull]
        public string Query { get; set; }

        #endregion
    }
}
