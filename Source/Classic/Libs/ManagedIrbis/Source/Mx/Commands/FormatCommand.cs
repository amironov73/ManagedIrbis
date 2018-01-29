// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FormatCommand.cs -- 
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
    public sealed class FormatCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FormatCommand()
            : base("Format")
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
            if (!string.IsNullOrEmpty(argument))
            {
                executive.DescriptionFormat = argument;

                if (executive.Provider.Connected
                    && executive.Records.Count != 0)
                {
                    int[] mfns = executive.Records.Select(r => r.Mfn)
                        .ToArray();
                    string[] formatted = executive.Provider.FormatRecords
                        (
                            mfns,
                            executive.DescriptionFormat
                        );
                    for (int i = 0; i < mfns.Length; i++)
                    {
                        executive.Records[i].Description = formatted[i];
                    }
                }
            }
            else
            {
                executive.WriteMessage(string.Format
                    (
                        "Format is: {0}",
                        executive.DescriptionFormat
                    ));
            }

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
