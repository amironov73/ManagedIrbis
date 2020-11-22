// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* NetworkChecker.cs -- проверка возможности связи с удаленным хостом.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.Net.NetworkInformation;
using System.Text;

using ManagedIrbis;

using RestfulIrbis.OsmiCards;

#endregion

namespace DicardsConfig
{
    public static class NetworkChecker
    {
        #region Public method

        /// <summary>
        /// Пингуем указанный хост.
        /// </summary>
        /// <param name="hostname">Имя или адрес хоста.</param>
        /// <param name="timeout">Таймаут, миллисекунды.</param>
        /// <returns>Результат проверки.</returns>
        public static IPStatus PingHost
            (
                string hostname,
                int timeout = 3000
            )
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            string data = "Alexey Mironov";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            PingReply reply = pingSender.Send
                (
                    hostname,
                    timeout,
                    buffer,
                    options
                );

            return reply?.Status ?? IPStatus.Unknown;
        }

        /// <summary>
        /// Проверяем связь с DiCards.
        /// </summary>
        /// <param name="configuration">Конфигурация,
        /// из которой берётся адрес хоста.</param>
        /// <returns>Результат проверки.</returns>
        public static IPStatus PingDicards
            (
                DicardsConfiguration configuration
            )
        {
            string hostname = new Uri(configuration.BaseUri).Host;

            return PingHost(hostname);
        }

        /// <summary>
        /// Проверяем связь с сервером ИРБИС64.
        /// </summary>
        /// <param name="configuration">Конфигурация,
        /// из которой берётся адрес сервера.</param>
        /// <returns>Результат проверки.</returns>
        public static IPStatus PingIrbis
            (
                DicardsConfiguration configuration
            )
        {
            IrbisConnection connection = new IrbisConnection();
            connection.ParseConnectionString(configuration.ConnectionString);
            string hostname = connection.Host;

            return PingHost(hostname);
        }

        #endregion
    }
}
