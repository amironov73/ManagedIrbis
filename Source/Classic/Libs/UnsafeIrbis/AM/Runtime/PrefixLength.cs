// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PrefixLength.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Runtime
{
    /// <summary>
    ///
    /// </summary>

    [PublicAPI]
    public enum PrefixLength
    {
        /// <summary>
        /// Class name only.
        /// </summary>
        Short,

        /// <summary>
        /// Class name with namespace.
        /// </summary>
        Moderate,

        /// <summary>
        /// Assembly-qualified name.
        /// </summary>
        Full
    }
}
