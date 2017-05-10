// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IsisInterop.cs --
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
    /// ISIS32.DLL interop.
    /// </summary>
    public static class IsisInterop
    {
        #region Constants

        /// <summary>
        /// DLL name.
        /// </summary>
        public const string DllName = "ISIS32.DLL";

        #endregion

        #region Private members



        #endregion

        #region Public methods

        /// <summary>
        /// ISIS32.DLL version.
        /// </summary>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern float IsisDllVersion();

        /// <summary>
        /// Specifies the ISIS alphabetic character table used in world
        /// indexing technique 4.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisAppAcTab
            (
                int handle,
                string fileName
            );

        /// <summary>
        /// Specifies the way ISIS32.DLL handles run-time error messages.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        /// <remarks>The default value is DEBUG_LIGHT.</remarks>
        [DllImport(DllName)]
        public static extern int IsisAppDebug
            (
                int handle,
                IsisDebugFlags flags
            );

        /// <summary>
        /// Deletes an ISIS Application object and frees all 
        /// associated memory.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisAppDelete
            (
                int handle
            );

        /// <summary>
        /// Specifies the name of a log file ISIS32.DLL uses to log
        /// run-time errors.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisAppLogFile
            (
                int handle,
                string fileName
            );

        /// <summary>
        /// The IsisAppNew function creates an ISIS Application.
        /// It returns an application handle to create ISIS Spaces
        /// and displays a message-box window containing the
        /// ISIS32.DLL copyright message.
        /// </summary>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisAppNew();

        /// <summary>
        /// Retrieves a physical file name from a logical one
        /// using file/path conversion table.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="parinp"></param>
        /// <param name="paroutp"></param>
        /// <param name="areasize"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisAppParGet
            (
                int handle,
                string parinp,
                StringBuilder paroutp,
                int areasize
            );

        /// <summary>
        /// Specifies the file/path conversion table.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="pararea">Specifies the file.par file name
        /// or a string describing the table. If it is a file name,
        /// it must be preceded by @. The especification of the
        /// extension is not required.</param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisAppParSet
            (
                int handle,
                string pararea
            );

        /// <summary>
        /// Specifies the ISIS upppercase conversion table.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fileName">Specifies the isisuc.tab
        /// file name or a string describing the 256 character
        /// table. If it is a file name, it must be preceded by @.
        /// The especification of the extension is not required.
        /// </param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisAppUcTab
            (
                int handle,
                string fileName
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSpaDb
            (
                int handle,
                string name
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisSpaDelete
            (
                int handle
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSpaDf
            (
                int handle,
                string name
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSpaFdt
            (
                int handle,
                string name
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSpaFst
            (
                int handle,
                string name
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSpaGf
            (
                int handle,
                string name
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSpaHeaderMap
            (
                int handle,
                string header
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fname"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSpaIf
            (
                int handle,
                string fname
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisSpaIfCreate
            (
                int handle
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="recordDelimiter"></param>
        /// <param name="fieldDelimiter"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSpaIsoDelim
            (
                int handle,
                string recordDelimiter,
                string fieldDelimiter
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fname"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSpaIsoIn
            (
                int handle,
                string fname
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fname"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSpaIsoOut
            (
                int handle,
                string fname
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisSpaIsoOutCreate
            (
                int handle
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fname"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSpaMf
            (
                int handle,
                string fname
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisSpaMfCreate
            (
                int handle
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisSpaMfUnlockForce
            (
                int handle
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisSpaMultiOn
            (
                int handle
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisSpaMultiOff
            (
                int handle
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSpaName
            (
                int handle,
                string name
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appHandle"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisSpaNew
            (
                int appHandle
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSpaPft
            (
                int handle,
                string format
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisSpaRecDelim
            (
                int handle,
                string begin,
                string end
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="maxMst"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisSpaRecShelves
            (
                int handle,
                int maxMst
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSpaStw
            (
                int handle,
                string name
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="maxTrm"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisSpaTrmShelves
            (
                int handle,
                int maxTrm
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spaceHandle"></param>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecControlMap
            (
                int spaceHandle,
                out IsisRecordControl ctrl
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromHandle"></param>
        /// <param name="fromIndex"></param>
        /// <param name="toHandle"></param>
        /// <param name="toIndex"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecCopy
            (
                int fromHandle,
                int fromIndex,
                int toHandle,
                int toIndex
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecDummy
            (
                int handle,
                int index
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spaceHandle"></param>
        /// <param name="index"></param>
        /// <param name="area"></param>
        /// <param name="areaSize"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisRecDump
            (
                int spaceHandle,
                int index,
                StringBuilder area,
                int areaSize
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="tag"></param>
        /// <param name="occurence"></param>
        /// <param name="area"></param>
        /// <param name="areaSize"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisRecField
            (
                int handle,
                int index,
                int tag,
                int occurence,
                StringBuilder area,
                int areaSize
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecFieldOcc
            (
                int handle,
                int index,
                int tag
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="pos"></param>
        /// <param name="area"></param>
        /// <param name="areaSize"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisRecFieldN
            (
                int handle,
                int index,
                int pos,
                StringBuilder area,
                int areaSize
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spaceHandle"></param>
        /// <param name="index"></param>
        /// <param name="updateString"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisRecFieldUpdate
            (
                int spaceHandle,
                int index,
                string updateString
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="area"></param>
        /// <param name="areaSize"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisRecFormat
            (
                int handle,
                int index,
                StringBuilder area,
                int areaSize
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="lineSize"></param>
        /// <param name="area"></param>
        /// <param name="areaSize"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisRecFormatEx
            (
                int handle,
                int index,
                int lineSize,
                StringBuilder area,
                int areaSize
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="mfn"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecIfUpdate
            (
                int handle,
                int mfn
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="beginMfn"></param>
        /// <param name="endMfn"></param>
        /// <param name="keepPending"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecIfUpdateEx
            (
                int handle,
                int beginMfn,
                int endMfn,
                int keepPending
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecIsoRead
            (
                int handle,
                int index
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecIsoWrite
            (
                int handle,
                int index
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="beginMfn"></param>
        /// <param name="endMfn"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecLnk
            (
                int handle,
                int beginMfn,
                int endMfn
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromHandle"></param>
        /// <param name="fromIndex"></param>
        /// <param name="toHandle"></param>
        /// <param name="toIndex"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecMerge
            (
                int fromHandle,
                int fromIndex,
                int toHandle,
                int toIndex
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecMfn
            (
                int handle,
                int index
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="mfn"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecMfnChange
            (
                int handle,
                int index,
                int mfn
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecNew
            (
                int handle,
                int index
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecNewLock
            (
                int handle,
                int index
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecNvf
            (
                int handle,
                int index
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="mfn"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecRead
            (
                int handle,
                int index,
                int mfn
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="mfn"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecReadLock
            (
                int handle,
                int index,
                int mfn
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="mem"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecShelfSize
            (
                int handle,
                int index,
                int mem
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="tag"></param>
        /// <param name="occurence"></param>
        /// <param name="subField"></param>
        /// <param name="area"></param>
        /// <param name="areaSize"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisRecSubField
            (
                int handle,
                int index,
                int tag,
                int occurence,
                string subField,
                StringBuilder area,
                int areaSize
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="tag"></param>
        /// <param name="occurence"></param>
        /// <param name="subField"></param>
        /// <param name="subOccurence"></param>
        /// <param name="area"></param>
        /// <param name="areaSize"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisRecSubField
            (
                int handle,
                int index,
                int tag,
                int occurence,
                string subField,
                int subOccurence,
                StringBuilder area,
                int areaSize
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecUndelete
            (
                int handle,
                int index
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecUnlock
            (
                int handle,
                int index
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecUnlockForce
            (
                int handle,
                int index
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="sparser"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisRecUpdate
            (
                int handle,
                int index,
                string sparser
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecWrite
            (
                int handle,
                int index
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecWriteLock
            (
                int handle,
                int index
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisRecWriteUnlock
            (
                int handle,
                int index
            );

        #endregion

        #region IsisLnk...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisLnkIfLoad
            (
                int handle
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="reset"></param>
        /// <param name="posts"></param>
        /// <param name="balan"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisLnkIfLoadEx
            (
                int handle,
                int reset,
                int posts,
                int balan
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisLnkSort
            (
                int handle
            );

        #endregion

        #region IsisSrc...

        /// <summary>
        /// Copies the search header control data to an
        /// application program area.
        /// </summary>
        /// <param name="handle">Identifies the ISIS Application.</param>
        /// <param name="tsfnum">Specifies the number (non negative
        /// integer) of the temporary search log file containing
        /// the desired search result.</param>
        /// <param name="searchnum">Specifies the search number
        /// (non negative integer). If this parameter is zero,
        /// the last search submitted will be retrieved.</param>
        /// <param name="sstrup">Specifies the area where
        /// the data will be retrieved.</param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisSrcHeaderMap
            (
                int handle,
                int tsfnum,
                int searchnum,
                ref IsisSearchHeader sstrup
            );

        /// <summary>
        /// Copies a subset of search hits to an application
        /// program area.
        /// </summary>
        /// <param name="handle">Identifies an ISIS Application.</param>
        /// <param name="tsfnum">Specifies the number of the
        /// temporary search log file (non negative integer)
        /// containing the desired search results.</param>
        /// <param name="searchnum">Specifies the search number
        /// (non negative integer). If this parameter is
        /// zero, the last search submitted will be retrieved.</param>
        /// <param name="firstpos">Initial range position of
        /// retrieved directory elements (firstpos > 0).</param>
        /// <param name="lastpos">Last range position in the
        /// list of retrieved directory elements.</param>
        /// <param name="areap">Specifies the area where the data
        /// will be copied.</param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisSrcHitMap
            (
                int handle,
                int tsfnum,
                int searchnum,
                int firstpos,
                int lastpos,
                IsisSearchHit[] areap
            );

        /// <summary>
        /// Deletes the contents of a search log file.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="lognumber"></param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisSrcLogFileFlush
            (
                int handle,
                int lognumber
            );

        /// <summary>
        /// Save permanently the contents of a search log file 
        /// in a given moment.
        /// </summary>
        /// <param name="handle">Identifies an ISIS Application.</param>
        /// <param name="lognumber">Specifies the number (non negative
        /// integer) of the search log file used by the search
        /// algorithmn containing the desired search results.</param>
        /// <param name="filename">Specifies the log file name.</param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSrcLogFileSave
            (
                int handle,
                int lognumber,
                string filename
            );

        /// <summary>
        /// Assigns to a search log file the searchs previously
        /// saved by means of IsisSrcLogFileSave function.
        /// </summary>
        /// <param name="handle">Identifies an ISIS Application.</param>
        /// <param name="filename"></param>
        /// <param name="lognumber"></param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSrcLogFileUse
            (
                int handle,
                string filename,
                int lognumber
            );

        /// <summary>
        /// Copies a subset of mfns from a search hit list and moves
        /// it into an application program area.
        /// </summary>
        /// <param name="handle">Identifies an ISIS Application.</param>
        /// <param name="tsfnum">Specifies the number (non negative 
        /// integer) of the temporary search log file used by the
        /// search algorithm containing the desired search results.
        /// </param>
        /// <param name="searchnum">Specifies the search number
        /// (non negative integer). If this parameter is zero,
        /// the last search submitted will be retrieved.</param>
        /// <param name="firstpos">Initial range position in
        /// the list of retrieved mfns (firstpos >0).</param>
        /// <param name="lastpos">Last range position in the
        /// list of retrieved mfns.</param>
        /// <param name="mfnareap">Specifies an array of
        /// MFN where the data will be copied.</param>
        /// <returns>Returns number of MFNs or error code.</returns>
        [DllImport(DllName)]
        public static extern int IsisSrcMfnMap
            (
                int handle,
                int tsfnum,
                int searchnum,
                int firstpos,
                int lastpos,
                int[] mfnareap
            );

        /// <summary>
        /// Executes a search expression over an ISIS Space inverted 
        /// file and copies basic data on the result to an application
        /// program area.
        /// </summary>
        /// <param name="handle">Identifies the ISIS Space.</param>
        /// <param name="tsfnum">Specifies the number (non negative
        /// integer) of the temporary search log file to be used
        /// by the search algoritm to store search results.</param>
        /// <param name="express">Specifies the search expression
        /// using the CDS/ISIS search language.</param>
        /// <param name="areap">Specifies the area where the data
        /// will be copied.</param>
        /// <returns></returns>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisSrcSearch
            (
                int handle,
                int tsfnum,
                string express,
                ref IsisSearchHeader areap
            );

        /// <summary>
        /// Copies a subset of mfns from a term posting list
        /// and movies it into an application program area.
        /// The posting corresponds to the term stored at
        /// shelf index.
        /// </summary>
        /// <param name="handle">Identifies the ISIS Space.</param>
        /// <param name="index">Specifies the term shelf number.</param>
        /// <param name="firstpos">Initial range position in the
        /// list of retrieved MFNs (firstpos > 0).</param>
        /// <param name="lastpos">Last range position in the list
        /// of retrieved MFNs.</param>
        /// <param name="mfnareap">Specifies an array of MFNs
        /// where the data will be copied.</param>
        /// <returns>Returns number of MFNs or error code.</returns>
        [DllImport(DllName)]
        public static extern int IsisTrmMfnMap
            (
                int handle,
                int index,
                int firstpos,
                int lastpos,
                int[] mfnareap
            );

        /// <summary>
        /// Copies a subset of postings into an application
        /// program area. The postings correspond to the term
        /// stored at shelf index.
        /// </summary>
        /// <param name="handle">Identifies the ISIS Space.</param>
        /// <param name="index">Specifies the term shelf number.</param>
        /// <param name="firstpos">Initial range position in the
        /// list of retrieved postings elements (firstpos > 0).</param>
        /// <param name="lastops">Last range position int the list
        /// of retrieved postings elements.</param>
        /// <param name="parea">Specifies an array of IsisTermPosting
        /// structures where the data will be copied.</param>
        /// <returns>Returns number of of retrieved postings
        /// or error code.</returns>
        [DllImport(DllName)]
        public static extern int IsisTrmPostingMap
            (
                int handle,
                int index,
                int firstpos,
                int lastops,
                IsisTermPosting[] parea
            );

        /// <summary>
        /// Loads an inverted file term into a shelf and
        /// returns the number of postings or an error condition.
        /// </summary>
        /// <param name="handle">Identifies the ISIS Space.</param>
        /// <param name="index">Specifies the term shelf number.</param>
        /// <param name="key">Specifies the IsisTrmRead structure
        /// with the inverted file term to be loaded.</param>
        /// <returns>Returns total number of postings or error code.
        /// </returns>
        /// <remarks>Unless the shelf index had not been previously
        /// allocated with zero bytes, this function loads the
        /// corresponding posting header segment.
        /// IsisTrmReadMap always converts key to uppercase.</remarks>
        [DllImport(DllName)]
        public static extern int IsisTrmReadMap
            (
                int handle,
                int index,
                ref IsisTrmRead key
            );

        /// <summary>
        /// Loads the next inverted file term and returns
        /// the number of postings or an error condition.
        /// </summary>
        /// <param name="handle">Identifies an ISIS Space.</param>
        /// <param name="index">Specifies the term shelf number.</param>
        /// <param name="key">Specifies the IsisTrmReadStructure
        /// where the next inverted file term will be copied.</param>
        /// <returns>Returns total number of postings or error code.
        /// </returns>
        /// <remarks>Unless the shelf index had not been previously
        /// allocated with zero bytes, this function loads the
        /// corresponding posting header segment.
        /// A call to IsisTrmReadNext must be preceded by a call
        /// to IsisTrmReadMap, IsisTrmReadPrevious or
        /// another IsisTrmReadNext.</remarks>
        [DllImport(DllName)]
        public static extern int IsisTrmReadNext
            (
                int handle,
                int index,
                ref IsisTrmRead key
            );

        /// <summary>
        /// Loads the previous inverted file term and returns
        /// the number of postings or an error condition.
        /// </summary>
        /// <param name="handle">Identifies the ISIS Space.</param>
        /// <param name="index">Specifies the term shelf number.</param>
        /// <param name="prefix">Specifies the lower limit for 
        /// the previous key.</param>
        /// <param name="key">Specifies the IsisTrmRead structure
        /// where the previous inverted file term will be copied.
        /// </param>
        /// <returns>Total number of postings or error code.</returns>
        /// <remarks>Unless the shelf index had not been previously
        /// allocated with zero bytes, this function loads the
        /// corresponding posting header segment.
        /// A call to IsisTrmReadPrevious must be preceded by a call
        /// to IsisTrmReadMap, IsisTrmReadNext or
        /// another IsisTrmReadPrevious.</remarks>
        [DllImport(DllName, CharSet = CharSet.Ansi)]
        public static extern int IsisTrmReadPrevious
            (
                int handle,
                int index,
                string prefix,
                ref IsisTrmRead key
            );

        /// <summary>
        /// Changes the size of an inverted file shelf.
        /// </summary>
        /// <param name="handle">Identifies the ISIS Space.</param>
        /// <param name="index">Specifies the term shelf number.</param>
        /// <param name="mem">Specifies the number of bytes 
        /// to be allocated.</param>
        /// <returns></returns>
        [DllImport(DllName)]
        public static extern int IsisTrmShelfSize
            (
                int handle,
                int index,
                int mem
            );

        #endregion
    }
}
