// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisConstants.cs -- common constants
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace UnsafeIrbis
{
    /// <summary>
    /// Common constants.
    /// </summary>
    [PublicAPI]
    public static class IrbisConstants
    {
        #region Constants

        /// <summary>
        /// Database list for administrator.
        /// </summary>
        public const string AdministratorDatabaseList = "dbnam1.mnu";

        /// <summary>
        /// Database list for cataloger.
        /// </summary>
        public const string CatalogerDatabaseList = "dbnam2.mnu";

        /// <summary>
        /// Max postings in the packet.
        /// </summary>
        public const int MaxPostings = 32758;

        /// <summary>
        /// Database list for reader.
        /// </summary>
        public const string ReaderDatabaseList = "dbnam3.mnu";

        #endregion
    }
}
