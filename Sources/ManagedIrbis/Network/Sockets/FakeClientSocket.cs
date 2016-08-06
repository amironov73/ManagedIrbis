/* FakeClientSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network.Sockets
{
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FakeClientSocket
        : AbstractClientSocket
    {
        #region Properties

        [NotNull]
        public string StoragePath
        {
            get { return _storagePath; }
        }

        #endregion

        #region Construction

        public FakeClientSocket
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string storagePath
            )
            : base(connection)
        {
            Code.NotNullNorEmpty(storagePath, "storagePath");

            _storagePath = storagePath;
        }

        #endregion

        #region Private members

        private readonly string _storagePath;

        private int _counter;

        #endregion

        #region Public methods

        #endregion

        #region AbstractClientSocket members

        public override void AbortRequest()
        {
            // TODO implement?
        }

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
