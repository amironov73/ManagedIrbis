/* IrbisConnection.cs -- client for IRBIS-server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status:moderate
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Network;
using ManagedIrbis.Network.Commands;
using ManagedIrbis.Network.Sockets;
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
        /// 
        /// </summary>
        public const string DefaultHost = "127.0.0.1";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultDatabase = "IBIS";

        /// <summary>
        /// 
        /// </summary>
        public const IrbisWorkstation DefaultWorkstation
            = IrbisWorkstation.Cataloger;

        /// <summary>
        /// 
        /// </summary>
        public const int DefaultPort = 6666;

        ///// <summary>
        ///// 
        ///// </summary>
        //public const string DefaultUsername = "1";

        ///// <summary>
        ///// 
        ///// </summary>
        //public const string DefaultPassword = "1";

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

        // Not supported in .NET Core
        ///// <summary>
        ///// Версия клиента.
        ///// </summary>
        //public static Version ClientVersion = Assembly
        //    .GetExecutingAssembly()
        //    .GetName()
        //    .Version;

        /// <summary>
        /// Признак занятости клиента.
        /// </summary>
        [NotNull]
        public BusyState Busy { get; private set; }

        /// <summary>
        /// Адрес сервера.
        /// </summary>
        /// <value>Адрес сервера в цифровом виде.</value>
        [DefaultValue(DefaultHost)]
        public string Host { get; set; }

        /// <summary>
        /// Порт сервера.
        /// </summary>
        /// <value>Порт сервера (по умолчанию 6666).</value>
        [DefaultValue(DefaultPort)]
        public int Port { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        /// <value>Имя пользователя.</value>
        //[DefaultValue(DefaultUsername)]
        public string Username { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        /// <value>Пароль пользователя.</value>
        //[DefaultValue(DefaultPassword)]
        public string Password { get; set; }

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        /// <value>Служебное имя базы данных (например, "IBIS").</value>
        [DefaultValue(DefaultDatabase)]
        public string Database
        {
            get { return _database; }
            set { _database = value; }
        }

        /// <summary>
        /// Тип АРМ.
        /// </summary>
        /// <value>По умолчанию <see cref="IrbisWorkstation.Cataloger"/>.
        /// </value>
        [DefaultValue(DefaultWorkstation)]
        public IrbisWorkstation Workstation { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public int ClientID { get { return _clientID; } }

        /// <summary>
        /// Номер команды.
        /// </summary>
        public int QueryID { get { return _queryID; } }

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
        /// <see cref="Disconnect"/> или <see cref="Dispose"/>.</value>
        public bool Connected
        {
            get { return _connected; }
        }

        ///// <summary>
        ///// Для ожидания окончания запроса.
        ///// </summary>
        //public WaitHandle WaitHandle
        //{
        //    get { return _waitHandle; }
        //}

        ///// <summary>
        ///// Поток для вывода отладочной информации.
        ///// </summary>
        ///// <remarks><para><c>null</c> означает, что вывод отладочной 
        ///// информации не нужен.</para>
        ///// <para>Обратите внимание, что <see cref="DebugWriter"/>
        ///// не сериализуется, т. к. большинство потоков не умеют
        ///// сериализоваться. Так что при восстановлении клиента
        ///// вам придётся восстанавливать <see cref="DebugWriter"/>
        ///// самостоятельно.</para>
        ///// </remarks>
        //[DefaultValue(null)]
        //public TextWriter DebugWriter
        //{
        //    get { return _debugWriter; }
        //    set { _debugWriter = value; }
        //}

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
        public bool Interrupted { get; set; }

        /// <summary>
        /// Socket.
        /// </summary>
        [NotNull]
        public AbstractClientSocket Socket { get; private set; }

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

            Host = DefaultHost;
            Port = DefaultPort;
            Database = DefaultDatabase;
            Username = null;
            Password = null;
            Workstation = DefaultWorkstation;

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
            ParseConnectionString(connectionString);
            Connect();
        }

        #endregion

        #region Private members

        //private string _configuration;
        private bool _connected;

        // ReSharper disable InconsistentNaming
        private int _clientID;
        private int _queryID;
        // ReSharper restore InconsistentNaming

        private string _database;

        private static Random _random = new Random();

#if !PocketPC
        //private IrbisSocket _socket;
#endif

        private readonly Stack<string> _databaseStack
            = new Stack<string>();

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

        internal void Disconnect()
        {
            Disposing.Raise
                (
                    this
                );

            if (_connected)
            {
                UniversalCommand command = new UniversalCommand
                    (
                        this,
                        CommandCode.UnregisterClient,
                        Username
                    )
                {
                    AcceptAnyResponse = true
                };

                ExecuteCommand(command);
                _connected = false;
            }
        }

        #endregion

        // =========================================================

        #region Public methods

        /// <summary>
        /// Актуализация всех неактуализированных записей
        /// в указанной базе данных.
        /// </summary>
        public void ActualizeDatabase
            (
                [NotNull] string database
            )
        {
            Code.NotNullNorEmpty(database, "database");

            ActualizeRecord
                (
                    database,
                    0
                );
        }

        // =========================================================

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

            ExecuteCommand
                (
                    CommandCode.ActualizeRecord,
                    database,
                    mfn
                );
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
            if (!_connected)
            {
                ConnectCommand command = new ConnectCommand(this);
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
                = new CreateDatabaseCommand(this)
                {
                    Database = databaseName,
                    Description = description,
                    ReaderAccess = readerAccess,
                    Template = template
                };
            ExecuteCommand(command);
        }

        // =========================================================

        /// <summary>
        /// Create dictionary index for specified database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        public void CreateDictionary
            (
                [NotNull] string databaseName
            )
        {
            Code.NotNullNorEmpty(databaseName, "databaseName");

            UniversalCommand command = new UniversalCommand
                (
                    this,
                    CommandCode.CreateDictionary,
                    databaseName
                )
            {
                RelaxResponse = true
            };

            ExecuteCommand(command);
        }

        // ========================================================

        /// <summary>
        /// Delete the database.
        /// </summary>
        /// <remarks>For Administrator only.</remarks>
        public void DeleteDatabase
            (
                [NotNull] string databaseName
            )
        {
            Code.NotNullNorEmpty(databaseName, "databaseName");

            ExecuteCommand
                (
                    CommandCode.DeleteDatabase,
                    databaseName
                );
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

            if (!Connected)
            {
                throw new IrbisException("Not connected");
            }

            command.Verify(true);

            using (new BusyGuard(Busy))
            {
                ClientQuery query = command.CreateQuery();
                query.Verify(true);

                ServerResponse result = command.Execute(query);
                result.Verify(true);
                command.CheckResponse(result);

                return result;
            }
        }

        /// <summary>
        /// Execute any command.
        /// </summary>
        [NotNull]
        public ServerResponse ExecuteCommand
            (
                [NotNull] AbstractCommand command,
                params object[] arguments
            )
        {
            Code.NotNull(command, "command");

            if (!Connected)
            {
                throw new IrbisException("Not connected");
            }

            command.Verify(true);

            using (new BusyGuard(Busy))
            {
                ClientQuery query = command.CreateQuery();

                foreach (object argument in arguments)
                {
                    query.Add(argument);
                }

                query.Verify(true);

                ServerResponse result = command.Execute(query);
                result.Verify(true);
                command.CheckResponse(result);

                return result;
            }
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

            UniversalCommand command = new UniversalCommand
                (
                    this,
                    commandCode
                );

            return ExecuteCommand
                (
                    command,
                    arguments
                );
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

            FormatCommand command = new FormatCommand(this)
            {
                FormatSpecification = format
            };
            command.MfnList.Add(mfn);
            ExecuteCommand(command);

            if (command.FormatResult.IsNullOrEmpty())
            {
                throw new IrbisNetworkException("result is empty");
            }

            string result = command.FormatResult[0];

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

            FormatCommand command = new FormatCommand(this)
            {
                FormatSpecification = format,
                VirtualRecord = record
            };
            ExecuteCommand(command);

            if (command.FormatResult.IsNullOrEmpty())
            {
                throw new IrbisNetworkException("result is empty");
            }

            string result = command.FormatResult[0];

            return result;
        }

        /// <summary>
        /// Форматирование записей.
        /// </summary>
        [NotNull]
        public string[] FormatRecords
            (
                [NotNull] string format,
                [NotNull] IEnumerable<int> mfnList
            )
        {
            Code.NotNull(mfnList, "mfnList");
            Code.NotNull(format, "format");

            FormatCommand command = new FormatCommand(this)
            {
                FormatSpecification = format
            };
            command.MfnList.AddRange(mfnList);

            if (command.MfnList.Count == 0)
            {
                return new string[0];
            }

            ExecuteCommand(command);

            if (command.FormatResult.IsNullOrEmpty())
            {
                throw new IrbisNetworkException("result is empty");
            }

            string[] result = command.FormatResult;

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

            UniversalCommand command = new UniversalCommand
                (
                    this,
                    CommandCode.RecordList,
                    databaseName
                );
            ServerResponse response = ExecuteCommand(command);
            DatabaseInfo result
                = DatabaseInfo.ParseServerResponse(response);
            result.Name = databaseName;

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

            DatabaseStatCommand command = new DatabaseStatCommand(this)
            {
                Definition = definition
            };
            ExecuteCommand(command);

            string result = command.Result;

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

            UniversalCommand command = new UniversalCommand
                (
                    this,
                    CommandCode.GetMaxMfn,
                    database
                );

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
            ServerResponse response = ExecuteCommand
                (
                    CommandCode.GetServerStat
                );
            ServerStat result = ServerStat.Parse(response);

            return result;
        }

        // =========================================================

        /// <summary>
        /// Get server version.
        /// </summary>
        [NotNull]
        public IrbisVersion GetServerVersion()
        {
            ServerResponse response
                = ExecuteCommand(CommandCode.ServerInfo);
            IrbisVersion result
                = IrbisVersion.ParseServerResponse(response);

            return result;
        }

        // =========================================================

        /// <summary>
        /// Get list of the databases.
        /// </summary>
        [NotNull]
        public DatabaseInfo[] ListDatabases
            (
                [NotNull] string listFile
            )
        {
            Code.NotNull(listFile, "listFile");

            string menuFile = ReadTextFile(IrbisPath.Data, listFile);
            string[] lines = menuFile.SplitLines();
            DatabaseInfo[] result
                = DatabaseInfo.ParseMenu(lines);

            return result;
        }

        /// <summary>
        /// Get list of the databases.
        /// </summary>
        [NotNull]
        public DatabaseInfo[] ListDatabases()
        {
            return ListDatabases
                (
                    IrbisConstants.AdministratorDatabaseList
                );
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

            string[] result = command.Files;
            if (ReferenceEquals(result, null))
            {
                throw new IrbisException("file list is null");
            }

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

            string[] result = command.Files;
            if (ReferenceEquals(result, null))
            {
                throw new IrbisException("file list is null");
            }

            return result;
        }

        // =========================================================

        /// <summary>
        /// List server processes.
        /// </summary>
        [NotNull]
        public IrbisProcessInfo[] ListProcesses()
        {
            ServerResponse response = ExecuteCommand
                (
                    CommandCode.GetProcessList
                );
            IrbisProcessInfo[] result = IrbisProcessInfo.Parse(response);

            return result;
        }

        // =========================================================

        /// <summary>
        /// List users.
        /// </summary>
        [NotNull]
        public UserInfo[] ListUsers()
        {
            ServerResponse response = ExecuteCommand
                (
                    CommandCode.GetUserList
                );
            UserInfo[] result = UserInfo.Parse(response);

            return result;
        }

        // =========================================================

        /// <summary>
        /// No operation.
        /// </summary>
        public void NoOp()
        {
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
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            connectionString = Regex.Replace
                (
                    connectionString,
                    @"\s+",
                    string.Empty
                );
            if (string.IsNullOrEmpty(connectionString)
                 || !connectionString.Contains("="))
            {
                throw new ArgumentException("connectionString");
            }

            Regex regex = new Regex
                (
                    "(?<name>[^=;]+?)=(?<value>[^;]+)",
                    RegexOptions.IgnoreCase
                    | RegexOptions.IgnorePatternWhitespace
                );
            MatchCollection matches = regex.Matches(connectionString);
            foreach (Match match in matches)
            {
                string name =
                    match.Groups["name"].Value.ToLower();
                string value = match.Groups["value"].Value;
                switch (name)
                {
                    case "host":
                    case "server":
                    case "address":
                        Host = value;
                        break;
                    case "port":
                        Port = int.Parse(value);
                        break;
                    case "user":
                    case "username":
                    case "name":
                    case "login":
                        Username = value;
                        break;
                    case "pwd":
                    case "password":
                        Password = value;
                        break;
                    case "db":
                    case "catalog":
                    case "database":
                        Database = value;
                        break;
                    case "arm":
                    case "workstation":
                        Workstation = (IrbisWorkstation)(byte)(value[0]);
                        break;
                    case "debug":
                        SetLogging(value);
                        break;
                    //case "data":
                    //    UserData = value;
                    //    break;
                    //case "debug":
                    //    StartDebug(value);
                    //    break;
                    //case "etr":
                    //case "stage":
                    //    StageOfWork = value;
                    //    break;
                    default:
                        throw new ArgumentException("connectionString");
                }
            }
        }

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

            PrintTableCommand command = new PrintTableCommand (this)
            {
                Definition = tableDefinition
            };
            ExecuteCommand(command);

            return command.Result;
        }

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
                = new ReadBinaryFileCommand(this)
                {
                    File = file
                };
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
                [CanBeNull] string databaseName,
                [NotNull] string term,
                int numberOfPostings,
                int firstPosting
            )
        {
            Code.NotNullNorEmpty(term, "term");

            ReadPostingsCommand command = new ReadPostingsCommand
                (
                    this
                )
            {
                Database = databaseName,
                Term = term,
                NumberOfPostings = numberOfPostings,
                FirstPosting = firstPosting
            };

            ExecuteCommand(command);

            return command.Postings.ToArray();
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

            ReadRecordCommand command = new ReadRecordCommand(this)
            {
                Mfn = mfn,
                Database = database,
                Lock = lockFlag,
                Format = format
            };
            ExecuteCommand(command);

            return command.ReadedRecord;
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

            ReadRecordCommand command = new ReadRecordCommand(this)
            {
                Mfn = mfn,
                Database = database,
                VersionNumber = versionNumber,
                Format = format
            };
            ExecuteCommand(command);

            return command.ReadedRecord;
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

            FormatCommand command = new FormatCommand(this)
            {
                Database = database,
                FormatSpecification = IrbisFormat.All
            };
            command.MfnList.AddRange(mfnList);

            ServerResponse response = ExecuteCommand(command);

            MarcRecord[] result = MarcRecordUtility.ParseAllFormat
                (
                    database,
                    response
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
                bool reverseOrder
            )
        {
            Code.NotNull(startTerm, "startTerm");

            ReadTermsCommand command
                = new ReadTermsCommand (this)
            {
                Database = Database,
                StartTerm = startTerm,
                NumberOfTerms = numberOfTerms,
                ReverseOrder = reverseOrder
            };
            ExecuteCommand(command);

            return command.Terms.ToArray();
        }

        // ========================================================

        /// <summary>
        /// Чтение текстового файла с сервера.
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

            ServerResponse response = ExecuteCommand(command);
            string[] result = command.GetFileText(response);

            return result[0];
        }


        /// <summary>
        /// Чтение текстового файла с сервера.
        /// </summary>
        [CanBeNull]
        public string ReadTextFile
            (
                IrbisPath path,
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            FileSpecification fileSpecification = new FileSpecification
                (
                    path,
                    Database,
                    fileName
                );

            return ReadTextFile(fileSpecification);
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

            ReadFileCommand command = new ReadFileCommand(this);
            command.Files.AddRange(files);

            ServerResponse response = ExecuteCommand(command);
            string[] result = command.GetFileText(response);

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
            ExecuteCommand(CommandCode.RestartServer);
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

            SearchCommand command = new SearchCommand(this)
            {
                SearchQuery = expression
            };
            ServerResponse response = ExecuteCommand(command);
            int[] result = FoundItem.ParseMfnOnly(response);

            return result;
        }

        // =========================================================

        /// <summary>
        /// Sequential search.
        /// </summary>
        [NotNull]
        public int[] SequentialSearch
            (
                [CanBeNull] string database,
                [NotNull] string expression,
                int firstRecord,
                int numberOfRecords,
                int minMfn,
                int maxMfn,
                [NotNull] string sequential,
                [CanBeNull] string format
            )
        {
            Code.NotNull(expression, "expression");
            Code.NotNull(sequential, "sequential");

            if (string.IsNullOrEmpty(database))
            {
                database = Database;
            }

            SearchCommand command = new SearchCommand(this)
            {
                Database = database,
                SearchQuery = expression,
                FirstRecord = firstRecord,
                NumberOfRecords = numberOfRecords,
                MinMfn = minMfn,
                MaxMfn = maxMfn,
                SequentialSpecification = sequential,
                FormatSpecification = format
            };

            ServerResponse response = ExecuteCommand(command);
            int[] result = FoundItem.ParseMfnOnly(response);

            return result;
        }

        // =========================================================

        /// <summary>
        /// Set logging socket, gather debug info to specified path.
        /// </summary>
        public void SetLogging
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
                = (AbstractClientSocket) Activator.CreateInstance(type);
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

            Socket = socket;
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
            Code.NotNull(text, "text");

            if (text.Length == 0)
            {
                return;
            }

            UniversalTextCommand command = new UniversalTextCommand
                (
                    this,
                    CommandCode.UpdateIniFile,
                    text,
                    IrbisEncoding.Ansi
                );
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

            WriteFileCommand command = new WriteFileCommand (this);
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
            WriteFileCommand command = new WriteFileCommand(this);
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
            Disconnect();
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
