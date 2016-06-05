/* ManagedClient64.cs -- client for IRBIS-server
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
using System.Threading;
using System.Threading.Tasks;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion


namespace ManagedClient
{
    /// <summary>
    /// Client for IRBIS-server.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class ManagedClient64
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
        /// Вызывается, когда меняется состояние Busy;
        /// </summary>
        public event EventHandler BusyChanged;

        /// <summary>
        /// Вызывается перед уничтожением объекта.
        /// </summary>
        public event EventHandler Disposing;

        #endregion

        #region Properties

        /// <summary>
        /// Версия клиента.
        /// </summary>
        public static Version ClientVersion = Assembly
            .GetExecutingAssembly()
            .GetName()
            .Version;

        /// <summary>
        /// Признак занятости клиента.
        /// </summary>
        public bool Busy { get; private set; }

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
        /// Конфигурация клиента.
        /// </summary>
        /// <value>Высылается сервером при подключении.</value>
        public string Configuration
        {
            get { return _configuration; }
        }

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

        /// <summary>
        /// Для ожидания окончания запроса.
        /// </summary>
        public WaitHandle WaitHandle
        {
            get { return _waitHandle; }
        }

        /// <summary>
        /// Поток для вывода отладочной информации.
        /// </summary>
        /// <remarks><para><c>null</c> означает, что вывод отладочной 
        /// информации не нужен.</para>
        /// <para>Обратите внимание, что <see cref="DebugWriter"/>
        /// не сериализуется, т. к. большинство потоков не умеют
        /// сериализоваться. Так что при восстановлении клиента
        /// вам придётся восстанавливать <see cref="DebugWriter"/>
        /// самостоятельно.</para>
        /// </remarks>
        [DefaultValue(null)]
        public TextWriter DebugWriter
        {
            get { return _debugWriter; }
            set { _debugWriter = value; }
        }

        /// <summary>
        /// Разрешение делать шестнадцатиричный дамп полученных от сервера пакетов.
        /// </summary>
        [DefaultValue(false)]
        public bool AllowHexadecimalDump { get; set; }

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
        public ManagedClient64()
        {
            _waitHandle = new ManualResetEvent(true);

#if !PocketPC
            // По умолчанию создаем простой синхронный сокет.
            //_socket = new IrbisSocket();
#endif

            Host = DefaultHost;
            Port = DefaultPort;
            Database = DefaultDatabase;
            //Username = DefaultUsername;
            //Password = DefaultPassword;
            Username = null;
            Password = null;
            Workstation = DefaultWorkstation;
            RetryCount = DefaultRetryCount;
        }

        /// <summary>
        /// Конструктор с подключением.
        /// </summary>
        public ManagedClient64
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

        private string _configuration;
        private bool _connected;

        [NonSerialized]
        private ManualResetEvent _waitHandle;

        [NonSerialized]
        private TextWriter _debugWriter;

        [NonSerialized]
        private TcpClient _client;

        private int _userID;
        private int _queryID;

#if !PocketPC
        //[NonSerialized]
        //private IrbisIniFile _settings;

        //private IrbisSearchEngine SearchEngine;
#endif

        private string _database;

        private readonly Encoding _utf8 = new UTF8Encoding(false, false);
        private readonly Encoding _cp1251 = Encoding.GetEncoding(1251);

#if !PocketPC
        //private IrbisSocket _socket;
#endif

        private readonly Stack<string> _databaseStack
            = new Stack<string>();

        #endregion

        #region Public methods

        /// <summary>
        /// Подключение к серверу.
        /// </summary>
        public void Connect()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Отключение от сервера.
        /// </summary>
        public void Disconnect()
        {
            throw new NotImplementedException();
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
        /// Парсинг строки подключения.
        /// </summary>
        public void ParseConnectionString
            (
                [NotNull] string connectionString
            )
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            _configuration = reader.ReadNullableString();
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
                .WritePackedInt32((int) Workstation)
                .WriteNullable(Configuration);
        }

        #endregion

        #region Object members

        #endregion
    }
}
