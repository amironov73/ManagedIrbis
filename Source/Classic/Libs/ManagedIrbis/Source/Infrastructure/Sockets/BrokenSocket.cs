// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BrokenSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public sealed class BrokenSocket
        : AbstractClientSocket
    {
        #region Constants

        /// <summary>
        /// Default value for
        /// </summary>
        public const double DefaultProbability = 0.07;

        #endregion

        #region Properties

        /// <summary>
        /// Probability of error event.
        /// </summary>
        public double Probability { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BrokenSocket
            (
                [NotNull] IrbisConnection connection,
                [NotNull] AbstractClientSocket innerSocket
            )
            : base(connection)
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(innerSocket, "innerSocket");

            Probability = DefaultProbability;
            InnerSocket = innerSocket;

            _random = new Random();
        }

        #endregion

        #region Private members

        private readonly Random _random;

        #endregion

        #region Public methods

        #endregion

        #region AbstractClientSocket members

        /// <inheritdoc cref="AbstractClientSocket.AbortRequest"/>
        public override void AbortRequest()
        {
            InnerSocket.AbortRequest();
        }

        /// <inheritdoc cref="AbstractClientSocket.ExecuteRequest"/>
        public override byte[] ExecuteRequest
            (
                byte[] request
            )
        {
            Code.NotNull(request, "request");

            double probability = Probability;
            if (probability > 0.0
                && probability < 1.0)
            {
                double value = _random.NextDouble();
                if (value < probability)
                {
                    throw new IrbisNetworkException
                        (
                            "Broken network event"
                        );
                }
            }

            byte[] result = InnerSocket.ExecuteRequest(request);

            return result;
        }

        #endregion
    }
}
