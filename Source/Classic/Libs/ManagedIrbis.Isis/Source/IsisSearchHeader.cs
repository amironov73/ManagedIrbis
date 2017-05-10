// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IsisSearchHeader.cs --
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
    /// The function IsisSrcSearch executes a search expression over
    /// an ISIS Space Inverted file, stores the result in the search
    /// log file and moves basic resulting data of the expression
    /// executed into an application area called search header 
    /// structure.
    /// A search header structure has the following data.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct IsisSearchHeader
    {
        #region Public members

        /// <summary>
        /// Search number.
        /// </summary>
        public int No;

        /// <summary>
        /// Total number of hits.
        /// </summary>
        public int Hits;

        /// <summary>
        /// Total number of records.
        /// </summary>
        public int Recs;

        /// <summary>
        /// Segment postings.
        /// </summary>
        public int Segs;

        /// <summary>
        /// Database name.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Dbname;

        /// <summary>
        /// Search expression.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string Expr;

        #endregion
    }
}
