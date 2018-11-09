// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PerfSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    /// Намеренно замедленный сокет для оптимизации работы в медленных сетях.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PerfSocket
        : AbstractClientSocket
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PerfSocket
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

        #region Public members

        /// <summary>
        /// Save the <see cref="PerfRecord"/>.
        /// </summary>
        public virtual void SavePerfRecord
            (
                [NotNull] PerfRecord record
            )
        {
            Code.NotNull(record, "record");

            // TODO implement
        }

        #endregion

        #region AbstractClientSocket members

        /// <inheritdoc cref="AbstractClientSocket.AbortRequest" />
        public override void AbortRequest()
        {
            InnerSocket.ThrowIfNull().AbortRequest();
        }

        /// <inheritdoc cref="AbstractClientSocket.ExecuteRequest" />
        public override byte[] ExecuteRequest
            (
                byte[][] request
            )
        {
            Code.NotNull(request, "request");

            byte[] result = EmptyArray<byte>.Value;
            Exception catchedException = null;
            string errorMessage = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                result = InnerSocket.ThrowIfNull().ExecuteRequest(request);
            }
            catch (Exception exception)
            {
                catchedException = exception;
                errorMessage = exception.Message;
            }

            stopwatch.Stop();

            PerfRecord record = new PerfRecord
            {
                Moment = DateTime.Now, // TODO mock
                Code = "?", // TODO obtain
                OutgoingSize = request.Sum(array => array.Length),
                IncomingSize = result.Length,
                ElapsedTime = stopwatch.ElapsedMilliseconds,
                ErrorMessage = errorMessage
            };
            SavePerfRecord(record);

            if (!ReferenceEquals(catchedException, null))
            {
                throw catchedException;
            }

            return result;
        }

        #endregion
    }
}