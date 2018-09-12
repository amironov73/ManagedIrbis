// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ClientRequest.cs --
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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Client request.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ClientRequest
    {
        #region Properties

        /// <summary>
        /// Общая длина запроса в байтах.
        /// </summary>
        public int RequestLength { get; set; }

        /// <summary>
        /// Код команды (первая копия).
        /// </summary>
        public string CommandCode1 { get; set; }

        /// <summary>
        /// Код АРМ.
        /// </summary>
        public string Workstation { get; set; }

        /// <summary>
        /// Код команды (вторая копия).
        /// </summary>
        public string CommandCode2 { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Номер команды.
        /// </summary>
        public string CommandNumber { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        public string Login { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor for mocking.
        /// </summary>
        public ClientRequest()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ClientRequest
            (
                [NotNull] TcpClient connection
            )
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        ///
        /// </summary>
        [CanBeNull]
        public string GetAnsiString()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        [NotNull]
        public string RequireAnsiString()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        [CanBeNull]
        public string GetUtfString()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        [NotNull]
        public string RequireUtfString()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
