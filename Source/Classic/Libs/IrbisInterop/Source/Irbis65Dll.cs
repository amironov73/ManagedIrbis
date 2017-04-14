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

using JetBrains.Annotations;

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
        [DllImport(DllName, EntryPoint = "IrbisDllVersion", CharSet = CharSet.Ansi)]
        public static extern void IrbisDllVersion
            (
                StringBuilder buffer,
                int bufsize
            );


        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisInitDeposit", CharSet = CharSet.Ansi)]
        public static extern int IrbisInitDeposit
            (
                string path
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisUatabInit", CharSet = CharSet.Ansi)]
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
        [DllImport(DllName, EntryPoint = "IrbisClose")]
        public static extern void IrbisClose
            (
                IntPtr space
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisCloseMst")]
        public static extern void IrbisCloseMst
            (
                IntPtr space
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisNewRec")]
        public static extern int IrbisNewRec
            (
                IntPtr space,
                int shelf
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisFldAdd", CharSet = CharSet.Ansi)]
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
        [DllImport(DllName, EntryPoint = "IrbisInitPft", CharSet = CharSet.Ansi)]
        public static extern int IrbisInitPft
            (
                IntPtr space,
                string line
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisFormat", CharSet = CharSet.Ansi)]
        public static extern int IrbisFormat
            (
                IntPtr space,
                int shelf,
                int altShelf,
                int trmShelf,
                int lwLn,
                string fmtExitDll
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisInitMst", CharSet = CharSet.Ansi)]
        public static extern int IrbisInitMst
            (
                IntPtr space,
                string dataBase,
                int aNumberShelfs
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisInitTerm", CharSet = CharSet.Ansi)]
        public static extern int IrbisInitTerm
            (
                IntPtr space,
                string dataBase
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisMaxMfn")]
        public static extern int IrbisMaxMfn
            (
                IntPtr space
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisRecord")]
        public static extern int IrbisRecord
            (
                IntPtr space,
                int shelf,
                int mfn
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisSetOptions")]
        public static extern void IrbisSetOptions
            (
                int cashable,
                int precompiled,
                int errorFirstBreak
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisMainIniInit", CharSet = CharSet.Ansi)]
        public static extern void IrbisMainIniInit
            (
                string iniFile
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisInitUactab")]
        public static extern void IrbisInitUactab
            (
                IntPtr space
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisFind")]
        public static extern int IrbisFind
            (
                IntPtr space,
                byte[] term
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisNextTerm")]
        public static extern int IrbisNextTerm
            (
                IntPtr space,
                byte[] term
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisPrevTerm")]
        public static extern int IrbisPrevTerm
        (
            IntPtr space,
            byte[] term
        );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisNPosts")]
        public static extern int IrbisNPosts
            (
                IntPtr space
            );

        /// <summary>
        /// Разобрать ссылку на: mfn, tag, occ, cnt.
        /// Возможные значения opt:
        /// opt = 1 функция возвращает mfn (номер записи);
        /// opt = 2 функция возвращает tag (метка поля);
        /// opt = 3 функция возвращает occ (повторение поля);
        /// opt = 4 функция возвращает cnt (номер слова).
        /// Возвращает: в случае успеха положительное значение (больше ноля).
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisPosting")]
        public static extern int IrbisPosting
            (
                IntPtr space,
                short opt
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisNextPost")]
        public static extern int IrbisNextPost
            (
                IntPtr space
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisInitNewDb", CharSet = CharSet.Ansi)]
        public static extern int IrbisInitNewDb
            (
                string path
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisMfn")]
        public static extern int IrbisMfn
            (
                IntPtr space,
                int shelf
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisNFields")]
        public static extern int IrbisNFields
            (
                IntPtr space,
                int shelf
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisIsLocked")]
        public static extern int IrbisIsLocked
            (
                IntPtr space,
                int shelf
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisIsActualized")]
        public static extern int IrbisIsDeleted
            (
                IntPtr space,
                int shelf
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisIsActualized")]
        public static extern int IrbisIsActualized
            (
                IntPtr space,
                int shelf
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisIsDbLocked")]
        public static extern int IrbisIsDbLocked
            (
                IntPtr space
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisIsReallyLocked")]
        public static extern int IrbisIsRealyLocked
            (
                IntPtr space,
                int mfn
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisRecLock0")]
        public static extern int IrbisRecLock0
            (
                IntPtr space,
                int shelf,
                int mfn
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisRecUnlock0")]
        public static extern int IrbisRecUnLock0
            (
                IntPtr space,
                int mfn
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisReadVersion")]
        public static extern int IrbisReadVersion
            (
                IntPtr space,
                int mfn
        );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisRecordBack")]
        public static extern int IrbisRecordBack
            (
                IntPtr space,
                int shelf,
                int mfn,
                int step
        );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisRecUpdate0")]
        public static extern int IrbisRecUpdate0
            (
                IntPtr space,
                int shelf,
                int keepLock
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisRecIfUpdate0")]
        public static extern int IrbisRecIfUpdate0
            (
                IntPtr space,
                int shelf,
                int mfn
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisRecDelete")]
        public static extern void IrbisRecDelete
            (
                IntPtr space,
                int shelf
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisRecUndelete")]
        public static extern void IrbisRecUndelete
            (
                IntPtr space,
                int shelf
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "Unifor")]
        public static extern int Unifor
            (
                IntPtr space,
                int currentShelf,
                int termShelf,
                int lwExit,
                int occExit,
                byte[] sp1,
                byte[] sp2
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "Umarci")]
        public static extern int Umarci
            (
                IntPtr space,
                int currentShelf,
                int termShelf,
                int lwExit,
                int occExit,
                byte[] sp1,
                byte[] sp2
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisDbEmptyTime")]
        public static extern int IrbisDbEmptyTime
            (
                IntPtr space,
                int seconds
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisLockDbTime")]
        public static extern int IrbisLockDbTime
            (
                IntPtr space,
                int seconds
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisUnLockDbTime")]
        public static extern int IrbisUnLockDbTime
            (
                IntPtr space,
                int seconds
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisRecLockTime")]
        public static extern int IrbisRecLockTime
            (
                IntPtr space,
                int shelf,
                int mfn,
                int seconds
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisRecUnlockTime")]
        public static extern int IrbisRecUnlockTime
            (
                IntPtr space,
                int mfn,
                int seconds
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisRecUpdateTime")]
        public static extern int IrbisRecUpdateTime
            (
                IntPtr space,
                int shelf,
                int keepLock,
                int updif,
                int seconds,
                ref int updateResult,
                ref int updifResult
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "IrbisRecIfUpdateTime")]
        public static extern int IrbisRecIfUpdateTime
            (
                IntPtr space,
                int shelf,
                int mfn,
                int seconds
            );

        // =========================================================

        //
        // Functions from BORLNDMM.DLL
        //

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "SysGetMem")]
        public static extern IntPtr SysGetMem
            (
                int size
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "SysFreeMem")]
        public static extern int SysFreeMem
            (
                IntPtr pointer
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "SysReallocMem")]
        public static extern IntPtr SysReallocMem
            (
                IntPtr pointer,
                int size
            );

        /// <summary>
        /// 
        /// </summary>
        [DllImport(DllName, EntryPoint = "SysAllocMem")]
        public static extern IntPtr SysAllocMem
            (
                int size
            );

        // =========================================================

        /// <summary>
        /// Current version is 100.
        /// </summary>
        [DllImport(DllName, EntryPoint = "InteropVersion")]
        public static extern int InteropVersion();

        /// <summary>
        /// Determines whether the function is available.
        /// </summary>
        [DllImport(DllName, EntryPoint = "IsFunctionAvailable",
            CharSet = CharSet.Ansi)]
        public static extern int IsFunctionAvailable
            (
                [NotNull] string functionName
            );

        #endregion
    }
}
