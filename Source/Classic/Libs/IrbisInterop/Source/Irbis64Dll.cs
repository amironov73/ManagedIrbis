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
    public static class Irbis64Dll
    {
        #region Constants

        /// <summary>
        /// Name for DLL.
        /// </summary>
        public const string DllName = "irbis65.dll";

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "Irbisinit65")]
        public static extern IntPtr IrbisInit();

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "Irbis_uatab_init65")]
        public static extern int IrbisUatabInit
            (
                string uctab,
                string lctab,
                string actab,
                string aExecDir,
                string aDataPath
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "Irbisclose65")]
        public static extern void IrbisClose
            (
                IntPtr space
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "Irbisclosemst65")]
        public static extern void IrbisCloseMST
            (
                IntPtr space
            );

        [DllImport(DllName, EntryPoint = "Irbisnewrec65")]
        public static extern int IrbisNewRec
            (
                IntPtr space,
                int shelf
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "Irbisfldadd65")]
        public static extern int IrbisFldAdd
            (
                IntPtr space, 
                int shelf, 
                int met, 
                int nf, 
                string pole
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "Irbis_InitPFT65")]
        public static extern int IrbisInitPFT
            (
                IntPtr space,
                string line
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "Irbis_Format65")]
        public static extern int IrbisFormat
            (
                IntPtr space,
                int shelf,
                int alt_shelf, 
                int trm_shelf,
                int lwLn,
                string fmtExitDLL
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "Irbisinitmst65")]
        public static extern int IrbisInitMST
            (
                IntPtr space,
                string dataBase,
                int aNumberShelfs
            );

        #endregion
    }
}
