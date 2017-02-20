// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TransactionInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Proxy2017
{
    /// <summary>
    /// Вся информация о выполненной команде.
    /// </summary>
    class TransactionInfo
    {
        /// <summary>
        /// Порядковый номер.
        /// </summary>
        public long Index { get; set; }

        /// <summary>
        /// Стадия, до которой дошло выполнение команды.
        /// </summary>
        public int Stage { get; set; }

        /// <summary>
        /// Момент поступления команды.
        /// </summary>
        public DateTime Moment { get; set; }

        /// <summary>
        /// Адрес, с которого пришла команда
        /// </summary>
        public EndPoint EndPoint { get; set; }

        /// <summary>
        /// Запрос клиента в необработанном виде
        /// (включая строки заголовка)
        /// </summary>
        public byte[] Request { get; set; }

        /// <summary>
        /// Ответ сервера в необработанном виде
        /// (включая строки заголовка)
        /// </summary>
        public byte[] Response { get; set; }

        /// <summary>
        /// Общая продолжительность обработки запроса
        /// со всеми пересылками туда-сюда
        /// (в миллисекундах).
        /// </summary>
        public long RoundtripDuration { get; set; }

        /// <summary>
        /// Продолжительность обработки запроса сервером
        /// ИРБИС (в миллисекундах).
        /// </summary>
        public long IrbisDuration { get; set; }
    }
}
