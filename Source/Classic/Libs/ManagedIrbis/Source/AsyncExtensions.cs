// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsyncExtensions.cs -- extension methods for async calls
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4 || NETCORE || DROID || UAP

#region Using directives

using System.Threading.Tasks;

using AM.IO;
using AM.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedIrbis
{
    /// <summary>
    /// Extension methods for async calls.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class AsyncExtensions
    {
        #region Public methods

        /// <summary>
        /// Connect.
        /// </summary>
        [NotNull]
        public static Task<IniFile> ConnectAsync
            (
                [NotNull] this IIrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Task<IniFile> result = Task.Factory.StartNew
                (
                    () => connection.Connect()
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Disconnect.
        /// </summary>
        [NotNull]
        public static Task DisconnectAsync
            (
                [NotNull] this IIrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Task result = Task.Factory.StartNew
                (
                    connection.Dispose
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Format record.
        /// </summary>
        [NotNull]
        public static Task<string> FormatRecordAsync
            (
                [NotNull] this IIrbisConnection connection,
                [NotNull] string format,
                int mfn
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(format, "format");
            Code.Positive(mfn, "mfn");

            Task<string> result = Task.Factory.StartNew
                (
                    () => connection.FormatRecord(format, mfn)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Format record.
        /// </summary>
        [NotNull]
        public static Task<string> FormatRecordAsync
            (
                [NotNull] this IIrbisConnection connection, 
                [NotNull] string format, 
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(format, "format");
            Code.NotNull(record, "record");

            Task<string> result = Task.Factory.StartNew
                (
                    () => connection.FormatRecord(format, record)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Get max MFN.
        /// </summary>
        [NotNull]
        public static Task<int> GetMaxMfnAsync
            (
                [NotNull] this IIrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Task<int> result = Task.Factory.StartNew
                (
                    () => connection.GetMaxMfn()
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// List databases.
        /// </summary>
        [NotNull]
        public static Task<DatabaseInfo[]> ListDatabasesAsync
            (
                [NotNull] this IIrbisConnection connection,
                [NotNull] string menuName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(menuName, "menuName");

            Task<DatabaseInfo[]> result = Task.Factory.StartNew
                (
                    () => connection.ListDatabases(menuName)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Empty operation.
        /// </summary>
        [NotNull]
        public static Task NoOpAsync
            (
                [NotNull] this IIrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Task result = Task.Factory.StartNew
                (
                    connection.NoOp
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Read INI-file.
        /// </summary>
        [NotNull]
        public static Task<IniFile> ReadIniFileAsync
            (
                [NotNull] this IIrbisConnection connection, 
                [NotNull] string fileName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(fileName, "fileName");

            Task<IniFile> result = Task.Factory.StartNew
                (
                    () => connection.ReadIniFile(fileName)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Read menu.
        /// </summary>
        [NotNull]
        public static Task<MenuFile> ReadMenuAsync
            (
                [NotNull] this IIrbisConnection connection,
                [NotNull] string menuName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(menuName, "menuName");

            Task<MenuFile> result = Task.Factory.StartNew
                (
                    () => connection.ReadMenu(menuName)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Read terms.
        /// </summary>
        [NotNull]
        public static Task<TermInfo[]> ReadTermsAsync
            (
                [NotNull] this IIrbisConnection connection, 
                [NotNull] string start, 
                int count
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(start, "start");
            Code.Nonnegative(count, "count");

            TermParameters parameters = new TermParameters
            {
                Database = connection.Database,
                StartTerm = start,
                NumberOfTerms = count
            };

            Task<TermInfo[]> result = Task.Factory.StartNew
                (
                    () => connection.ReadTerms(parameters)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Read search scenario from given file.
        /// </summary>
        [NotNull]
        public static Task<SearchScenario[]> ReadSearchScenarioAsync
            (
                [NotNull] this IIrbisConnection connection,
                [NotNull] string fileName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(fileName, "fileName");

            Task<SearchScenario[]> result = Task.Factory.StartNew
                (
                    () => connection.ReadSearchScenario(fileName)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Read record.
        /// </summary>
        [NotNull]
        public static Task<MarcRecord> ReadRecordAsync
            (
                [NotNull] this IIrbisConnection connection,
                int mfn
            )
        {
            Code.NotNull(connection, "connection");
            Code.Positive(mfn, "mfn");

            Task<MarcRecord> result = Task.Factory.StartNew
                (
                    () => connection.ReadRecord(mfn)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Read text file.
        /// </summary>
        [NotNull]
        public static Task<string> ReadTextFileAsync
            (
                [NotNull] this IIrbisConnection connection,
                [NotNull] FileSpecification specification
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(specification, "connection");

            Task<string> result = Task.Factory.StartNew
                (
                    () => connection.ReadTextFile(specification)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Search records.
        /// </summary>
        [NotNull]
        public static Task<int[]> SearchAsync
            (
                [NotNull] this IIrbisConnection connection, 
                [NotNull] string expression
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(expression, "expression");

            Task<int[]> result = Task.Factory.StartNew
                (
                    () => connection.Search(expression)
                )
                .ConfigureSafe();

            return result;
        }

        /// <summary>
        /// Write record.
        /// </summary>
        [NotNull]
        public static Task<MarcRecord> WriteRecordAsync
            (
                [NotNull] this IIrbisConnection connection, 
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(record, "record");

            Task<MarcRecord> result = Task.Factory.StartNew
                (
                    () => connection.WriteRecord(record)
                )
                .ConfigureSafe();

            return result;
        }

        #endregion
    }
}

#endif
