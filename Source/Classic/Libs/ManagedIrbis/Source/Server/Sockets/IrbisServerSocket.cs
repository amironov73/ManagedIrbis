// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisServerSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Абстрактный сокет, обслуживающий подключение клиента.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class IrbisServerSocket
        : IDisposable
    {

        #region Public methods

        /// <summary>
        /// Get remote address.
        /// </summary>
        [NotNull]
        public abstract string GetRemoteAddress();

        /// <summary>
        /// Receive all the data.
        /// </summary>
        [NotNull]
        public abstract MemoryStream ReceiveAll();

        /// <summary>
        /// Send the data.
        /// </summary>
        public abstract void Send
            (
                [NotNull] byte[][] data
            );

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public abstract void Dispose();

        #endregion
    }
}
