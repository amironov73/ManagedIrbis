/* IrbisConnection.cs -- client for IRBIS-server
 * Ars Magna project, http://arsmagna.ru
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
using ManagedIrbis.Network;
using ManagedIrbis.Network.Commands;
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
        /// Разделитель строк в пакете запроса к серверу.
        /// </summary>
        public const char QueryLineDelimiter = (char)0x0A;

        /// <summary>
        /// Разделитель строк в пакете ответа сервера.
        /// </summary>
        public const string ResponseLineDelimiter = "";

        /// <summary>
        /// 
        /// </summary>
        public const int MaxPostings = 32758;

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
        /// Количество попыток повторения команды по умолчанию.
        /// </summary>
        public const int DefaultRetryCount = 5;

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

        ///// <summary>
        ///// Разрешение делать шестнадцатиричный дамп полученных от сервера пакетов.
        ///// </summary>
        //[DefaultValue(false)]
        //public bool AllowHexadecimalDump { get; set; }

        /// <summary>
        /// Количество повторений команды при неудаче.
        /// </summary>
        [DefaultValue(DefaultRetryCount)]
        public int RetryCount { get; set; }

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
        public IrbisClientSocket Socket { get; private set; }

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
            //_waitHandle = new ManualResetEvent(true);

            Busy = new BusyState();

            Host = DefaultHost;
            Port = DefaultPort;
            Database = DefaultDatabase;
            //Username = DefaultUsername;
            //Password = DefaultPassword;
            Username = null;
            Password = null;
            Workstation = DefaultWorkstation;
            RetryCount = DefaultRetryCount;

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

        //[NonSerialized]
        //private ManualResetEvent _waitHandle;

        //[NonSerialized]
        //private TextWriter _debugWriter;


        // ReSharper disable InconsistentNaming
        private int _clientID;
        private int _queryID;
        // ReSharper restore InconsistentNaming

        private string _database;

        //private readonly Encoding _utf8 = new UTF8Encoding(false, false);
        //private readonly Encoding _cp1251 = Encoding.GetEncoding(1251);

        private static Random _random = new Random();

#if !PocketPC
        //private IrbisSocket _socket;
#endif

        private readonly Stack<string> _databaseStack
            = new Stack<string>();

        // ReSharper disable InconsistentNaming
        internal void GenerateClientID()
        {
            _clientID = _random.Next(1000000, 9999999);
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

            ActualizeRecord(0);
        }

        /// <summary>
        /// Актуализация записи с указанным MFN.
        /// </summary>
        /// <remarks>Если MFN=0, то актуализируются
        /// все неактуализированные записи БД.
        /// </remarks>
        public void ActualizeRecord
            (
                int mfn
            )
        {
            Code.Nonnegative(mfn, "mfn");

            ExecuteCommand
                (
                    CommandCode.ActualizeRecord,
                    Database,
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
            IrbisConnection result = new IrbisConnection
            {
                Host = Host,
                Port = Port,
                Username = Username,
                Password = Password,
                Database = Database,
                Workstation = Workstation,
                RetryCount = RetryCount,
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
        /// Подключение к серверу.
        /// </summary>
        public void Connect()
        {
            if (!_connected)
            {
                ConnectCommand command = new ConnectCommand(this);
                IrbisClientQuery query = command.CreateQuery();
                ServerResponse result = command.Execute(query);
                command.CheckResponse(result);
                _connected = true;
            }
        }

        /// <summary>
        /// Создание базы данных.
        /// </summary>
        public void CreateDatabase
            (
                [NotNull] string databaseName,
                bool readerAccess,
                [CanBeNull] string template
            )
        {
            Code.NotNullNorEmpty(databaseName, "databaseName");

            CreateDatabaseCommand command
                = new CreateDatabaseCommand(this)
                {
                    Database = databaseName,
                    ReaderAccess = readerAccess,
                    Template = template
                };
            ExecuteCommand(command);
        }

        /// <summary>
        /// Удаление указанной базы данных.
        /// </summary>
        /// <param name="databaseName"><c>null</c> означает
        /// текущую базу данных</param>
        public void DeleteDatabase
            (
                [CanBeNull] string databaseName
            )
        {
            if (string.IsNullOrEmpty(databaseName))
            {
                databaseName = Database;
            }

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
                IrbisClientQuery query = command.CreateQuery();
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
                IrbisClientQuery query = command.CreateQuery();

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
            ServerResponse response = ExecuteCommand(command);

            string result = response.GetUtfString();
            result = IrbisText.IrbisToWindows(result);

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
            ServerResponse response = ExecuteCommand(command);

            string result = response.GetUtfString();
            result = IrbisText.IrbisToWindows(result);

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

            ServerResponse response = ExecuteCommand(command);

            if (command.MfnList.Count == 1)
            {
                return new[]
                {
                    IrbisText.IrbisToWindows(response.GetUtfString())
                };
            }

            return FormatCommand.GetFormatResult(response);
        }

        #endregion

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

        /// <summary>
        /// Get list of the databases.
        /// </summary>
        [NotNull]
        public IrbisDatabaseInfo[] ListDatabases
            (
                [NotNull] string listFile
            )
        {
            Code.NotNull(listFile, "listFile");

            string menuFile = ReadTextFile(IrbisPath.Data, listFile);
            string[] lines = menuFile.SplitLines();
            IrbisDatabaseInfo[] result
                = IrbisDatabaseInfo.ParseMenu(lines);

            return result;
        }

        /// <summary>
        /// Get list of the databases.
        /// </summary>
        [NotNull]
        public IrbisDatabaseInfo[] ListDatabases()
        {
            return ListDatabases("dbnam1.mnu");
        }

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

        /// <summary>
        /// List users.
        /// </summary>
        [NotNull]
        public IrbisUserInfo[] ListUsers()
        {
            ServerResponse response = ExecuteCommand
                (
                    CommandCode.GetUserList
                );
            IrbisUserInfo[] result = IrbisUserInfo.Parse(response);

            return result;
        }

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
                int mfn,
                bool lockFlag,
                [CanBeNull] string format
            )
        {
            Code.Positive(mfn, "mfn");

            ReadRecordCommand command = new ReadRecordCommand(this)
            {
                Mfn = mfn,
                Database = Database,
                Lock = lockFlag,
                Format = format
            };
            ServerResponse response = ExecuteCommand(command);

            MarcRecord record = new MarcRecord
            {
                Database = Database
            };

            MarcRecord result = ProtocolText.ParseResponseForReadRecord
                (
                    response,
                    record
                );
            result.Verify(true);

            return result;
        }

        /// <summary>
        /// Чтение записи.
        /// </summary>
        [NotNull]
        public MarcRecord ReadRecord
            (
                int mfn
            )
        {
            return ReadRecord(mfn, false, null);
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

        /// <summary>
        /// Reload dictionary index for specified database.
        /// </summary>
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

        /// <summary>
        /// Reload master file for specified database.
        /// </summary>
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

        /// <summary>
        /// Restart server.
        /// </summary>
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

        /// <summary>
        /// Поиск записей
        /// </summary>
        [NotNull]
        [StringFormatMethod("format")]
        public int[] Search
            (
                string format,
                params object[] args
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Загрузка записей по результатам поиска.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        [StringFormatMethod("format")]
        public MarcRecord[] SearchRead
            (
                [NotNull] string format,
                params object[] args
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Загрузка одной записи по результатам поиска.
        /// </summary>
        [CanBeNull]
        [StringFormatMethod("format")]
        public MarcRecord SearchReadOneRecord
            (
                [NotNull] string format,
                params object[] args
            )
        {
            throw new NotImplementedException();
        }

        // =========================================================

        /// <summary>
        /// Опустошение базы данных.
        /// </summary>
        /// <param name="databaseName"><c>null</c> означает
        /// текущую базу данных.</param>
        public void TruncateDatabase
            (
                [CanBeNull] string databaseName
            )
        {
            if (string.IsNullOrEmpty(databaseName))
            {
                databaseName = Database;
            }

            ExecuteCommand
                (
                    CommandCode.EmptyDatabase,
                    databaseName
                );
        }

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

        /// <summary>
        /// Разблокирование записей.
        /// </summary>
        public void UnlockRecords
            (
                params int[] mfnList
            )
        {
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
                    arguments
                );
        }

        // ========================================================

        #region WriteRecord

        /// <summary>
        /// Сохранение записи.
        /// </summary>
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

            ServerResponse response = ExecuteCommand(command);

            ProtocolText.ParseResponseForWriteRecord
                (
                    response,
                    record
                );

            return record;
        }

        /// <summary>
        /// Сохранение записи.
        /// </summary>
        public MarcRecord WriteRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            return WriteRecord
                (
                    record,
                    false,
                    true
                );
        }

        #endregion

        // ========================================================

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
