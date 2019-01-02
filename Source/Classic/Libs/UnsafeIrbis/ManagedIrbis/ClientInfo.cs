// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ClientInfo.cs -- информация о клиенте, подключенном к серверу ИРБИС
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;

using JetBrains.Annotations;

#endregion

namespace UnsafeIrbis
{
    /// <summary>
    /// Информация о клиенте, подключенном к серверу ИРБИС
    /// (не обязательно о текущем).
    /// </summary>
    [PublicAPI]
    [DebuggerDisplay("{IPAddress} {Name} {Workstation}")]
    public sealed class ClientInfo
    {
        #region Properties

        /// <summary>
        /// Номер
        /// </summary>
        [CanBeNull]
        public string Number { get; set; }

        /// <summary>
        /// Адрес клиента
        /// </summary>
        [CanBeNull]
        // ReSharper disable once InconsistentNaming
        public string IPAddress { get; set; }

        /// <summary>
        /// Порт клиента
        /// </summary>
        [CanBeNull]
        public string Port { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор клиентской программы
        /// </summary>
        [CanBeNull]
        // ReSharper disable once InconsistentNaming
        public string ID { get; set; }

        /// <summary>
        /// Клиентский АРМ
        /// </summary>
        [CanBeNull]
        public string Workstation { get; set; }

        /// <summary>
        /// Время подключения к серверу
        /// </summary>
        [CanBeNull]
        public string Registered { get; set; }

        /// <summary>
        /// Последнее подтверждение
        /// </summary>
        [CanBeNull]
        public string Acknowledged { get; set; }

        /// <summary>
        /// Последняя команда
        /// </summary>
        [CanBeNull]
        public string LastCommand { get; set; }

        /// <summary>
        /// Номер последней команды
        /// </summary>
        [CanBeNull]
        public string CommandNumber { get; set; }

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString ()
        {
            return string.Format
                (
                    "Number: {0}, IPAddress: {1}, Port: {2}, "
                  + "Name: {3}, ID: {4}, Workstation: {5}, "
                  + "Registered: {6}, Acknowledged: {7}, "
                  + "LastCommand: {8}, CommandNumber: {9}",
                    Number,
                    IPAddress,
                    Port,
                    Name,
                    ID,
                    Workstation,
                    Registered,
                    Acknowledged,
                    LastCommand,
                    CommandNumber
                );
        }

        #endregion
    }
}
