/* PftSystem.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

#if CLASSIC || DESKTOP

using System.Diagnostics;

#endif

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Execute system command, capture its output.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftSystem
        : PftNode
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSystem()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSystem
            (
                [NotNull] PftToken token
            )
        {
            Code.NotNull(token, "token");
            //token.MustBe()
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

#if CLASSIC || DESKTOP

            string expression = "/c " + context.Evaluate(Children);
            ProcessStartInfo startInfo = new ProcessStartInfo
                (
                    "cmd.exe",
                    expression
                )
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                StandardOutputEncoding = IrbisEncoding.Oem
            };
            Process process = Process.Start(startInfo)
                .ThrowIfNull("process");
            process.WaitForExit();
            string output = process.StandardOutput.ReadToEnd();
            if (!string.IsNullOrEmpty(output))
            {
                context.Write(this, output);
            }

#endif

            OnAfterExecution(context);
        }

        #endregion

        #region Object members

        #endregion
    }
}
