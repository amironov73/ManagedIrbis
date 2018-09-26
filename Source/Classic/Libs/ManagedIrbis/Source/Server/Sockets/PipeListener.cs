// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PipeListener.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW46 || NETCORE || ANDROID

#region Using directives

using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Простой слушатель для System.IO.Pipes.
    /// Выдает подключение клиента <see cref="PipeSocket"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PipeListener
        : IrbisServerListener
    {
        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        [NotNull]
        public string Name { get; private set; }

        /// <summary>
        /// Instance count.
        /// </summary>
        public int InstanceCount { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Construction.
        /// </summary>
        public PipeListener
            (
                [NotNull] string name,
                int instanceCount,
                CancellationToken token
            )
        {
            Code.NotNullNorEmpty(name, "name");
            Code.Positive(instanceCount, "instanceCount");

            Name = name;
            InstanceCount = instanceCount;
            _token = token;
        }

        #endregion

        #region Private members

        private CancellationToken _token;

        private NamedPipeServerStream _stream;

        #endregion

        #region IrbisServerListener members

        /// <inheritdoc cref="IrbisServerListener.AcceptClientAsync" />
        public override Task<IrbisServerSocket> AcceptClientAsync()
        {
            TaskCompletionSource<IrbisServerSocket> result
                = new TaskCompletionSource<IrbisServerSocket>();

            Task task = _stream.WaitForConnectionAsync(_token);
            task.ContinueWith
                (
                    s1 =>
                    {
                        PipeSocket socket = new PipeSocket(_stream, _token);
                        result.SetResult(socket);
                    },
                    _token
                );

            return result.Task;
        }

        /// <inheritdoc cref="IrbisServerListener.GetLocalAddress" />
        public override string GetLocalAddress()
        {
            return Name;
        }

        /// <inheritdoc cref="IrbisServerListener.Start" />
        public override void Start()
        {
            if (ReferenceEquals(_stream, null))
            {
                _stream = new NamedPipeServerStream
                    (
                        Name,
                        PipeDirection.InOut,
                        InstanceCount,
                        PipeTransmissionMode.Message,
                        PipeOptions.Asynchronous
                    );
            }
        }

        /// <inheritdoc cref="IrbisServerListener.Stop" />
        public override void Stop()
        {
            if (!ReferenceEquals(_stream, null))
            {
                _stream.Dispose();
                _stream = null;
            }
        }

        /// <inheritdoc cref="IrbisServerListener.Dispose" />
        public override void Dispose()
        {
            if (!ReferenceEquals(_stream, null))
            {
                _stream.Dispose();
                _stream = null;
            }
        }

        #endregion
    }
}

#endif
