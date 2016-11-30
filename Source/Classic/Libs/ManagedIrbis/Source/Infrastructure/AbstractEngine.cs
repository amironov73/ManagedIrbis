// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AbstractEngine.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Abstract execution engine.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class AbstractEngine
    {
        #region Events

        /// <summary>
        /// Raised after execution.
        /// </summary>
        public event EventHandler<ExecutionEventArgs> AfterExecution;

        /// <summary>
        /// Raised before execution.
        /// </summary>
        public event EventHandler<ExecutionEventArgs> BeforeExecution;

        /// <summary>
        /// Raised on exception.
        /// </summary>
        public event EventHandler<ExecutionEventArgs> ExceptionOccurs;

        #endregion

        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Nested engine.
        /// </summary>
        [CanBeNull]
        public AbstractEngine NestedEngine { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbstractEngine
            (
                [NotNull] IrbisConnection connection,
                [CanBeNull] AbstractEngine nestedEngine
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
            NestedEngine = nestedEngine;
        }

        #endregion

        #region Private members

        /// <summary>
        /// Check whether connection established.
        /// </summary>
        protected virtual void CheckConnection
            (
                [NotNull] ExecutionContext context
            )
        {
            AbstractCommand command = context.Command
                .ThrowIfNull("Command");
            IrbisConnection connection = context.Connection
                .ThrowIfNull("Connection");

            if (command.RequireConnection)
            {
                if (!connection.Connected)
                {
                    throw new IrbisException("Not connected");
                }
            }
        }

        /// <summary>
        /// After command execution.
        /// </summary>
        protected void OnAfterExecute
            (
                [NotNull] ExecutionContext context
            )
        {
            EventHandler<ExecutionEventArgs> handler = AfterExecution;

            if (!ReferenceEquals(handler, null))
            {
                ExecutionEventArgs args = new ExecutionEventArgs(context);

                handler(this, args);
            }
        }

        /// <summary>
        /// Before command execution.
        /// </summary>
        protected void OnBeforeExecute
            (
                [NotNull] ExecutionContext context
            )
        {
            EventHandler<ExecutionEventArgs> handler = BeforeExecution;

            if (!ReferenceEquals(handler, null))
            {
                ExecutionEventArgs args = new ExecutionEventArgs(context);

                handler(this, args);
            }
        }

        /// <summary>
        /// Exception occurs.
        /// </summary>
        protected void OnException
            (
                [NotNull] ExecutionContext context
            )
        {
            EventHandler<ExecutionEventArgs> handler = ExceptionOccurs;

            if (!ReferenceEquals(handler, null))
            {
                ExecutionEventArgs args = new ExecutionEventArgs(context);

                handler(this, args);
            }
        }

        /// <summary>
        /// Standard command execution.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected ServerResponse StandardExecution
            (
                [NotNull] ExecutionContext context
            )
        {
            AbstractCommand command = context.Command
                .ThrowIfNull("Command");
            IrbisConnection connection = context.Connection
                .ThrowIfNull("Connection");

            command.Verify(true);

            using (new BusyGuard(connection.Busy))
            {
                ServerResponse result = ServerResponse
                    .GetEmptyResponse
                    (
                        connection
                    );

                connection.Interrupted = false;

                try
                {

                    ClientQuery query = command.CreateQuery();
                    query.Verify(true);

                    result = command.Execute(query);
                    result.Verify(true);
                    command.CheckResponse(result);

                }
                catch (Exception exception)
                {
                    context.Exception = exception;

                    OnException(context);

                    if (!context.ExceptionHandled)
                    {
                        throw;
                    }
                }

                context.Response = result;

                return result;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Execute specified command.
        /// </summary>
        [NotNull]
        public virtual ServerResponse ExecuteCommand
            (
                [NotNull] ExecutionContext context
            )
        {
            Code.NotNull(context, "context");

            context.Verify(true);

            OnBeforeExecute(context);

            ServerResponse result;

            if (ReferenceEquals(NestedEngine, null))
            {
                result = StandardExecution(context);
            }
            else
            {
                result = NestedEngine.ExecuteCommand(context);
            }

            context.Response = result;

            OnAfterExecute(context);

            return result;
        }

        #endregion
    }
}
