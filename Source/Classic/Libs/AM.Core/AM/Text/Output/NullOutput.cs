// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NullOutput.cs -- пустой объект вывода
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text.Output
{
    /// <summary>
    /// Пустой объект вывода.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class NullOutput
        : AbstractOutput
    {
        #region Private members

        #endregion

        #region AbstractOutput members

        /// <inheritdoc cref="AbstractOutput.HaveError" />
        public override bool HaveError { get; set; }
        
        /// <inheritdoc cref="AbstractOutput.Clear" />
        public override AbstractOutput Clear()
        {
            HaveError = false;

            return this;
        }

        /// <inheritdoc cref="AbstractOutput.Configure" />
        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // Noting to do here

            return this;
        }

        /// <inheritdoc cref="AbstractOutput.Write(string)" />
        public override AbstractOutput Write
            (
                string text
            )
        {
            // Nothing to do here

            return this;
        }

        /// <inheritdoc cref="AbstractOutput.WriteError(string)" />
        public override AbstractOutput WriteError
            (
                string text
            )
        {
            // Nothing to do here
            HaveError = true;

            return this;
        }

        #endregion
    }
}
