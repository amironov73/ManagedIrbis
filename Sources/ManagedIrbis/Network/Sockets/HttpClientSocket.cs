/* HttpClientSocket.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Threading;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network.Sockets
{
    public sealed class HttpClientSocket
        : AbstractClientSocket
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public HttpClientSocket
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region AbstractClientSocket members

        public override void AbortRequest()
        {
            throw new System.NotImplementedException();
        }

        public override byte[] ExecuteRequest(byte[] request)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
