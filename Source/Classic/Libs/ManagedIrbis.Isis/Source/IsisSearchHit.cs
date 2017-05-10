// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IsisSearchHit.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace ManagedIrbis.Isis
{
    /// <summary>
    /// Search hits, as well as term postings, have the following
    /// structure.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct IsisSearchHit
    {
        #region Public members

        /// <summary>
        /// Master file record number.
        /// </summary>
        public int Mfn;

        /// <summary>
        /// Field identifier.
        /// </summary>
        public int Id;

        /// <summary>
        /// Field occurence.
        /// </summary>
        public int Occ;

        /// <summary>
        /// Position withing the field.
        /// </summary>
        public int Pos;

        #endregion
    }
}
