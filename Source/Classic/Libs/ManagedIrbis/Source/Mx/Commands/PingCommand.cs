// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PingCommand.cs -- 
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
    public sealed class PingCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PingCommand()
            : base("ping")
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Do one ping operation.
        /// </summary>
        public long DoPing
            (
                int number,
                [NotNull] MxExecutive executive
            )
        {
            Code.NotNull(executive, "executive");

            long result = 0;

            try
            {

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                executive.Provider.NoOp();
                stopwatch.Stop();

                result = stopwatch.ElapsedMilliseconds;

                executive.WriteMessage(string.Format
                    (
                        "{0}: {1} ms",
                        number,
                        result
                    ));
            }
            catch
            {
                executive.WriteError("ERROR");
            }

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

            if (!executive.Provider.Connected)
            {
                executive.WriteLine("Not connected");
                return false;
            }

            long sum = 0;
            int ntries = 4;
            if (arguments.Length != 0)
            {
                int n = arguments[0].Text.SafeToInt32();
                if (n > 1)
                {
                    ntries = n;
                }
            }
            for (int i = 0; i < ntries; i++)
            {
                sum += DoPing(i + 1, executive);
            }

            executive.WriteMessage(string.Format
                (
                    "average = {0}", sum / ntries
                ));

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
