/* PftLocalEnvironment.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Environment
{
    /// <summary>
    /// Local operation mode.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftLocalEnvironment
        : PftEnvironmentAbstraction
    {
        #region Properties

        /// <summary>
        /// Data path.
        /// </summary>
        public string DataPath { get; set; }

        /// <summary>
        /// Root path.
        /// </summary>
        public string RootPath { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftLocalEnvironment()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftLocalEnvironment
            (
                string rootPath
            )
        {
            RootPath = rootPath;
            DataPath = rootPath + "/DataI";
        }

        #endregion

        #region PftEnvironmentAbstraction members

        /// <inheritdoc/>
        public override string ReadFile
            (
                FileSpecification fileSpecification
            )
        {
            string fileName = DataPath + "/Deposit" + fileSpecification.FileName;
            string result = File.ReadAllText(fileName, IrbisEncoding.Ansi);
            return result;
        }

        #endregion
    }
}
