// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisConnection.cs -- client for IRBIS-server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
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
    public class IrbisConnection
        : IIrbisConnection
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

#if !UAP

        // TODO Implement properly

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
                Code.NotNull(value, "value");

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

        /// <summary>
        /// Remote INI-file for the client.
        /// </summary>
        [CanBeNull]
        public IniFile IniFile
        {
            get { return _iniFile; }
        }

        /// <summary>
        /// Server version.
        /// </summary>
        [CanBeNull]
        public IrbisVersion ServerVersion { get; internal set; }

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
        /// Флаг отключения.
        /// </summary>
        public bool Disposed
        {
            get { return _disposed; }
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
            Log.Trace("IrbisConnection::Constructor");

            Busy = new BusyState();

            Host = ConnectionSettings.DefaultHost;
            Port = ConnectionSettings.DefaultPort;
            Database = ConnectionSettings.DefaultDatabase;
            //Username = "111";
            //Password = "111";
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

            // ReSharper disable VirtualMemberCallInConstructor
            ParseConnectionString(connectionString); //-V3068
            Connect(); //-V3068
            // ReSharper restore VirtualMemberCallInConstructor
        }

        #endregion

        #region Private members

        // ReSharper disable InconsistentNaming
        internal bool _connected;
        internal bool _disposed;
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

        private IniFile _iniFile;

        /// <summary>
        /// Raw last client query.
        /// </summary>
        [CanBeNull]
        internal byte[][] RawClientRequest { get; set; }

        /// <summary>
        /// Raw last server response.
        /// </summary>
        [CanBeNull]
        internal byte[] RawServerResponse { get; set; }

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

        /// <inheritdoc cref="IIrbisConnection.ActualizeRecord" />
        public virtual void ActualizeRecord
            (
                string database,
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

        /// <inheritdoc cref="IIrbisConnection.Clone()" />
        public virtual IrbisConnection Clone()
        {
            IrbisConnection result = Clone(Connected);

            return result;
        }

        /// <inheritdoc cref="IIrbisConnection.Clone(bool)" />
        public virtual IrbisConnection Clone
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

        /// <inheritdoc cref="IIrbisConnection.Connect" />
        public virtual IniFile Connect()
        {
            // TODO use Executive

            if (!_connected)
            {
                Log.Trace
                    (
                        "IrbisConnection::Connect"
                    );

                ConnectCommand command = CommandFactory.GetConnectCommand();
                ClientQuery query = command.CreateQuery();
                ServerResponse response = command.Execute(query);
                command.CheckResponse(response);
                _connected = true;

                string iniText = command.Configuration
                    .ThrowIfNull("command.Configuration");
                IniFile result = new IniFile();
                StringReader reader = new StringReader(iniText);
                result.Read(reader);
                _iniFile = result;

                if (!string.IsNullOrEmpty(command.ServerVersion))
                {
                    ServerVersion = new IrbisVersion
                    {
                        Version = command.ServerVersion
                    };
                }

                return result;
            }

            return _iniFile;
        }

        // ========================================================

        /// <inheritdoc cref="IIrbisConnection.CorrectVirtualRecord(string,MarcRecord,GblStatement[])"/>
        public virtual MarcRecord CorrectVirtualRecord
            (
                string database,
                MarcRecord record,
                GblStatement[] statements
            )
        {
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(record, "record");
            Code.NotNull(statements, "statements");

            GblVirtualCommand command = CommandFactory.GetGblVirtualCommand();
            command.Database = database;
            command.Record = record;
            command.Statements = statements;

            ExecuteCommand(command);

            return command.Result
                .ThrowIfNull("command.Result");
        }

        /// <inheritdoc cref="IIrbisConnection.CorrectVirtualRecord(string,MarcRecord,string)"/>
        public virtual MarcRecord CorrectVirtualRecord
            (
                string database,
                MarcRecord record,
                string filename
            )
        {
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(filename, "filename");

            GblVirtualCommand command = CommandFactory.GetGblVirtualCommand();
            command.Database = database;
            command.Record = record;
            command.FileName = filename;

            ExecuteCommand(command);

            return command.Result
                .ThrowIfNull("command.Result");
        }

        // ========================================================

        /// <inheritdoc cref="IIrbisConnection.CreateDatabase" />
        public virtual void CreateDatabase
            (
                string databaseName,
                string description,
                bool readerAccess,
                string template
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

        /// <inheritdoc cref="IIrbisConnection.CreateDictionary" />
        public virtual void CreateDictionary
            (
                string database
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

        /// <inheritdoc cref="IIrbisConnection.DeleteDatabase" />
        public virtual void DeleteDatabase
            (
                string database
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

        /// <inheritdoc cref="IIrbisConnection.ExecuteCommand(AbstractCommand)" />
        public virtual ServerResponse ExecuteCommand
            (
                AbstractCommand command
            )
        {
            Code.NotNull(command, "command");

            Log.Trace("IrbisConnection::ExecuteCommand");

            RawClientRequest = null;
            RawServerResponse = null;

            ExecutionContext context = new ExecutionContext
                (
                    this,
                    command
                );
            ServerResponse result = Executive.ExecuteCommand(context);

            RawClientRequest = null;
            RawServerResponse = null;

            return result;
        }

        /// <inheritdoc cref="IIrbisConnection.ExecuteCommand(string,object[])" />
        public virtual ServerResponse ExecuteCommand
            (
                string commandCode,
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

        /// <inheritdoc cref="IIrbisConnection.FormatRecord(string,int)" />
        public virtual string FormatRecord
            (
                string format,
                int mfn
            )
        {
            Code.NotNull(format, "format");
            Code.Positive(mfn, "mfn");

            FormatCommand command = CommandFactory.GetFormatCommand();
            command.FormatSpecification = format;
            command.MfnList.Add(mfn);

            ExecuteCommand(command);

            string result = command.FormatResult
                .ThrowIfNullOrEmpty("command.FormatResult")
                [0];

            return result;
        }

        /// <inheritdoc cref="IIrbisConnection.FormatRecord(string,MarcRecord)" />
        public string FormatRecord
            (
                string format,
                MarcRecord record
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

        /// <inheritdoc cref="IIrbisConnection.FormatRecords" />
        public virtual string[] FormatRecords
            (
                string database,
                string format,
                IEnumerable<int> mfnList
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
                return StringUtility.EmptyArray;
            }

            ExecuteCommand(command);

            string[] result = command.FormatResult
                .ThrowIfNull("command.FormatResult");

            return result;
        }

        #endregion

        // =========================================================

        /// <inheritdoc cref="IIrbisConnection.GetDatabaseInfo" />
        public virtual DatabaseInfo GetDatabaseInfo
            (
                string databaseName
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

        // =========================================================

        /// <inheritdoc cref="IIrbisConnection.GetDatabaseStat" />
        public virtual string GetDatabaseStat
            (
                StatDefinition definition
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

        /// <inheritdoc cref="IIrbisConnection.GetMaxMfn()" />
        public virtual int GetMaxMfn()
        {
            return GetMaxMfn(Database);
        }

        /// <inheritdoc cref="IIrbisConnection.GetMaxMfn(string)" />
        public virtual int GetMaxMfn
            (
                string database
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

        /// <inheritdoc cref="IIrbisConnection.GetServerStat" />
        public virtual ServerStat GetServerStat()
        {
            ServerStatCommand command
                = CommandFactory.GetServerStatCommand();
            ExecuteCommand(command);

            ServerStat result = command.Result
                .ThrowIfNull("command.Result");

            return result;
        }

        // =========================================================

        /// <inheritdoc cref="IIrbisConnection.GetServerVersion" />
        public virtual IrbisVersion GetServerVersion()
        {
            ServerVersionCommand command
                = CommandFactory.GetServerVersionCommand();
            ExecuteCommand(command);

            IrbisVersion result = command.Result
                .ThrowIfNull("command.Result");

            ServerVersion = result;

            return result;
        }

        // =========================================================

        /// <inheritdoc cref="IIrbisConnection.GlobalCorrection" />
        public virtual GblResult GlobalCorrection
            (
                GblSettings settings
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

        /// <inheritdoc cref="IIrbisConnection.ListFiles(FileSpecification)" />
        public virtual string[] ListFiles
            (
                FileSpecification specification
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

        /// <inheritdoc cref="IIrbisConnection.ListFiles(FileSpecification[])" />
        public virtual string[] ListFiles
            (
                FileSpecification[] specifications
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

        /// <inheritdoc cref="IIrbisConnection.ListProcesses" />
        public virtual IrbisProcessInfo[] ListProcesses()
        {
            ListProcessesCommand command
                = CommandFactory.GetListProcessCommand();
            ExecuteCommand(command);

            IrbisProcessInfo[] result = command.Result
                .ThrowIfNullOrEmpty("command.Result");

            return result;
        }

        // =========================================================

        /// <inheritdoc cref="IIrbisConnection.ListUsers" />
        public virtual UserInfo[] ListUsers()
        {
            ListUsersCommand command
                = CommandFactory.GetListUsersCommand();
            ExecuteCommand(command);

            UserInfo[] result = command.Result
                .ThrowIfNull("command.Result");

            return result;
        }

        // =========================================================

        /// <inheritdoc cref="IIrbisConnection.NoOp" />
        public virtual void NoOp()
        {
            NopCommand command = CommandFactory.GetNopCommand();

            ExecuteCommand(command);
        }

        /// <inheritdoc cref="IIrbisConnection.ParseConnectionString" />
        public virtual void ParseConnectionString
            (
                string connectionString
            )
        {
            Code.NotNull(connectionString, "connectionString");

            ConnectionSettings settings = new ConnectionSettings();
            settings.ParseConnectionString(connectionString);
            settings.ApplyToConnection(this);
        }

        // ========================================================

        /// <inheritdoc cref="IIrbisConnection.PopDatabase" />
        public virtual string PopDatabase()
        {
            string result = Database;

            if (_databaseStack.Count != 0)
            {
                Database = _databaseStack.Pop();
            }

            return result;
        }

        // ========================================================

        /// <inheritdoc cref="IIrbisConnection.PrintTable" />
        public virtual string PrintTable
            (
                TableDefinition tableDefinition
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

        /// <inheritdoc cref="IIrbisConnection.PushDatabase" />
        public virtual string PushDatabase
            (
                string newDatabase
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
        public virtual byte[] ReadBinaryFile
            (
                FileSpecification file
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
        public virtual TermPosting[] ReadPostings
            (
                PostingParameters parameters
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
        public virtual MarcRecord ReadRecord
            (
                string database,
                int mfn,
                bool lockFlag,
                string format
            )
        {
            Code.NotNullNorEmpty(database, "database");
            Code.Positive(mfn, "mfn");

            ReadRecordCommand command = CommandFactory.GetReadRecordCommand();
            command.Mfn = mfn;
            command.Database = database;
            command.Lock = lockFlag;
            command.Format = format;

            ExecuteCommand(command);

            return command.Record
                .ThrowIfNull("no record retrieved");
        }

        /// <summary>
        /// Чтение указанной версии и расформатирование записи.
        /// </summary>
        /// <remarks><c>null</c>означает, что затребованной
        /// версии записи нет.</remarks>
        [CanBeNull]
        public virtual MarcRecord ReadRecord
            (
                string database,
                int mfn,
                int versionNumber,
                string format
            )
        {
            Code.NotNullNorEmpty(database, "database");
            Code.Positive(mfn, "mfn");

            ReadRecordCommand command = CommandFactory.GetReadRecordCommand();
            command.Mfn = mfn;
            command.Database = database;
            command.VersionNumber = versionNumber;
            command.Format = format;

            ExecuteCommand(command);

            return command.Record;
        }

        #endregion

        // ========================================================

        /// <summary>
        /// Read search terms from index.
        /// </summary>
        [NotNull]
        public virtual TermInfo[] ReadTerms
            (
                TermParameters parameters
            )
        {
            Code.NotNull(parameters, "parameters");

            ReadTermsCommand command = CommandFactory.GetReadTermsCommand();
            command.ApplyParameters(parameters);

            ExecuteCommand(command);

            return command.Terms.ThrowIfNull("command.Terms");
        }

        // ========================================================

        /// <summary>
        /// Read text file from the server.
        /// </summary>
        [CanBeNull]
        public virtual string ReadTextFile
            (
                FileSpecification fileSpecification
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
        public virtual string[] ReadTextFiles
            (
                FileSpecification[] files
            )
        {
            Code.NotNull(files, "files");

            if (files.Length == 0)
            {
                return StringUtility.EmptyArray;
            }

            ReadFileCommand command = CommandFactory.GetReadFileCommand();
            command.Files.AddRange(files);

            ExecuteCommand(command);

            string[] result = command.Result
                .ThrowIfNullOrEmpty("command.Result");

            return result;
        }

        // =========================================================

        /// <summary>
        /// Reconnect to the server.
        /// </summary>
        public virtual void Reconnect()
        {
            Log.Trace
                (
                    "IrbisConnection::Reconnect"
                );

            if (_connected)
            {
                DisconnectCommand command
                    = CommandFactory.GetDisconnectCommand();

                ExecuteCommand(command);
            }

            if (!ReferenceEquals(_iniFile, null))
            {
                _iniFile.Dispose();
                _iniFile = null;
            }

            Connect();
        }

        // =========================================================

        /// <summary>
        /// Reload dictionary index for specified database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        public virtual void ReloadDictionary
            (
                string databaseName
            )
        {
            Code.NotNullNorEmpty(databaseName, "databaseName");

            ReloadDictionaryCommand command
                = CommandFactory.GetReloadDictionaryCommand();
            command.Database = databaseName;

            ExecuteCommand(command);
        }

        // =========================================================

        /// <summary>
        /// Reload master file for specified database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        public virtual void ReloadMasterFile
            (
                string databaseName
            )
        {
            Code.NotNullNorEmpty(databaseName, "databaseName");

            ReloadMasterFileCommand command
                = CommandFactory.GetReloadMasterFileCommand();
            command.Database = databaseName;

            ExecuteCommand(command);
        }

        // =========================================================

        /// <summary>
        /// Restart server.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        public virtual void RestartServer()
        {
            RestartServerCommand command
                = CommandFactory.GetRestartServerCommand();

            ExecuteCommand(command);
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

            if (ReferenceEquals(settings, null))
            {
                throw new IrbisException
                    (
                        "Decrypted state is null"
                    );
            }

            IrbisConnection result = new IrbisConnection();
            settings.ApplyToConnection(result);

            return result;
        }

        // =========================================================

        /// <summary>
        /// Поиск записей.
        /// </summary>
        [NotNull]
        public virtual int[] Search
            (
                string expression
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
        public virtual int[] SequentialSearch
            (
                SearchParameters parameters
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
        public virtual CommandFactory SetCommandFactory
            (
                CommandFactory newFactory
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
        public virtual CommandFactory SetCommandFactory
            (
                string typeName
            )
        {
            Code.NotNull(typeName, "typeName");

#if !WINMOBILE && !PocketPC

            Type type = Type.GetType(typeName, true);
            CommandFactory newFactory
                = (CommandFactory)Activator.CreateInstance
                (
                    type,
                    this
                );
            CommandFactory previous = SetCommandFactory(newFactory);

            return previous;

#else
            return CommandFactory;

#endif
        }

        // =========================================================

        /// <summary>
        /// Set execution engine.
        /// </summary>
        [NotNull]
        public virtual AbstractEngine SetEngine
            (
                AbstractEngine engine
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
        public virtual AbstractEngine SetEngine
            (
                string typeName
            )
        {
            Code.NotNull(typeName, "typeName");

#if !WINMOBILE && !PocketPC

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

#else

            return Executive;

#endif
        }

        // =========================================================

        /// <summary>
        /// Set logging socket, gather debug info to specified path.
        /// </summary>
        public virtual void SetNetworkLogging
            (
                string loggingPath
            )
        {
            Code.NotNullNorEmpty(loggingPath, "loggingPath");

#if !WINMOBILE && !PocketPC

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

            SetSocket(newSocket);

#endif

        }

        // =========================================================

        /// <summary>
        ///
        /// </summary>
        public virtual void SetRetry
            (
                int retryCount,
                Func<Exception, bool> resolver
            )
        {
            RetryClientSocket oldSocket
                = ClientSocketUtility.FindSocket<RetryClientSocket>(this);

            if (retryCount <= 0)
            {
                if (!ReferenceEquals(oldSocket, null))
                {
                    SetSocket
                        (
                            oldSocket.InnerSocket
                            .ThrowIfNull("oldSocket.InnerSocket")
                        );
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
        public virtual void SetSocket
            (
                AbstractClientSocket socket
            )
        {
            Code.NotNull(socket, "socket");

            if (Connected)
            {
                throw new IrbisException
                    (
                        "Can't set socket while connected"
                    );
            }

            socket.Connection = this;
            Socket = socket;
        }

        // =========================================================

        /// <summary>
        /// Temporary "shutdown" the connection for some reason.
        /// </summary>
        [NotNull]
        public virtual string Suspend()
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
        public virtual void TruncateDatabase
            (
                string databaseName
            )
        {
            Code.NotNullNorEmpty(databaseName, "databaseName");

            TruncateDatabaseCommand command
                = CommandFactory.GetTruncateDatabaseCommand();
            command.Database = databaseName;

            ExecuteCommand(command);
        }

        // =========================================================

        /// <summary>
        /// Unlock the specified database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        public virtual void UnlockDatabase
            (
                string databaseName
            )
        {
            Code.NotNullNorEmpty(databaseName, "databaseName");

            UnlockDatabaseCommand command
                = CommandFactory.GetUnlockDatabaseCommand();
            command.Database = databaseName;

            ExecuteCommand(command);
        }

        // =========================================================

        /// <summary>
        /// Unlock specified records.
        /// </summary>
        public virtual void UnlockRecords
            (
                string database,
                params int[] mfnList
            )
        {
            // TODO: write UnlockRecordsTest

            Code.NotNullNorEmpty(database, "database");

            if (mfnList.Length == 0)
            {
                return;
            }

            UnlockRecordsCommand command
                = CommandFactory.GetUnlockRecordsCommand();
            command.Database = database;
            command.Records.AddRange(mfnList);

            ExecuteCommand(command);
        }

        // =========================================================

        /// <summary>
        /// Update server INI-file for current client.
        /// </summary>
        public virtual void UpdateIniFile
            (
                string[] lines
            )
        {
            if (lines.IsNullOrEmpty())
            {
                return;
            }

            UpdateIniFileCommand command
                = CommandFactory.GetUpdateIniFileCommand();
            command.Lines = lines;

            ExecuteCommand(command);
        }

        // ========================================================

        /// <inheritdoc cref="IIrbisConnection.UpdateUserList" />
        public virtual void UpdateUserList
            (
                UserInfo[] userList
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

        /// <inheritdoc cref="IIrbisConnection.WriteRecord(ManagedIrbis.MarcRecord,bool,bool,bool)" />
        public virtual MarcRecord WriteRecord
            (
                MarcRecord record,
                bool lockFlag,
                bool actualize,
                bool dontParseResponse
            )
        {
            Code.NotNull(record, "record");

            WriteRecordCommand command = new WriteRecordCommand(this)
            {
                Record = record,
                Actualize = actualize,
                Lock = lockFlag,
                DontParseResponse = dontParseResponse
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

        /// <inheritdoc cref="IIrbisConnection.WriteRecords" />
        public virtual MarcRecord[] WriteRecords
            (
                MarcRecord[] records,
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

        /// <inheritdoc cref="IIrbisConnection.WriteTextFile" />
        public virtual void WriteTextFile
            (
                FileSpecification file
            )
        {
            Code.NotNull(file, "file");

            WriteFileCommand command
                = CommandFactory.GetWriteFileCommand();
            command.Files.Add(file);

            ExecuteCommand(command);
        }

        /// <inheritdoc cref="IIrbisConnection.WriteTextFiles" />
        public virtual void WriteTextFiles
            (
                params FileSpecification[] files
            )
        {
            WriteFileCommand command = CommandFactory.GetWriteFileCommand();
            foreach (FileSpecification file in files)
            {
                command.Files.Add(file);
            }

            ExecuteCommand(command);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public virtual void Dispose()
        {
            Log.Trace("IrbisConnection::Dispose");

            Disposing.Raise(this);

            if (_disposed)
            {
                Log.Warn("IrbisConnection::Dispose: already disposed");
            }

            if (_connected)
            {
                DisconnectCommand command = CommandFactory.GetDisconnectCommand();
                ExecuteCommand(command);
            }

            if (!ReferenceEquals(_iniFile, null))
            {
                _iniFile.Dispose();
                _iniFile = null;
            }

            Socket.Dispose();

            _disposed = true;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public virtual void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Host = reader.ReadNullableString()
                .ThrowIfNull("Host");
            Port = reader.ReadPackedInt32();
            string username = reader.ReadNullableString();
            if (!string.IsNullOrEmpty(username))
            {
                Username = username;
            }
            string password = reader.ReadNullableString();
            if (!ReferenceEquals(password, null))
            {
                Password = password;
            }
            Database = reader.ReadNullableString()
                .ThrowIfNull("Database");
            Workstation = (IrbisWorkstation)reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public virtual void SaveToStream
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
    }
}
