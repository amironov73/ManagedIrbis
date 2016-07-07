/* IrbisConnection.cs -- client for IRBIS-server
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using AM.IO;
using AM.Runtime;
using AM.Threading;
using CodeJam;

using JetBrains.Annotations;
using ManagedClient.ImportExport;
using ManagedClient.Network;
using ManagedClient.Network.Commands;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion


namespace ManagedClient
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
        private TextWriter _debugWriter;

        //[NonSerialized]
        private TcpClient _client;

        private int _clientID;
        private int _queryID;

#if !PocketPC
        //[NonSerialized]
        //private IrbisIniFile _settings;

        //private IrbisSearchEngine SearchEngine;
#endif

        private string _database;

        //private readonly Encoding _utf8 = new UTF8Encoding(false, false);
        //private readonly Encoding _cp1251 = Encoding.GetEncoding(1251);

        private static Random _random = new Random();

#if !PocketPC
        //private IrbisSocket _socket;
#endif

        private readonly Stack<string> _databaseStack
            = new Stack<string>();

        internal void GenerateClientID()
        {
            _clientID = _random.Next(1000000, 9999999);
        }

        internal int IncrementCommandNumber()
        {
            return ++_queryID;
        }

        internal void ResetCommandNumber()
        {
            _queryID = 0;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Подключение к серверу.
        /// </summary>
        public void Connect()
        {
            if (!_connected)
            {
                ConnectCommand command = new ConnectCommand(this);
                IrbisClientQuery query = command.CreateQuery();
                IrbisServerResponse result = command.Execute(query);
                command.CheckResponse(result);
                _connected = true;
            }
        }

        /// <summary>
        /// Отключение от сервера.
        /// </summary>
        public void Disconnect()
        {
            if (_connected)
            {
                DisconnectCommand command = new DisconnectCommand(this);
                IrbisClientQuery query = command.CreateQuery();
                IrbisServerResponse result = command.Execute(query);
                command.CheckResponse(result);
                _connected = false;
            }
        }

        /// <summary>
        /// Execute any command.
        /// </summary>
        [NotNull]
        public IrbisServerResponse ExecuteCommand
            (
                [NotNull] AbstractCommand command
            )
        {
            Code.NotNull(command, "command");

            if (!Connected)
            {
                throw new IrbisException("Not connected");
            }

            using (new BusyGuard(Busy))
            {
                IrbisClientQuery query = command.CreateQuery();

                IrbisServerResponse result = command.Execute(query);
                command.CheckResponse(result);

                return result;
            }
        }

        /// <summary>
        /// Execute any command.
        /// </summary>
        [NotNull]
        public IrbisServerResponse ExecuteCommand
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

            using (new BusyGuard(Busy))
            {
                IrbisClientQuery query = command.CreateQuery();

                foreach (object argument in arguments)
                {
                    query.Add(argument);
                }

                IrbisServerResponse result = command.Execute(query);
                command.CheckResponse(result);

                return result;
            }
        }

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get server version.
        /// </summary>
        [NotNull]
        public IrbisVersion GetServerVersion()
        {
            IrbisServerResponse response
                = ExecuteCommand(new VersionCommand(this));

            
            IrbisVersion result
                = IrbisVersion.ParseServerResponse(response);

            return result;
        }

        /// <summary>
        /// No operation.
        /// </summary>
        public void NoOp()
        {
            ExecuteCommand(new NopCommand(this));
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
        /// Временное переключение на другую базу данных.
        /// </summary>
        [CanBeNull]
        public string PushDatabase
            (
                [NotNull] string newDatabase
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Чтение записи.
        /// </summary>
        public IrbisRecord ReadRecord
            (
                int mfn
            )
        {
            ReadRecordCommand command = new ReadRecordCommand(this)
            {
                Mfn = mfn
            };
            IrbisServerResponse response = ExecuteCommand(command);

            return ProtocolText.ParseResponseForSingleRecord(response);
        }

        /// <summary>
        /// Возврат к предыдущей базе данных.
        /// </summary>
        /// <returns></returns>
        [CanBeNull]
        public string PopDatabase ()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
        public IrbisRecord[] SearchRead
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
        public IrbisRecord SearchReadOneRecord
            (
                [NotNull] string format,
                params object[] args
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сохранение записи.
        /// </summary>
        public void WriteRecord
            (
                [NotNull] IrbisRecord record,
                bool needLock,
                bool invertedFileUpdate
            )
        {
            throw new NotImplementedException();
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
            Workstation = (IrbisWorkstation) reader.ReadPackedInt32();
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
                .WritePackedInt32((int) Workstation);
        }

        #endregion

        #region Object members

        #endregion
    }
}
