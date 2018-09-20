// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UnlockRecordsCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Linq;

using AM;
using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis.Direct;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class UnlockRecordsCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnlockRecordsCommand
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
                ServerContext context = engine.RequireAdministratorContext(Data);
                Data.Context = context;
                UpdateContext();

                ClientRequest request = Data.Request.ThrowIfNull();
                string database = request.RequireAnsiString();
                string[] lines = request.RemainingAnsiStrings();
                int[] mfns = lines.Select(line => FastNumber.ParseInt32(line)).ToArray();
                using (DirectAccess64 direct = engine.GetDatabase(database))
                {
                    int maxMfn = direct.GetMaxMfn();
                    foreach (int mfn in mfns)
                    {
                        if (mfn > 0 && mfn <= maxMfn)
                        {
                            direct.Xrf.LockRecord(mfn, false);
                        }
                    }
                }

                ServerResponse response = Data.Response.ThrowIfNull();
                response.WriteInt32(0).NewLine();
                SendResponse();
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }
            catch (Exception exception)
            {
                Log.TraceException("UnlockRecordsCommand::Execute", exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
