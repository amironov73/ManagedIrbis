// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FstTerm.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Fst
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FstTerm
    {
        #region Properties

        /// <summary>
        /// Текстовое значение термина.
        /// </summary>
        [CanBeNull]
        public string Text { get; set; }

        /// <summary>
        /// Метка поля (берётся из FST).
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Ссылки.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        public TermLink[] Links { get; set; }

        #endregion
    }
}
