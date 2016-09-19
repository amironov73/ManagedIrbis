/* RfidSystemInfo.cs --
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Rfid
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// RFID system info.
    /// </summary>
    [PublicAPI]
    public sealed class RfidSystemInfo
    {
        #region Properties

        /// <summary>
        /// Transponder type.
        /// </summary>
        public byte TransponderType { get; set; }

        /// <summary>
        /// UID.
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// DSFID.
        /// </summary>
        public byte DSFID { get; set; }

        /// <summary>
        /// AFI.
        /// </summary>
        public byte AFI { get; set; }

        /// <summary>
        /// Memory size.
        /// </summary>
        public int MemorySize { get; set; }

        /// <summary>
        /// IC reference.
        /// </summary>
        public byte ICReference { get; set; }

        #endregion
    }
}
