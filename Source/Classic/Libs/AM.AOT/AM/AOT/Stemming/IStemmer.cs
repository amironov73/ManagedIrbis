// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IStemmer.cs -- common stemmer interface
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.AOT.Stemming
{
    /// <summary>
    /// Common stemmer interface.
    /// </summary>
    [PublicAPI]
    public interface IStemmer
    {
        /// <summary>
        /// Stem the word.
        /// </summary>
        [NotNull]
        string Stem([NotNull] string word);
    }
}
