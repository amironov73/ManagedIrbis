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

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace OfficialWrapper
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class NativeMethods64
    {
        #region Constants

        private const string DllName = "irbis64_client.dll";

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
                string host,
                string port,
                char workstation,
                string username,
                string password,
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
                string username
            );

        /// <summary>
        /// Установка режима работы через Web-шлюз.
        /// </summary>
        /// <param name="enable">Включить режим веб-шлюза.</param>
        /// <returns></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_set_webserver
            (
                bool enable
            );

        /// <summary>
        /// Установка имени шлюза при работе через Web-шлюз.
        /// </summary>
        /// <param name="name">Имя шлюза.</param>
        /// <returns></returns>
        /// <remarks>По умолчанию /cgi-bin/wwwirbis.exe</remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_set_webcgi
            (
                string name
            );

        /// <summary>
        /// Установка режима ожидания ответа от сервера.
        /// </summary>
        /// <param name="enable">Включить режим "мертвого" ожидания ответа
        /// от сервера.</param>
        /// <returns></returns>
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
        /// <returns></returns>
        /// <remarks>По умолчанию - 1 минута.</remarks>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_set_client_time_live
            (
                int time
            );

        /// <summary>
        /// Определение завершения очередного обращения к серверу.
        /// </summary>
        /// <returns>true - обращение к серверу не завершено.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool IC_isbusy
            (
            );

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_update_ini
            (
                string inifile
            );

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_getresourse
            (
                IrbisPath path,
                string database,
                string filename,
                ref IntPtr buffer,
                int bufferSize
            );

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_clearresourse
            (
            );

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_getresourcegroup
            (
                byte[] context,
                byte[] buffer,
                int bufferSize
            );

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_getbinaryresourse
            (
                IrbisPath path,
                string database,
                string filename,
                ref IrbisBuffer buffer
            );

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_putresourse
            (
                IrbisPath path,
                string database,
                string filename,
                string resource
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
                string database,
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
        /// <returns></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_update
            (
                string database,
                bool lockFlag,
                bool ifUpdate,
                ref IntPtr buffer,
                int bufferSize
            );

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_nooperation
            (
            );

        /// <summary>
        /// Расформатирование записи, заданной по номеру.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="mfn">MFN исходной записи.</param>
        /// <param name="format">Формат.</param>
        /// <param name="buffer">Буфер для возвращаемых данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_sformat
            (
                string database,
                int mfn,
                byte[] format,
                byte[] buffer,
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
        /// <returns></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_record_sformat
            (
                string database,
                byte[] format,
                byte[] record,
                byte [] buffer,
                int bufferSize
            );



        /// <summary>
        /// Получает максимальный MFN базы данных.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <returns></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_maxmfn
            (
                string database
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
                byte [] buffer,
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
        /// <param name="format">формат для расформатирования найденных записей, может быть задан следующими способами:
        /// – строка непосредственного формата на языке форматирвания ИРБИС;
        /// – имя файла формата, предваряемого символом @ (например @brief);
        /// – символ @ - в этом случае производится ОПТИМИЗИРОВАННОЕ форматирование (т.е. имя формата определяется видом записи);
        /// – пустая строка. В этом случае расформатирование записей не производится
        /// </param>
        /// <param name="buffer">Буфер для возвращаемых данных.</param>
        /// <param name="bufferSize">Размер буфера.</param>
        /// <returns>Стандартный код завершения: неотрицательное число, 
        /// равное количеству найденных записей, или код ошибки.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IrbisReturnCode IC_search
            (
                string database,
                byte [] expression,
                int howMany,
                int first,
                byte [] format,
                byte [] buffer,
                int bufferSize
            );

        #endregion

        #region Utility routines

        [DllImport("Kernel32")]
        public static extern IntPtr GlobalSize(IntPtr hMem);

        public static IntPtr AllocateMemory ( int size )
        {
            return Marshal.AllocHGlobal(size);
        }

        public static void FreeMemory ( IntPtr pointer )
        {
            Marshal.FreeHGlobal(pointer);
        }

        public static Encoding GetUtfEncoding ()
        {
            return new UTF8Encoding(false,true);
        }

        public static byte[] ToUtf ( string text )
        {
            Encoding utf = GetUtfEncoding();
            int len = utf.GetByteCount(text);
            byte [] result = new byte[len + 5];
            utf.GetBytes(text, 0, text.Length, result, 0);
            //DumpBuffer(result,0,result.Length);
            return result;
        }

        public static byte[] ToUtfLen(string text)
        {
            Encoding utf = GetUtfEncoding();
            int len = utf.GetByteCount(text);
            byte[] result = new byte[len + 5];
            Array.Copy
                (
                    BitConverter.GetBytes(len), 
                    0, 
                    result, 
                    0, 
                    4
                );
            utf.GetBytes
                (
                    text.ToCharArray(), 
                    0, 
                    text.Length, 
                    result, 
                    4
                );
            return result;
        }

        public static string FromUtf(byte[] bytes)
        {
            return GetUtfEncoding().GetString(bytes);
        }

        public static string FromUtfZ(byte[] bytes)
        {
            int index = Array.IndexOf<byte>(bytes, 0);
            if (index < 0)
            {
                index = bytes.Length;
            }
            return GetUtfEncoding().GetString
                (
                    bytes,
                    0,
                    index
                );
        }

        public static string TrimAtZero ( string text )
        {
            int index = text.IndexOf((char) 0);
            return (index >= 0)
                       ? text.Substring(0, index)
                       : text;
        }

        public static void DumpBuffer 
            ( 
                byte[] buffer, 
                int start, 
                int length 
            )
        {
            length = Math.Max(0, Math.Min(buffer.Length - start, length));
            for (int i = 0; i < length; i++)
            {
                byte b = buffer[start + i];
                Console.Write("{0} ", b.ToString("X2"));
            }
        }

        #endregion
    }
}
