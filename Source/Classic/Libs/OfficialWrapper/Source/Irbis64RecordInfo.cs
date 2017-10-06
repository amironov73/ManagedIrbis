// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Irbis64RecordInfo.cs -- information about Irbis64 record
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

#endregion

namespace OfficialWrapper
{
    /// <summary>
    /// Information about Irbis64 record.
    /// </summary>
    [Serializable]
    public sealed class Irbis64RecordInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets the return code.
        /// </summary>
        /// <value>
        /// The return code.
        /// </value>
        public int ReturnCode { get; set; }

        /// <summary>
        /// Gets or sets the MFN.
        /// </summary>
        /// <value>
        /// The MFN.
        /// </value>
        public int Mfn { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format
                (
                    "ReturnCode: {0}, Mfn: {1}, Status: {2}, Version: {3}",
                    ReturnCode,
                    Mfn,
                    Status,
                    Version
                );
        }

        #endregion
    }
}