// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IsisTermPosting.cs --
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
    /// Each posting has a data structure that identifies the
    /// source of the term occurence in the database.
    /// A posting structure has 20 bytes.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct IsisTermPosting
    {
        #region Public members

        /// <summary>
        /// Posting sequential number.
        /// </summary>
        public int Post;

        /// <summary>
        /// Master file record mfn that originated the term occurence.
        /// </summary>
        public int Mfn;

        /// <summary>
        /// Field identifier, as defined in the fst table.
        /// </summary>
        public int Id;

        /// <summary>
        /// Field occurence.
        /// </summary>
        public int Occ;

        /// <summary>
        /// Position within the field.
        /// </summary>
        public int Pos;

        #endregion
    }
}
