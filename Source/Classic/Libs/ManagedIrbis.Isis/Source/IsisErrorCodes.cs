// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IsisErrorCodes.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis.Isis
{
    /// <summary>
    /// Error codes for ISIS32.DLL.
    /// </summary>
    [Flags]
    public enum IsisErrorCodes
    {
        #region No errors

        /// <summary>
        /// No errors.
        /// </summary>
        ZERO = 0,

        /// <summary>
        /// No errors.
        /// </summary>
        NO_ERROR = 0,

        /// <summary>
        /// No errors.
        /// </summary>
        NO_ERRORS = 0,

        #endregion

        #region Database errors

        /// <summary>
        /// Database access denied (data entry lock).
        /// </summary>
        ERR_DBDELOCK = -101,

        /// <summary>
        /// Database access denied (probably exclusive write lock).
        /// </summary>
        ERR_DBEWLOCK = -102,

        /// <summary>
        /// Database access is single-user.
        /// </summary>
        ERR_DBMONOUSR = -103,

        /// <summary>
        /// Database access is multi-user.
        /// </summary>
        ERR_DBMULTUSR = -104,

        #endregion

        #region File manipulation errors

        /// <summary>
        /// File create error.
        /// </summary>
        ERR_FILECREATE = -201,

        /// <summary>
        /// File delete error.
        /// </summary>
        ERR_FILEDELETE = -202,

        /// <summary>
        /// File empty.
        /// </summary>
        ERR_FILEEMPTY = -203,

        /// <summary>
        /// File flush error.
        /// </summary>
        ERR_FILEFLUSH = -204,

        /// <summary>
        /// File doesn't exist (fmt).
        /// </summary>
        ERR_FILEFMT = -205,

        /// <summary>
        /// File doesn't exist (fst).
        /// </summary>
        ERR_FILEFST = -206,

        /// <summary>
        /// File doesn't exist (inverted).
        /// </summary>
        ERR_FILEINVERT = -207,

        /// <summary>
        /// File doesn't exist (ISO).
        /// </summary>
        ERR_FILEISO = -208,

        /// <summary>
        /// File doesn't exist (master).
        /// </summary>
        ERR_FILEMASTER = -209,

        /// <summary>
        /// File missing.
        /// </summary>
        ERR_FILEMISSING = -210,

        /// <summary>
        /// File open error.
        /// </summary>
        ERR_FILEOPEN = -211,

        /// <summary>
        /// File doesn't exist (pft).
        /// </summary>
        ERR_FILEPFT = -212,

        /// <summary>
        /// File read error.
        /// </summary>
        ERR_FILEREAD = -213,

        /// <summary>
        /// File rename error.
        /// </summary>
        ERR_FILERENAME = -214,

        /// <summary>
        /// File doesn't exist (stw).
        /// </summary>
        ERR_FILESTW = -215,

        /// <summary>
        /// File write error.
        /// </summary>
        ERR_FILEWRITE = -216,

        /// <summary>
        /// File end.
        /// </summary>
        ERR_FILEEOF = -217,

        #endregion

        #region Low level engine errors

        /// <summary>
        /// Cisis low level error trap.
        /// </summary>
        ERR_LLCISISETRAP = -301,

        /// <summary>
        /// Isis low level error trap.
        /// </summary>
        ERR_LLISISETRAP = -302,

        /// <summary>
        /// JIsis low level error trap.
        /// </summary>
        ERR_LLJISISETRAP = -303,

        /// <summary>
        /// Corba low level error trap.
        /// </summary>
        ERR_LLCORBAETRAP = -304,

        /// <summary>
        /// PHP low level error trap.
        /// </summary>
        ERR_LLPHPISISETRAP = -305,

        #endregion

        #region Memory manipulation errors

        /// <summary>
        /// Memory allocation error.
        /// </summary>
        ERR_MEMALLOCAT = -401,

        #endregion

        #region Parameter specification errors

        /// <summary>
        /// Invalid application handle.
        /// </summary>
        ERR_PARAPPHAND = -501,

        /// <summary>
        /// Invalid file name size.
        /// </summary>
        ERR_PARFILNSIZ = -502,

        /// <summary>
        /// Syntax error (field update).
        /// </summary>
        ERR_PARFLDSYNT = -503,

        /// <summary>
        /// Syntax error (format).
        /// </summary>
        ERR_PARFMTSYNT = -504,

        /// <summary>
        /// Null pointer.
        /// </summary>
        ERR_PARNULLPNT = -505,

        /// <summary>
        /// String with zero size.
        /// </summary>
        ERR_PARNULLSTR = -506,

        /// <summary>
        /// Parameter out of range.
        /// </summary>
        ERR_PAROUTRANG = -507,

        /// <summary>
        /// Invalid space handle.
        /// </summary>
        ERR_PARSPAHAND = -508,

        /// <summary>
        /// Syntax error (search).
        /// </summary>
        ERR_PARSRCSYNT = -509,

        /// <summary>
        /// Invalid subfield specification.
        /// </summary>
        ERR_PARSUBFSPC = -510,

        /// <summary>
        /// Syntax error (record update).
        /// </summary>
        ERR_PARUPDSYNT = -511,

        #endregion

        #region Record errors

        /// <summary>
        /// Record eof: found eof in database.
        /// </summary>
        ERR_RECEOF = -601,

        /// <summary>
        /// Record locked.
        /// </summary>
        ERR_RECLOCKED = -602,

        /// <summary>
        /// Record logically deleted.
        /// </summary>
        ERR_RECLOGIDEL = -603,

        /// <summary>
        /// Record condition is not RCNORMAL.
        /// </summary>
        ERR_RECNOTNORM = -604,

        /// <summary>
        /// Record physically deleted.
        /// </summary>
        ERR_RECPHYSDEL = -605,

        #endregion

        #region Term errors

        /// <summary>
        /// Term eof: found eof in database.
        /// </summary>
        ERR_TRMEOF = -701,

        /// <summary>
        /// Term next: key not found.
        /// </summary>
        ERR_TRMNEXT = -702,

        #endregion

        #region Unexpected errors

        /// <summary>
        /// Unexpected error.
        /// </summary>
        ERR_UNEXPECTED = -999

        #endregion
    }
}
