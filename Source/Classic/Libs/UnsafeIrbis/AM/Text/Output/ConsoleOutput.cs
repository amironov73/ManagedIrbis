// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConsoleOutput.cs -- консольный вывод.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using UnsafeAM.ConsoleIO;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Text.Output
{
    /// <summary>
    /// Консольный вывод.
    /// </summary>
    [PublicAPI]
    public sealed class ConsoleOutput
        : AbstractOutput
    {
        #region AbstractOutput members

        /// <inheritdoc cref="AbstractOutput.HaveError" />
        public override bool HaveError { get; set; }

        /// <inheritdoc cref="AbstractOutput.Clear" />
        public override AbstractOutput Clear()
        {
            HaveError = false;
            ConsoleInput.Clear();

            return this;
        }

        /// <inheritdoc cref="AbstractOutput.Configure"/>
        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // TODO: implement properly
            return this;
        }

        /// <inheritdoc cref="AbstractOutput.Write(string)" />
        public override AbstractOutput Write
            (
                string text
            )
        {
            ConsoleInput.Write(text);

            return this;
        }

        /// <inheritdoc cref="AbstractOutput.WriteError(string)" />
        public override AbstractOutput WriteError
            (
                string text
            )
        {
            HaveError = true;

            // TODO implement properly: System.Console.Error.Write(text);
            ConsoleInput.Write(text);

            return this;
        }

        #endregion
    }
}

