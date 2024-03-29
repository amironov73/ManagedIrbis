﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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
        /// Чтение записи с расформатированием.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="mfn">MFN записи.</param>
        /// <param name="lockFlag">Блокировать ли запись.</param>
        /// <param name="format">Непосредственный формат (на языке
        /// форматирования ИРБИС) или имя файла формата без расширения
        /// с предшествующим символом @ - например @brief, 
        /// - в соответствии с которым будет расформатироваться документ.
        /// </param>
        /// <param name="buffer1">Буфер, в котором возвращается
        /// запрашиваемая запись.</param>
        /// <param name="bufferSize1">Размер буфера под запрашиваемую
        /// запись.</param>
        /// <param name="buffer2">Буфер для результата
        /// расформатирования.</param>
        /// <param name="bufferSize2">Размер буфера для расформатирования.
        /// </param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_readformat
            (
                [NotNull] string database,
                int mfn,
                bool lockFlag,
                [NotNull] string format,
                ref IntPtr buffer1,
                int bufferSize1,
                ref IntPtr buffer2,
                int bufferSize2
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
        /// <param name="bufferSize">Размер буфера.</param>
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
        /// Групповая запись/обновление записей в базе данных.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="lockFlag">Блокировать ли записи.</param>
        /// <param name="ifUpdate">Актуализировать ли записи.</param>
        /// <param name="buffer">Буфер с исходными записями;
        /// каждая запись представляется в виде одной строки
        /// – которую необходимо сформировать из клиентского
        /// представления записи путем замены реальных разделителей
        /// полей $0D0A на псевдоразделители $3130.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_updategroup
            (
                [NotNull] string database,
                bool lockFlag,
                bool ifUpdate,
                ref IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Разблокирование записи.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="mfn">MFN записи.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_runlock
            (
                [NotNull] string database,
                int mfn
            );

        /// <summary>
        /// Актуализация записи.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="mfn">MFN записи.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_ifupdate
            (
                [NotNull] string database,
                int mfn
            );

        /// <summary>
        /// Связанная групповая запись/обновление записей в базах данных.
        /// </summary>
        /// <param name="lockFlag">Блокировать ли записи.</param>
        /// <param name="ifUpdate">Актуализировать ли записи.</param>
        /// <param name="databases">Буфер со списком имен исходных баз
        /// данных, соответствующих исходным записям в answer,
        /// в виде набора строк.</param>
        /// <param name="buffer">Буфер с исходными записями;
        /// каждая запись представляется в виде одной строки
        /// – которую необходимо сформировать из клиентского
        /// представления записи путем замены реальных разделителей
        /// полей $0D0A на псевдоразделители $3130.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <remarks>Данная функция отличается от .IC_updategroup
        /// тем, что, во-первых, исходные записи могут быть из
        /// разных баз данных, и во-вторых – редактирование записей
        /// происходит только в том случае, если этот процесс
        /// оказывается успешным для ВСЕХ исходных записей.
        /// Откорректированные записи не изменяются в исходном буфере.
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_updategroup_sinhronize
            (
                bool lockFlag,
                bool ifUpdate,
                [NotNull] string databases,
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
        /// <remarks><para>
        /// buffer по возврату будет содержать набор строк, каждая
        /// из которых имеет структуру:</para>
        /// <para>mfn#результат расформатирования</para>
        /// <para>где:</para>
        /// <para>mfn – MFN соответствующей записи;</para>
        /// <para>результат расформатирования соответствующей записи,
        /// в котором реальные разделители строк $0D0A заменены 
        /// на псевдоразделители $3130.
        /// </para>
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int IC_search
            (
                [NotNull] string database,
                [NotNull] byte[] expression,
                int howMany,
                int first,
                [NotNull] byte[] format,
                IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Последовательный поиск по результату прямого поиска и/или
        /// по заданному диапазону записей.
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
        /// <param name="scanExpression">Поисковое выражение для
        /// последовательного поиска (представляет собой явный формат,
        /// который возвращает одно из двух значений:
        /// 0 – документ не соответствует критерию поиска,
        /// 1 – соответствует). Если задается выражение для прямого
        /// поиска (<paramref name="expression"/>) – последовательный
        /// поиск ведется по его результату (с учетом minMfn и
        /// maxMfn).</param>
        /// <param name="buffer">Буфер для возвращаемых данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <param name="minMfn">Ограничение снизу по MFN, если 0,
        /// то с начала базы данных.</param>
        /// <param name="maxMfn">Ограничение сверху по MFN, если 0,
        /// то до конца базы данных.</param>
        /// <returns>Код возврата – неотрицательное число, равное
        /// количеству найденных записей, или код ошибки.</returns>
        /// <remarks><para>
        /// По возврату <paramref name="buffer"/> будет будет содержать
        /// набор строк, каждая из которых имеет структуру:</para>
        /// <para>mfn#результат расформатирования</para>
        /// <para>где mfn – MFN соответствующей записи;</para>
        /// <para>результат расформатирования соответствующей записи,
        /// в котором реальные разделители строк $0D0A заменены
        /// на псевдоразделители $3130.</para>
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int IC_searchscan
            (
                [NotNull] string database,
                [NotNull] byte[] expression,
                int howMany,
                int first,
                [NotNull] byte[] format,
                int minMfn,
                int maxMfn,
                [NotNull] byte[] scanExpression,
                IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Определить порядковый номер поля в записи.
        /// </summary>
        /// <param name="record">Буфер с исходной записью
        /// в клиентском представлении.</param>
        /// <param name="tag">Метка поля.</param>
        /// <param name="occurrence">Номер повторения (начиная с 1).
        /// </param>
        /// <returns>Код возврата – порядковый номер поля (начиная с 1)
        /// или отрицательное число, если такового поля нет в записи.
        /// </returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int IC_fieldn
            (
                IntPtr record,
                int tag,
                int occurrence
            );

        /// <summary>
        /// Прочитать значение заданного поля/подполя.
        /// </summary>
        /// <param name="record">Буфер с исходной записью
        /// в клиентском представлении.</param>
        /// <param name="fieldNumber">Порядковый номер поля
        /// (полученный с помощью функции IC_fieldn).</param>
        /// <param name="code">Односимвольный разделитель подполя
        /// (если задается $00, то выдается значение поля целиком).</param>
        /// <param name="buffer">Буфер для возвращаемого значения.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_field
            (
                IntPtr record,
                int fieldNumber,
                char code,
                IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Добавления поля в запись.
        /// </summary>
        /// <param name="record">Буфер с исходной записью
        /// в клиентском представлении; в этот же буфер будет
        /// помещена обновленная запись.</param>
        /// <param name="tag">Метка добавляемого поля.</param>
        /// <param name="fieldNumber">Порядковый номер добавляемого
        /// поля; если задать 0 – поле будет добавлено последним
        /// по порядку.</param>
        /// <param name="value">Буфер со значением добавляемого поля.
        /// </param>
        /// <param name="bufferSize">Размер буфера
        /// <paramref name="record"/> для обновленной записи.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_fldadd
            (
                IntPtr record,
                int tag,
                int fieldNumber,
                [NotNull] byte[] value,
                int bufferSize
            );

        /// <summary>
        /// Замена поля в записи.
        /// </summary>
        /// <param name="record">Буфер с исходной записью
        /// в клиентском представлении; в этот же буфер будет
        /// помещена обновленная запись.</param>
        /// <param name="fieldNumber">Порядковый номер заменяемого
        /// поля (полученный с помощью функции IC_fieldn).</param>
        /// <param name="value">Буфер с заменяющим значением поля;
        /// если указать пустое значение – соответствующее поле
        /// удаляется из записи.</param>
        /// <param name="bufferSize">Размер буфера
        /// <paramref name="record"/> для обновленной записи.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_fldrep
            (
                IntPtr record,
                int fieldNumber,
                [CanBeNull] byte[] value,
                int bufferSize
            );


        /// <summary>
        /// Определение количества полей в записи.
        /// </summary>
        /// <param name="record">Буфер с исходной записью
        /// в клиентском представлении.</param>
        /// <returns>Код возврата – количество полей в исходной записи.
        /// </returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int IC_nfields
            (
                IntPtr record
            );

        /// <summary>
        /// Определение количества повторений поля с заданной меткой.
        /// </summary>
        /// <param name="record">Буфер с исходной записью
        /// в клиентском представлении.</param>
        /// <param name="tag">Метка поля.</param>
        /// <returns>Код возврата – количество повторений заданного поля.
        /// </returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int IC_nocc
            (
                IntPtr record,
                int tag
            );

        /// <summary>
        /// Определение метки поля с заданным порядковым номером.
        /// </summary>
        /// <param name="record">Буфер с исходной записью
        /// в клиентском представлении.</param>
        /// <param name="fieldNumber">Порядковый номер поля.</param>
        /// <returns>Код возврата – метка заданного поля или ERR_USER,
        /// если такового нет в записи.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int IC_fldtag
            (
                IntPtr record,
                int fieldNumber
            );

        /// <summary>
        /// Опустошение записи.
        /// </summary>
        /// <param name="record">Буфер с исходной записью в клиентском
        /// представлении; в этот же буфер будет помещена
        /// опустошенная запись.</param>
        /// <returns>Код возврата – ZERO.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_fldempty
            (
                IntPtr record
            );

        /// <summary>
        /// Изменение MFN записи.
        /// </summary>
        /// <param name="record">Буфер с исходной записью в клиентском
        /// представлении; в этот же буфер будет помещена измененная
        /// запись.</param>
        /// <param name="newMfn">Новое значение MFN.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_changemfn
            (
                IntPtr record,
                int newMfn
            );

        /// <summary>
        /// Установка в статусе записи признака логической удаленности.
        /// </summary>
        /// <param name="record">Буфер с исходной записью в клиентском
        /// представлении; в этот же буфер будет помещена измененная
        /// запись.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_recdel
            (
                IntPtr record
            );

        /// <summary>
        /// Снятие в статусе записи признака логической удаленности.
        /// </summary>
        /// <param name="record">Буфер с исходной записью в клиентском
        /// представлении; в этот же буфер будет помещена измененная
        /// запись.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_recundel
            (
                IntPtr record
            );

        /// <summary>
        /// Снятие в статусе записи признака блокировки.
        /// </summary>
        /// <param name="record">Буфер с исходной записью в клиентском
        /// представлении; в этот же буфер будет помещена измененная
        /// запись.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_recunlock
            (
                IntPtr record
            );

        /// <summary>
        /// Чтение MFN записи.
        /// </summary>
        /// <param name="record">Буфер с исходной записью
        /// в клиентском представлении.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int IC_getmfn
            (
                IntPtr record
            );

        /// <summary>
        /// Создание пустой записи.
        /// </summary>
        /// <param name="record">Буфер, в котором будет создаваться
        /// пустая запись в клиентском представлении.</param>
        /// <param name="bufferSize">Размер записи.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_recdummy
            (
                IntPtr record,
                int bufferSize
            );

        /// <summary>
        /// Прочитать в статусе записи признак актуализации.
        /// </summary>
        /// <param name="record">Буфер с исходной записью
        /// в клиентском представлении.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool IC_isActualized
            (
                IntPtr record
            );

        /// <summary>
        /// Прочитать в статусе записи признак блокировки.
        /// </summary>
        /// <param name="record">Буфер с исходной записью
        /// в клиентском представлении.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool IC_isLocked
            (
                IntPtr record
            );

        /// <summary>
        /// Прочитать в статусе записи признак логического удаления.
        /// </summary>
        /// <param name="record">Буфер с исходной записью
        /// в клиентском представлении.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool IC_isDeleted
            (
                IntPtr record
            );

        /// <summary>
        /// Получение списка терминов словаря, начиная с заданного.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="term">Исходный термин.</param>
        /// <param name="number">Количество запрашиваемых терминов;
        /// если задается 0 – будут выдаваться все термины до конца
        /// словаря, но не более, чем MAX_POSTINGS_IN_PACKET.</param>
        /// <param name="buffer">Буфер для списка возвращаемых
        /// терминов.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns><para>Код возврата – может принимать следующие значения:
        /// </para>
        /// <list type="bullet">
        /// <item><term>ZERO</term><description>исходный термин
        /// найден в словаре;</description></item>
        /// <item><term>TERM_NOT_EXISTS</term><description>исходный
        /// термин не найден в словаре, при этом возвращается number
        /// следующих (ближайших) терминов;</description></item>
        /// <item><term>TERM_LAST_IN_LIST</term><description>исходный
        /// термин больше последнего термина в словаре; при этом
        /// возвращается последний термин словаря;</description></item>
        /// <item><term>TERM_FIRST_IN_LIST</term><description>исходный
        /// термин меньше первого термина в словаре; при этом
        /// возвращается number первых терминов словаря;</description></item>
        /// <item><term>ERR_BUFSIZE</term><description>выходной буфер
        /// мал для запрашиваемых терминов; при этом ничего в выходном
        /// буфере не возвращается.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>Количество возвращаемых терминов может быть меньше
        /// number, если достигнут конец словаря.</remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_nexttrm
            (
                [NotNull] string database,
                [NotNull] byte[] term,
                int number,
                IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Получение списка терминов словаря, начиная с заданного,
        /// и расформатирование записи, соответствующие первой ссылке
        /// каждого термина.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="term">Исходный термин.</param>
        /// <param name="number">Количество запрашиваемых терминов;
        /// если задается 0 – будут выдаваться все термины до конца
        /// словаря, но не более, чем MAX_POSTINGS_IN_PACKET.</param>
        /// <param name="format">Формат, который может задаваться
        /// пятью способами:
        /// <list type="bullet">
        /// <item>строка непосредственного формата на языке
        /// форматирования ИРБИС;</item>
        /// <item>имя файла формата, предваряемого символом
        /// @ (например @brief);</item>
        /// <item>символ @ - в этом случае производится
        /// ОПТИМИЗИРОВАННОЕ форматирование (т. е. имя формата
        /// определяется видом записи);</item>
        /// <item>символ * - в этом случае производится форматирование
        /// строго в соответствии со ссылкой (например для ссылки
        /// в виде 1.200.2.3 берется 2-е повторение 200-го поля);</item>
        /// <item>пустая строка.В этом случае возвращается только
        /// список терминов.</item>
        /// </list>
        /// Во всех случаях перед форматированием выполняется
        /// следующая операция - в любом формате специальное
        /// сочетание символов вида*** (3 звездочки) заменяется
        /// на значение метки поля, взятое из 1-й ссылки для данного
        /// термина (например, для ссылки 1.200.1.1 формат
        /// вида v*** будет заменен на v200).
        /// </param>
        /// <param name="buffer">Буфер для списка возвращаемых
        /// терминов.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns><para>Код возврата – может принимать следующие значения:
        /// </para>
        /// <list type="table">
        /// <item><term>ZERO</term><description>исходный термин
        /// найден в словаре;</description></item>
        /// <item><term>TERM_NOT_EXISTS</term><description>исходный
        /// термин не найден в словаре, при этом возвращается number
        /// следующих (ближайших) терминов;</description></item>
        /// <item><term>TERM_LAST_IN_LIST</term><description>исходный
        /// термин больше последнего термина в словаре; при этом
        /// возвращается последний термин словаря;</description></item>
        /// <item><term>TERM_FIRST_IN_LIST</term><description>исходный
        /// термин меньше первого термина в словаре; при этом
        /// возвращается number первых терминов словаря;</description></item>
        /// <item><term>ERR_BUFSIZE</term><description>выходной буфер
        /// мал для запрашиваемых терминов; при этом ничего в выходном
        /// буфере не возвращается.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Буфер представляет собой набор строк, каждая из которых
        /// имеет следующую структуру (в случае не пустого format):
        /// nnn#ссылк>$30термин$30результат расформатирования
        /// где:
        /// nnn – количество ссылок для соответствующего термина;
        /// ссылка - первая ссылка термина в виде mfn#tag#occ#pos#;
        /// результат расформатирования - результат расформатирования,
        /// в котором реальные разделители строк $0D0A заменены
        /// на псевдоразделители $3130.
        /// Если в качестве format задано пустое значение,
        /// строки в буфере имеют структуру:
        /// nnn#термин
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_nexttrmgroup
            (
                [NotNull] string database,
                [NotNull] byte[] term,
                int number,
                [NotNull] byte[] format,
                IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Получение списка терминов словаря, начиная с заданного,
        /// в обратном порядке.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="term">Исходный термин.</param>
        /// <param name="number">Количество запрашиваемых терминов;
        /// если задается 0 – будут выдаваться все термины до конца
        /// словаря, но не более, чем MAX_POSTINGS_IN_PACKET.</param>
        /// <param name="buffer">Буфер для списка возвращаемых
        /// терминов.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns><para>Код возврата – может принимать следующие значения:
        /// </para>
        /// <list type="bullet">
        /// <item><term>ZERO</term><description>исходный термин
        /// найден в словаре;</description></item>
        /// <item><term>TERM_NOT_EXISTS</term><description>исходный
        /// термин не найден в словаре, при этом возвращается number
        /// следующих (ближайших) терминов;</description></item>
        /// <item><term>TERM_LAST_IN_LIST</term><description>исходный
        /// термин больше последнего термина в словаре; при этом
        /// возвращается последний термин словаря;</description></item>
        /// <item><term>TERM_FIRST_IN_LIST</term><description>исходный
        /// термин меньше первого термина в словаре; при этом
        /// возвращается number первых терминов словаря;</description></item>
        /// <item><term>ERR_BUFSIZE</term><description>выходной буфер
        /// мал для запрашиваемых терминов; при этом ничего в выходном
        /// буфере не возвращается.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>Количество возвращаемых терминов может быть меньше
        /// number, если достигнут конец словаря.</remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_prevtrm
            (
                [NotNull] string database,
                [NotNull] byte[] term,
                int number,
                IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Получение списка терминов словаря, начиная с заданного,
        /// в обратном порядке и расформатирование записи,
        /// соответствующие первой ссылке каждого термина.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="term">Исходный термин.</param>
        /// <param name="number">Количество запрашиваемых терминов;
        /// если задается 0 – будут выдаваться все термины до конца
        /// словаря, но не более, чем MAX_POSTINGS_IN_PACKET.</param>
        /// <param name="format">Формат, который может задаваться
        /// пятью способами:
        /// <list type="bullet">
        /// <item>строка непосредственного формата на языке
        /// форматирования ИРБИС;</item>
        /// <item>имя файла формата, предваряемого символом
        /// @ (например @brief);</item>
        /// <item>символ @ - в этом случае производится
        /// ОПТИМИЗИРОВАННОЕ форматирование (т. е. имя формата
        /// определяется видом записи);</item>
        /// <item>символ * - в этом случае производится форматирование
        /// строго в соответствии со ссылкой (например для ссылки
        /// в виде 1.200.2.3 берется 2-е повторение 200-го поля);</item>
        /// <item>пустая строка.В этом случае возвращается только
        /// список терминов.</item>
        /// </list>
        /// Во всех случаях перед форматированием выполняется
        /// следующая операция - в любом формате специальное
        /// сочетание символов вида*** (3 звездочки) заменяется
        /// на значение метки поля, взятое из 1-й ссылки для данного
        /// термина (например, для ссылки 1.200.1.1 формат
        /// вида v*** будет заменен на v200).
        /// </param>
        /// <param name="buffer">Буфер для списка возвращаемых
        /// терминов.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns><para>Код возврата – может принимать следующие значения:
        /// </para>
        /// <list type="table">
        /// <item><term>ZERO</term><description>исходный термин
        /// найден в словаре;</description></item>
        /// <item><term>TERM_NOT_EXISTS</term><description>исходный
        /// термин не найден в словаре, при этом возвращается number
        /// следующих (ближайших) терминов;</description></item>
        /// <item><term>TERM_LAST_IN_LIST</term><description>исходный
        /// термин больше последнего термина в словаре; при этом
        /// возвращается последний термин словаря;</description></item>
        /// <item><term>TERM_FIRST_IN_LIST</term><description>исходный
        /// термин меньше первого термина в словаре; при этом
        /// возвращается number первых терминов словаря;</description></item>
        /// <item><term>ERR_BUFSIZE</term><description>выходной буфер
        /// мал для запрашиваемых терминов; при этом ничего в выходном
        /// буфере не возвращается.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Буфер представляет собой набор строк, каждая из которых
        /// имеет следующую структуру (в случае не пустого format):
        /// nnn#ссылк>$30термин$30результат расформатирования
        /// где:
        /// nnn – количество ссылок для соответствующего термина;
        /// ссылка - первая ссылка термина в виде mfn#tag#occ#pos#;
        /// результат расформатирования - результат расформатирования,
        /// в котором реальные разделители строк $0D0A заменены
        /// на псевдоразделители $3130.
        /// Если в качестве format задано пустое значение,
        /// строки в буфере имеют структуру:
        /// nnn#термин
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_prevtrmgroup
            (
                [NotNull] string database,
                [NotNull] byte[] term,
                int number,
                [NotNull] byte[] format,
                IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Получение списка ссылок для заданного термина.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="term">Исходный термин.</param>
        /// <param name="number">Количество запрашиваемых ссылок;
        /// если задается 0 – будут выдаваться все  ссылки термина,
        /// но не более, чем MAX_POSTINGS_IN_PACKET.</param>
        /// <param name="first">Порядковый номер первой из запрашиваемых
        /// ссылок; если задается 0 – то функция возвращает только
        /// общее количество ссылок заданного термина.</param>
        /// <param name="buffer">Буфер для возвращаемых данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns>Код возврата – ZERO, если термин найден;
        /// общее число ссылок, если Afirst=0; или код ошибки.</returns>
        /// <remarks><para>При коде возврата ZERO буфер содержит набор строк,
        /// каждая из которых является ссылкой вида:</para>
        /// <para>mfn#tag#occ#pos#</para>
        /// <para>Для выделения отдельных составляющих ссылки следует
        /// пользоваться вспомогательной функцией IC_getposting.
        /// </para>
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int IC_posting
            (
                [NotNull] string database,
                [NotNull] byte[] term,
                int number,
                int first,
                IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Получение списка первых ссылок для списка заданных терминов.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="terms">Буфер со списком исходных терминов в виде набора строк.</param>
        /// <param name="buffer">Буфер для возвращаемых данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns>Код возврата определяется тем, найден ли первый
        /// термин из заданного списка: ZERO, если термин найден;
        /// код ошибки в противном случае.
        /// </returns>
        /// <remarks>
        /// <para>Буфер будет содержать набор строк (по количеству
        /// исходных терминов), каждая из которых имеет структуру:
        /// </para>
        /// <para>
        /// nnn#ссылка
        /// </para>
        /// <para>
        /// где nnn – общее количество ссылок для соответствующего термина,
        /// ссылка - первая ссылка термина.
        /// </para>
        /// <para>
        /// Если соответствующий термин не найден в словаре,
        /// nnn=0, а ссылка - пустая строка.
        /// </para>
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_postinggroup
            (
                [NotNull] string database,
                [NotNull] byte[] terms,
                [NotNull] byte[] buffer,
                int bufferSize
            );

        /// <summary>
        /// Получение списка ссылок для заданного термина
        /// и расформатирование записей им соответствующих.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="term">Исходный термин.</param>
        /// <param name="number">Количество запрашиваемых ссылок;
        /// если задается 0 – будут выдаваться все  ссылки термина,
        /// но не более, чем MAX_POSTINGS_IN_PACKET.</param>
        /// <param name="first">Порядковый номер первой из запрашиваемых
        /// ссылок; если задается 0 – то функция возвращает только
        /// общее количество ссылок заданного термина.</param>
        /// <param name="format">Формат, который может задаваться
        /// пятью способами:
        /// <list type="bullet">
        /// <item>строка непосредственного формата на языке
        /// форматирования ИРБИС;</item>
        /// <item>имя файла формата, предваряемого символом
        /// @ (например @brief);</item>
        /// <item>символ @ - в этом случае производится
        /// ОПТИМИЗИРОВАННОЕ форматирование (т. е. имя формата
        /// определяется видом записи);</item>
        /// <item>символ * - в этом случае производится форматирование
        /// строго в соответствии со ссылкой (например для ссылки
        /// в виде 1.200.2.3 берется 2-е повторение 200-го поля);</item>
        /// <item>пустая строка.В этом случае возвращается только
        /// список терминов.</item>
        /// </list>
        /// Во всех случаях перед форматированием выполняется
        /// следующая операция - в любом формате специальное
        /// сочетание символов вида*** (3 звездочки) заменяется
        /// на значение метки поля, взятое из 1-й ссылки для данного
        /// термина (например, для ссылки 1.200.1.1 формат
        /// вида v*** будет заменен на v200).
        /// </param>
        /// <param name="buffer1">Буфер для возвращаемых результатов
        /// расформатирования.</param>
        /// <param name="bufferSize1">Размер <paramref name="buffer1"/>.
        /// </param>
        /// <param name="buffer2">Буфер для возвращаемого списка ссылок.
        /// </param>
        /// <param name="bufferSize2">Размер <paramref name="buffer2" />.
        /// </param>
        /// <returns>Код возврата определяется тем, найден ли
        /// исходный термин: ZERO, если найден;
        /// код ошибки в противном случае.</returns>
        /// <remarks>
        /// <para> buffer1 будет содержать набор строк, каждая из которых
        /// имеет следующую структуру:</para>
        /// <para>mfn#результат расформатирования</para>
        /// <para>где:</para>
        /// <para>mfn – MFN соответствующей записи;</para>
        /// <para>результат расформатирования соответствующей записи,
        /// в котором реальные разделители строк $0D0A заменены 
        /// на псевдоразделители $3130;</para>
        /// <para>buffer2 будет содержать набор строк (соответствующих
        /// строкам в buffer1), каждая из которых содержит ссылку в виде:
        /// </para>
        /// <para>mfn#tag#occ#pos#</para>
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_postingformat
            (
                [NotNull] string database,
                [NotNull] byte[] term,
                int number,
                int first,
                [NotNull] byte[] format,
                IntPtr buffer1,
                int bufferSize1,
                IntPtr buffer2,
                int bufferSize2
            );

        /// <summary>
        /// Расформатирование записи, заданной по номеру (MFN).
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="mfn">MFN исходной записи.</param>
        /// <param name="format">Формат для расформатирования исходной
        /// записи, может быть задан следующими способами:
        /// – строка непосредственного формата на языке форматирвания ИРБИС;
        /// – имя файла формата, предваряемого символом @ (например @brief);
        /// – символ @ - в этом случае производится ОПТИМИЗИРОВАННОЕ
        /// форматирование (т. е. имя формата определяется видом записи);
        /// </param>
        /// <param name="buffer">Буфер для возвращаемых данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns>Код возврата – код завершения форматирования
        /// или код ошибки.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_sformat
            (
                [NotNull] string database,
                int mfn,
                [NotNull] byte[] format,
                IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Расформатирование записи в клиентском представлении.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="format">Формат для расформатирования исходной
        /// записи, может быть задан следующими способами:
        /// – строка непосредственного формата на языке форматирвания ИРБИС;
        /// – имя файла формата, предваряемого символом @ (например @brief);
        /// – символ @ - в этом случае производится ОПТИМИЗИРОВАННОЕ
        /// форматирование (т. е. имя формата определяется видом записи);
        /// </param>
        /// <param name="record">Буфер, содержащий исходную запись
        /// в клиентском представлении (см. описание функции IC_read).
        /// </param>
        /// <param name="buffer">Буфер для возвращаемых данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns>Код возврата – код завершения форматирования
        /// или код ошибки.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_record_sformat
            (
                [NotNull] string database,
                [NotNull] byte[] format,
                [NotNull] byte[] record,
                IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Расформатирование группы записей.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="mfnList">Буфер, содержащий сведения
        /// об исходных записях; может быть двух видов:
        /// 1) набор из трех строк, следующего вида:
        /// 0
        /// minMfn
        /// maxMfn
        ///
        /// где minMfn  и maxMfn определяют диапазон MFN документов,
        /// подлежащих расформатированию;
        /// 2) набор строк следующего вида:
        /// N
        /// mfn1
        /// mfn2
        /// ...
        /// mfnN
        /// где N – количество документов, подлежащих расформатированию.
        /// </param>
        /// <param name="format">Формат для расформатирования исходной
        /// записи, может быть задан следующими способами:
        /// – строка непосредственного формата на языке форматирвания ИРБИС;
        /// – имя файла формата, предваряемого символом @ (например @brief);
        /// – символ @ - в этом случае производится ОПТИМИЗИРОВАННОЕ
        /// форматирование (т. е. имя формата определяется видом записи);
        /// </param>
        /// <param name="buffer">Буфер для возвращаемых данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns>Код возврата – код завершения форматирования
        /// или код ошибки.</returns>
        /// <remarks>
        /// По возврату <paramref name="buffer"/> будет содержать
        /// набор строк, каждая из которых имеет структуру:
        /// mfn#результат расформатирования
        /// где:
        /// mfn – MFN соответствующей записи;
        /// - результат расформатирования соответствующей записи,
        /// в котором реальные разделители строк $0D0A заменены
        /// на псевдоразделители $3130.
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_sformatgroup
            (
                [NotNull] string database,
                [NotNull] string mfnList,
                [NotNull] byte[] format,
                IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Формирование выходной табличной формы.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="table">Имя табличной формы
        /// с предшествующим символом @ (например: @tabflw).</param>
        /// <param name="head">Заголовки над таблицей (до 3 строк).
        /// Реальные разделители строк $0D0A заменены
        /// на псевдоразделители $3130.</param>
        /// <param name="model">Значение модельного поля,
        /// которое передается (только на период расформатирования)
        /// в каждую результирующую запись.</param>
        /// <param name="search">Поисковое выражение для прямого
        /// поиска на языке ИРБИС.</param>
        /// <param name="minMfn">Минимальный MFN или 0.</param>
        /// <param name="maxMfn">Максимальный MFN или 0.</param>
        /// <param name="sequential">Выражение для последовательного
        /// поиска.</param>
        /// <param name="mfnList">Список MFN записей,
        /// организованный одним из трех следующих способов:
        /// 1) диапазон номеров – в виде трех строк следующей структуры:
        /// 0
        /// minmfn
        /// maxmfn
        /// 2) список номеров – в виде набора строк:
        /// N
        /// mfn1
        /// mfn2
        /// ...
        /// mfnN
        /// 3) отрицательный список номеров(«кроме указанных»)
        /// – в виде набора строк:
        /// -N
        /// mfn1
        /// mfn2
        /// ...
        /// mfnN
        /// </param>
        /// <param name="buffer">Буфер для возвращаемых данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns>Код возврата – ZERO или код ошибки.</returns>
        /// <remarks>
        /// Список результирующих документов формируется как результат
        /// пересечения трех списков:
        /// - списка записей, найденных в результате прямого поиска
        /// (search);
        /// - списка записей, полученных в результате последовательного
        /// поиска (minMfn, maxMfn, sequential);
        /// - списка записей, указанных с помощью mfnList.
        /// </remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_print
            (
                [NotNull] string database,
                [NotNull] string table,
                [NotNull] byte[] head,
                [NotNull] byte[] model,
                [NotNull] byte[] search,
                int minMfn,
                int maxMfn,
                [NotNull] byte[] sequential,
                [NotNull] string mfnList,
                IntPtr buffer,
                int bufferSize
            );

        /// <summary>
        /// Формирование выходной формы в виде статистических распределений.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="stat">Список заданий на статистическую обработку,
        /// в виде набора строк, в котором реальные разделители
        /// строк $0D0A заменены на псевдоразделители $3130.
        /// Каждое задание представляет собой строку вида:
        /// FMT,N1,N2,N3
        /// где:
        /// FMT – элемент данных, задаваемый в виде поле^подполе
        /// (например 200^a) или как непосредственный формат
        /// на языке форматирования ИРБИС;
        /// N1 – анализируемая длина элемента данных;
        /// N2 – максимальное количество значений элемента данных;
        /// N3 – вид сортировки, который может принимать три значения:
        /// 0 – без сортировки;
        /// 1 – сортировка по убыванию;
        /// 2 – сортировка по возрастанию.
        /// </param>
        /// <param name="minMfn">Минимальный MFN или 0.</param>
        /// <param name="maxMfn">Максимальный MFN или 0.</param>
        /// <param name="sequential">Выражение для последовательного
        /// поиска.</param>
        /// <param name="mfnList">Список MFN записей,
        /// организованный одним из трех следующих способов:
        /// 1) диапазон номеров – в виде трех строк следующей структуры:
        /// 0
        /// minmfn
        /// maxmfn
        /// 2) список номеров – в виде набора строк:
        /// N
        /// mfn1
        /// mfn2
        /// ...
        /// mfnN
        /// 3) отрицательный список номеров(«кроме указанных»)
        /// – в виде набора строк:
        /// -N
        /// mfn1
        /// mfn2
        /// ...
        /// mfnN
        /// </param>
        /// <param name="buffer">Буфер для возвращаемых данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns>Код возврата – ZERO или код ошибки.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_stat
            (
                [NotNull] string database,
                [NotNull] byte[] stat,
                int minMfn,
                int maxMfn,
                [NotNull] byte[] sequential,
                [NotNull] string mfnList,
                IntPtr buffer,
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
