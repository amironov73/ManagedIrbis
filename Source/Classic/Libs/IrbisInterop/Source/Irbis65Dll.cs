// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Irbis65Dll.cs -- 
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
    public static class Irbis65Dll
    {
        #region Constants

        /// <summary>
        /// Name for DLL.
        /// </summary>
        public const string DllName = "Irbis65.dll";

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisInit")]
        public static extern IntPtr IrbisInit();

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisDllVersion")]
        public static extern void IrbisDllVersion
            (
                StringBuilder buffer,
                int bufsize
            );

        ///// <summary>
        ///// 
        ///// </summary>
        //[DllImport(DllName, EntryPoint = "IrbiUatabInit")]
        //public static extern int IrbisUatabInit
        //    (
        //        string uctab,
        //        string lctab,
        //        string actab,
        //        string aExecDir,
        //        string aDataPath
        //    );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisClose")]
        public static extern void IrbisClose
            (
                IntPtr space
            );

        ///// <summary>
        ///// 
        ///// </summary>
        //[DllImport(DllName, EntryPoint = "IrbisCloseMst")]
        //public static extern void IrbisCloseMST
        //    (
        //        IntPtr space
        //    );

        ///// <summary>
        ///// 
        ///// </summary>
        //[DllImport(DllName, EntryPoint = "IrbisNewRec")]
        //public static extern int IrbisNewRec
        //    (
        //        IntPtr space,
        //        int shelf
        //    );

        ///// <summary>
        ///// 
        ///// </summary>
        //[DllImport(DllName, EntryPoint = "IrbisFldAdd")]
        //public static extern int IrbisFldAdd
        //    (
        //        IntPtr space,
        //        int shelf,
        //        int met,
        //        int nf,
        //        string pole
        //    );

        ///// <summary>
        ///// 
        ///// </summary>
        //[DllImport(DllName, EntryPoint = "IrbisInitPft")]
        //public static extern int IrbisInitPft
        //    (
        //        IntPtr space,
        //        string line
        //    );

        ///// <summary>
        ///// 
        ///// </summary>
        //[DllImport(DllName, EntryPoint = "IrbisFormat")]
        //public static extern int IrbisFormat
        //    (
        //        IntPtr space,
        //        int shelf,
        //        int alt_shelf,
        //        int trm_shelf,
        //        int lwLn,
        //        string fmtExitDLL
        //    );

        ///// <summary>
        ///// 
        ///// </summary>
        //[DllImport(DllName, EntryPoint = "IrbisInitMst")]
        //public static extern int IrbisInitMST
        //    (
        //        IntPtr space,
        //        string dataBase,
        //        int aNumberShelfs
        //    );

        /// <summary>
        /// Current version is 100.
        /// </summary>
        [DllImport(DllName, EntryPoint = "InteropVersion")]
        public static extern int InteropVersion();

        #endregion
    }
}
