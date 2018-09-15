// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConnectCommand.cs --
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
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ConnectCommand
        : ServerCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConnectCommand
            (
                [NotNull] WorkData data
            )
            : base(data)
        {
        }

        #endregion

        #region ServerCommand members

        /// <inheritdoc cref="ServerCommand.Execute" />
        public override void Execute()
        {
            IrbisServerEngine engine = Data.Engine.ThrowIfNull();
            engine.OnBeforeExecute(Data);

            try
            {
                ClientRequest request = Data.Request.ThrowIfNull();
                string clientId = request.ClientId.ThrowIfNull();
                ServerContext context = engine.FindContext(clientId);
                if (!ReferenceEquals(context, null))
                {
                    // Клиент с таким идентификатором уже зарегистрирован
                    throw new IrbisException(-3337);
                }

                string username = request.RequireAnsiString();
                string password = request.RequireAnsiString();

                context = Data.Engine.CreateContext(clientId);
                Data.Context = context;
                context.Address = Data.Socket.Client.Client.RemoteEndPoint.ToString();
                context.Username = username;
                context.Password = password;

                SendResponse();
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
