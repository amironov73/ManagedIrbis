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
    public sealed class ClientRequest
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ClientRequest()
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
