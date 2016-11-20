/* PftCleanup.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// PFT cleanup flags.
    /// </summary>
    [Flags]
    public enum PftCleanup
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Double text removal.
        /// </summary>
        DoubleText = 1,

        /// <summary>
        /// RTF markup removal.
        /// </summary>
        Rtf = 2,

        /// <summary>
        /// HTML markup removal.
        /// </summary>
        Html = 4
    }
}
