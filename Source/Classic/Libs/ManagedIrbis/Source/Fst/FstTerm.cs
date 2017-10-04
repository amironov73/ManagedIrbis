// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FstTerm.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

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
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

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
        /// Номер повторения (начиная с 1).
        /// </summary>
        public int Occurrence { get; set; }

        /// <summary>
        /// Смещение от начала поля.
        /// </summary>
        public int Offset { get; set; }

        #endregion
    }
}
