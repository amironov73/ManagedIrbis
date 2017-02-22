// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisProxy.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Proxy2017
{
    /// <summary>
    /// Рабочий код прокси.
    /// </summary>
    static class IrbisProxy
    {
        public static Version Version = new Version(1, 0, 10, 26);

        /// <summary>
        /// Признак остановки.
        /// </summary>
        public static volatile bool Stop;

        /// <summary>
        /// Нагрузка на сервер: количество одновременных
        /// соединений в данный момент.
        /// </summary>
        public static int Load;

        /// <summary>
        /// Простая последовательная очередь задач
        /// на запись в логи.
        /// </summary>
        private static TaskQueue<TransactionInfo> Engine;

        /// <summary>
        /// Прослушивающий сокет.
        /// </summary>
        private static Socket ListenerSocket;

        /// <summary>
        /// Последовательный номер обрабатываемой транзакции
        /// «запрос-ответ».
        /// </summary>
        private static long TransactionIndex;

        /// <summary>
        /// Таймаут приема данных в миллисекундах.
        /// 0 — не устанавливать.
        /// -1 – установить равным бесконечности.
        /// </summary>
        private static int ReceiveTimeout;

        /// <summary>
        /// Таймаут посылки данных в миллисекундах.
        /// 0 – не устанавливать.
        /// -1 – установить равным бесконечности.
        /// </summary>
        private static int SendTimeout;

        /// <summary>
        /// Номер прослушивающего порта.
        /// </summary>
        private static int LocalPort;

        /// <summary>
        /// Длина очереди прослушивающего сокета.
        /// </summary>
        private static int Backlog;

        /// <summary>
        /// Адрес сервера ИРБИС64.
        /// </summary>
        private static IPAddress RemoteIP;

        /// <summary>
        /// Порт, прослушиваемый сервером ИРБИС64.
        /// </summary>
        private static int RemotePort;

        /// <summary>
        /// Имя файла, в который выводится дамп.
        /// Если не задано, то используется консоль.
        /// </summary>
        private static string DumpTo;

        /// <summary>
        /// Выводить в консоль общую информацию о транзакции?
        /// </summary>
        private static bool DumpGeneralInfo;

        /// <summary>
        /// Выводить в консоль заголовок запроса?
        /// </summary>
        private static bool DumpRequestHeader;

        /// <summary>
        /// Выводить в консоль тело запроса?
        /// </summary>
        private static bool DumpRequestBody;

        /// <summary>
        /// Выводить в консоль заголовок ответа?
        /// </summary>
        private static bool DumpResponseHeader;

        /// <summary>
        /// Выводить в консоль тело ответа?
        /// </summary>
        private static bool DumpResponseBody;

        /// <summary>
        /// Выводить ли сведения об ошибках?
        /// </summary>
        private static bool DumpErrors;

        /// <summary>
        /// Писать лог подключений?
        /// </summary>
        private static bool WriteAccessLog;

        /// <summary>
        /// Имя файла лога подключений.
        /// </summary>
        private static string AccessLogFileName;

        /// <summary>
        /// Писать лог поисковых запросов?
        /// </summary>
        private static bool WriteSearchLog;

        /// <summary>
        /// Имя файла лога поисковых запросов.
        /// </summary>
        private static string SearchLogFileName;

        /// <summary>
        /// Имя файла с логами ошибок.
        /// </summary>
        private static string ErrorLogFileName;

        /// <summary>
        /// Записывать сетевую активность.
        /// </summary>
        private static bool WriteNetworkLog;

        /// <summary>
        /// Имя файла лога сетевой активности.
        /// </summary>
        private static string NetworkLogFileName;

        /// <summary>
        /// Команда, запускаемая для сообщения
        /// сисадмину о возникших проблемах.
        /// </summary>
        private static string WakeupCommand;

        /// <summary>
        /// Аргументы, передаваемые программе
        /// для извещения сисадмина.
        /// </summary>
        private static string WakeupArguments;

        /// <summary>
        /// Промежуток (в секундах) между
        /// последовательными запусками
        /// команды для сообщения сисадмину.
        /// </summary>
        private static int WakeupInterval;

        /// <summary>
        /// Добавлять "* HAVE=1" в запросы
        /// АРМ "Читатель"
        /// </summary>
        private static bool AddHave;

        /// <summary>
        /// Фильтр входящих соединений.
        /// </summary>
        private static IpFilter IpFilter;

        // ====================================================================

        public static void DumpSettings()
        {
            DrawLine();
            DrawLine();
            WriteDump("STARTED={0}", DateTime.Now);
            WriteDump("PROXY={0}", Version);
            WriteDump("CLR={0}", Environment.Version);
            WriteDump("ENVIROMENT={0}", Environment.OSVersion);
            WriteDump("USED={0}", Environment.WorkingSet);
            WriteDump("receive-timeout={0}", ReceiveTimeout);
            WriteDump("send-timeout={0}", SendTimeout);
            WriteDump("local-port={0}", LocalPort);
            WriteDump("backlog={0}", Backlog);
            WriteDump("remote-ip={0}", RemoteIP);
            WriteDump("remote-port={0}", RemotePort);
            WriteDump("dump-to={0}", DumpTo);
            WriteDump("dump-general-info={0}", DumpGeneralInfo);
            WriteDump("dump-request-header={0}", DumpRequestHeader);
            WriteDump("dump-request-body={0}", DumpRequestBody);
            WriteDump("dump-response-header={0}", DumpResponseHeader);
            WriteDump("dump-response-body={0}", DumpResponseBody);
            WriteDump("dump-errors={0}", DumpErrors);
            WriteDump("write-access-log={0}", WriteAccessLog);
            WriteDump("access-log-file-name={0}", AccessLogFileName);
            WriteDump("write-search-log={0}", WriteSearchLog);
            WriteDump("search-log-file-name={0}", SearchLogFileName);
            WriteDump("error-log-file-name={0}", ErrorLogFileName);
            WriteDump("write-network-log={0}", WriteNetworkLog);
            WriteDump("network-log-file-name={0}", NetworkLogFileName);
            WriteDump("wakeup-command={0}", WakeupCommand);
            WriteDump("wakeup-arguments={0}", WakeupArguments);
            WriteDump("wakeup-interval={0}", WakeupInterval);
            WriteDump("add-have={0}", AddHave);
            WriteDump("ip-filter={0}", IpFilter);
            DrawLine();
        }

        // ====================================================================

        /// <summary>
        /// Инициализация: считываем конфигурацию.
        /// </summary>
        public static void Initialize()
        {
            Engine = new TaskQueue<TransactionInfo>();

            ReceiveTimeout = CM.GetInt32
                (
                    "receive-timeout",
                    0
                );
            SendTimeout = CM.GetInt32
                (
                    "send-timeout",
                    0
                );
            LocalPort = CM.GetInt32
                (
                    "local-port",
                    5555
                );
            Backlog = CM.GetInt32
                (
                    "backlog",
                    10
                );
            RemoteIP = CM.GetAddress
                (
                    "remote-ip",
                    IPAddress.Loopback
                );
            RemotePort = CM.GetInt32
                (
                    "remote-port",
                    6666
                );
            DumpTo = CM.GetString
                (
                    "dump-to",
                    null
                );
            DumpGeneralInfo = CM.GetBoolean
                (
                    "dump-general-info",
                    false
                );
            DumpRequestHeader = CM.GetBoolean
                (
                    "dump-request-header",
                    false
                );
            DumpRequestBody = CM.GetBoolean
                (
                    "dump-request-body",
                    false
                );
            DumpResponseHeader = CM.GetBoolean
                (
                    "dump-response-header",
                    false
                );
            DumpResponseBody = CM.GetBoolean
                (
                    "dump-response-body",
                    false
                );
            DumpErrors = CM.GetBoolean
                (
                    "dump-errors",
                    false
                );
            WriteAccessLog = CM.GetBoolean
                (
                    "write-access-log",
                    false
                );
            AccessLogFileName = CM.GetString
                (
                    "access-log-file-name",
                    @"C:\access.log"
                );
            WriteSearchLog = CM.GetBoolean
                (
                    "write-search-log",
                    false
                );
            SearchLogFileName = CM.GetString
                (
                    "search-log-file-name",
                    @"C:\search.log"
                );
            ErrorLogFileName = CM.GetString
                (
                    "error-log-file-name",
                    @"C:\errors.log"
                );
            WriteNetworkLog = CM.GetBoolean
                (
                    "write-network-log",
                    false
                );
            NetworkLogFileName = CM.GetString
                (
                    "network-log-file-name",
                    @"C:\network.log"
                );
            WakeupCommand = CM.GetString
                (
                    "wakeup-command",
                    string.Empty
                );
            WakeupArguments = CM.GetString
                (
                    "wakeup-arguments",
                    null
                );
            WakeupInterval = CM.GetInt32
                (
                    "wakeup-interval",
                    300
                );
            AddHave = CM.GetBoolean
                (
                    "add-have",
                    false
                );
            IpFilter = new IpFilter
                (
                    CM.GetString
                    (
                        "ip-filter",
                        "*"
                    )
                );

            DumpSettings();
        }

        // ====================================================================

        public static void WriteException
            (
                Exception exception
            )
        {
            if (!DumpErrors
                || string.IsNullOrEmpty(ErrorLogFileName))
            {
                return;
            }

            try
            {
                string fileName = GetTodayFile(ErrorLogFileName);
                using (StreamWriter writer = new StreamWriter
                    (
                    fileName,
                    true
                    ))
                {
                    writer.WriteLine("------------------------------------------------------------");
                    writer.WriteLine("DATE, TIME: {0}", DateTime.Now);
                    writer.WriteLine(exception.ToString());
                    writer.WriteLine();
                }
            }
            catch
            {
                DumpErrors = false;
            }
        }

        // ====================================================================

        public static void WriteDump
            (
                string format,
                params object[] args
            )
        {
            if (string.IsNullOrEmpty(DumpTo))
            {
                Console.WriteLine(format, args);
                return;
            }

            try
            {
                string fileName = GetTodayFile(DumpTo);
                using (StreamWriter writer = new StreamWriter
                    (
                        fileName,
                        true
                    ))
                {
                    writer.WriteLine(format, args);
                }
            }
            catch
            {
                DumpTo = null;
            }
        }

        // ====================================================================

        static void DrawLine()
        {
            WriteDump
                (
                    "------------------------------------------------------------"
                );
        }

        /// <summary>
        /// Настройка сокета (таймаут подключения и посылка
        /// без задержек.
        /// </summary>
        static void SetupSocket
            (
                Socket socket
            )
        {
            if (SendTimeout != 0)
            {
                socket.SendTimeout = SendTimeout;
            }
            if (ReceiveTimeout != 0)
            {
                socket.ReceiveTimeout = ReceiveTimeout;
            }

            socket.NoDelay = true;
        }

        // ====================================================================

        /// <summary>
        /// Прочитать из сокета не менее указанного количества байт.
        /// </summary>
        /// <returns>Удалось ли?</returns>
        static bool ReceiveExact
            (
                Socket socket,
                MemoryStream memory,
                int howMany
            )
        {
            try
            {
                byte[] buffer = new byte[1000];
                int toRead = Math.Min(buffer.Length, howMany);
                while (toRead > 0)
                {
                    int readed = socket.Receive
                        (
                            buffer,
                            0,
                            toRead,
                            SocketFlags.None
                        );
                    if (readed <= 0)
                    {
                        return false;
                    }
                    memory.Write
                        (
                            buffer,
                            0,
                            readed
                        );
                    howMany -= readed;
                    toRead = Math.Min(buffer.Length, howMany);
                }
            }
            catch (Exception ex)
            {
                WriteException(ex);
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        // ====================================================================

        /// <summary>
        /// Прочитать из сокета максимально возможное число байт.
        /// </summary>
        /// <returns>Удалось ли?</returns>
        static bool ReceiveAll
            (
                Socket socket,
                MemoryStream memory
            )
        {
            byte[] buffer = new byte[1000];
            try
            {
                int readed;
                do
                {
                    readed = socket.Receive(buffer);
                    if (readed > 0)
                    {
                        memory.Write(buffer, 0, readed);
                    }
                } while (readed > 0);
            }
            catch (Exception ex)
            {
                WriteException(ex);
                Debug.WriteLine(ex);
                return false;
            }

            return (memory.Length != 0);
        }

        // ====================================================================

        /// <summary>
        /// Отправить в сеть байты.
        /// </summary>
        /// <returns>Удалось ли?</returns>
        private static bool SendAll
            (
                Socket socket,
                byte[] buffer
            )
        {
            try
            {
                return (socket.Send(buffer) == buffer.Length);
            }
            catch (Exception ex)
            {
                WriteException(ex);
                Debug.WriteLine(ex);
                return false;
            }
        }

        // ====================================================================

        static string GetTodayFile
            (
                string path
            )
        {
            string directory = Path.GetDirectoryName(path);
            string name = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path);
            string result = string.Format
                (
                    "{0}{1}{2}{3:-yyyy-MM-dd}{4}",
                    directory,
                    Path.DirectorySeparatorChar,
                    name,
                    DateTime.Today,
                    ext
                );
            return result;
        }

    }
}
