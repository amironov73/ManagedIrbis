/* IrbisConnectionUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.Text;

#if !NETCORE && !WINMOBILE && !PocketPC && !SILVERLIGHT && !ANDROID && !UAP && !WIN81
using AM.Configuration;
#endif

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Batch;
using ManagedIrbis.Menus;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Infrastructure.Sockets;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisConnectionUtility
    {
        #region Constants

        #endregion

        #region Properties

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Актуализация всех неактуализированных записей
        /// в указанной базе данных.
        /// </summary>
        public static void ActualizeDatabase
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string database
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");

            connection.ActualizeRecord
                (
                    database,
                    0
                );
        }

        // =========================================================

        /// <summary>
        /// Delete given record (mark as deleted on the server).
        /// </summary>
        public static void DeleteRecord
            (
                [NotNull] this IrbisConnection connection,
                int mfn
            )
        {
            Code.NotNull(connection, "connection");

            MarcRecord record = connection.ReadRecord(mfn);
            if (!record.Deleted)
            {
                record.Deleted = true;
                connection.WriteRecord(record);
            }
        }

        // ========================================================

        /// <summary>
        /// Delete some records (mark as deleted on the server).
        /// </summary>
        public static void DeleteRecords
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string database,
                [NotNull] IEnumerable<int> mfnList
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(mfnList, "mfnList");

            MarcRecord[] records = connection.ReadRecords
                (
                    database,
                    mfnList
                );

            if (records.Length == 0)
            {
                return;
            }

            MarcRecord[] liveRecords = records
                .Where(record => !record.Deleted)
                .ToArray();

            foreach (MarcRecord record in liveRecords)
            {
                record.Deleted = true;
                connection.WriteRecord(record);
            }
        }

        // ========================================================

        /// <summary>
        /// Execute arbitrary command.
        /// </summary>
        [NotNull]
        public static ServerResponse ExecuteArbitraryCommand
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string commandCode,
                params object[] arguments
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(commandCode, "commandCode");

            UniversalCommand command = connection.CommandFactory
                .GetUniversalCommand
                (
                    commandCode,
                    arguments
                );
            command.AcceptAnyResponse = true;

            ServerResponse result = connection.ExecuteCommand(command);

            return result;
        }

        // ========================================================

        /// <summary>
        /// Format specified record using UTF8 encoding.
        /// </summary>
        public static string FormatUtf8
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string format,
                int mfn
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(format, "format");
            Code.Positive(mfn, "mfn");

            FormatCommand command = connection.CommandFactory
                .GetFormatCommand();
            command.FormatSpecification = format;
            command.UtfFormat = true;
            command.MfnList.Add(mfn);

            connection.ExecuteCommand(command);

            string result = command.FormatResult
                .ThrowIfNullOrEmpty("command.FormatResult")
                [0];

            return result;

        }

        /// <summary>
        /// Format specified record using UTF8 encoding.
        /// </summary>
        public static string FormatUtf8
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string format,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(format, "format");
            Code.NotNull(record, "record");

            FormatCommand command = connection.CommandFactory
                .GetFormatCommand();
            command.FormatSpecification = format;
            command.UtfFormat = true;
            command.VirtualRecord = record;

            connection.ExecuteCommand(command);

            string result = command.FormatResult
                .ThrowIfNullOrEmpty("command.FormatResult")
                [0];

            return result;

        }

        /// <summary>
        /// Format specified records using UTF8 encoding.
        /// </summary>
        [NotNull]
        public static string[] FormatUtf8
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string database,
                [NotNull] string format,
                [NotNull] IEnumerable<int> mfnList
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(mfnList, "mfnList");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(format, "format");

            FormatCommand command = connection.CommandFactory
                .GetFormatCommand();
            command.Database = database;
            command.FormatSpecification = format;
            command.UtfFormat = true;
            command.MfnList.AddRange(mfnList);

            if (command.MfnList.Count == 0)
            {
                return new string[0];
            }

            connection.ExecuteCommand(command);

            string[] result = command.FormatResult
                .ThrowIfNull("command.FormatResult");

            return result;
        }

#if !NETCORE && !WINMOBILE && !PocketPC && !SILVERLIGHT && !ANDROID && !UAP && !WIN81

        // ========================================================

        /// <summary>
        /// Получаем строку подключения в app.settings.
        /// </summary>
        public static string GetStandardConnectionString()
        {
            return ConfigurationUtility.FindSetting
                (
                    ListStandardConnectionStrings()
                );
        }

        // ========================================================

        /// <summary>
        /// Получаем уже подключенного клиента.
        /// </summary>
        /// <exception cref="IrbisException">
        /// Если строка подключения в app.settings не найдена.
        /// </exception>
        public static IrbisConnection GetClientFromConfig()
        {
            string connectionString = GetStandardConnectionString();
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new IrbisException
                    (
                    "Connection string not specified!"
                    );
            }

            IrbisConnection result = new IrbisConnection(connectionString);

            return result;
        }

#endif

        // =========================================================

        /// <summary>
        /// Get list of the databases.
        /// </summary>
        [NotNull]
        public static DatabaseInfo[] ListDatabases
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string listFile
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(listFile, "listFile");

            string menuFile = connection.ReadTextFile
                (
                    IrbisPath.Data,
                    listFile
                );
            string[] lines = menuFile.SplitLines();
            DatabaseInfo[] result
                = DatabaseInfo.ParseMenu(lines);

            return result;
        }

        /// <summary>
        /// Get list of the databases.
        /// </summary>
        [NotNull]
        public static DatabaseInfo[] ListDatabases
            (
                [NotNull] this IrbisConnection connection
            )
        {
            return ListDatabases
                (
                    connection,
                    IrbisConstants.AdministratorDatabaseList
                );
        }


        // ========================================================

        /// <summary>
        /// Стандартные наименования для ключа строки подключения
        /// к серверу ИРБИС64.
        /// </summary>
        public static string[] ListStandardConnectionStrings()
        {
            return new[]
            {
                "irbis-connection",
                "irbis-connection-string",
                "irbis64-connection",
                "irbis64",
                "connection-string"
            };
        }

        // ========================================================

        /// <summary>
        /// Lock the record on the server.
        /// </summary>
        public static void LockRecord
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string database,
                int mfn
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");

            ReadRawRecord
                (
                    connection,
                    database,
                    mfn,
                    true,
                    null
                );
        }

        // ========================================================

        /// <summary>
        /// Lock some records on the server.
        /// </summary>
        public static void LockRecords
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string database,
                [NotNull] int[] mfnList
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(mfnList, "mfnList");

            foreach (int mfn in mfnList)
            {
                ReadRawRecord
                    (
                        connection,
                        database,
                        mfn,
                        true,
                        null
                    );
            }
        }

        // ========================================================

        /// <summary>
        /// Read menu from server.
        /// </summary>
        [NotNull]
        public static MenuFile ReadMenu
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string fileName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(fileName, "fileName");

            FileSpecification fileSpecification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    connection.Database,
                    fileName
                );
            string text = connection.ReadTextFile(fileSpecification);
            MenuFile result = MenuFile.ParseServerResponse
                (
                    text.ThrowIfNull("text")
                );

            return result;
        }

        // ========================================================

        /// <summary>
        /// Read server representation of record from server.
        /// </summary>
        [CanBeNull]
        public static RawRecord ReadRawRecord
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string database,
                int mfn
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");

            ReadRawRecordCommand command
                = connection.CommandFactory.GetReadRawRecordCommand();
            command.Database = database;
            command.Mfn = mfn;

            connection.ExecuteCommand(command);

            return command.RawRecord;
        }

        // ========================================================

        /// <summary>
        /// Read server representation of record from server.
        /// </summary>
        [CanBeNull]
        public static RawRecord ReadRawRecord
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string database,
                int mfn,
                bool lockFlag,
                [CanBeNull] string format
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");

            ReadRawRecordCommand command
                = connection.CommandFactory.GetReadRawRecordCommand();
            command.Database = database;
            command.Mfn = mfn;
            command.Lock = lockFlag;
            command.Format = format;

            connection.ExecuteCommand(command);

            return command.RawRecord;
        }

        // ========================================================

        /// <summary>
        /// Read server representation of record from server.
        /// </summary>
        [CanBeNull]
        public static RawRecord ReadRawRecord
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string database,
                int mfn,
                int version,
                [CanBeNull] string format
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");

            ReadRawRecordCommand command
                = connection.CommandFactory.GetReadRawRecordCommand();
            command.Database = database;
            command.Mfn = mfn;
            command.VersionNumber = version;
            command.Format = format;

            connection.ExecuteCommand(command);

            return command.RawRecord;
        }

        // ========================================================

        /// <summary>
        /// Read server representation of record from server.
        /// </summary>
        [NotNull]
        public static string[] ReadRawRecords
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string database,
                [NotNull] int[] mfnList
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(mfnList, "mfnList");

            if (mfnList.Length == 0)
            {
                return new string[0];
            }

            List<object> arguments = new List<object>
                (
                    mfnList.Length + 3
                )
            {
                database,
                IrbisFormat.All,
                mfnList.Length
            };
            foreach (int mfn in mfnList)
            {
                arguments.Add(mfn);
            }

            UniversalCommand command
                = connection.CommandFactory.GetUniversalCommand
                    (
                        CommandCode.FormatRecord,
                        arguments.ToArray()
                    );
            command.AcceptAnyResponse = true;

            ServerResponse response = connection.ExecuteCommand(command);
            
            List<string> result = response.RemainingUtfStrings();

            return result.ToArray();
        }

        // ========================================================

        /// <summary>
        /// Read multiple records.
        /// </summary>
        [NotNull]
        public static MarcRecord[] ReadRecords
            (
                [NotNull] this IrbisConnection connection,
                [CanBeNull] string database,
                [NotNull] IEnumerable<int> mfnList
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(mfnList, "mfnList");

            BatchAccessor batch = new BatchAccessor(connection);
            MarcRecord[] result = batch.ReadRecords
                (
                    database,
                    mfnList
                );

            return result;
        }

        // ========================================================

        /// <summary>
        /// Remove logging from socket.
        /// </summary>
        public static void RemoveLogging
            (
                [NotNull] this IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

#if !WINMOBILE && !PocketPC && !WIN81

            LoggingClientSocket oldSocket = connection.Socket
                as LoggingClientSocket;
            if (!ReferenceEquals(oldSocket, null))
            {
                AbstractClientSocket newSocket = oldSocket.InnerSocket;
                connection.SetSocket(newSocket);
            }

#endif
        }

        // ========================================================

        /// <summary>
        /// Retrieve history for given record.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static MarcRecord[] RecordHistory
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string database,
                int mfn,
                [CanBeNull] string format
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.Positive(mfn, "mfn");

            List<MarcRecord> result = new List<MarcRecord>();
            MarcRecord record = connection.ReadRecord
                (
                    database,
                    mfn,
                    false,
                    format
                );
            result.Add(record);

            for (int version = 2; version < int.MaxValue; version++)
            {
                record = connection.ReadRecord
                    (
                        database,
                        mfn,
                        version,
                        format
                    );
                if (ReferenceEquals(record, null))
                {
                    break;
                }
                result.Add(record);
            }

            return result.ToArray();
        }

        // ========================================================

        /// <summary>
        /// Чтение записи.
        /// </summary>
        [NotNull]
        public static MarcRecord ReadRecord
            (
                [NotNull] this IrbisConnection connection,
                int mfn
            )
        {
            Code.NotNull(connection, "connection");

            MarcRecord result = connection.ReadRecord
                (
                    connection.Database,
                    mfn,
                    false,
                    null
                );

            return result;
        }

        // ========================================================

        /// <summary>
        /// Read text file from the server.
        /// </summary>
        [CanBeNull]
        public static string ReadTextFile
            (
                [NotNull] this IrbisConnection connection,
                IrbisPath path,
                [NotNull] string fileName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(fileName, "fileName");

            FileSpecification fileSpecification = new FileSpecification
                (
                    path,
                    connection.Database,
                    fileName
                );

            string result = connection.ReadTextFile(fileSpecification);

            return result;
        }

        // ========================================================

#if !NETCORE && !SILVERLIGHT && !UAP && !WIN81

        /// <summary>
        /// Require minimal client version.
        /// </summary>
        public static bool RequireClientVersion
            (
                [CanBeNull] this IrbisConnection connection,
                [NotNull] string minimalVersion,
                bool throwException
            )
        {
            Code.NotNullNorEmpty(minimalVersion, "minimalVersion");

            Version requiredVersion = new Version(minimalVersion);
            Version actualVersion = IrbisConnection.ClientVersion;
            bool result = actualVersion
                .CompareTo(requiredVersion) >= 0;

            if (!result
                 && throwException
                )
            {
                string message = string.Format
                    (
                        "Required client version {0}, found version {1}",
                        minimalVersion,
                        actualVersion
                    );
                throw new IrbisException(message);
            }

            return result;
        }

#endif

        // ========================================================

        /// <summary>
        /// Require minimal server version.
        /// </summary>
        public static bool RequireServerVersion
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string minimalVersion,
                bool throwException
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(minimalVersion, "minimalVersion");

            IrbisVersion actualVersion
                = connection.GetServerVersion();
            bool result = string.CompareOrdinal
                (
                    actualVersion.Version,
                    "64." + minimalVersion
                ) >= 0;

            if (!result
                 && throwException
                )
            {
                string message = string.Format
                    (
                        "Required server version {0}, found version {1}",
                        minimalVersion,
                        actualVersion.Version
                    );
                throw new IrbisException(message);
            }

            return result;
        }

        // ========================================================

        /// <summary>
        /// Search for records with formattable expression.
        /// </summary>
        [NotNull]
        [StringFormatMethod("format")]
        public static int[] Search
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(format, "format");

            string expression = string.Format
                (
                    format,
                    args
                );

            return connection.Search(expression);
        }

        // ========================================================

        /// <summary>
        /// Определение количества записей, которые будут
        /// найдены по указанному выражению.
        /// </summary>
        public static int SearchCount
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string searchExpression
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(searchExpression, "searchExpression");

            SearchCommand command
                = connection.CommandFactory.GetSearchCommand();
            command.Database = connection.Database;
            command.SearchExpression = searchExpression;
            command.FirstRecord = 0;

            connection.ExecuteCommand(command);
            
            int result = command.FoundCount;

            return result;
        }

        // ========================================================

        /// <summary>
        /// Поиск с одновременным расформатированием.
        /// Для формата используется кодировка ANSI.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static FoundItem[] SearchFormat
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string searchExpression,
                [NotNull] string formatSpecification
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(searchExpression, "searchExpression");
            Code.NotNullNorEmpty(formatSpecification, "formatSpecification");

            SearchCommand command
                = connection.CommandFactory.GetSearchCommand();
            command.Database = connection.Database;
            command.SearchExpression = searchExpression;
            command.FormatSpecification = formatSpecification;

            connection.ExecuteCommand(command);
            
            FoundItem[] result = command.Found
                .ThrowIfNull("command.Found")
                .ToArray();

            return result;
        }

        /// <summary>
        /// Поиск с одновременным расформатированием.
        /// Для формата используется кодировка UTF-8.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static FoundItem[] SearchFormatUtf8
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string searchExpression,
                [NotNull] string formatSpecification
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(searchExpression, "searchExpression");
            Code.NotNullNorEmpty(formatSpecification, "formatSpecification");

            SearchCommand command
                = connection.CommandFactory.GetSearchCommand();
            command.Database = connection.Database;
            command.SearchExpression = searchExpression;
            command.FormatSpecification = formatSpecification;
            command.UtfFormat = true;

            connection.ExecuteCommand(command);

            FoundItem[] result = command.Found
                .ThrowIfNull("command.Found")
                .ToArray();

            return result;
        }

        // ========================================================

        /// <summary>
        /// Raw search.
        /// </summary>
        [NotNull]
        public static string[] SearchRaw
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] SearchParameters parameters
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(parameters, "parameters");

            SearchRawCommand command = connection.CommandFactory
                .GetSearchRawCommand();
            command.ApplyParameters(parameters);

            connection.ExecuteCommand(command);
            string[] result = command.Found
                .ThrowIfNull("command.Found");
            
            return result;
        }

        // ========================================================

        /// <summary>
        /// Загрузка записей по результатам поиска.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        [StringFormatMethod("format")]
        public static MarcRecord[] SearchRead
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(format, "format");

            string expression = string.Format
                (
                    format,
                    args
                );

            SearchReadCommand command
                = connection.CommandFactory.GetSearchReadCommand();
            command.Database = connection.Database;
            command.SearchExpression = expression;

            connection.ExecuteCommand(command);

            MarcRecord[] result = command.Records
                .ThrowIfNull("command.Records");

            return result;
        }

        // ========================================================

        /// <summary>
        /// Загрузка одной записи по результатам поиска.
        /// </summary>
        [CanBeNull]
        [StringFormatMethod("format")]
        public static MarcRecord SearchReadOneRecord
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(format, "format");

            string expression = string.Format
                (
                    format,
                    args
                );
            SearchReadCommand command
                = connection.CommandFactory.GetSearchReadCommand();
            command.Database = connection.Database;
            command.SearchExpression = expression;
            command.NumberOfRecords = 1;

            connection.ExecuteCommand(command);
            
            MarcRecord result = command.Records
                .ThrowIfNull("command.Records")
                .GetItem(0);

            return result;
        }

        // ========================================================

        /// <summary>
        /// Raw sequential search.
        /// </summary>
        [NotNull]
        public static string[] SequentialSearchRaw
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string database,
                [NotNull] string expression,
                int firstRecord,
                int numberOfRecords,
                int minMfn,
                int maxMfn,
                [NotNull] string sequential,
                [CanBeNull] string format
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNullNorEmpty(expression, "expression");
            Code.NotNullNorEmpty(sequential, "sequential");

            UniversalCommand command
                = connection.CommandFactory.GetUniversalCommand
                (
                    CommandCode.Search,
                    database,
                    new TextWithEncoding (expression, IrbisEncoding.Utf8),
                    numberOfRecords,
                    firstRecord,
                    new TextWithEncoding(format, IrbisEncoding.Ansi),
                    minMfn,
                    maxMfn,
                    new TextWithEncoding(sequential, IrbisEncoding.Utf8)
                );

            ServerResponse response = connection.ExecuteCommand(command);

            List<string> result = response.RemainingUtfStrings();

            return result.ToArray();
        }

        // ========================================================

        /// <summary>
        /// Undelete given record (mark as live on the server).
        /// </summary>
        public static void UndeleteRecord
            (
                [NotNull] this IrbisConnection connection,
                int mfn
            )
        {
            Code.NotNull(connection, "connection");

            MarcRecord record = connection.ReadRecord(mfn);
            if (record.Deleted)
            {
                record.Deleted = false;
                connection.WriteRecord(record);
            }
        }

        // ========================================================

        /// <summary>
        /// Undelete some records (mark as live on the server).
        /// </summary>
        public static void UndeleteRecords
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string database,
                [NotNull] IEnumerable<int> mfnList
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(mfnList, "mfnList");

            MarcRecord[] records = connection.ReadRecords
                (
                    database,
                    mfnList
                );

            if (records.Length == 0)
            {
                return;
            }

            MarcRecord[] deletedRecords = records
                .Where(record => record.Deleted)
                .ToArray();

            foreach (MarcRecord record in deletedRecords)
            {
                record.Deleted = false;
                connection.WriteRecord(record);
            }
        }

        // ========================================================

        /// <summary>
        /// Unlock record through E command.
        /// </summary>
        public static bool UnlockRecordAlternative
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string databaseName,
                int mfn
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(databaseName, "databaseName");

            ServerResponse response = connection.ExecuteArbitraryCommand
                (
                    "E",
                    databaseName,
                    mfn
                );
            bool result = response.GetReturnCode() >= 0;

            return result;
        }

        // ========================================================

        /// <summary>
        /// Write record in raw representation.
        /// </summary>
        [NotNull]
        public static string WriteRawRecord
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string database,
                [NotNull] string record,
                bool lockFlag,
                bool actualize
            )
        {
            // TODO Create WriteRawRecordCommand

            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNullNorEmpty(record, "record");

            UniversalCommand command
                = connection.CommandFactory.GetUniversalCommand
                (
                    CommandCode.UpdateRecord,
                    database,
                    lockFlag,
                    actualize,
                    new TextWithEncoding
                        (
                            record,
                            IrbisEncoding.Utf8
                        )
                );
            ServerResponse response = connection.ExecuteCommand(command);
            
            string result = response.RemainingUtfText();

            return result;
        }

        // ========================================================

        /// <summary>
        /// Write record in raw representation.
        /// </summary>
        [NotNull]
        public static string[] WriteRawRecords
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string database,
                [NotNull] string[] records,
                bool lockFlag,
                bool actualize
            )
        {
            // TODO Create WriteRawRecordsCommand

            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(records, "records");

            if (records.Length == 0)
            {
                return new string[0];
            }

            List<object> arguments = new List<object>
                (
                    records.Length + 2
                )
            {
                lockFlag,
                actualize
            };
            foreach (string record in records)
            {
                string reference = database
                    + IrbisText.IrbisDelimiter
                    + record;
                arguments.Add
                    (
                        new TextWithEncoding
                            (
                                reference,
                                IrbisEncoding.Utf8
                            )
                    );
            }

            UniversalCommand command
                = connection.CommandFactory.GetUniversalCommand
                (
                    CommandCode.SaveRecordGroup,
                    arguments.ToArray()
                );

            ServerResponse response = connection.ExecuteCommand(command);
            
            List<string> result = response.RemainingUtfStrings();

            return result.ToArray();
        }

        // ========================================================

        /// <summary>
        /// Create or update existing record in the database.
        /// </summary>
        [NotNull]
        public static MarcRecord WriteRecord
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(record, "record");

            return connection.WriteRecord
                (
                    record,
                    false,
                    true
                );
        }

        #endregion
    }
}
