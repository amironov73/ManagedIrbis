// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FastEngine.cs -- fast and dirty execution engine
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using AM;
using AM.Logging;
using AM.Threading;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure.Commands;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Fast and dirty execution engine.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class FastEngine
        : StandardEngine
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FastEngine
            (
                [NotNull] IIrbisConnection connection,
                [CanBeNull] AbstractEngine nestedEngine
            )
            : base(connection, nestedEngine)
        {
            Log.Trace("FastEngine::Constructor");
        }

        #endregion

        #region AbstractEngine members

        /// <inheritdoc cref="AbstractEngine.ExecuteCommand" />
        public override ServerResponse ExecuteCommand
            (
                ExecutionContext context
            )
        {
            AbstractCommand command = context.Command
                .ThrowIfNull("Command");
            IIrbisConnection connection = context.Connection
                .ThrowIfNull("Connection");

            using (new BusyGuard(connection.Busy))
            {
                ClientQuery query = command.CreateQuery();
                ServerResponse result = command.Execute(query);
                context.Response = result;
                command.CheckResponse(result);
                return result;
            }
        }

        /// <inheritdoc cref="AbstractEngine.ObtainResponse"/>
        protected internal override ServerResponse ObtainResponse
            (
                ClientQuery query,
                bool relaxResponse
            )
        {
            List<byte[]> list = new List<byte[]>
            {
                new byte[] {49, 10}
            };

            MemoryStream stream = GetMemoryStream(GetType());
            stream
                .EncodeString(query.CommandCode)      .EncodeDelimiter()
                .EncodeWorkstation(query.Workstation) .EncodeDelimiter()
                .EncodeString(query.CommandCode)      .EncodeDelimiter()
                .EncodeInt32(query.ClientID)          .EncodeDelimiter()
                .EncodeInt32(query.CommandNumber)     .EncodeDelimiter()
                .EncodeString(query.UserPassword)     .EncodeDelimiter()
                .EncodeString(query.UserLogin)        .EncodeDelimiter()

                // Three empty lines
                .EncodeDelimiter()
                .EncodeDelimiter()
                .EncodeDelimiter();

            list.Add(stream.ToArray());
            stream.Dispose();

            if (query.Arguments.Count != 0)
            {
                int countMinus1 = query.Arguments.Count - 1;
                for (int i = 0; i < countMinus1; i++)
                {
                    stream = GetMemoryStream(GetType());
                    stream.EncodeAny(query.Arguments[i]);
                    stream.EncodeDelimiter();
                    list.Add(stream.ToArray());
                    stream.Dispose();
                }
                for (int i = countMinus1; i < query.Arguments.Count; i++)
                {
                    stream = GetMemoryStream(GetType());
                    stream.EncodeAny(query.Arguments[i]);
                    // DO NOT add delimiter to the last line!
                    list.Add(stream.ToArray());
                    stream.Dispose();
                }
            }

            byte[][] request = list.ToArray();
            byte[] answer = Connection.Socket
                .ExecuteRequest(request);

            ServerResponse result = new ServerResponse
                (
                    Connection,
                    answer,
                    request,
                    relaxResponse
                );

            return result;
        }

        #endregion
    }
}
