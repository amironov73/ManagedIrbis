/* IoCounters.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Contains I/O accounting information for a process
    /// or a job object. For a job object, the counters
    /// include all operations performed by all processes
    /// that have ever been associated with the job,
    /// in addition to all processes currently associated
    /// with the job.
    /// </summary>
    [CLSCompliant(false)]
    [StructLayout(LayoutKind.Sequential)]
    public struct IoCounters
    {
        /// <summary>
        /// The number of read operations performed.
        /// </summary>
        public UInt64 ReadOperationCount;

        /// <summary>
        /// The number of write operations performed.
        /// </summary>
        public UInt64 WriteOperationCount;

        /// <summary>
        /// The number of I/O operations performed,
        /// other than read and write operations.
        /// </summary>
        public UInt64 OtherOperationCount;

        /// <summary>
        /// The number of bytes read.
        /// </summary>
        public UInt64 ReadTransferCount;

        /// <summary>
        /// The number of bytes written.
        /// </summary>
        public UInt64 WriteTransferCount;

        /// <summary>
        /// The number of bytes transferred during
        /// operations other than read and write operations.
        /// </summary>
        public UInt64 OtherTransferCount;
    }
}
