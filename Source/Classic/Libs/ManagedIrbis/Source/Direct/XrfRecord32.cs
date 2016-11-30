// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* XrfRecord32.cs -- XRF file record.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Usingd directives

using System.Diagnostics;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Contains information about record offset and status.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("Offset={AbsoluteOffset}, Status={Status}")]
#endif
    public sealed class XrfRecord32
    {
        #region Constants

        /// <summary>
        /// Fixed record size.
        /// </summary>
        public const int RecordSize = 4;

        #endregion

        #region Properties

        /// <summary>
        /// Absolute offset.
        /// </summary>
        public int AbsoluteOffset { get; set; }

        /// <summary>
        /// Block number.
        /// </summary>
        public int BlockNumber { get; set; }

        /// <summary>
        /// Block offset.
        /// </summary>
        public int BlockOffset { get; set; }

        /// <summary>
        /// Record status.
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
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
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
