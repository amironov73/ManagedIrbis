/* XrfRecord32.cs
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Usingd directives

using System;
using System.Diagnostics;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Contains information about record offset and status.
    /// </summary>
    [DebuggerDisplay("Offset={AbsoluteOffset}, Status={Status}")]
    public sealed class XrfRecord32
    {
        #region Constants

        /// <summary>
        /// Fixed record size.
        /// </summary>
        public const int RecordSize = 4;

        #endregion

        #region Properties

        public int AbsoluteOffset { get; set; }

        public int BlockNumber { get; set; }

        public int BlockOffset { get; set; }

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
            get
            {
                return ((Status & 
                    (
                        RecordStatus.LogicallyDeleted 
                        | RecordStatus.PhysicallyDeleted
                    )) != 0);
            }
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
        public override string ToString()
        {
            return string.Format
                (
                    "Offset: {0}, Status: {1}",
                    AbsoluteOffset,
                    Status
                );
        }

        #endregion
    }
}
