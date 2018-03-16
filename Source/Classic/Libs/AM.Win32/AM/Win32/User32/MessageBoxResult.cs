// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MessageBoxResult.cs -- MessageBox function return codes
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// MessageBox function return codes.
    /// </summary>
    [PublicAPI]
    public enum MessageBoxResult
    {
        /// <summary>
        /// Error occured.
        /// </summary>
        ERROR = 0,

        /// <summary>
        /// OK button was pressed.
        /// </summary>
        IDOK = 1,

        /// <summary>
        /// Cancel button was pressed.
        /// </summary>
        IDCANCEL = 2,

        /// <summary>
        /// Abort button was pressed.
        /// </summary>
        IDABORT = 3,

        /// <summary>
        /// Retry button was pressed.
        /// </summary>
        IDRETRY = 4,

        /// <summary>
        /// Ignore button was pressed.
        /// </summary>
        IDIGNORE = 5,

        /// <summary>
        /// Yes button was pressed.
        /// </summary>
        IDYES = 6,

        /// <summary>
        /// No button was pressed.
        /// </summary>
        IDNO = 7,

        /// <summary>
        /// Close button was pressed.
        /// </summary>
        IDCLOSE = 8,

        /// <summary>
        /// Help button was pressed.
        /// </summary>
        IDHELP = 9,

        /// <summary>
        /// Try Again button was pressed.
        /// </summary>
        IDTRYAGAIN = 10,

        /// <summary>
        /// Continue button was pressed.
        /// </summary>
        IDCONTINUE = 11,

        /// <summary>
        /// Timeout.
        /// </summary>
        IDTIMEOUT = 32000
    }
}
