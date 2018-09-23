// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerWorker.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Threading.Tasks;

using AM.Logging;

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
    public class ServerWorker
    {
        #region Properties

        /// <summary>
        /// All the data.
        /// </summary>
        [NotNull]
        public WorkData Data { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ServerWorker
            (
                [NotNull] WorkData data
            )
        {
            Code.NotNull(data, "data");

            Data = data;
            data.Task = new Task(DoWork);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Do the work.
        /// </summary>
        public void DoWork()
        {
            try
            {
                ClientRequest request = new ClientRequest(Data);
                Data.Request = request;

                Log.Trace("ServerWorker::DoWork: request: address="
                          + Data.Socket.GetRemoteAddress()
                          + ", command=" + request.CommandCode1
                          + ", login=" + request.Login
                          + ", workstation=" + request.Workstation);

                Data.Response = new ServerResponse(Data.Request);
                Data.Command = Data.Engine.Mapper.MapCommand(Data);
                Data.Command.Execute();

                Log.Trace("ServerWorker::DoWork: success: address="
                          + Data.Socket.GetRemoteAddress()
                          + ", command=" + request.CommandCode1
                          + ", login=" + request.Login
                          + ", workstation=" + request.Workstation);
            }
            catch (Exception exception)
            {
                Log.TraceException("ServerWorker::DoWork", exception);
            }
            finally
            {
                Data.Socket.Dispose();
                lock (Data.Engine.SyncRoot)
                {
                    Data.Engine.Workers.Remove(this);
                }
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
