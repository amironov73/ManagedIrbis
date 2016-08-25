/* XrfRecord64.cs
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Usingd directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Contains information about record offset and status.
    /// </summary>
    [DebuggerDisplay("Offset={Offset}, Status={Status}")]
    public sealed class XrfRecord64
    {
        #region Constants

        /// <summary>
        /// Fixed record size.
        /// </summary>
        public const int RecordSize 
            = sizeof ( long ) + sizeof ( int );

        #endregion

        #region Properties

        /// <summary>
        /// MFN
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// 8-byte offset of the record in the MST file.
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        /// Status of the record.
        /// </summary>
        public RecordStatus Status { get; set; }

        /// <summary>
        /// Is the record locked.
        /// </summary>
        public bool Locked
        {
            get { return ((Status & RecordStatus.Locked) != 0); }
            set
            {
                if (value)
                {
                    Status |= RecordStatus.Locked;
                }
                else
                {
                    Status &= ~RecordStatus.Locked;
                }
            }
        }

        /// <summary>
        /// Is the record deleted.
        /// </summary>
        public bool Deleted
        {
            get { return ((Status & (RecordStatus.LogicallyDeleted | RecordStatus.PhysicallyDeleted)) != 0); }
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <summary>
        /// 
        /// </summary>
        public override string ToString ()
        {
            return string.Format 
                (
                    "MFN: {0}, Offset: {1}, Status: {2}",
                    Mfn,
                    Offset, 
                    Status 
                );
        }

        #endregion
    }
}
