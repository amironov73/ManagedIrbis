// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblLogger.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// Logger for global corrections.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class GblLogger
    {
        #region Properties

        /// <summary>
        /// Output.
        /// </summary>
        [NotNull]
        public TextWriter Output { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblLogger ()
        {
            Output = new StringWriter();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get output text.
        /// </summary>
        [NotNull]
        public string GetText()
        {
            return ((StreamWriter) Output).ToString();
        }

        /// <summary>
        /// Write the line.
        /// </summary>
        public void WriteLine
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(format, "format");

            Output.WriteLine(format, args);
        }

        #endregion
    }
}