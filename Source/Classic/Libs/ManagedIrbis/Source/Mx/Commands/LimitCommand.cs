// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LimitCommand.cs -- 
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
    public sealed class LimitCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LimitCommand()
            : base("Limit")
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region MxCommand members

        /// <inheritdoc/>
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
                if (!string.IsNullOrEmpty(argument))
                {
                    int newLimit;
                    if (!NumericUtility.TryParseInt32(argument, out newLimit))
                    {
                        executive.WriteLine("format error");
                    }
                    executive.Limit = newLimit;
                    executive.WriteLine
                        (
                            3,
                            "Limit changed to {0}",
                            executive.Limit
                        );
                }
            }
            else
            {
                executive.WriteLine("Limit is: {0}", executive.Limit);
            }

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
