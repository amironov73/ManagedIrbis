// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IIrbisConnection.cs -- IRBIS64-client interface.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.IO;
using AM.Runtime;
using AM.Threading;

using JetBrains.Annotations;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Search;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// IRBIS64-client interface
    /// </summary>
    public interface IIrbisConnection
        : IDisposable,
        IHandmadeSerializable
    {
        /// <summary>
        /// Вызывается перед уничтожением объекта.
        /// </summary>
        event EventHandler Disposing;

        /// <summary>
        /// Признак занятости клиента.
        /// </summary>
        BusyState Busy { get; }

        /// <summary>
        /// Адрес сервера.
        /// </summary>
        /// <value>Адрес сервера в цифровом виде.</value>
        string Host { get; set; }

        /// <summary>
        /// Порт сервера.
        /// </summary>
        /// <value>Порт сервера (по умолчанию 6666).</value>
        int Port { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        /// <value>Имя пользователя.</value>
        string Username { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        /// <value>Пароль пользователя.</value>
        string Password { get; set; }

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        /// <value>Служебное имя базы данных (например, "IBIS").
        /// </value>
        string Database { get; set; }

        /// <summary>
        /// Тип АРМ.
        /// </summary>
        /// <value>По умолчанию
        /// <see cref="IrbisWorkstation.Cataloger"/>.
        /// </value>
        IrbisWorkstation Workstation { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        int ClientID { get; }

        /// <summary>
        /// Номер команды.
        /// </summary>
        int QueryID { get; }

        /// <summary>
        /// Executive engine.
        /// </summary>
        AbstractEngine Executive { get; }

        /// <summary>
        /// Command factory.
        /// </summary>
        CommandFactory CommandFactory { get; }

        /// <summary>
        /// Remote INI-file for the client.
        /// </summary>
        IniFile IniFile { get; }

        /// <summary>
        /// Server version.
        /// </summary>
        IrbisVersion ServerVersion { get; }

        /// <summary>
        /// Статус подключения к серверу.
        /// </summary>
        /// <value>Устанавливается в true при успешном выполнении
        /// <see cref="Connect"/>, сбрасывается при выполнении
        /// Dispose.
        /// </value>
        bool Connected { get; }

        /// <summary>
        /// Флаг отключения.
        /// </summary>
        bool Disposed { get; }

        /// <summary>
        /// Таймаут получения ответа от сервера в миллисекундах
        /// (для продвинутых функций).
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Признак: команда прервана.
        /// </summary>
        bool Interrupted { get; }

        /// <summary>
        /// Socket.
        /// </summary>
        AbstractClientSocket Socket { get; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        object UserData { get; set; }

        /// <summary>
        /// Actualize given record (if not yet).
        /// </summary>
        /// <remarks>If MFN=0, then all non actualized
        /// records in the database will be actualized.
        /// </remarks>
        void ActualizeRecord
            (
                [NotNull] string database,
                int mfn
            );

        /// <summary>
        /// Clone the connection.
        /// </summary>
        [NotNull]
        IrbisConnection Clone();

        /// <summary>
        /// Clone the connection.
        /// </summary>
        [NotNull]
        IrbisConnection Clone
            (
                bool connect
            );

        /// <summary>
        /// Establish connection (if not yet).
        /// </summary>
        [NotNull]
        IniFile Connect();

        /// <summary>
        /// GBL for virtual record.
        /// </summary>
        [NotNull]
        MarcRecord CorrectVirtualRecord
            (
                [NotNull] string database,
                [NotNull] MarcRecord record,
                [NotNull] GblStatement[] statements
            );

        /// <summary>
        /// GBL for virtual record.
        /// </summary>
        [NotNull]
        MarcRecord CorrectVirtualRecord
            (
                [NotNull] string database,
                [NotNull] MarcRecord record,
                [NotNull] string filename
            );

        /// <summary>
        /// Create the database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        void CreateDatabase
            (
                [NotNull] string databaseName,
                [NotNull] string description,
                bool readerAccess,
                [CanBeNull] string template
            );

        /// <summary>
        /// Create dictionary index for specified database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        void CreateDictionary
            (
                [NotNull] string database
            );

        /// <summary>
        /// Delete the database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        void DeleteDatabase
            (
                [NotNull] string database
            );

        /// <summary>
        /// Execute any command.
        /// </summary>
        [NotNull]
        ServerResponse ExecuteCommand
            (
                [NotNull] AbstractCommand command
            );

        /// <summary>
        /// Execute command.
        /// </summary>
        [NotNull]
        ServerResponse ExecuteCommand
            (
                [NotNull] string commandCode,
                params object[] arguments
            );

        /// <summary>
        /// Format specified record using ANSI encoding.
        /// </summary>
        [CanBeNull]
        string FormatRecord
            (
                [NotNull] string format,
                int mfn
            );

        /// <summary>
        /// Format specified record using ANSI encoding.
        /// </summary>
        [CanBeNull]
        string FormatRecord
            (
                [NotNull] string format,
                [NotNull] MarcRecord record
            );

        /// <summary>
        /// Format specified records using ANSI encoding.
        /// </summary>
        [NotNull]
        string[] FormatRecords
            (
                [NotNull] string database,
                [NotNull] string format,
                [NotNull] IEnumerable<int> mfnList
            );

        /// <summary>
        /// Получение информации о базе данных.
        /// </summary>
        /// <returns>Cписок логически удаленных, физически удаленных,
        /// неактуализированных и заблокированных записей.</returns>
        [NotNull]
        DatabaseInfo GetDatabaseInfo
            (
                [NotNull] string databaseName
            );

        /// <summary>
        /// Get stat for the database.
        /// </summary>
        [NotNull]
        string GetDatabaseStat
            (
                [NotNull] StatDefinition definition
            );

        /// <summary>
        /// Get next mfn for current database.
        /// </summary>
        int GetMaxMfn();

        /// <summary>
        /// Get next mfn for given database.
        /// </summary>
        int GetMaxMfn
            (
                [CanBeNull] string database
            );

        /// <summary>
        /// Get term postings for given mfn and prefix.
        /// </summary>
        [NotNull]
        TermPosting[] GetRecordPostings
            (
                int mfn,
                [NotNull] string prefix
            );

        /// <summary>
        /// Get server stat.
        /// </summary>
        [NotNull]
        ServerStat GetServerStat();

        /// <summary>
        /// Get server version.
        /// </summary>
        [NotNull]
        IrbisVersion GetServerVersion();

        /// <summary>
        /// Global correction.
        /// </summary>
        [NotNull]
        GblResult GlobalCorrection
            (
                [NotNull] GblSettings settings
            );

        /// <summary>
        /// List server files by the specification.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        string[] ListFiles
            (
                [NotNull] FileSpecification specification
            );

        /// <summary>
        /// List server files by the specification.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        string[] ListFiles
            (
                [NotNull] FileSpecification[] specifications
            );

        /// <summary>
        /// List server processes.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        IrbisProcessInfo[] ListProcesses();

        /// <summary>
        /// List users.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        UserInfo[] ListUsers();

        /// <summary>
        /// No operation.
        /// </summary>
        void NoOp();

        /// <summary>
        /// Парсинг строки подключения.
        /// </summary>
        void ParseConnectionString
            (
                [NotNull] string connectionString
            );

        /// <summary>
        /// Возврат к предыдущей базе данных.
        /// </summary>
        /// <returns>Текущая база данных.</returns>
        [CanBeNull]
        string PopDatabase();

        /// <summary>
        /// Print table.
        /// </summary>
        [NotNull]
        string PrintTable
            (
                [NotNull] TableDefinition tableDefinition
            );

        /// <summary>
        /// Временное переключение на другую базу данных.
        /// </summary>
        /// <returns>Предыдущая база данных.</returns>
        [CanBeNull]
        string PushDatabase
            (
                [NotNull] string newDatabase
            );

        /// <summary>
        /// Read binary file from server file system.
        /// </summary>
        byte[] ReadBinaryFile
            (
                [NotNull] FileSpecification file
            );

        /// <summary>
        /// Read term postings.
        /// </summary>
        TermPosting[] ReadPostings
            (
                [NotNull] PostingParameters parameters
            );

        /// <summary>
        /// Чтение, блокирование и расформатирование записи.
        /// </summary>
        MarcRecord ReadRecord
            (
                [NotNull] string database,
                int mfn,
                bool lockFlag,
                [CanBeNull] string format
            );

        /// <summary>
        /// Чтение указанной версии и расформатирование записи.
        /// </summary>
        /// <remarks><c>null</c>означает, что затребованной
        /// версии записи нет.</remarks>
        MarcRecord ReadRecord
            (
                [NotNull] string database,
                int mfn,
                int versionNumber,
                [CanBeNull] string format
            );

        /// <summary>
        /// Read search terms from index.
        /// </summary>
        TermInfo[] ReadTerms
            (
                [NotNull] TermParameters parameters
            );

        /// <summary>
        /// Read text file from the server.
        /// </summary>
        string ReadTextFile
            (
                [NotNull] FileSpecification fileSpecification
            );

        /// <summary>
        /// Чтение текстовых файлов с сервера.
        /// </summary>
        string[] ReadTextFiles
            (
                [NotNull] FileSpecification[] files
            );

        /// <summary>
        /// Reconnect to the server.
        /// </summary>
        void Reconnect();

        /// <summary>
        /// Reload dictionary index for specified database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        void ReloadDictionary
            (
                [NotNull] string databaseName
            );

        /// <summary>
        /// Reload master file for specified database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        void ReloadMasterFile
            (
                [NotNull] string databaseName
            );

        /// <summary>
        /// Restart server.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        void RestartServer();

        /// <summary>
        /// Поиск записей.
        /// </summary>
        int[] Search
            (
                [NotNull] string expression
            );

        /// <summary>
        /// Sequential search.
        /// </summary>
        int[] SequentialSearch
            (
                [NotNull] SearchParameters parameters
            );

        /// <summary>
        /// Set new <see cref="IrbisConnection.CommandFactory"/>.
        /// </summary>
        /// <returns>Previous <see cref="IrbisConnection.CommandFactory"/>.
        /// </returns>
        CommandFactory SetCommandFactory
            (
                [NotNull] CommandFactory newFactory
            );

        /// <summary>
        /// Set new <see cref="IrbisConnection.CommandFactory"/>.
        /// </summary>
        /// <returns>Previous <see cref="IrbisConnection.CommandFactory"/>.
        /// </returns>
        CommandFactory SetCommandFactory
            (
                [NotNull] string typeName
            );

        /// <summary>
        /// Set execution engine.
        /// </summary>
        AbstractEngine SetEngine
            (
                [NotNull] AbstractEngine engine
            );

        /// <summary>
        /// Set new <see cref="IrbisConnection.Executive"/>.
        /// </summary>
        /// <returns>Previous <see cref="IrbisConnection.Executive"/>.
        /// </returns>
        AbstractEngine SetEngine
            (
                [NotNull] string typeName
            );

        /// <summary>
        /// Set logging socket, gather debug info to specified path.
        /// </summary>
        void SetNetworkLogging
            (
                [NotNull] string loggingPath
            );

        /// <summary>
        ///
        /// </summary>
        void SetRetry
            (
                int retryCount,
                [CanBeNull] Func<Exception, bool> resolver
            );

        /// <summary>
        /// Set
        /// <see cref="T:ManagedIrbis.Network.Sockets.AbstractClientSocket"/>.
        /// </summary>
        void SetSocket
            (
                [NotNull] AbstractClientSocket socket
            );

        /// <summary>
        /// Temporary "shutdown" the connection for some reason.
        /// </summary>
        string Suspend();

        /// <summary>
        /// Опустошение базы данных.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        void TruncateDatabase
            (
                [NotNull] string databaseName
            );

        /// <summary>
        /// Unlock the specified database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        void UnlockDatabase
            (
                [NotNull] string databaseName
            );

        /// <summary>
        /// Unlock specified records.
        /// </summary>
        void UnlockRecords
            (
                [NotNull] string database,
                params int[] mfnList
            );

        /// <summary>
        /// Update server INI-file for current client.
        /// </summary>
        void UpdateIniFile
            (
                [CanBeNull] string[] lines
            );

        /// <summary>
        /// Update user list on the server.
        /// </summary>
        void UpdateUserList
            (
                [NotNull] UserInfo[] userList
            );

        /// <summary>
        /// Create or update existing record in the database.
        /// </summary>
        [NotNull]
        MarcRecord WriteRecord
            (
                [NotNull] MarcRecord record,
                bool lockFlag,
                bool actualize,
                bool dontParseResponse
            );

        /// <summary>
        /// Create or update many records.
        /// </summary>
        [NotNull]
        MarcRecord[] WriteRecords
            (
                [NotNull] MarcRecord[] records,
                bool lockFlag,
                bool actualize
            );

        /// <summary>
        /// Write text file to the server.
        /// </summary>
        void WriteTextFile
            (
                [NotNull] FileSpecification file
            );

        /// <summary>
        /// Write text files to the server.
        /// </summary>
        void WriteTextFiles
            (
                params FileSpecification[] files
            );
    }
}