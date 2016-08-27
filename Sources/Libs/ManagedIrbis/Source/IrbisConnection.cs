/* IrbisConnection.cs -- client for IRBIS-server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Infrastructure.Sockets;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Client for IRBIS-server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisConnection
        : IDisposable,
        IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Таймаут получения ответа от сервера по умолчанию.
        /// </summary>
        public const int DefaultTimeout = 30000;

        #endregion

        #region Events

        /// <summary>
        /// Вызывается перед уничтожением объекта.
        /// </summary>
        public event EventHandler Disposing;

        #endregion

        #region Properties

#if !NETCORE

        /// <summary>
        /// Версия клиента.
        /// </summary>
        public static Version ClientVersion = Assembly
            .GetExecutingAssembly()
            .GetName()
            .Version;

#endif

        /// <summary>
        /// Признак занятости клиента.
        /// </summary>
        [NotNull]
        public BusyState Busy { get; private set; }

        /// <summary>
        /// Адрес сервера.
        /// </summary>
        /// <value>Адрес сервера в цифровом виде.</value>
        [NotNull]
        public string Host
        {
            get { return _host; }
            set
            {
                Code.NotNullNorEmpty(value, "value");

                ThrowIfConnected();
                _host = value;
            }
        }

        /// <summary>
        /// Порт сервера.
        /// </summary>
        /// <value>Порт сервера (по умолчанию 6666).</value>
        public int Port
        {
            get { return _port; }
            set
            {
                Code.Positive(value, "value");

                ThrowIfConnected();
                _port = value;
            }
        }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        /// <value>Имя пользователя.</value>
        [NotNull]
        public string Username
        {
            get { return _username; }
            set
            {
                Code.NotNullNorEmpty(value, "value");

                ThrowIfConnected();
                _username = value;
            }
        }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        /// <value>Пароль пользователя.</value>
        [NotNull]
        public string Password
        {
            get { return _password; }
            set
            {
                Code.NotNullNorEmpty(value, "value");

                ThrowIfConnected();
                _password = value;
            }
        }

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        /// <value>Служебное имя базы данных (например, "IBIS").
        /// </value>
        [NotNull]
        public string Database
        {
            get { return _database; }
            set
            {
                Code.NotNullNorEmpty(value, "value");

                ThrowIfConnected();
                _database = value;
            }
        }

        /// <summary>
        /// Тип АРМ.
        /// </summary>
        /// <value>По умолчанию
        /// <see cref="IrbisWorkstation.Cataloger"/>.
        /// </value>
        public IrbisWorkstation Workstation
        {
            get { return _workstation; }
            set
            {
                ThrowIfConnected();
                _workstation = value;
            }
        }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public int ClientID { get { return _clientID; } }

        /// <summary>
        /// Номер команды.
        /// </summary>
        public int QueryID { get { return _queryID; } }

        /// <summary>
        /// Executive engine.
        /// </summary>
        [NotNull]
        public AbstractEngine Executive { get; private set; }

        /// <summary>
        /// Command factory.
        /// </summary>
        [NotNull]
        public CommandFactory CommandFactory { get; private set; }

        ///// <summary>
        ///// Конфигурация клиента.
        ///// </summary>
        ///// <value>Высылается сервером при подключении.</value>
        //public string Configuration
        //{
        //    get { return _configuration; }
        //}

        /// <summary>
        /// Статус подключения к серверу.
        /// </summary>
        /// <value>Устанавливается в true при успешном выполнении
        /// <see cref="Connect"/>, сбрасывается при выполнении
        /// <see cref="Dispose"/>.
        /// </value>
        public bool Connected
        {
            get { return _connected; }
        }

        /// <summary>
        /// Таймаут получения ответа от сервера в миллисекундах
        /// (для продвинутых функций).
        /// </summary>
        [DefaultValue(DefaultTimeout)]
        public int Timeout { get; set; }

        /// <summary>
        /// Признак: команда прервана.
        /// </summary>
        [DefaultValue(false)]
        public bool Interrupted { get; internal set; }

        /// <summary>
        /// Socket.
        /// </summary>
        [NotNull]
        public AbstractClientSocket Socket { get; private set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        public object UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <remarks>
        /// Обратите внимание, деструктор не нужен!
        /// Он помешает сохранению состояния клиента
        /// при сериализации и последующему восстановлению,
        /// т. к. попытается закрыть уже установленное
        /// соединение. Восстановленная копия клиента
        /// ломанётся в закрытое соедиение, и выйдет облом.
        /// </remarks>
        public IrbisConnection()
        {
            Busy = new BusyState();

            Host = ConnectionSettings.DefaultHost;
            Port = ConnectionSettings.DefaultPort;
            Database = ConnectionSettings.DefaultDatabase;
            Username = "111";
            Password = "111";
            Workstation = ConnectionSettings.DefaultWorkstation;

            Executive = new StandardEngine(this, null);
            CommandFactory = CommandFactory
                .GetDefaultFactory(this);
            Socket = new SimpleClientSocket(this);
        }

        /// <summary>
        /// Конструктор с подключением.
        /// </summary>
        public IrbisConnection
            (
                [NotNull] string connectionString
            )
            : this()
        {
            Code.NotNullNorEmpty(connectionString, "connectionString");

            ParseConnectionString(connectionString);
            Connect();
        }

        #endregion

        #region Private members

        // ReSharper disable InconsistentNaming
        internal bool _connected;
        private int _clientID;
        private int _queryID;
        // ReSharper restore InconsistentNaming

        private static Random _random = new Random();

        private readonly Stack<string> _databaseStack
            = new Stack<string>();

        private string _host;
        private int _port;
        private string _username;
        private string _password;
        private string _database;
        private IrbisWorkstation _workstation;

        internal void ThrowIfConnected()
        {
            if (Connected)
            {
                throw new IrbisException("Already connected");
            }
        }

        // ReSharper disable InconsistentNaming
        internal int GenerateClientID()
        {
            _clientID = _random.Next(1000000, 9999999);

            return _clientID;
        }
        // ReSharper restore InconsistentNaming

        internal int IncrementCommandNumber()
        {
            return ++_queryID;
        }

        internal void ResetCommandNumber()
        {
            _queryID = 0;
        }

        #endregion

        // =========================================================

        #region Public methods

        /// <summary>
        /// Actualize given record (if not yet).
        /// </summary>
        /// <remarks>If MFN=0, then all non actualized
        /// records in the database will be actualized.
        /// </remarks>
        public void ActualizeRecord
            (
                [NotNull] string database,
                int mfn
            )
        {
            Code.NotNullNorEmpty(database, "database");
            Code.Nonnegative(mfn, "mfn");

            ActualizeRecordCommand command
                = CommandFactory.GetActualizeRecordCommand();
            command.Database = database;
            command.Mfn = mfn;

            ExecuteCommand(command);
        }

        // =========================================================

        /// <summary>
        /// Clone the connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Clone()
        {
            IrbisConnection result = Clone(Connected);

            return result;
        }

        /// <summary>
        /// Clone the connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Clone
            (
                bool connect
            )
        {
            // TODO clone socket?

            IrbisConnection result = new IrbisConnection
            {
                Host = Host,
                Port = Port,
                Username = Username,
                Password = Password,
                Database = Database,
                Workstation = Workstation,
                Timeout = Timeout,
                // Socket = Socket.Clone ()
            };

            if (connect)
            {
                result.Connect();
            }

            return result;
        }

        // ========================================================

        /// <summary>
        /// Establish connection (if not yet).
        /// </summary>
        public string Connect()
        {
            // TODO use Executive

            if (!_connected)
            {
                ConnectCommand command
                    = CommandFactory.GetConnectCommand();
                ClientQuery query = command.CreateQuery();
                ServerResponse result = command.Execute(query);
                command.CheckResponse(result);
                _connected = true;
                return command.Configuration;
            }

            return null;
        }

        // ========================================================

        /// <summary>
        /// GBL for virtual record.
        /// </summary>
        [NotNull]
        public MarcRecord CorrectVirtualRecord
            (
                [NotNull] string database,
                [NotNull] MarcRecord record,
                [NotNull] GblStatement[] statements
            )
        {
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(record, "record");
            Code.NotNull(statements, "statements");

            GblVirtualCommand command
                = CommandFactory.GetGblVirtualCommand();
            command.Database = database;
            command.Record = record;
            command.Statements = statements;

            ExecuteCommand(command);

            return command.Result
                .ThrowIfNull("command.Result");
        }

        /// <summary>
        /// GBL for virtual record.
        /// </summary>
        [NotNull]
        public MarcRecord CorrectVirtualRecord
            (
                [NotNull] string database,
                [NotNull] MarcRecord record,
                [NotNull] string filename
            )
        {
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(filename, "filename");

            GblVirtualCommand command
                = CommandFactory.GetGblVirtualCommand();
            command.Database = database;
            command.Record = record;
            command.FileName = filename;

            ExecuteCommand(command);

            return command.Result
                .ThrowIfNull("command.Result");
        }

        // ========================================================

        /// <summary>
        /// Create the database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        public void CreateDatabase
            (
                [NotNull] string databaseName,
                [NotNull] string description,
                bool readerAccess,
                [CanBeNull] string template
            )
        {
            Code.NotNullNorEmpty(databaseName, "databaseName");
            Code.NotNullNorEmpty(description, "description");

            CreateDatabaseCommand command
                = CommandFactory.GetCreateDatabaseCommand();
            command.Database = databaseName;
            command.Description = description;
            command.ReaderAccess = readerAccess;
            command.Template = template;

            ExecuteCommand(command);
        }

        // =========================================================

        /// <summary>
        /// Create dictionary index for specified database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        public void CreateDictionary
            (
                [NotNull] string database
            )
        {
            Code.NotNullNorEmpty(database, "database");

            CreateDictionaryCommand command
                = CommandFactory.GetCreateDictionaryCommand();
            command.Database = database;
            command.RelaxResponse = true;

            ExecuteCommand(command);
        }

        // ========================================================

        /// <summary>
        /// Delete the database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        public void DeleteDatabase
            (
                [NotNull] string database
            )
        {
            Code.NotNullNorEmpty(database, "database");

            DeleteDatabaseCommand command
                = CommandFactory.GetDeleteDatabaseCommand();
            command.Database = database;

            ExecuteCommand(command);
        }

        // =========================================================

        #region ExecuteCommand

        /// <summary>
        /// Execute any command.
        /// </summary>
        [NotNull]
        public ServerResponse ExecuteCommand
            (
                [NotNull] AbstractCommand command
            )
        {
            Code.NotNull(command, "command");

            ExecutionContext context = new ExecutionContext
                (
                    this,
                    command
                );
            ServerResponse result
                = Executive.ExecuteCommand(context);

            return result;
        }

        /// <summary>
        /// Execute command.
        /// </summary>
        [NotNull]
        public ServerResponse ExecuteCommand
            (
                [NotNull] string commandCode,
                params object[] arguments
            )
        {
            Code.NotNullNorEmpty(commandCode, "commandCode");

            UniversalCommand command
                = CommandFactory.GetUniversalCommand
                (
                    commandCode,
                    arguments
                );

            return ExecuteCommand(command);
        }

        #endregion

        // =========================================================

        #region FormatRecord

        /// <summary>
        /// Форматирование записи.
        /// </summary>
        [CanBeNull]
        public string FormatRecord
            (
                [NotNull] string format,
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");
            Code.NotNull(format, "format");

            FormatCommand command = CommandFactory.GetFormatCommand();
            command.FormatSpecification = format;
            command.MfnList.Add(mfn);

            ExecuteCommand(command);

            string result = command.FormatResult
                .ThrowIfNullOrEmpty("command.FormatResult")
                [0];

            return result;
        }

        /// <summary>
        /// Форматирование записи.
        /// </summary>
        [CanBeNull]
        public string FormatRecord
            (
                [NotNull] string format,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(format, "format");
            Code.NotNull(record, "record");

            FormatCommand command = CommandFactory.GetFormatCommand();
            command.FormatSpecification = format;
            command.VirtualRecord = record;

            ExecuteCommand(command);

            string result = command.FormatResult
                .ThrowIfNullOrEmpty("command.FormatResult")
                [0];

            return result;
        }

        /// <summary>
        /// Форматирование записей.
        /// </summary>
        [NotNull]
        public string[] FormatRecords
            (
                [NotNull] string database,
                [NotNull] string format,
                [NotNull] IEnumerable<int> mfnList
            )
        {
            Code.NotNull(mfnList, "mfnList");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(format, "format");

            FormatCommand command = CommandFactory.GetFormatCommand();
            command.Database = database;
            command.FormatSpecification = format;
            command.MfnList.AddRange(mfnList);

            if (command.MfnList.Count == 0)
            {
                return new string[0];
            }

            ExecuteCommand(command);

            string[] result = command.FormatResult
                .ThrowIfNull("command.FormatResult");

            return result;
        }

        #endregion

        // =========================================================

        /// <summary>
        /// Получение информации о базе данных.
        /// </summary>
        /// <returns>Cписок логически удаленных, физически удаленных, 
        /// неактуализированных и заблокированных записей.</returns>
        [NotNull]
        public DatabaseInfo GetDatabaseInfo
            (
                [NotNull] string databaseName
            )
        {
            Code.NotNullNorEmpty(databaseName, "databaseName");

            DatabaseInfoCommand command
                = CommandFactory.GetDatabaseInfoCommand();
            command.Database = databaseName;

            ExecuteCommand(command);
            DatabaseInfo result = command.Result
                .ThrowIfNull("command.Result");

            return result;
        }

        /// <summary>
        /// Get stat for the database.
        /// </summary>
        [NotNull]
        public string GetDatabaseStat
            (
                [NotNull] StatDefinition definition
            )
        {
            Code.NotNull(definition, "definition");

            DatabaseStatCommand command
                = CommandFactory.GetDatabaseStatCommand();
            command.Definition = definition;

            ExecuteCommand(command);
            string result = command.Result
                .ThrowIfNull("command.Result");

            return result;
        }

        // =========================================================

        /// <summary>
        /// Get next mfn for current database.
        /// </summary>
        public int GetMaxMfn()
        {
            return GetMaxMfn(Database);
        }

        /// <summary>
        /// Get next mfn for given database.
        /// </summary>
        public int GetMaxMfn
            (
                [CanBeNull] string database
            )
        {
            if (ReferenceEquals(database, null))
            {
                database = Database;
            }


            MaxMfnCommand command = CommandFactory.GetMaxMfnCommand();
            command.Database = database;

            ServerResponse response = ExecuteCommand(command);
            int result = response.ReturnCode;

            return result;
        }

        // =========================================================

        /// <summary>
        /// Get server stat.
        /// </summary>
        [NotNull]
        public ServerStat GetServerStat()
        {
            ServerStatCommand command
                = CommandFactory.GetServerStatCommand();
            ExecuteCommand(command);

            ServerStat result = command.Result
                .ThrowIfNull("command.Result");

            return result;
        }

        // =========================================================

        /// <summary>
        /// Get server version.
        /// </summary>
        [NotNull]
        public IrbisVersion GetServerVersion()
        {
            ServerVersionCommand command
                = CommandFactory.GetServerVersionCommand();
            ExecuteCommand(command);

            IrbisVersion result = command.Result
                .ThrowIfNull("command.Result");

            return result;
        }

        // =========================================================

        /// <summary>
        /// Global correction.
        /// </summary>
        [NotNull]
        public GblResult GlobalCorrection
            (
                [NotNull] GblSettings settings
            )
        {
            Code.NotNull(settings, "settings");

            if (string.IsNullOrEmpty(settings.Database))
            {
                settings.Database = Database;
            }

            GblCommand command
                = CommandFactory.GetGblCommand(settings);

            ExecuteCommand(command);

            return command.Result
                .ThrowIfNull("command.Result");
        }

        // =========================================================

        /// <summary>
        /// List server files by the specification.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] ListFiles
            (
                [NotNull] FileSpecification specification
            )
        {
            Code.NotNull(specification, "specification");

            specification.Verify(true);

            ListFilesCommand command = new ListFilesCommand(this);
            command.Specifications.Add(specification);

            ExecuteCommand(command);

            string[] result = command.Files
                .ThrowIfNull("command.Files");

            return result;
        }

        /// <summary>
        /// List server files by the specification.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] ListFiles
            (
                [NotNull] FileSpecification[] specifications
            )
        {
            Code.NotNull(specifications, "specifications");

            ListFilesCommand command = new ListFilesCommand(this);
            foreach (FileSpecification specification in specifications)
            {
                specification.Verify(true);
                command.Specifications.Add(specification);
            }

            ExecuteCommand(command);

            string[] result = command.Files
                .ThrowIfNull("command.Files");

            return result;
        }

        // =========================================================

        /// <summary>
        /// List server processes.
        /// </summary>
        [NotNull]
        public IrbisProcessInfo[] ListProcesses()
        {
            ListProcessesCommand command
                = CommandFactory.GetListProcessCommand();
            ExecuteCommand(command);

            IrbisProcessInfo[] result = command.Result
                .ThrowIfNullOrEmpty("command.Result");

            return result;
        }

        // =========================================================

        /// <summary>
        /// List users.
        /// </summary>
        [NotNull]
        public UserInfo[] ListUsers()
        {
            ListUsersCommand command
                = CommandFactory.GetListUsersCommand();
            ExecuteCommand(command);

            UserInfo[] result = command.Result
                .ThrowIfNull("command.Result");

            return result;
        }

        // =========================================================

        /// <summary>
        /// No operation.
        /// </summary>
        public void NoOp()
        {
            // TODO Create NopCommand

            ExecuteCommand(CommandCode.Nop);
        }

        /// <summary>
        /// Парсинг строки подключения.
        /// </summary>
        public void ParseConnectionString
            (
                [NotNull] string connectionString
            )
        {
            Code.NotNull(connectionString, "connectionString");

            ConnectionSettings settings = new ConnectionSettings();
            settings.ParseConnectionString(connectionString);
            settings.ApplyToConnection(this);
        }

        // ========================================================

        /// <summary>
        /// Возврат к предыдущей базе данных.
        /// </summary>
        /// <returns>Текущая база данных.</returns>
        [CanBeNull]
        public string PopDatabase()
        {
            string result = Database;

            if (_databaseStack.Count != 0)
            {
                Database = _databaseStack.Pop();
            }

            return result;
        }

        // ========================================================

        /// <summary>
        /// Print table.
        /// </summary>
        [NotNull]
        public string PrintTable
            (
                [NotNull] TableDefinition tableDefinition
            )
        {
            Code.NotNull(tableDefinition, "tableDefinition");

            PrintTableCommand command
                = CommandFactory.GetPrintTableCommand();
            command.Definition = tableDefinition;

            ExecuteCommand(command);

            return command.Result;
        }

        // ========================================================

        /// <summary>
        /// Временное переключение на другую базу данных.
        /// </summary>
        /// <returns>Предыдущая база данных.</returns>
        [CanBeNull]
        public string PushDatabase
            (
                [NotNull] string newDatabase
            )
        {
            Code.NotNullNorEmpty(newDatabase, "newDatabase");

            string result = Database;
            _databaseStack.Push(Database);
            Database = newDatabase;

            return result;
        }

        // ========================================================

        /// <summary>
        /// Read binary file from server file system.
        /// </summary>
        [CanBeNull]
        public byte[] ReadBinaryFile
            (
                [NotNull] FileSpecification file
            )
        {
            Code.NotNull(file, "file");

            ReadBinaryFileCommand command
                = CommandFactory.GetReadBinaryFileCommand();
            command.File = file;

            ExecuteCommand(command);

            return command.Content;
        }

        // ========================================================

        /// <summary>
        /// Read term postings.
        /// </summary>
        [NotNull]
        public TermPosting[] ReadPostings
            (
                [NotNull] PostingParameters parameters
            )
        {
            Code.NotNull(parameters, "parameters");

            ReadPostingsCommand command
                = CommandFactory.GetReadPostingsCommand();
            command.ApplyParameters(parameters);

            ExecuteCommand(command);

            return command.Postings
                .ThrowIfNull("command.Postings");
        }

        // ========================================================

        #region ReadRecord

        /// <summary>
        /// Чтение, блокирование и расформатирование записи.
        /// </summary>
        [NotNull]
        public MarcRecord ReadRecord
            (
                [NotNull] string database,
                int mfn,
                bool lockFlag,
                [CanBeNull] string format
            )
        {
            Code.NotNullNorEmpty(database, "database");
            Code.Positive(mfn, "mfn");

            ReadRecordCommand command
                = CommandFactory.GetReadRecordCommand();
            command.Mfn = mfn;
            command.Database = database;
            command.Lock = lockFlag;
            command.Format = format;

            ExecuteCommand(command);

            return command.ReadRecord
                .ThrowIfNull("no record retrieved");
        }

        /// <summary>
        /// Чтение указанной версии и расформатирование записи.
        /// </summary>
        /// <remarks><c>null</c>означает, что затребованной
        /// версии записи нет.</remarks>
        [CanBeNull]
        public MarcRecord ReadRecord
            (
                [NotNull] string database,
                int mfn,
                int versionNumber,
                [CanBeNull] string format
            )
        {
            Code.NotNullNorEmpty(database, "database");
            Code.Positive(mfn, "mfn");

            ReadRecordCommand command
                = CommandFactory.GetReadRecordCommand();
            command.Mfn = mfn;
            command.Database = database;
            command.VersionNumber = versionNumber;
            command.Format = format;

            ExecuteCommand(command);

            return command.ReadRecord;
        }

        /// <summary>
        /// Read multiple records.
        /// </summary>
        [NotNull]
        public MarcRecord[] ReadRecords
            (
                [CanBeNull] string database,
                [NotNull] IEnumerable<int> mfnList
            )
        {
            Code.NotNull(mfnList, "mfnList");

            if (string.IsNullOrEmpty(database))
            {
                database = Database;
            }

            FormatCommand command = CommandFactory.GetFormatCommand();
            command.Database = database;
            command.FormatSpecification = IrbisFormat.All;
            command.MfnList.AddRange(mfnList);

            if (command.MfnList.Count == 0)
            {
                return new MarcRecord[0];
            }

            if (command.MfnList.Count == 1)
            {
                int mfn = command.MfnList[0];

                MarcRecord record = ReadRecord
                    (
                        database,
                        mfn,
                        false,
                        null
                    );

                return new[] { record };
            }

            ExecuteCommand(command);

            MarcRecord[] result = MarcRecordUtility.ParseAllFormat
                (
                    database,
                    this,
                    command.FormatResult
                        .ThrowIfNullOrEmpty("command.FormatResult")
                );
            Debug.Assert
                (
                    command.MfnList.Count == result.Length,
                    "some records not retrieved"
                );

            return result;
        }

        #endregion

        // ========================================================

        /// <summary>
        /// Read search terms from index.
        /// </summary>
        [NotNull]
        public TermInfo[] ReadTerms
            (
                [NotNull] string startTerm,
                int numberOfTerms,
                bool reverseOrder,
                [CanBeNull] string format
            )
        {
            Code.NotNull(startTerm, "startTerm");

            ReadTermsCommand command
                = CommandFactory.GetReadTermsCommand();
            command.Database = Database;
            command.StartTerm = startTerm;
            command.NumberOfTerms = numberOfTerms;
            command.ReverseOrder = reverseOrder;
            command.Format = format;

            ExecuteCommand(command);

            return command.Terms.ThrowIfNull("command.Terms");
        }

        // ========================================================

        /// <summary>
        /// Read text file from the server.
        /// </summary>
        [CanBeNull]
        public string ReadTextFile
            (
                [NotNull] FileSpecification fileSpecification
            )
        {
            Code.NotNull(fileSpecification, "fileSpecification");

            ReadFileCommand command = new ReadFileCommand(this);
            command.Files.Add(fileSpecification);

            ExecuteCommand(command);
            string result = command.Result
                .ThrowIfNullOrEmpty("command.Result")[0];

            return result;
        }

        /// <summary>
        /// Чтение текстовых файлов с сервера.
        /// </summary>
        [NotNull]
        public string[] ReadTextFiles
            (
                [NotNull] FileSpecification[] files
            )
        {
            Code.NotNull(files, "files");

            if (files.Length == 0)
            {
                return new string[0];
            }

            ReadFileCommand command
                = CommandFactory.GetReadFileCommand();
            command.Files.AddRange(files);

            ExecuteCommand(command);

            string[] result = command.Result
                .ThrowIfNullOrEmpty("command.Result");

            return result;
        }

        // =========================================================

        /// <summary>
        /// Reload dictionary index for specified database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        public void ReloadDictionary
            (
                [NotNull] string databaseName
            )
        {
            // TODO Create ReloadDictionaryCommand

            Code.NotNullNorEmpty(databaseName, "databaseName");

            ExecuteCommand
                (
                    CommandCode.ReloadDictionary,
                    databaseName
                );
        }

        // =========================================================

        /// <summary>
        /// Reload master file for specified database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        public void ReloadMasterFile
            (
                [NotNull] string databaseName
            )
        {
            // TODO Create ReloadMasterFileCommand

            Code.NotNullNorEmpty(databaseName, "databaseName");

            ExecuteCommand
                (
                    CommandCode.ReloadMasterFile,
                    databaseName
                );
        }

        // =========================================================

        /// <summary>
        /// Restart server.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        public void RestartServer()
        {
            // TODO Create RestartServerCommand

            ExecuteCommand(CommandCode.RestartServer);
        }

        // =========================================================

        /// <summary>
        /// Restore previously suspended connection.
        /// </summary>
        [NotNull]
        public static IrbisConnection Restore
            (
                [NotNull] string state
            )
        {
            Code.NotNullNorEmpty(state, "state");

            ConnectionSettings settings
                = ConnectionSettings.Decrypt(state);
            IrbisConnection result = new IrbisConnection();
            settings.ApplyToConnection(result);

            return result;
        }

        // =========================================================

        /// <summary>
        /// Поиск записей.
        /// </summary>
        [NotNull]
        public int[] Search
            (
                [NotNull] string expression
            )
        {
            Code.NotNull(expression, "expression");

            SearchCommand command = CommandFactory.GetSearchCommand();
            command.SearchExpression = expression;

            ExecuteCommand(command);

            int[] result = FoundItem.ConvertToMfn
                (
                    command.Found.ThrowIfNull("Found")
                );

            return result;
        }

        // =========================================================

        /// <summary>
        /// Sequential search.
        /// </summary>
        [NotNull]
        public int[] SequentialSearch
            (
                [NotNull] SearchParameters parameters
            )
        {
            Code.NotNull(parameters, "parameters");

            SearchCommand command = CommandFactory.GetSearchCommand();
            command.ApplyParameters(parameters);

            ExecuteCommand(command);

            int[] result = FoundItem.ConvertToMfn
                (
                    command.Found.ThrowIfNull("Found")
                );

            return result;
        }

        // =========================================================

        /// <summary>
        /// Set new <see cref="CommandFactory"/>.
        /// </summary>
        /// <returns>Previous <see cref="CommandFactory"/>.
        /// </returns>
        [NotNull]
        public CommandFactory SetCommandFactory
            (
                [NotNull] CommandFactory newFactory
            )
        {
            Code.NotNull(newFactory, "newFactory");

            CommandFactory previous = CommandFactory;
            newFactory.Connection = this;
            CommandFactory = newFactory;

            return previous;
        }

        /// <summary>
        /// Set new <see cref="CommandFactory"/>.
        /// </summary>
        /// <returns>Previous <see cref="CommandFactory"/>.
        /// </returns>
        [NotNull]
        public CommandFactory SetCommandFactory
            (
                [NotNull] string typeName
            )
        {
            Code.NotNull(typeName, "typeName");

            Type type = Type.GetType(typeName, true);
            CommandFactory newFactory
                = (CommandFactory)Activator.CreateInstance
                (
                    type,
                    this
                );
            CommandFactory previous = SetCommandFactory(newFactory);

            return previous;
        }

        // =========================================================

        /// <summary>
        /// Set execution engine.
        /// </summary>
        [NotNull]
        public AbstractEngine SetEngine
            (
                [NotNull] AbstractEngine engine
            )
        {
            Code.NotNull(engine, "engine");

            AbstractEngine previous = Executive;
            Executive = engine;

            return previous;
        }

        /// <summary>
        /// Set new <see cref="Executive"/>.
        /// </summary>
        /// <returns>Previous <see cref="Executive"/>.
        /// </returns>
        [NotNull]
        public AbstractEngine SetEngine
            (
                [NotNull] string typeName
            )
        {
            Code.NotNull(typeName, "typeName");

            Type type = Type.GetType(typeName, true);
            AbstractEngine newEngine
                = (AbstractEngine)Activator.CreateInstance
                (
                    type,
                    this,
                    Executive
                );
            AbstractEngine previous = SetEngine(newEngine);

            return previous;
        }


        // =========================================================

        /// <summary>
        /// Set logging socket, gather debug info to specified path.
        /// </summary>
        public void SetNetworkLogging
            (
                [NotNull] string loggingPath
            )
        {
            Code.NotNullNorEmpty(loggingPath, "loggingPath");

            AbstractClientSocket oldSocket = Socket;
            if (oldSocket is LoggingClientSocket)
            {
                return;
            }

            LoggingClientSocket newSocket = new LoggingClientSocket
                (
                    this,
                    Socket,
                    loggingPath
                );

            DirectoryUtility.ClearDirectory(loggingPath);

            SetSocket(newSocket);
        }

        // =========================================================

        /// <summary>
        /// 
        /// </summary>
        public void SetRetry
            (
                int retryCount,
                [CanBeNull] Func<Exception, bool> resolver
            )
        {
            RetryClientSocket oldSocket = Socket
                as RetryClientSocket;

            if (retryCount <= 0)
            {
                if (!ReferenceEquals(oldSocket, null))
                {
                    SetSocket(oldSocket.InnerSocket);
                }
            }
            else
            {
                RetryClientSocket newSocket = new RetryClientSocket
                    (
                        this,
                        Socket,
                        new RetryManager(retryCount, resolver)
                    );

                if (ReferenceEquals(oldSocket, null))
                {
                    SetSocket(newSocket);
                }
            }
        }

        // =========================================================

        /// <summary>
        /// Set
        /// <see cref="T:ManagedIrbis.Network.Sockets.AbstractClientSocket"/>.
        /// </summary>
        public void SetSocket
            (
                [NotNull] string typeName
            )
        {
            Code.NotNullNorEmpty(typeName, "typeName");

            Type type = Type.GetType(typeName, true);
            AbstractClientSocket socket
                = (AbstractClientSocket)Activator.CreateInstance
                (
                    type,
                    this
                );
            SetSocket(socket);
        }

        /// <summary>
        /// Set
        /// <see cref="T:ManagedIrbis.Network.Sockets.AbstractClientSocket"/>.
        /// </summary>
        public void SetSocket
            (
                [NotNull] AbstractClientSocket socket
            )
        {
            Code.NotNull(socket, "socket");

            socket.Connection = this;
            Socket = socket;
        }

        // =========================================================

        /// <summary>
        /// Temporary "shutdown" the connection for some reason.
        /// </summary>
        [NotNull]
        public string Suspend()
        {
            ConnectionSettings settings
                = ConnectionSettings.FromConnection(this);
            string result = settings.Encrypt();

            _connected = false;

            return result;
        }

        // =========================================================

        /// <summary>
        /// Опустошение базы данных.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        public void TruncateDatabase
            (
                [NotNull] string databaseName
            )
        {
            // TODO TruncateDatabaseCommand

            Code.NotNullNorEmpty(databaseName, "databaseName");

            ExecuteCommand
                (
                    CommandCode.EmptyDatabase,
                    databaseName
                );
        }

        // =========================================================

        /// <summary>
        /// Unlock specified database.
        /// </summary>
        public void UnlockDatabase
            (
                [NotNull] string databaseName
            )
        {
            // TODO UnlockDatabaseCommand

            Code.NotNullNorEmpty(databaseName, "databaseName");

            ExecuteCommand
                (
                    CommandCode.UnlockDatabase,
                    databaseName
                );
        }

        // =========================================================

        /// <summary>
        /// Unlock specified records.
        /// </summary>
        public void UnlockRecords
            (
                [NotNull] string database,
                params int[] mfnList
            )
        {
            // TODO UnlockRecordsCommand

            Code.NotNullNorEmpty(database, "database");

            if (mfnList.Length == 0)
            {
                return;
            }

            List<object> arguments
                = new List<object>(mfnList.Length + 1) { Database };
            arguments.AddRange(mfnList.Cast<object>());

            ExecuteCommand
                (
                    CommandCode.UnlockRecords,
                    arguments.ToArray()
                );
        }

        // =========================================================

        /// <summary>
        /// Update server INI-file for current client.
        /// </summary>
        public void UpdateIniFile
            (
                [NotNull] string[] text
            )
        {
            // TODO UpdateIniFileCommand

            Code.NotNull(text, "text");

            if (text.Length == 0)
            {
                return;
            }

            UniversalTextCommand command =
                CommandFactory.GetUniversalTextCommand
                (
                    CommandCode.UpdateIniFile,
                    text,
                    IrbisEncoding.Ansi
                );

            ExecuteCommand(command);
        }

        // ========================================================

        /// <summary>
        /// Update user list on the server.
        /// </summary>
        public void UpdateUserList
            (
                [NotNull] UserInfo[] userList
            )
        {
            Code.NotNull(userList, "userList");

            UpdateUserListCommand command
                = CommandFactory.GetUpdateUserListCommand();
            command.UserList = userList;

            ExecuteCommand(command);
        }

        // ========================================================

        #region WriteRecord

        /// <summary>
        /// Create or update existing record in the database.
        /// </summary>
        [NotNull]
        public MarcRecord WriteRecord
            (
                [NotNull] MarcRecord record,
                bool lockFlag,
                bool actualize
            )
        {
            Code.NotNull(record, "record");

            WriteRecordCommand command = new WriteRecordCommand(this)
            {
                Record = record,
                Actualize = actualize,
                Lock = lockFlag
            };

            ExecuteCommand(command);

            MarcRecord result = command.Record;

            if (ReferenceEquals(result, null))
            {
                throw new IrbisException("result record is null");
            }

            return result;
        }

        // ========================================================

        /// <summary>
        /// Create or update many records.
        /// </summary>
        [NotNull]
        public MarcRecord[] WriteRecords
            (
                [NotNull] MarcRecord[] records,
                bool lockFlag,
                bool actualize
            )
        {
            Code.NotNull(records, "records");

            if (records.Length == 0)
            {
                return records;
            }

            WriteRecordsCommand command = new WriteRecordsCommand(this)
            {
                Actualize = actualize,
                Lock = lockFlag
            };
            foreach (MarcRecord record in records)
            {
                RecordReference reference = new RecordReference(record)
                {
                    HostName = Host,
                    Database = Database
                };
                command.References.Add(reference);
            }

            ExecuteCommand(command);

            return records;
        }


        #endregion

        // ========================================================

        /// <summary>
        /// Write text file to the server.
        /// </summary>
        public void WriteTextFile
            (
                [NotNull] FileSpecification file
            )
        {
            Code.NotNull(file, "file");

            WriteFileCommand command
                = CommandFactory.GetWriteFileCommand();
            command.Files.Add(file);

            ExecuteCommand(command);
        }

        /// <summary>
        /// Write text files to the server.
        /// </summary>
        public void WriteTextFiles
            (
                params FileSpecification[] files
            )
        {
            WriteFileCommand command
                = CommandFactory.GetWriteFileCommand();
            foreach (FileSpecification file in files)
            {
                command.Files.Add(file);
            }

            ExecuteCommand(command);
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated
        /// with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Disposing.Raise(this);

            if (_connected)
            {
                DisconnectCommand command
                    = CommandFactory.GetDisconnectCommand();

                ExecuteCommand(command);
            }
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Host = reader.ReadNullableString();
            Port = reader.ReadPackedInt32();
            Username = reader.ReadNullableString();
            Password = reader.ReadNullableString();
            Database = reader.ReadNullableString();
            Workstation = (IrbisWorkstation)reader.ReadPackedInt32();
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Host)
                .WritePackedInt32(Port)
                .WriteNullable(Username)
                .WriteNullable(Password)
                .WriteNullable(Database)
                .WritePackedInt32((int)Workstation);
        }

        #endregion

        #region Object members

        #endregion
    }
}
