/* FeigSystemInformation.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM.Rfid
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// System information.
    /// </summary>
    [PublicAPI]
    public sealed class FeigSystemInformation
    {
        #region Properties

        /// <summary>
        /// Manufacturer name.
        /// </summary>
        public string ManufacturerName { get; set; }

        /// <summary>
        /// Tag name.
        /// </summary>
        public string TagName { get; set; }

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

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format
            (
                "ManufacturerName: {0}{7}"
                +   "TagName: {1}{7}"
                +   "UID: {2}{7}"
                +   "DSFID: {3}{7}"
                +   "AFI: {4}{7}"
                +   "MemorySize: {5}{7}"
                +   "ICReference: {6}", 
                ManufacturerName, 
                TagName, 
                UID, 
                DSFID, 
                AFI, 
                MemorySize, 
                ICReference,
                Environment.NewLine
            );
        }

        #endregion
    }
}
