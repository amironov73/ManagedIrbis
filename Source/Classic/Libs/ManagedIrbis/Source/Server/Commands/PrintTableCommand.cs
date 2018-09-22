// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PrintTableCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;
using AM.Logging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PrintTableCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PrintTableCommand
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
                ServerContext context = engine.RequireContext(Data);
                Data.Context = context;
                UpdateContext();

                ClientRequest request = Data.Request.ThrowIfNull();
                string database = request.RequireAnsiString();
                string table = request.RequireAnsiString();
                string headers = request.GetUtfString();
                string mode = request.GetAnsiString();
                string searchQuery = request.GetUtfString();
                int minMfn = request.GetInt32();
                int maxMfn = request.GetInt32();
                string sequentialQuery = request.GetUtfString();
                int[] mfnList = new int[0]; // TODO get mfnList

                // TODO implement

                ServerResponse response = Data.Response.ThrowIfNull();
                response.WriteAnsiString(string.Empty).NewLine();
                SendResponse();
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }
            catch (Exception exception)
            {
                Log.TraceException("PrintTableCommand::Execute", exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
