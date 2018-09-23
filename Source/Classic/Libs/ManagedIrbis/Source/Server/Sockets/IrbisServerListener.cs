// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisServerListener.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Абстрактный слушатель, его задача -- выдать сокет
    /// с подключенным клиентом.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class IrbisServerListener
        : IDisposable
    {
        #region Public methods

        /// <summary>
        /// Accept the client.
        /// </summary>
        [NotNull]
        public abstract Task<IrbisServerSocket> AcceptClientAsync();

        /// <summary>
        /// Get local address.
        /// </summary>
        [NotNull]
        public abstract string GetLocalAddress();

        /// <summary>
        /// Start to listen.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Stop to listen.
        /// </summary>
        public abstract void Stop();

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public abstract void Dispose();

        #endregion
    }
}
