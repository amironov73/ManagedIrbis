// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Irbis64Dll.cs -- 
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

namespace IrbisInterop
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Irbis64Dll
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Shelf number.
        /// </summary>
        public int Shelf { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor
        /// </summary>
        public Irbis64Dll()
        {
            //_space = Irbis65Dll.IrbisInit();
        }

        #endregion

        #region Private members

        private IntPtr _space;

        #endregion

        #region Public methods

        #endregion

        #region IDisposable members

        /// <inheritdoc />
        public void Dispose()
        {
            //Irbis65Dll.IrbisClose(_space);
        }

        #endregion
    }
}
