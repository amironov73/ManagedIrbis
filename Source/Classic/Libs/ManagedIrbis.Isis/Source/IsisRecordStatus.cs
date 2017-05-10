// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IsisRecordStatus.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace ManagedIrbis.Isis
{
    /// <summary>
    /// Record status as reported by ISIS32.DLL.
    /// </summary>
    [Flags]
    [Serializable]
    public enum IsisRecordStatus
    {
        /// <summary>
        /// Normal (present) record.
        /// </summary>
        NormalRecord = 0,

        /// <summary>
        /// Record was logically deleted.
        /// </summary>
        LogicallyDeleted = 1,

        /// <summary>
        /// Record was physically deleted.
        /// </summary>
        PhysicallyDeleted = 2,

        /// <summary>
        /// Record not exists.
        /// </summary>
        NonExisting = 4,

        /// <summary>
        /// Status of record unknown.
        /// </summary>
        Unknown = 8
    }
}
