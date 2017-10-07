// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Irbis64Client.cs -- wrapper for irbis64_client.dll
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

using AM;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

#endregion

// ReSharper disable RedundantNameQualifier

namespace OfficialWrapper
{
    /// <summary>
    /// Wrapper for irbis64_client.dll
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class Irbis64Client
        : Component
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public const int MaxPostings = 32758;

        /// <summary>
        /// 
        /// </summary>
        public const int MaxBuffer = 30 * 1024;

        /// <summary>
        /// 
        /// </summary>
        public const int DefaultBufferSize = 320 * 1024;

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
            = IrbisWorkstation.Administrator;

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultPort = "6666";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultUsername = "1";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultPassword = "1";

        #endregion

        #region Private members

        /// <summary>
        /// Используется для поиска MFN в выдаче.
        /// Они выглядят так:
        /// 1#
        /// 5#
        /// 100#
        /// и т. п.
        /// </summary>
        private readonly Regex _mfnRegex = new Regex
            (
            @"^(?<MFN>\d+)#",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.IgnorePatternWhitespace
            );

        private string _configuration;
        private bool _connected;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Irbis64Client"/> class.
        /// </summary>
        public Irbis64Client
            (

            )
        {
            Host = DefaultHost;
            Port = DefaultPort;
            Database = DefaultDatabase;
            Username = DefaultUsername;
            Password = DefaultPassword;
            Workstation = DefaultWorkstation;
            BufferSize = DefaultBufferSize;
        }

        #endregion

