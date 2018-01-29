// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SortCommand.cs -- 
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
    public sealed class SortCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SortCommand()
            : base("Sort")
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

            string argument = null;
            if (arguments.Length != 0)
            {
                argument = arguments[0].Text;
            }

            if (string.IsNullOrEmpty(argument))
            {
                string sort = executive.OrderFormat;
                if (string.IsNullOrEmpty(sort))
                {
                    sort = "OFF";
                }
                executive.WriteMessage(string.Format
                    (
                        "SORT is: {0}",
                        sort
                    ));
            }
            else if (argument.SameString("off"))
            {
                executive.OrderFormat = null;
                executive.WriteMessage("SORT is OFF now");
            }
            else
            {
                executive.OrderFormat = argument;
            }

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
