// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsyncExtensions.cs -- extension methods for async calls
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4

#region Using directives

using System.Threading.Tasks;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Extension methods for async calls.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class AsyncExtensions
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Connect.
        /// </summary>
        [NotNull]
        public static Task<IniFile> ConnectAsync
            (
                [NotNull] this IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            // ReSharper disable ConvertClosureToMethodGroup
            Task<IniFile> result = Task.Factory.StartNew
                (
                    () => connection.Connect()
                );
            // ReSharper restore ConvertClosureToMethodGroup

            return result;
        }

        /// <summary>
        /// Disconnect.
        /// </summary>
        [NotNull]
        public static Task DisconnectAsync
            (
                [NotNull] this IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Task result = Task.Factory.StartNew
                (
                    connection.Dispose
                );

            return result;
        }

        /// <summary>
        /// Format record.
        /// </summary>
        [NotNull]
        public static Task<string> FormatRecordAsync
            (
                [NotNull] this IrbisConnection connection,
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
                );

            return result;
        }

        /// <summary>
        /// Format record.
        /// </summary>
        [NotNull]
        public static Task<string> FormatRecordAsync
            (
                [NotNull] this IrbisConnection connection, 
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
                );

            return result;
        }

        /// <summary>
        /// Get max MFN.
        /// </summary>
        [NotNull]
        public static Task<int> GetMaxMfnAsync
            (
                [NotNull] this IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Task<int> result = Task.Factory.StartNew
                (
                    connection.GetMaxMfn
                );

            return result;
        }

        /// <summary>
        /// List databases.
        /// </summary>
        [NotNull]
        public static Task<DatabaseInfo[]> ListDatabasesAsync
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string menuName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(menuName, "menuName");

            Task<DatabaseInfo[]> result = Task.Factory.StartNew
                (
                    () => connection.ListDatabases(menuName)
                );

            return result;
        }

        /// <summary>
        /// Empty operation.
        /// </summary>
        [NotNull]
        public static Task NoOpAsync
            (
                [NotNull] this IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Task result = Task.Factory.StartNew
            (
                connection.NoOp
            );

            return result;
        }

        /// <summary>
        /// Read INI-file.
        /// </summary>
        [NotNull]
        public static Task<IniFile> ReadIniFileAsync
            (
                [NotNull] this IrbisConnection connection, 
                [NotNull] string fileName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(fileName, "fileName");

            Task<IniFile> result = Task.Factory.StartNew
            (
                () => connection.ReadIniFile(fileName)
            );

            return result;
        }

        /// <summary>
        /// Read menu.
        /// </summary>
        [NotNull]
        public static Task<MenuFile> ReadMenuAsync
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string menuName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(menuName, "menuName");

            Task<MenuFile> result = Task.Factory.StartNew
            (
                () => connection.ReadMenu(menuName)
            );

            return result;
        }

        /// <summary>
        /// Read terms.
        /// </summary>
        [NotNull]
        public static Task<TermInfo[]> ReadTermsAsync
            (
                [NotNull] this IrbisConnection connection, 
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
                );

            return result;
        }

        /// <summary>
        /// Read search scenario from given file.
        /// </summary>
        [NotNull]
        public static Task<SearchScenario[]> ReadSearchScenarioAsync
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string fileName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(fileName, "fileName");

            Task<SearchScenario[]> result = Task.Factory.StartNew
                (
                    () => connection.ReadSearchScenario(fileName)
                );

            return result;
        }

        /// <summary>
        /// Read record.
        /// </summary>
        [NotNull]
        public static Task<MarcRecord> ReadRecordAsync
            (
                [NotNull] this IrbisConnection connection,
                int mfn
            )
        {
            Code.NotNull(connection, "connection");
            Code.Positive(mfn, "mfn");

            Task<MarcRecord> result = Task.Factory.StartNew
                (
                    () => connection.ReadRecord(mfn)
                );

            return result;
        }

        /// <summary>
        /// Read text file.
        /// </summary>
        [NotNull]
        public static Task<string> ReadTextFileAsync
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] FileSpecification specification
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(specification, "connection");

            Task<string> result = Task.Factory.StartNew
                (
                    () => connection.ReadTextFile(specification)
                );

            return result;
        }

        /// <summary>
        /// Search records.
        /// </summary>
        [NotNull]
        public static Task<int[]> SearchAsync
            (
                [NotNull] this IrbisConnection connection, 
                [NotNull] string expression
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(expression, "expression");

            Task<int[]> result = Task.Factory.StartNew
                (
                    () => connection.Search(expression)
                );

            return result;
        }

        /// <summary>
        /// Write record.
        /// </summary>
        [NotNull]
        public static Task<MarcRecord> WriteRecordAsync
            (
                [NotNull] this IrbisConnection connection, 
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(record, "record");

            Task<MarcRecord> result = Task.Factory.StartNew
                (
                    () => connection.WriteRecord(record)
                );

            return result;
        }

        #endregion
    }
}

#endif
