/* ClientInfo.cs -- информация о клиенте, подключенном к серверу ИРБИС
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Информация о клиенте, подключенном к серверу ИРБИС
    /// (не обязательно о текущем).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ClientInfo
    {
        #region Properties

        /// <summary>
        /// Номер
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Адрес клиента
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Порт клиента
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор клиентской программы
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Клиентский АРМ
        /// </summary>
        public string Workstation { get; set; }

        /// <summary>
        /// Время подключения к серверу
        /// </summary>
        public string Registered { get; set; }

        /// <summary>
        /// Последнее подтверждение
        /// </summary>
        public string Acknowledged { get; set; }

        /// <summary>
        /// Последняя команда
        /// </summary>
        public string LastCommand { get; set; }

        /// <summary>
        /// Номер последней команды
        /// </summary>
        public string CommandNumber { get; set; }

        #endregion

        #region Public methods
        

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
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
