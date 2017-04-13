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
        private static IntPtr GetRawRecordBufferPtr
        (
            IntPtr space
        )
        {
            var ptrBuffer = new byte[4];
            Marshal.Copy(space + 626, ptrBuffer, 0, 4);
            IntPtr fmtBuffer = new IntPtr(BitConverter.ToInt32(ptrBuffer, 0));
            return fmtBuffer;
        }

        /// <summary>
        /// 
        /// </summary>
        private static IntPtr GetFormattedRecordBufferPtr
            (
                IntPtr space
            )
        {
            var ptrBuffer = new byte[4];
            Marshal.Copy(space + 654, ptrBuffer, 0, 4);
            IntPtr fmtBuffer = new IntPtr(BitConverter.ToInt32(ptrBuffer, 0));
            return fmtBuffer;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetFormattedRecord
            (
                IntPtr space
            )
        {
            var strBuffer = new byte[32000];
            var fmtBuffer = GetFormattedRecordBufferPtr(space);
            Marshal.Copy(fmtBuffer, strBuffer, 0, strBuffer.Length);
            int pos;
            for (pos = 0; pos < strBuffer.Length; pos++)
            {
                if (strBuffer[pos] == 0)
                {
                    break;
                }
            }

            var formattedRecord = Encoding.UTF8.GetString
                (
                    strBuffer,
                    0,
                    pos
                );

            return formattedRecord;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetRawRecordText
            (
                IntPtr space
            )
        {
            var strBuffer = new byte[32000];
            var fmtBuffer = GetRawRecordBufferPtr(space);
            Marshal.Copy(fmtBuffer, strBuffer, 0, strBuffer.Length);
            var formattedRecord = Encoding.UTF8.GetString(strBuffer).TrimEnd((char)0);

            return formattedRecord;
        }

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
        /// Current version is 100.
        /// </summary>
        [DllImport(DllName, EntryPoint = "InteropVersion")]
        public static extern int InteropVersion();

        #endregion
    }
}
