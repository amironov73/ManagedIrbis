// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LogManager.cs -- manager for LOGDB records
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Manager for LOGDB records.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class LogManager
    {
        #region Constants

        /// <summary>
        /// Database name.
        /// </summary>
        public const string DatabaseName = "LOGDB";

        #endregion

        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IIrbisConnection Connection { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LogManager
            (
                [NotNull] IIrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get records for the dates.
        /// </summary>
        [NotNull]
        public LogRecord[] ForDates
            (
                DateTime fromDate,
                DateTime toDate
            )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
