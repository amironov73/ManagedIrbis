/* FakeClientSocket.cs -- socket for off-site debugging.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WINMOBILE && !PocketPC && !WIN81


#region Using directives

using System.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    /// Fake client socket for off-site debugging.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FakeClientSocket
        : AbstractClientSocket
    {
        #region Properties

        /// <summary>
        /// Path to store queries and responses.
        /// </summary>
        [NotNull]
        public string StoragePath{ get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FakeClientSocket
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string storagePath
            )
            : base(connection)
        {
            Code.NotNullNorEmpty(storagePath, "storagePath");

            StoragePath = storagePath;
        }

        #endregion

        #region Private members

        private int _counter;

        #endregion

        #region Public methods

        #endregion

        #region AbstractClientSocket members

        /// <summary>
        /// Abort the request.
        /// </summary>
        public override void AbortRequest()
        {
            // TODO implement?
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

            _counter++;
            string path = Path.Combine
                (
                    StoragePath,
                    string.Format
                    (
                        "{0:00000000}dn.packet",
                        _counter
                    )
                );
            byte[] result = File.ReadAllBytes
                (
                    path
                );

            return result;
        }

        #endregion
    }
}

#endif

