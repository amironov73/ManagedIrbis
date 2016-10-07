/* PrefixLength.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.Runtime
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
