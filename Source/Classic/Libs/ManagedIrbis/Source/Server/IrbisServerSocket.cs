// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisServerSocket.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class IrbisServerSocket
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Client.
        /// </summary>
        [NotNull]
        public TcpClient Client { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Construction.
        /// </summary>
        public IrbisServerSocket
            (
                [NotNull] TcpClient client
            )
        {
            Code.NotNull(client, "client");

            Client = client;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IDisposable

        /// <inheritdoc/>
        public void Dispose()
        {
#if NETCORE

            Client.Dispose();

#else

            Client.Close();

#endif
        }

        #endregion

        #region Object members

        #endregion
    }
}
