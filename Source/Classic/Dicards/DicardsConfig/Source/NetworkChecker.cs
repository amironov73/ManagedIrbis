// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* NetworkChecker.cs -- проверка возможности связи с удаленным хостом.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.Net.NetworkInformation;
using System.Text;

using JetBrains.Annotations;

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
                [NotNull] string hostname,
                int timeout = 3000
            )
        {
            var pingSender = new Ping();
            var options = new PingOptions
            {
                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                DontFragment = true
            };

            var data = "Alexey Mironov";
            var buffer = Encoding.ASCII.GetBytes(data);
            var reply = pingSender.Send
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
                [NotNull] DicardsConfiguration configuration
            )
        {
            var baseUri = configuration.BaseUri;
            if (string.IsNullOrEmpty(baseUri))
            {
                return IPStatus.BadOption;
            }

            var hostname = new Uri(baseUri).Host;

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
            var connectionString = configuration.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                return IPStatus.BadOption;
            }

            var connection = new IrbisConnection();
            connection.ParseConnectionString(connectionString);
            string hostname = connection.Host;

            return PingHost(hostname);
        }

        #endregion
    }
}
