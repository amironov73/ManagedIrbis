// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DynamicCommand.cs -- fully dynamic command.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// Dynamic command. All actions configured
    /// during runtime.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DynamicCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Handle return codes.
        /// </summary>
        public Func<DynamicCommand, int[]> GoodReturnCodesHandler
        { get; set; }

        /// <summary>
        /// Create query.
        /// </summary>
        public Func<DynamicCommand, ClientQuery> CreateQueryHandler
        { get; set; }

        /// <summary>
        /// Check server response.
        /// </summary>
        public Action<DynamicCommand, ServerResponse> CheckResponseHandler
        { get; set; }

        /// <summary>
        /// Execute command.
        /// </summary>
        public Func<DynamicCommand, ClientQuery, ServerResponse> ExecuteHandler
        { get; set; }

        /// <summary>
        /// Verify command settings.
        /// </summary>
        public Func<DynamicCommand, bool, bool> VerifyHandler
        { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DynamicCommand
            (
                [NotNull] IIrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Basic implementation of <see cref="GoodReturnCodes"/>.
        /// </summary>
        public int[] BaseGoodReturnCodes()
        {
            int[] result = base.GoodReturnCodes;

            return result;
        }

        /// <summary>
        /// Basic implementation of <see cref="CreateQuery"/>.
        /// </summary>
        public ClientQuery BaseCreateQuery()
        {
            ClientQuery result = base.CreateQuery();

            return result;
        }

        /// <summary>
        /// Basic implementation of <see cref="CheckResponse"/>.
        /// </summary>
        public void BaseCheckResponse
            (
                ServerResponse response
            )
        {
            base.CheckResponse(response);
        }

        /// <summary>
        /// Basic implementation of <see cref="Execute"/>.
        /// </summary>
        public ServerResponse BaseExecute
            (
                ClientQuery query
            )
        {
            ServerResponse result = base.Execute(query);

            return result;
        }

        /// <summary>
        /// Basic implementation of <see cref="AbstractCommand.Verify"/>.
        /// </summary>
        public bool BaseVerify
            (
                bool throwOnError
            )
        {
            bool result = Verify(throwOnError);

            return result;
        }

        #endregion

        #region AbstractCommand members

        /// <inheritdoc cref="AbstractCommand.GoodReturnCodes" />
        public override int[] GoodReturnCodes
        {
            get
            {
                Func<DynamicCommand, int[]> handler
                    = GoodReturnCodesHandler;

                int[] result = !ReferenceEquals(handler, null)
                      ? handler(this)
                      : BaseGoodReturnCodes();

                return result;
            }
        }

        /// <inheritdoc cref="AbstractCommand.CheckResponse" />
        public override void CheckResponse
            (
                ServerResponse response
            )
        {
            Action<DynamicCommand, ServerResponse> handler
                = CheckResponseHandler;

            if (!ReferenceEquals(handler, null))
            {
                handler(this, response);
            }
            else
            {
                BaseCheckResponse(response);
            }
        }

        /// <inheritdoc cref="AbstractCommand.CreateQuery" />
        public override ClientQuery CreateQuery()
        {
            Func<DynamicCommand, ClientQuery> handler
                = CreateQueryHandler;

            ClientQuery result = !ReferenceEquals(handler, null)
                ? handler(this)
                : BaseCreateQuery();

            return result;
        }

        /// <inheritdoc cref="AbstractCommand.Execute" />
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            Func<DynamicCommand, ClientQuery, ServerResponse> handler
                = ExecuteHandler;

            ServerResponse result = !ReferenceEquals(handler, null)
                ? handler(this, query)
                : BaseExecute(query);

            return result;
        }

        #endregion

        #region IVerifiable members

        // TODO Fix this

        ///// <inheritdoc cref="AbstractCommand.Verify" />
        //public override bool Verify
        //    (
        //        bool throwOnError
        //    )
        //{
        //    Func<DynamicCommand, bool, bool> handler = VerifyHandler;

        //    bool result = !ReferenceEquals(handler, null)
        //      ? handler(this, throwOnError)
        //      : BaseVerify(throwOnError);

        //    return result;
        //}

        #endregion

        #region Object members

        #endregion
    }
}
