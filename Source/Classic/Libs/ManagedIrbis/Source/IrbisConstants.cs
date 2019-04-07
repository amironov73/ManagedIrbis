// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisConstants.cs -- common constants
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Common constants.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
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
        /// Длина записи (размер полки) -- ограничение при форматировании.
        /// </summary>
        public const int MaxRecord = 32000;

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
