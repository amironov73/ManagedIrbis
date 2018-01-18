// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConnectCommand.cs -- connect to the server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
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
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Client;
using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    /// Connect to the server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ConnectCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConnectCommand()
            : base("Connect")
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Initialize the provider.
        /// </summary>
        [NotNull]
        public static IrbisProvider InitializeProvider
            (
                [NotNull] string argument
            )
        {
            Code.NotNull(argument, "argument");

            IrbisProvider result = ProviderManager.GetAndConfigureProvider(argument);

            return result;
        }

        #endregion

        #region MxCommand members

        /// <inheritdoc cref="MxCommand.Execute" />
        public override bool Execute
            (
                MxExecutive executive,
                MxArgument[] arguments
            )
        {
            OnBeforeExecute();

            if (arguments.Length != 0)
            {
                string argument = arguments[0].Text;
                executive.Client.Dispose();
                if (string.IsNullOrEmpty(argument))
                {
                    executive.Client = ProviderManager.GetPreconfiguredProvider();
                }
                else
                {
                    executive.Client = InitializeProvider(argument);
                }
                executive.WriteLine
                    (
                        3,
                        "Connected, current database: {0}",
                        executive.Client.Database
                    );
            }

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
