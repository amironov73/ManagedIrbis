// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DatabaseInfoCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

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
    public class DatabaseInfoCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DatabaseInfoCommand
            (
                [NotNull] WorkData data
            )
            : base(data)
        {
        }

        #endregion

        #region Private members

        private static void _WriteRecords
            (
                [NotNull] ServerResponse response,
                [CanBeNull] int[] mfns
            )
        {
            if (!ReferenceEquals(mfns, null) && mfns.Length != 0)
            {
                string line = StringUtility.Join(",", mfns);
                response.WriteAnsiString(line);
            }

            response.NewLine();
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

                DatabaseInfo info;
                using (DirectAccess64 direct = engine.GetDatabase(database))
                {
                    info = direct.GetDatabaseInfo();
                }

                ServerResponse response = Data.Response.ThrowIfNull();
                response.WriteInt32(0).NewLine();
                _WriteRecords(response, info.LogicallyDeletedRecords);
                _WriteRecords(response, info.PhysicallyDeletedRecords);
                _WriteRecords(response, info.NonActualizedRecords);
                _WriteRecords(response, info.LockedRecords);
                response.WriteInt32(info.MaxMfn).NewLine();
                response.WriteInt32(info.DatabaseLocked ? 1 : 0).NewLine();
                SendResponse();
            }
            catch (IrbisException exception)
            {
                SendError(exception.ErrorCode);
            }
            catch (Exception exception)
            {
                Log.TraceException("DatabaseInfoCommand::Execute", exception);
                SendError(-8888);
            }

            engine.OnAfterExecute(Data);
        }

        #endregion
    }
}
