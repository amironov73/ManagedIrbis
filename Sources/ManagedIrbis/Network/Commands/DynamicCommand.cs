/* DynamicCommand.cs --
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

using AM;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Network.Commands
{
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DynamicCommand
        : AbstractCommand
    {
        #region Properties

        public Func<DynamicCommand, int[]> GoodReturnCodesHandler
        { get; set; }

        public Func<DynamicCommand,ClientQuery> CreateQueryHandler 
        { get; set; }

        public Action<DynamicCommand, ServerResponse> CheckResponseHandler
        { get; set; }

        public Func<DynamicCommand, ClientQuery, ServerResponse> ExecuteHandler
        { get; set; }

        public Func<DynamicCommand, bool, bool> VerifyHandler
        { get; set; }

        #endregion

        #region Construction

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

        public int[] BaseGoodReturnCodes()
        {
            int[] result = base.GoodReturnCodes;

            return result;
        }

        public ClientQuery BaseCreateQuery()
        {
            ClientQuery result = base.CreateQuery();

            return result;
        }

        public void BaseCheckResponse
            (
                ServerResponse response
            )
        {
            base.CheckResponse(response);
        }

        public ServerResponse BaseExecute
            (
                ClientQuery query
            )
        {
            ServerResponse result = base.Execute(query);

            return result;
        }

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

        public override ClientQuery CreateQuery()
        {
            ClientQuery result = CreateQueryHandler != null
                ? CreateQueryHandler(this)
                : BaseCreateQuery();

            return result;
        }

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
