// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HistoryCommand.cs -- 
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
    public sealed class HistoryCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public HistoryCommand()
            : base("history")
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

            string command = null;
            if (arguments.Length != 0)
            {
                command = arguments[0].Text;
            }

            string[] history = executive.History.ToArray();

            if (string.IsNullOrEmpty(command))
            {
                for (int i = 0; i < history.Length; i++)
                {
                    executive.WriteLine("{0}: {1}", i + 1, history[i]);
                }
            }
            else
            {
                int index;
                if (NumericUtility.TryParseInt32(command, out index))
                {
                    string argument = history.GetOccurrence
                        (
                            history.Length - index
                        );
                    if (string.IsNullOrEmpty(argument))
                    {
                        executive.WriteLine("No such entry");
                    }
                    else
                    {
                        SearchCommand searchCommand = executive.Commands
                            .OfType<SearchCommand>().FirstOrDefault();
                        if (!ReferenceEquals(searchCommand, null))
                        {
                            executive.WriteLine(argument);
                            MxArgument[] newArguments =
                            {
                                new MxArgument {Text = argument}
                            };
                            searchCommand.Execute(executive, newArguments);
                            executive.History.Pop();
                        }
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
