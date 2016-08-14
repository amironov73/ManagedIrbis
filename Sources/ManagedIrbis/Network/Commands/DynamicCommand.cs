/* DynamicCommand.cs -- fully dynamic command.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network.Commands
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
        public Func<DynamicCommand,ClientQuery> CreateQueryHandler 
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
                [NotNull] IrbisConnection connection
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
        /// Basic implementation of <see cref="Verify"/>.
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

        /// <summary>
        /// Good return codes.
        /// </summary>
        public override int[] GoodReturnCodes
        {
            get
            {
                int[] result =
                    GoodReturnCodesHandler != null
                        ? GoodReturnCodesHandler(this)
                        : BaseGoodReturnCodes();

                return result;
            }
        }

        /// <summary>
        /// Check the server response.
        /// </summary>
        public override void CheckResponse
            (
                ServerResponse response
            )
        {
            if (CheckResponseHandler != null)
            {
                CheckResponseHandler(this, response);
            }
            else
            {
                BaseCheckResponse(response);
            }
        }

        /// <summary>
        /// Create client query.
        /// </summary>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = CreateQueryHandler != null
                ? CreateQueryHandler(this)
                : BaseCreateQuery();

            return result;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            ServerResponse result = ExecuteHandler != null
                ? ExecuteHandler(this, query)
                : BaseExecute(query);

            return result;
        }

        /// <summary>
        /// Verify the object state.
        /// </summary>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            bool result = VerifyHandler != null
                ? VerifyHandler(this, throwOnError)
                : BaseVerify(throwOnError);

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
