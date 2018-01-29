// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PrintCommand.cs -- 
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
    public sealed class PrintCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PrintCommand()
            : base("Print")
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

            MxRecord[] records = executive.Records.ToArray();

            if (records.Length == 0)
            {
                executive.WriteLine("No records");
            }
            else
            {
                if (!string.IsNullOrEmpty(executive.OrderFormat))
                {
                    int[] mfns = records.Select(r => r.Mfn).ToArray();
                    string[] order = executive.Provider.FormatRecords(mfns, executive.OrderFormat);
                    for (int i = 0; i < order.Length; i++)
                    {
                        records[i].Order = order[i];
                    }

                    records = records.OrderBy(r => r.Order).ToArray();
                }

                if (!string.IsNullOrEmpty(executive.DescriptionFormat))
                {
                    int[] mfns = records.Select(r => r.Mfn).ToArray();
                    string[] formatted = executive.Provider.FormatRecords(mfns, executive.DescriptionFormat);
                    for (int i = 0; i < formatted.Length; i++)
                    {
                        records[i].Order = formatted[i];
                    }
                }

                foreach (MxRecord record in records)
                {
                    if (string.IsNullOrEmpty(record.Description))
                    {
                        executive.WriteOutput(record.Mfn.ToInvariantString());
                    }
                    else
                    {
                        executive.WriteOutput(record.Description);
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
