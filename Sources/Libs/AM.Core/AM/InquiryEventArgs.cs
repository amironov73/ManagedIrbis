/* InquiryEventArgs.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public class InquiryEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets or sets a success indication.
        /// </summary>
        public bool Success { get; set; }

        #endregion
    }
}