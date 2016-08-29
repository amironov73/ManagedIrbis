/* RfidCapabilities.cs --
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

#endregion

namespace AM.Rfid
{
    /// <summary>
    /// RFID capabilities.
    /// </summary>
    [Flags]
    public enum RfidCapabilities
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// EAS.
        /// </summary>
        EAS = 0x01,

        /// <summary>
        /// AFI.
        /// </summary>
        AFI = 0x02,

        /// <summary>
        /// Select.
        /// </summary>
        Select = 0x04,

        /// <summary>
        /// System info.
        /// </summary>
        SystemInfo = 0x08
    }
}