        #region Properties

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
        public string Port { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        /// <value>Имя пользователя.</value>
        [DefaultValue(DefaultUsername)]
        public string Username { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        /// <value>Пароль пользователя.</value>
        [DefaultValue(DefaultPassword)]
        public string Password { get; set; }

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        /// <value>Служебное имя базы данных (например, "IBIS").</value>
        [DefaultValue(DefaultDatabase)]
        public string Database { get; set; }

        /// <summary>
        /// Тип АРМ.
        /// </summary>
        /// <value>По умолчанию <see cref="IrbisWorkstation.Administrator"/>.
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
        /// Автоматическое переподключение.
        /// </summary>
        /// <value>Выполнять ли переподключение к серверу.</value>
        public bool AutoReconnect { get; set; }

        /// <summary>
        /// Слушатель для сброса логов.
        /// </summary>
        /// <value>null - логи не нужны.</value>
        public TraceListener LogListener { get; set; }

        /// <summary>
        /// Размер буфера для interop-функций.
        /// </summary>
        /// <value></value>
        [DefaultValue(DefaultBufferSize)]
        public int BufferSize { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Регистрация клиента на сервере.
        /// </summary>
        public void Connect()
        {
            WriteLog("Connect ENTER");
            WriteLog
                (
                    "\thost={0}, port={1}, username={2}, password={3}",
                    Host,
                    Port,
                    Username,
                    Password
                );

            using (var buffer = new InteropBuffer())
            {
                IrbisReturnCode returnCode = NativeMethods64.IC_reg
                    (
                        Host,
                        Port,
                        (char)IrbisWorkstation.Administrator,
                        Username,
                        Password,
                        ref buffer.Handle,
                        buffer.Size
                    );
                CheckReturnCode(returnCode);
                _configuration = buffer.AnsiZString;
            }
            _connected = true;

            WriteLog("Connect LEAVE");
        }

        /// <summary>
        /// Разрегистрация клиента на сервере.
        /// </summary>
        public void Disconnect()
        {
            WriteLog("Disconnect ENTER");

            if (Connected)
            {
                NativeMethods64.IC_unreg(Username);
                _connected = false;
            }

            WriteLog("Disconnect LEAVE");
        }

        /// <summary>
        /// Установка режима работы через Web-шлюз.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> [enable].</param>
        public void SetWebServerMode(bool enable)
        {
            WriteLog
                (
                    "SerWebServerMode ENTER: enable={0}",
                    enable);
            CheckConnected();
            IrbisReturnCode returnCode = NativeMethods64.IC_set_webserver
                (
                    enable
                );
            CheckReturnCode(returnCode);
            WriteLog("SerWebServerMode LEAVE");
        }

        /// <summary>
        /// Установка имени шлюза при работе через Web-шлюз.
        /// </summary>
        /// <param name="name">The name.</param>
        public void SetWebCgiMode(string name)
        {
            WriteLog
                (
                    "SetWebCgiMode ENTER: name={0}",
                    name);

            CheckConnected();
            IrbisReturnCode returnCode = NativeMethods64.IC_set_webcgi
                (
                    name
                );
            CheckReturnCode(returnCode);

            WriteLog("SetWebCgiMode LEAVE");
        }

        /// <summary>
        /// Установка режима ожидания ответа от сервера.
        /// </summary>
        /// <param name="enable">Включить режим "мертвого" ожидания
        /// ответа от сервера.</param>
        public void SetSocketBlockingMode(bool enable)
        {
            WriteLog
                (
                    "SetSocketBlockingMode ENTER: enable={0}",
                    enable);

            CheckConnected();
            IrbisReturnCode returnCode = NativeMethods64
                .IC_set_blocksocket
                (
                    enable
                );
            CheckReturnCode(returnCode);

            WriteLog("SetSocketBlockingMode LEAVE");
        }

        /// <summary>
        /// Установка таймаута соединения с сервером.
        /// Установка времени появления заставки ожидания.
        /// </summary>
        /// <param name="timeout">Интервала максимального ожидания 
        /// ответа от сервера (в секундах), после чего прерванная функция 
        /// возвращает ERR_USER. По умолчанию этот интервал - 30 сек.
        /// </param>
        public void SetShowWaiting(int timeout)
        {
            WriteLog
                (
                    "SetShowWaiting ENTER: timeout={0}",
                    timeout);

            IrbisReturnCode returnCode = NativeMethods64
                .IC_set_show_waiting
                (
                    timeout
                );
            CheckReturnCode(returnCode);

            WriteLog("SetShowWaiting LEAVE");
        }

        /// <summary>
        /// Установка интервала подтверждения.
        /// </summary>
        /// <param name="interval">Значение интервала в минутах.</param>
        /// <remarks>Функция определяет интервал времени, по истечении 
        /// которого автоматически выдается подтверждение о том, 
        /// что клиентское приложение находится в рабочем состоянии 
        /// («живет») – если в течение этого интервала не выдавался 
        /// ни один запрос на сервер. По умолчанию – 1 минута. 
        /// Автоматическое подтверждение выполняется с помощью 
        /// функции IC_nooperation.</remarks>
        public void SetClientTimeLive(int interval)
        {
            WriteLog
                (
                    "SetClientTimeLive ENTER: interval={0}",
                    interval);

            CheckConnected();
            IrbisReturnCode returnCode = NativeMethods64
                .IC_set_client_time_live
                (
                    interval
                );
            CheckReturnCode(returnCode);

            WriteLog("SetClientTimeLive LEAVE");
        }

        /// <summary>
        /// Определение завершения очередного обращения к серверу.
        /// </summary>
        /// <returns>Обращение к серверу еще не завершено.</returns>
        public bool IsBusy()
        {
            //WriteLog("IsBusy ENTER");

            CheckConnected();
            bool result = NativeMethods64.IC_isbusy();

            //WriteLog("IsBusy LEAVE: result={0}", result);

            return result;
        }

        /// <summary>
        /// Обновление клиентской конфигурации на сервере.
        /// </summary>
        /// <param name="inifile">The inifile.</param>
        public void UpdateConfiguration(string inifile)
        {
            WriteLog("UpdateConfiguration ENTER");

            CheckConnected();
            IrbisReturnCode returnCode;
            do
            {
                returnCode = NativeMethods64.IC_update_ini
                    (
                        inifile
                    );
            } while (TryReconnect(returnCode));
            CheckReturnCode(returnCode);

            WriteLog("UpdateConfiguration LEAVE");
        }

        /// <summary>
        /// Чтение текстового ресурса.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public string ReadTextResource
            (
                IrbisPath path,
                string filename,
                int size
            )
        {
            WriteLog
                (
                    "ReadTextResource ENTER: path={0}, "
                    + "database={1}, filename={2}, size={3}",
                    path,
                    Database,
                    filename,
                    size
                );

            CheckConnected();
            using (var buffer = new InteropBuffer(size))
            {
                IrbisReturnCode returnCode;
                do
                {
                    returnCode = NativeMethods64.IC_getresourse
                        (
                            path,
                            Database,
                            filename,
                            ref buffer.Handle,
                            size
                        );
                } while (TryReconnect(returnCode));
                CheckReturnCode(returnCode);
                string result = buffer.AnsiZString;
                WriteLog("ReadTextResource LEAVE");
                return result;
            }
        }

        /// <summary>
        /// Очистка памяти кэша.
        /// </summary>
        public void ClearResourceCache()
        {
            WriteLog("ClearResourceCache ENTER");

            CheckConnected();
            IrbisReturnCode returnCode;
            do
            {
                returnCode = NativeMethods64.IC_clearresourse();
            } while (TryReconnect(returnCode));
            CheckReturnCode(returnCode);

            WriteLog("ClearResourceCache LEAVE");
        }

        public Dictionary<string, string> ReadTextResourceGroup
            (
            IEnumerable<string> names
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Чтение двоичного ресурса.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public byte[] ReadBinaryResource
            (
            IrbisPath path,
            string filename
            )
        {
            WriteLog
                (
                    "ReadBinaryResource ENTER: path={0}, "
                    + "filename={1}",
                    path,
                    filename
                );

            CheckConnected();

            IrbisBuffer result = null;
            IrbisReturnCode returnCode;
            do
            {
                returnCode
                    = NativeMethods64.IC_getbinaryresourse
                        (
                            path,
                            Database,
                            filename,
                            ref result
                        );
            } while (TryReconnect(returnCode));
            CheckReturnCode(returnCode);

            WriteLog("ReadBinaryResource LEAVE");
            return result.Data;
        }

        /// <summary>
        /// Запись текстового ресурса на сервер.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="text">The text.</param>
        public void WriteTextResource
            (
            IrbisPath path,
            string filename,
            string text
            )
        {
            WriteLog
                (
                    "WriteTextResource ENTER: path={0}, "
                    + "filename={1}, text={2}",
                    path,
                    filename,
                    text
                );

            CheckConnected();
            IrbisReturnCode returnCode;
            do
            {
                returnCode = NativeMethods64.IC_putresourse
                    (
                        path,
                        Database,
                        filename,
                        text
                    );
            } while (TryReconnect(returnCode));
            CheckReturnCode(returnCode);

            WriteLog("WriteTextResource LEAVE");
        }

        /// <summary>
        /// Reads the raw record.
        /// </summary>
        /// <param name="mfn">The MFN.</param>
        /// <param name="lockFlag">if set to <c>true</c> [lock flag].</param>
        /// <returns></returns>
        public string ReadRawRecord
            (
            int mfn,
            bool lockFlag
            )
        {
            WriteLog
                (
                    "ReadRawRecord ENTER: mnf={0}, lockFlag={1}",
                    mfn,
                    lockFlag
                );

            CheckConnected();
            string result;
            using (InteropBuffer buffer
                    = new InteropBuffer(BufferSize)
                )
            {
                IrbisReturnCode returnCode;
                do
                {
                    returnCode = NativeMethods64.IC_read
                        (
                            Database,
                            mfn,
                            lockFlag,
                            ref buffer.Handle,
                            BufferSize
                        );
                } while (TryReconnect(returnCode)
                    || TryIncreaseBuffer(returnCode, buffer));
                CheckReturnCode(returnCode);
                result = buffer.Utf8ZString;
            }

            WriteLog("ReadRawRecord LEAVE");
            return result;
        }

        ///// <summary>
        ///// Reads the record.
        ///// </summary>
        ///// <param name="mfn">The MFN.</param>
        ///// <param name="lockFlag">if set to <c>true</c> [lock flag].</param>
        ///// <param name="info">The info.</param>
        ///// <returns></returns>
        //public MarcRecord ReadRecord
        //    (
        //        int mfn,
        //        bool lockFlag,
        //        out Irbis64RecordInfo info
        //    )
        //{
        //    WriteLog
        //        (
        //            "ReadRecord ENTER: mfn={0}, lockFlag={1}",
        //            mfn,
        //            lockFlag
        //        );
        //    string text = ReadRawRecord
        //        (
        //            mfn,
        //            lockFlag
        //        );
        //    IrbisEncoder encoder = new IrbisEncoder();
        //    MarcRecord result = encoder.DecodeRecord(text);
        //    info = encoder.DecodeRecordInfo(text);

        //    WriteLog("ReadRecord LEAVE");
        //    return result;
        //}

        //public MarcRecord ReadRecord(int mfn)
        //{
        //    Irbis64RecordInfo info;
        //    return ReadRecord
        //        (
        //            mfn,
        //            false,
        //            out info
        //        );
        //}

        public string WriteRawRecord
            (
            int mfn,
            string record,
            bool lockFlag,
            bool ifUpdate
            )
        {
            WriteLog
                (
                    "WriteRawRecord ENTER: mfn={0}, lockFlag={1}, ifUpdate={2}",
                    mfn,
                    lockFlag,
                    ifUpdate
                );

            CheckConnected();
            string result;
            using (var buffer = new InteropBuffer(BufferSize))
            {
                IrbisReturnCode returnCode;
                do
                {
                    buffer.Utf8ZString = record;
                    returnCode = NativeMethods64.IC_update
                        (
                            Database,
                            lockFlag,
                            ifUpdate,
                            ref buffer.Handle,
                            BufferSize
                        );
                } while (TryReconnect(returnCode)
                    || TryIncreaseBuffer(returnCode, buffer));
                CheckReturnCode(returnCode);
                result = buffer.Utf8ZString;
            }

            WriteLog("WriteRawRecord LEAVE");
            return result;
        }

        /// <summary>
        /// Writes the record.
        /// </summary>
        /// <param name="mfn">The MFN.</param>
        /// <param name="version">The version.</param>
        /// <param name="status">The status.</param>
        /// <param name="record">The record.</param>
        /// <param name="lockFlag">if set to <c>true</c> [lock flag].</param>
        /// <param name="ifUpdate">if set to <c>true</c> [if update].</param>
        /// <returns>Запись после обработки сервером.</returns>
        public string WriteRecord
            (
            int mfn,
            int version,
            int status,
            MarcRecord record,
            bool lockFlag,
            bool ifUpdate
            )
        {
            WriteLog
                (
                    "WriteRecord ENTER: mfn={0}, lockFlag={1}, ifUpdate={2}",
                    mfn,
                    lockFlag,
                    ifUpdate
                );

            string raw = new IrbisEncoder()
                .EncodeRecord
                (
                    record,
                    0,
                    mfn,
                    status,
                    version
                );
            string result = WriteRawRecord
                (
                    mfn,
                    raw,
                    lockFlag,
                    ifUpdate
                );

            WriteLog("WriteRecord LEAVE");
            return result;
        }

        /// <summary>
        /// Formats the record.
        /// </summary>
        /// <param name="mfn">The MFN.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public string FormatRecord
            (
            int mfn,
            string format
            )
        {
            WriteLog
                (
                    "FormatRecord ENTER: mfn={0}, format={1}",
                    mfn,
                    format
                );

            CheckConnected();
            IrbisReturnCode returnCode;
            byte[] buffer;

            do
            {
                buffer = new byte[BufferSize];
                returnCode = NativeMethods64.IC_sformat
                    (
                        Database,
                        mfn,
                        ToUtf(format),
                        buffer,
                        BufferSize
                    );
            } while (TryReconnect(returnCode)
                || TryIncreaseBuffer(returnCode));

            CheckReturnCode(returnCode);

            string result = NativeUtility.FromUtfZ(buffer);

            if (!string.IsNullOrEmpty(result))
            {
                string[] parts = result.Split
                    (
                        new[] { Environment.NewLine },
                        2,
                        StringSplitOptions.None
                    );
                if (parts.Length > 1)
                {
                    result = parts[1];
                }
            }

            WriteLog("FormatRecord LEAVE");
            return result;
        }

        /// <summary>
        /// Formats the raw record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public string FormatRawRecord
            (
            string record,
            string format
            )
        {
            WriteLog("FormatRawRecord ENTER");

            CheckConnected();
            IrbisReturnCode returnCode;
            byte[] buffer;

            do
            {
                buffer = new byte[BufferSize];
                returnCode = NativeMethods64.IC_record_sformat
                    (
                        Database,
                        ToUtf(format),
                        ToUtf(record),
                        buffer,
                        BufferSize
                    );
            } while (TryReconnect(returnCode)
                || TryIncreaseBuffer(returnCode));

            CheckReturnCode(returnCode);
            string result = FromUtfZ(buffer);
            if (!string.IsNullOrEmpty(result))
            {
                result = result
                    .SplitLines()
                    .Skip(1)
                    .MergeLines();
            }

            WriteLog("FormatRawRecord LEAVE");
            return result;
        }

        /// <summary>
        /// Searches the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public string Search
            (
            string expression,
            string format
            )
        {
            WriteLog
                (
                    "Search ENTER: expression={0}, format={1}",
                    expression,
                    format
                );

            IrbisReturnCode returnCode;

            CheckConnected();
            byte[] buffer;
            do
            {
                buffer = new byte[BufferSize];
                returnCode = NativeMethods64
                    .IC_search
                    (
                        Database,
                        ToUtf(expression),
                        0,
                        1,
                        ToUtf(format),
                        buffer,
                        buffer.Length
                    );
            } while (TryReconnect(returnCode)
                || TryIncreaseBuffer(returnCode));

            string result = FromUtfZ(buffer);

            WriteLog("Search LEAVE");
            return result;
        }

        /// <summary>
        /// Searches the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public int[] Search
            (
            string expression
            )
        {
            WriteLog
                (
                    "Search ENTER: expression={0}",
                    expression
                );

            string found = Search
                (
                    expression,
                    String.Empty
                );
            int[] mfns = ExtractMFNs(found);

            WriteLog("Search LEAVE");

            return mfns;
        }

        /// <summary>
        /// Pings this instance.
        /// </summary>
        public void Ping()
        {
            WriteLog("Ping ENTER");

            CheckConnected();
            NativeMethods64.IC_nooperation();

            WriteLog("Ping LEAVE");
        }

        /// <summary>
        /// Gets the max MFN.
        /// </summary>
        /// <returns></returns>
        public int GetMaxMfn()
        {
            WriteLog("GetMaxMfn ENTER");

            CheckConnected();
            IrbisReturnCode returnCode = NativeMethods64
                .IC_maxmfn
                (
                    Database
                );
            CheckReturnCode(returnCode);

            WriteLog
                (
                    "GetMaxMfn LEAVE, return={0}",
                    returnCode);
            return (int)returnCode;
        }

        #endregion

        #region Utility

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
                    match.Groups["name"].Value.ToLowerInvariant();
                string value = match.Groups["value"].Value;
                switch (name)
                {
                    case "host":
                    case "server":
                        Host = value;
                        break;
                    case "port":
                        Port = value;
                        break;
                    case "user":
                    case "username":
                    case "name":
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
                    default:
                        throw new ArgumentException("connectionString");
                }
            }
        }

        /// <summary>
        /// Checks the connected.
        /// </summary>
        public void CheckConnected()
        {
            if (!Connected)
            {
                WriteLog("NOT CONNECTED!!!");
                throw new IrbisException("Not connected");
            }
        }

        /// <summary>
        /// Checks the return code.
        /// </summary>
        /// <param name="returnCode">The return code.</param>
        public void CheckReturnCode(IrbisReturnCode returnCode)
        {
            if (returnCode < 0)
            {
                WriteLog
                    (
                        "Irbis return code={0}",
                        returnCode
                    );
                throw new IrbisException
                    (
                    string.Format
                        (
                            "Irbis return code={0}",
                            returnCode
                        )
                    );
            }
        }

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void WriteLog
            (
            string format,
            params object[] args
            )
        {
            if (LogListener != null)
            {
                LogListener.WriteLine
                    (
                        string.Format
                            (
                                format,
                                args
                            )
                    );
            }
        }

        /// <summary>
        /// Tries the reconnect.
        /// </summary>
        /// <param name="returnCode">The return code.</param>
        /// <returns></returns>
        public bool TryReconnect(IrbisReturnCode returnCode)
        {
            if (AutoReconnect &&
                 (returnCode == IrbisReturnCode.ClientNotInUse))
            {
                _connected = false;
                Connect();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries the increase buffer.
        /// </summary>
        /// <param name="returnCode">The return code.</param>
        /// <returns></returns>
        public bool TryIncreaseBuffer(IrbisReturnCode returnCode)
        {
            if (returnCode == IrbisReturnCode.BadBufferSize)
            {
                BufferSize *= 2;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries the increase buffer.
        /// </summary>
        /// <param name="returnCode">The return code.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public bool TryIncreaseBuffer
            (
                IrbisReturnCode returnCode,
                InteropBuffer buffer
            )
        {
            if (returnCode == IrbisReturnCode.BadBufferSize)
            {
                return buffer.Increase();
            }
            return false;
        }

        /// <summary>
        /// Toes the utf.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public byte[] ToUtf(string text)
        {
            return NativeUtility.ToUtf(text);
        }

        public string FromUtf(byte[] buffer)
        {
            return NativeUtility.FromUtf(buffer);
        }

        /// <summary>
        /// Froms the utf Z.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public string FromUtfZ(byte[] buffer)
        {
            return NativeUtility.FromUtfZ(buffer);
        }

        /// <summary>
        /// Extracts the MF ns.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public int[] ExtractMFNs(string text)
        {
            MatchCollection matches = _mfnRegex.Matches(text);

            return matches
                .OfType<Match>()
                .Select(_ => int.Parse(_.Groups["MFN"].Value))
                .ToArray();

            //List<int> result = new List<int>();
            //foreach (Match match in matches)
            //{
            //    int mfn = int.Parse(match.Groups["MFN"].Value);
            //    result.Add(mfn);
            //}
            //return result.ToArray();
        }

        #endregion

        /// <summary>
        /// Releases the unmanaged resources used by the 
        /// <see cref="T:System.ComponentModel.Component"/> 
        /// and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both 
        /// managed and unmanaged resources; false to release 
        /// only unmanaged resources.</param>
        protected override void Dispose
            (
                bool disposing
            )
        {
            WriteLog
                (
                    "Dispose ENTER: disposing={0}",
                    disposing
                );

            if (Connected)
            {
                Disconnect();
            }

            WriteLog("Dispose LEAVE");
        }
    }
}