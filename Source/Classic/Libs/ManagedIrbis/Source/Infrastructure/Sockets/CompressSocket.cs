// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CompressSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    /// Сжимает содержимое пакета для экономии трафика.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CompressSocket
        : AbstractClientSocket
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CompressSocket
            (
                [NotNull] IrbisConnection connection,
                [NotNull] AbstractClientSocket innerSocket
            )
            : base(connection)
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(innerSocket, "innerSocket");

            InnerSocket = innerSocket;
        }

        #endregion

        #region Public methods

        #endregion

        #region AbstractClientSocket members

        /// <inheritdoc cref="AbstractClientSocket.AbortRequest"/>
        public override void AbortRequest()
        {
            InnerSocket.ThrowIfNull().AbortRequest();
        }

        /// <inheritdoc cref="AbstractClientSocket.ExecuteRequest"/>
        public override byte[] ExecuteRequest
            (
                byte[][] request
            )
        {
            Code.NotNull(request, "request");

            byte[][] compressed = new byte[1][];
            byte[] merged = ArrayUtility.Merge(request);
            compressed[0] = CompressionUtility.Compress(merged);
            byte[] result = InnerSocket.ThrowIfNull().ExecuteRequest(compressed);
            result = CompressionUtility.Decompress(result);

            return result;
        }

        #endregion
    }
}
