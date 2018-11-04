// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerWorker.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
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
            data.Started = DateTime.Now;
            data.Task = new Task(DoWork);
        }

        #endregion

        #region Private members

        private void _WritePacket
            (
                [NotNull] string prefix,
                [NotNull] byte[][] chunks
            )
        {
            // TODO async

            IrbisServerEngine engine = Data.Engine;
            ClientRequest request = Data.Request;
            string fileName = string.Format
                (
                    "{0}_{1}_{2}.packet",
                    prefix,
                    request.ClientId,
                    request.CommandNumber
                );
            fileName = Path.Combine(engine.WorkDir, fileName);
            using (FileStream stream = File.Create(fileName))
            {
                foreach (byte[] chunk in chunks)
                {
                    stream.Write(chunk, 0, chunk.Length);
                }
            }
        }

        private void _LogRequest()
        {
            ClientRequest request = Data.Request;
            MemoryStream memory = request.Memory;
            long savedPosition = memory.Position;
            memory.Position = 0;
            byte[][] packet = new byte[1][];
            packet[0] = memory.ToArray();
            memory.Position = savedPosition;
            _WritePacket("rqst", packet);
        }

        private void _LogResponse()
        {
            ServerResponse response = Data.Response;
            MemoryStream memory = response.Memory;
            long savedPosition = memory.Position;
            memory.Position = 0;
            byte[][] packet = response.Encode(null);
            memory.Position = savedPosition; //-V3008
            _WritePacket("rsps", packet);
        }

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

                _LogRequest();

                Log.Trace("ServerWorker::DoWork: request: address="
                          + Data.Socket.GetRemoteAddress()
                          + ", command=" + request.CommandCode1
                          + ", login=" + request.Login
                          + ", workstation=" + request.Workstation);

                Data.Response = new ServerResponse(Data.Request);
                Data.Command = Data.Engine.Mapper.MapCommand(Data);
                Data.Command.Execute();

                _LogResponse();

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
