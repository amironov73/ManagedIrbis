// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NativeMethods64.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Runtime.InteropServices;
using System.Text;

using AM.ConsoleIO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace OfficialWrapper
{
    //
    // Библиотека irbis64_client.dll предназначена
    // для полнофункционального доступа к базам данных ИРБИС64
    // на основе клиент-серверной архитектуры
    //(т.е.путем взаимодействия с TCP/IP сервером ИРБИС64).
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class NativeMethods64
    {
        #region Constants

        /// <summary>
        /// DLL file name.
        /// </summary>
        public const string DllName = "irbis64_client.dll";

        #endregion

        #region Interop routines

        /// <summary>
        /// Регистрация клиента на сервере.
        /// </summary>
        /// <param name="host">Адрес сервера в числовом виде.</param>
        /// <param name="port">Рабочий порт сервера (6666).</param>
        /// <param name="workstation">Тип клиента.</param>
        /// <param name="username">Имя пользователя,
        /// зарегистрированного на сервере.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="buffer">Выходной буфер для возвращаемых
        /// данных.</param>
        /// <param name="bufferSize">Размер выходного буфера.</param>
        /// <returns>В случае успешной регистрации в выходном буфере
        /// возвращается профиль пользователя в ANSI кодировке.</returns>
        /// <remarks>Данная функция обязательно должна выполняться
        /// первой в клиентском приложении.</remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_reg
            (
                [NotNull] string host,
                [NotNull] string port,
                char workstation,
                [NotNull] string username,
                [NotNull] string password,
                ref IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Разрегистрация клиента на сервере.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <returns></returns>
        /// <remarks>Сервер производит автоматическую разрегистрацию
        /// клиентов, не подающих запросов в течение определенного
        /// времени.</remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_unreg
            (
                [NotNull] string username
            );

        /// <summary>
        /// Установка режима работы через Web-шлюз.
        /// </summary>
        /// <param name="enable">Включить режим веб-шлюза.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_set_webserver
            (
                bool enable
            );

        /// <summary>
        /// Установка имени шлюза при работе через Web-шлюз.
        /// </summary>
        /// <param name="name">Имя шлюза.</param>
        /// <remarks>По умолчанию /cgi-bin/wwwirbis.exe</remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_set_webcgi
            (
                [NotNull] string name
            );

        /// <summary>
        /// Установка режима ожидания ответа от сервера.
        /// </summary>
        /// <param name="enable">Включить режим "мертвого" ожидания ответа
        /// от сервера.</param>
        /// <remarks>По умолчанию отключен.</remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_set_blocksocket
            (
                bool enable
            );

        /// <summary>
        /// Установка времени появления заставки ожидания.
        /// </summary>
        /// <param name="seconds">Интервал времени в секундах.</param>
        /// <returns></returns>
        /// <remarks>По умолчанию - 3 секунды.</remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_set_show_waiting
            (
                int seconds
            );

        /// <summary>
        /// Установка интервала подтверждения.
        /// </summary>
        /// <param name="time">Интервал времени в минутах.</param>
        /// <remarks>По умолчанию - 1 минута.</remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_set_client_time_live
            (
                int time
            );

        /// <summary>
        /// Определение завершения очередного обращения к серверу.
        /// </summary>
        /// <returns><c>true</c> - обращение к серверу не завершено.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool IC_isbusy();

        /// <summary>
        /// Обновление INI-файла – профиля пользователя на сервере.
        /// </summary>
        /// <param name="inifile">Набор строк в виде структуры
        /// INI-файла (в ANSI-кодировке).</param>
        /// <remarks>
        /// В результате успешного выполнения функции обновляется
        /// (пополняется) INI-файл – профиль пользователя на сервере,
        /// в соответствии с которым выполнялась его регистрация
        /// (см. функцию IC_reg).
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_update_ini
            (
                [NotNull] string inifile
            );

        /// <summary>
        /// Чтение текстового ресурса (файла).
        /// </summary>
        /// <param name="path">Код, определяющий относительный путь
        /// размещения ресурса ИРБИС64.</param>
        /// <param name="database">Имя базы данных (не используется,
        /// если в качестве Apath указывается SYSPATH или DATAPATH).
        /// </param>
        /// <param name="filename">Имя требуемого текстового файла
        /// (ресурса) с расширением.</param>
        /// <param name="buffer">Буфер, в котором возвращается
        /// требуемый ресурс.</param>
        /// <param name="bufferSize">Размер буфера для возвращаемых
        /// данных.</param>
        /// <remarks>При успешном завершении функции в answer 
        /// возвращается требуемый текстовый ресурс.
        /// Данные возвращаются в ANSI-кодировке.</remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_getresourse
            (
                IrbisPath path,
                [NotNull] string database,
                [NotNull] string filename,
                ref IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Очистка памяти кэша.
        /// </summary>
        /// <remarks>В результате выполнения функции очищается кэш,
        /// в котором сохраняются запрошенные текстовые ресурсы
        /// (после чего при их запросе они берутся с сервера).
        /// При выполнении функции не производится обращение
        /// на сервер.</remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_clearresourse();

        /// <summary>
        /// Групповое чтение текстовых ресурсов.
        /// </summary>
        /// <param name="context">Набор строк, каждая из которых
        /// соответствует одному запрашиваемому ресурсу и имеет вид:
        /// Apath+’.’+Adbn+’.’+Afilename
        /// Например: DBNPATH10.IBIS.BRIEF.PFT
        /// </param>
        /// <param name="buffer">Буфер для возвращаемых данных</param>
        /// <param name="bufferSize"></param>
        /// <remarks>
        /// После успешного выполнения функции в acontext сохраняются
        /// строки, соответствующие НЕ ПУСТЫМ ресурсам
        /// (число и порядок строк в acontext может измениться).
        /// В answer возвращается набор строк, каждая из которых
        /// соответствует строке в acontext и содержит соответствующий
        /// текстовый ресурс – в котором реальные разделители
        /// строк $0D0A заменены на псевдоразделители $3130.
        /// (Для подобных преобразований предлагаются специальные
        /// вспомогательные функции IC_reset_delim и IC_delim_reset).
        /// Данные возвращаются в ANSI-кодировке.
        /// Содержимое запрашиваемых ресурсов кэшируется
        /// в памяти IRBIS64_CLIENT.DLL.
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_getresourcegroup
            (
                [NotNull] byte[] context,
                [NotNull] byte[] buffer,
                int bufferSize
            );

        /// <summary>
        /// Чтение двоичного ресурса.
        /// </summary>
        /// <param name="path">Код, определяющий относительный путь
        /// размещения ресурса ИРБИС64.</param>
        /// <param name="database">Имя базы данных (не используется,
        /// если в качестве Apath указывается SYSPATH или DATAPATH).
        /// </param>
        /// <param name="filename">Имя требуемого текстового файла
        /// (ресурса) с расширением.</param>
        /// <param name="buffer">Указатель на специальный буфер
        /// для двоичного ресурса, тип которого описан
        /// в irbis64_client.pas. Память для этого буфера
        /// выделяется и освобождается в irbis64_client.dll.</param>
        /// <remarks>После успешного выполнения функции сведения
        /// о двоичном ресурсе возвращаются в Abuffer
        /// (собственно двоичный ресурс в Abuffer^.data).
        /// Содержимое запрашиваемого ресурса кэшируется в памяти.
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Ansi)]
        public static extern IrbisReturnCode IC_getbinaryresourse
            (
                IrbisPath path,
                [NotNull] string database,
                [NotNull] string filename,
                ref IrbisBuffer buffer
            );

        /// <summary>
        /// Запись текстового ресурса на сервер.
        /// </summary>
        /// <param name="path">Код, определяющий относительный путь
        /// размещения ресурса ИРБИС64.</param>
        /// <param name="database">Имя базы данных (не используется,
        /// если в качестве Apath указывается SYSPATH или DATAPATH).
        /// </param>
        /// <param name="filename">Имя требуемого текстового файла
        /// (ресурса) с расширением.</param>
        /// <param name="resource">Буфер, содержащий исходный
        /// текстовый ресурс (в ANSI-кодировке).</param>
        /// <remarks>Содержимое исходного ресурса кэшируется в памяти;
        /// если таковой уже запрашивался (находился в кэше)
        /// – он обновляется.</remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Ansi)]
        public static extern IrbisReturnCode IC_putresourse
            (
                IrbisPath path,
                [NotNull] string database,
                [NotNull] string filename,
                [NotNull] string resource
            );

        /// <summary>
        /// Чтение записи без преобразований.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="mfn">MFN записи.</param>
        /// <param name="lockFlag">Блокировать ли запись.</param>
        /// <param name="buffer">Буфер для возвращаемых даных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns>0 при успешном выполнении, иначе код ошибки.</returns>
        /// <remarks><para>Формат возвращаемой записи:<br/>
        /// 0#код возврата<br/>
        /// MFN#статус записи<br/>
        /// 0#номер версии записи<br/>
        /// далее следуют строки вида:<br/>
        /// TAG#значение поля<br/>
        /// где TAG – числовая метка поля.
        /// </para>
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_read
            (
                [NotNull] string database,
                int mfn,
                bool lockFlag,
                ref IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Запись/обновление записи в базе данных.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="lockFlag">Блокировать ли запись.</param>
        /// <param name="ifUpdate">Актуализировать ли запись.</param>
        /// <param name="buffer">Буфер, содержащий исходную запись 
        /// в клиентском представлении; в нем же будет возвращаться 
        /// обновленная запись при успешном выполнении.</param>
        /// <param name="bufferSize">Размер записи.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_update
            (
                [NotNull] string database,
                bool lockFlag,
                bool ifUpdate,
                ref IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Подтверждение регистрации.
        /// </summary>
        /// <remarks>Функцию необходимо выполнять периодически
        /// с учетом того, что сервер автоматически разрегистрирует
        /// клиента, если от него не поступает никаких запросов
        /// в течение времени, которое определяется параметром
        /// CLIENT_TIME_LIVE в INI-файле сервера (irbis_server.ini).
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_nooperation();

        /// <summary>
        /// Расформатирование записи, заданной по номеру.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="mfn">MFN исходной записи.</param>
        /// <param name="format">Формат.</param>
        /// <param name="buffer">Буфер для возвращаемых данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_sformat
            (
                [NotNull] string database,
                int mfn,
                [NotNull] byte[] format,
                [NotNull] byte[] buffer,
                int bufferSize
            );


        /// <summary>
        /// Расформатирование записи в клиентском представлении.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="format">Формат.</param>
        /// <param name="record">Исходная запись в клиентском
        /// представлении.</param>
        /// <param name="buffer">Буфер для возвращаемых данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_record_sformat
            (
                [NotNull] string database,
                [NotNull] byte[] format,
                [NotNull] byte[] record,
                [NotNull] byte[] buffer,
                int bufferSize
            );

        /// <summary>
        /// Получает максимальный MFN базы данных.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_maxmfn
            (
                [NotNull]string database
            );

        /// <summary>
        /// Создание пустой записи.
        /// </summary>
        /// <param name="buffer">Буфер, в котором будет создаваться 
        /// пустая запись в клиентском представлении.</param>
        /// <param name="bufferSize">Размер буфера/</param>
        /// <returns>Код завершения.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_recdummy
            (
                [NotNull] byte[] buffer,
                int bufferSize
            );

        /// <summary>
        /// Прямой (по словарю) поиск записей по заданному 
        /// поисковому выражению.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="expression">Поисковое выражение.</param>
        /// <param name="howMany">Количество возвращаемых записей 
        /// (из числа найденных); если задается 0 – возвращаются 
        /// все найденные документы, но не более 
        /// MAX_POSTINGS_IN_PACKET.</param>
        /// <param name="first">Номер первой возвращаемой записи 
        /// из общего числа найденных; если задается 0 – возвращается 
        /// только количество найденных записей.</param>
        /// <param name="format">формат для расформатирования
        /// найденных записей, может быть задан следующими способами:
        /// – строка непосредственного формата на языке форматирвания ИРБИС;
        /// – имя файла формата, предваряемого символом @ (например @brief);
        /// – символ @ - в этом случае производится ОПТИМИЗИРОВАННОЕ
        /// форматирование (т.е. имя формата определяется видом записи);
        /// – пустая строка. В этом случае расформатирование записей
        /// не производится
        /// </param>
        /// <param name="buffer">Буфер для возвращаемых данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns>Стандартный код завершения: неотрицательное число, 
        /// равное количеству найденных записей, или код ошибки.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_search
            (
                [NotNull] string database,
                [NotNull] byte[] expression,
                int howMany,
                int first,
                [NotNull] byte[] format,
                [NotNull] byte[] buffer,
                int bufferSize
            );

        #endregion

        #region Utility routines

        /// <summary>
        /// Retrieves the current size of the specified global
        /// memory object, in bytes.
        /// </summary>
        /// <param name="hMem">A handle to the global memory object.
        /// This handle is returned by either the GlobalAlloc
        /// or GlobalReAlloc function.</param>
        /// <returns>If the function succeeds, the return value
        /// is the size of the specified global memory object, in bytes.
        /// If the specified handle is not valid or if the object
        /// has been discarded, the return value is zero.
        /// To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("Kernel32")]
        public static extern IntPtr GlobalSize(IntPtr hMem);

        #endregion
    }
}
