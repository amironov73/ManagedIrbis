// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IsisRecordControl.cs --
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
    /// <para>Each master file has a record with MFN 0 called Master File
    /// Control Record. It is always stored at the beginning of the file.
    /// It has a fixed length and it has no corresponding 
    /// cross-reference record.</para>
    /// <para>The control record has the following fixed data fields.</para>
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct IsisRecordControl
    {
        #region Public members

        /// <summary>
        /// Master file number (always zero).
        /// </summary>
        public int Ctlmfn;

        /// <summary>
        /// Next master file number to be assigned.
        /// </summary>
        public int Nxtmfn;

        /// <summary>
        /// Address of the next master file record;
        /// 512 bytes block number.
        /// </summary>
        public int Nxtmfb;

        /// <summary>
        /// Address of the next master file record:
        /// offset within the block.
        /// </summary>
        public int Nxtmfp;

        /// <summary>
        /// Type of master file (always zero).
        /// </summary>
        public int Mftype;

        /// <summary>
        /// Not used (always zero).
        /// </summary>
        public int Reccnt;

        /// <summary>
        /// Not used (always zero).
        /// </summary>
        public int mfcxx1;

        /// <summary>
        /// Zero or number of applications with data entry lock.
        /// </summary>
        public int mfcxx2;

        /// <summary>
        /// Zero or 1 for exclusive write lock granted 
        /// to one application.
        /// </summary>
        public int mfcxx3;

        #endregion
    }
}
