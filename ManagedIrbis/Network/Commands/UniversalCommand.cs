/* UniversalCommand.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Network.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class UniversalCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Accept any server response.
        /// </summary>
        public bool AcceptAnyResponse { get; set; }

        /// <summary>
        /// Arguments.
        /// </summary>
        [CanBeNull]
        public object[] Arguments { get; private set; }

        /// <summary>
        /// Command code
        /// </summary>
        [NotNull]
        public string CommandCode { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public UniversalCommand
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string commandCode
            )
            : base(connection)
        {
            Code.NotNullNorEmpty(commandCode, "commandCode");

            CommandCode = commandCode;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UniversalCommand
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string commandCode,
                params object[] arguments
            )
            : this (connection, commandCode)
        {
            Arguments = arguments;
        }

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Check the server response.
        /// </summary>
        public override void CheckResponse
            (
                ServerResponse response
            )
        {
            if (!AcceptAnyResponse)
            {
                base.CheckResponse(response);
            }
        }

        /// <summary>
        /// Create client query.
        /// </summary>
        public override IrbisClientQuery CreateQuery()
        {
            IrbisClientQuery result = base.CreateQuery();

            result.CommandCode = CommandCode;
            if (!ReferenceEquals(Arguments, null))
            {
                result.Arguments.AddRange(Arguments);
            }

            return result;
        }

        #endregion
    }
}
