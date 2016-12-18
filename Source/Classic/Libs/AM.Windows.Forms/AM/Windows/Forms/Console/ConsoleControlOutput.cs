// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConsoleControlOutput.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Drawing;

using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Output to ConsoleControl.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ConsoleControlOutput
        : AbstractOutput
    {
        #region Properties

        /// <summary>
        /// Console control.
        /// </summary>
        [NotNull]
        public ConsoleControl ConsoleControl { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConsoleControlOutput
            (
                [NotNull] ConsoleControl console
            )
        {
            Code.NotNull(console, "console");

            ConsoleControl = console;
        }

        #endregion

        #region AbstractOutput members

        /// <inheritdoc />
        public override bool HaveError { get; set; }

        /// <inheritdoc />
        public override AbstractOutput Clear()
        {
            ConsoleControl.Clear();
            HaveError = false;

            return this;
        }

        /// <inheritdoc />
        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override AbstractOutput Write
            (
                string text
            )
        {
            ConsoleControl.Write(text);

            return this;
        }

        /// <inheritdoc />
        public override AbstractOutput WriteError
            (
                string text
            )
        {
            ConsoleControl.Write(Color.Red, text);
            HaveError = true;

            return this;
        }

        #endregion
    }
}
