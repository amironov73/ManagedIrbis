// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TestingSocket.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TestingSocket
        : AbstractClientSocket
    {
        #region Properties

        /// <summary>
        /// Actual request.
        /// </summary>
        [CanBeNull]
        public byte[] ActualRequest { get; set; }

        /// <summary>
        /// Answer.
        /// </summary>
        [CanBeNull]
        public byte[] Response { get; set; }

        /// <summary>
        /// Expected request.
        /// </summary>
        [CanBeNull]
        public byte[] ExpectedRequest { get; set; }

        /// <inheritdoc cref="AbstractClientSocket.RequireConnection" />
        public override bool RequireConnection
        {
            get { return false; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TestingSocket
            (
                [NotNull] IIrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region AbstractClientSocket members

        /// <inheritdoc cref="AbstractClientSocket.AbortRequest" />
        public override void AbortRequest()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="AbstractClientSocket.ExecuteRequest" />
        public override byte[] ExecuteRequest
            (
                byte[] request
            )
        {
            Code.NotNull(request, "request");

            ActualRequest = request;

            if (!ReferenceEquals(ExpectedRequest, null))
            {
                if (!ArrayUtility.Coincide(ExpectedRequest, 0, request, 0, request.Length))
                {
                    throw new Exception();
                }
            }

            byte[] answer = Response;
            if (ReferenceEquals(answer, null))
            {
                throw new Exception();
            }

            return answer;
        }

        #endregion
    }
}
