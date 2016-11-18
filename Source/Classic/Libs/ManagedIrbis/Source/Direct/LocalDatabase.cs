/* LocalDatabase.cs -- 
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
using System.Security.Cryptography;
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

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class LocalDatabase
    {
        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [CanBeNull]
        public string Description { get; set; }

        /// <summary>
        /// Read only?
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// MST file path.
        /// </summary>
        [CanBeNull]
        public string MasterFilePath { get; set; }

        /// <summary>
        /// XRF file path.
        /// </summary>
        public string CrossReferencePath { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse PAR-file.
        /// </summary>
        [NotNull]
        public static LocalDatabase FromParFile
            (
                [NotNull] ParFile parFile
            )
        {
            Code.NotNull(parFile, "parFile");

            LocalDatabase result = new LocalDatabase();

            return result;
        }

        /// <summary>
        /// Construct <see cref="LocalDatabase"/> from given path.
        /// </summary>
        [NotNull]
        public static LocalDatabase FromPath
            (
                [NotNull] string mstPath
            )
        {
            Code.NotNullNorEmpty(mstPath, "mstPath");

            LocalDatabase result = new LocalDatabase();

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return Name.ToVisibleString();
            }

            return string.Format
                (
                    "{0} - {1}",
                    Name,
                    Description
                );
        }

        #endregion
    }
}
