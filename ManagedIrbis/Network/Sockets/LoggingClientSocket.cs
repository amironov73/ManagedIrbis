/* LoggingClientSocket.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network.Sockets
{
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class LoggingClientSocket
        : AbstractClientSocket
    {
        #region Properties

        public string DebugPath
        {
            get { return _debugPath; }
        }

        public AbstractClientSocket InnerSocket
        {
            get { return _innerSocket; }
        }

        #endregion

        #region Construction

        public LoggingClientSocket
            (
                [NotNull] IrbisConnection connection,
                [NotNull] AbstractClientSocket innerSocket,
                [NotNull] string debugPath
            ) : base(connection)
        {
            Code.NotNull(innerSocket, "innerSocket");
            Code.NotNullNorEmpty(debugPath, "debugPath");

            if (!Directory.Exists(debugPath))
            {
                throw new IrbisNetworkException
                    (
                        "directory not exist: " + debugPath
                    );
            }

            _debugPath = debugPath;
            _innerSocket = innerSocket;
        }

        #endregion

        #region Private members

        private int _counter;

        private readonly string _debugPath;

        private readonly AbstractClientSocket _innerSocket;

        private void _DumpGeneralInfo
            (
                string suffix,
                string text
            )
        {
            int counter = Interlocked.Increment(ref _counter);

            string path = Path.Combine
                (
                    _debugPath,
                    string.Format
                    (
                        "{0:00000000}{1}.packet",
                        counter,
                        suffix
                    )
                );
            File.WriteAllText
                (
                    path,
                    text
                );
        }

        private void _DumpException
            (
                Exception exception
            )
        {
            _DumpGeneralInfo
                (
                    "ex",
                    exception.ToString()
                );
        }

        private void _DumpPackets
            (
                byte[] request,
                byte[] answer
            )
        {
            int counter = Interlocked.Increment(ref _counter);

            string upPath = Path.Combine
                (
                    _debugPath,
                    string.Format
                    (
                        "{0:00000000}up.packet",
                        counter
                    )
                );
            File.WriteAllBytes(upPath, request);

            string downPath = Path.Combine
                (
                    _debugPath,
                    string.Format
                    (
                        "{0:00000000}dn.packet",
                        counter
                    )
                );
            File.WriteAllBytes(downPath, answer);
        }

        #endregion

        #region Public methods

        #endregion

        #region AbstractClientSocket members

        /// <summary>
        /// Abort the request.
        /// </summary>
        public override void AbortRequest()
        {
            InnerSocket.AbortRequest();
        }

        /// <summary>
        /// Send request to server and receive answer.
        /// </summary>
        public override byte[] ExecuteRequest
            (
                byte[] request
            )
        {
            Code.NotNull(request, "request");

            byte[] result;
            try
            {
                result = _innerSocket.ExecuteRequest(request);
            }
            catch (Exception exception)
            {
                Task.Factory.StartNew
                    (
                        () => _DumpException(exception)
                    );
                throw;
            }

            Task.Factory.StartNew
                (
                    () => _DumpPackets(request, result)
                );

            return result;
        }

        #endregion
    }
}
