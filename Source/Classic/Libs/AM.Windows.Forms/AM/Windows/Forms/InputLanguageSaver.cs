// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InputLanguageSaver.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM.Logging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class InputLanguageSaver
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Previous language.
        /// </summary>
        [NotNull]
        public InputLanguage PreviousLanguage { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public InputLanguageSaver()
        {
            PreviousLanguage = InputLanguage.CurrentInputLanguage;

            Log.Trace
                (
                    "InputLanguageSaver::Constructor: "
                    + "previous language="
                    + PreviousLanguage
                );
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Log.Trace
                (
                    "InputLanguageSaver::Dispose: "
                    + "previous language="
                    + PreviousLanguage
                );

            InputLanguage.CurrentInputLanguage = PreviousLanguage;
        }

        #endregion

        #region Object members

        #endregion
    }
}
