// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisContextSaver.cs -- сохранение текущего контекста клиента
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using ManagedIrbis;

#endregion

namespace InventoryControl
{
    /// <summary>
    /// Простая сохранялка текущего контекста для клиента.
    /// </summary>
    public class IrbisContextSaver
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Адрес сервера.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Порт сервера.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Тип АРМ.
        /// </summary>
        public IrbisWorkstation Workstation { get; set; }

        /// <summary>
        /// Клиент, для которого сохранён контекст.
        /// </summary>
        public IrbisConnection Connection { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Сохраняет контекст для указанного клиента.
        /// </summary>
        public IrbisContextSaver
            (
                IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
            SaveContext();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Сохраняет контекст.
        /// </summary>
        /// <returns>Клиента.</returns>
        public IrbisConnection SaveContext()
        {
            Host = Connection.Host;
            Port = Connection.Port;
            Username = Connection.Username;
            Password = Connection.Password;
            Database = Connection.Database;
            Workstation = Connection.Workstation;

            return Connection;
        }

        /// <summary>
        /// Восстанавливает контекст.
        /// </summary>
        /// <returns>Клиента.</returns>
        public IrbisConnection RestoreContext()
        {
            Connection.Host = Host;
            Connection.Port = Port;
            Connection.Username = Username;
            Connection.Password = Password;
            Connection.Database = Database;
            Connection.Workstation = Workstation;

            return Connection;
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated
        /// with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            RestoreContext();
        }

        #endregion
    }
}
