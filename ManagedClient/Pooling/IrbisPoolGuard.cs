/* IrbisPoolGuard.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Pooling
{
    /// <summary>
    /// Следит за своевременным возвращением соединения в пул.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisPoolGuard
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Отслеживаемое подключение.
        /// </summary>
        [NotNull]
        public ManagedClient64 Connection { get; private set; }

        /// <summary>
        /// Отслеживаемый пул подключений.
        /// </summary>
        [NotNull]
        public IrbisConnectionPool Pool { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public IrbisPoolGuard
            (
                [NotNull] IrbisConnectionPool pool
            )
        {
            if (pool == null)
            {
                throw new ArgumentNullException("pool");
            }

            Pool = pool;
            Connection = Pool.AcquireConnection();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Неявное преобразование.
        /// </summary>
        public static implicit operator ManagedClient64
            (
                [NotNull] IrbisPoolGuard guard
            )
        {
            return guard.Connection;
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            Pool.ReleaseConnection(Connection);
        }

        #endregion
    }
}
