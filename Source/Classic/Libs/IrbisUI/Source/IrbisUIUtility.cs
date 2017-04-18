// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisUIUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure.Sockets;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// 
    /// </summary>
    public static class IrbisUIUtility
    {
        #region Public methods

        /// <summary>
        /// Setup retry form for the connection.
        /// </summary>
        [NotNull]
        public static IrbisConnection SetupRetryForm
            (
                [NotNull] this IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            RetryManager manager = new RetryManager
                (
                    int.MaxValue,
                    RetryForm.GetResolver()
                );
            RetryClientSocket socket = new RetryClientSocket
                (
                    connection,
                    connection.Socket,
                    manager
                );
            connection.SetSocket(socket);

            return connection;
        }

        #endregion
    }
}
