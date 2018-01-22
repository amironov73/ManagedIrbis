// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RefineCommand.cs -- 
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

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RefineCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RefineCommand()
            : base("refine")
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

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

            if (!executive.Client.Connected)
            {
                executive.WriteLine("Not connected");
                return false;
            }

            if (arguments.Length != 0
                && executive.History.Count != 0)
            {
                string argument = arguments[0].Text;
                string previous = executive.History.Peek();
                argument = string.Format("({0}) * ({1})", previous, argument);

                if (!string.IsNullOrEmpty(argument))
                {
                    SearchCommand searchCommand = executive.Commands
                        .OfType<SearchCommand>().FirstOrDefault();
                    if (!ReferenceEquals(searchCommand, null))
                    {
                        MxArgument[] newArguments =
                        {
                            new MxArgument {Text = argument}
                        };
                        searchCommand.Execute(executive, newArguments);
                    }
                }
            }

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
