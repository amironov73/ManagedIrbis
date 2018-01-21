// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchCommand.cs -- 
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
    public sealed class SearchCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchCommand()
            : base("Search")
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

            if (arguments.Length != 0)
            {
                string argument = arguments[0].Text;
                if (!string.IsNullOrEmpty(argument))
                {
                    int[] found = executive.Client.Search(argument);
                    int foundCount = found.Length;
                    executive.WriteLine
                        (
                            3,
                            "Found: {0}",
                            found.Length
                        );

                    if (executive.Limit > 0)
                    {
                        found = found.Take(executive.Limit).ToArray();
                        if (found.Length < foundCount)
                        {
                            executive.WriteLine
                                (
                                    3,
                                    "Limited to {0} records",
                                    found.Length
                                );
                        }
                    }
                    executive.Records.Clear();
                    for (int i = 0; i < found.Length; i++)
                    {
                        int mfn = found[i];
                        MxRecord record = new MxRecord
                        {
                            Database = executive.Client.Database,
                            Mfn = mfn,
                        };
                        executive.Records.Add(record);
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
