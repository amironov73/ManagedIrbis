// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisCatalog.cs --
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Common catalog-related stuff.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisCatalog
    {
        #region Constants

        /// <summary>
        /// Master file.
        /// </summary>
        public const string MasterFileExtension = "mst";

        /// <summary>
        /// Cross-reference.
        /// </summary>
        public const string CrossReferenceExtension = "xrf";

        /// <summary>
        /// Index (inverted) file.
        /// </summary>
        public const string IndexFileExtension = "ifp";

        /// <summary>
        /// Node file.
        /// </summary>
        public const string NodeFileExtension = "n01";

        /// <summary>
        /// Leaf node file.
        /// </summary>
        public const string LeafFileExtension = "l01";

        /// <summary>
        /// File selection table.
        /// </summary>
        public const string FileSelectionTableExtension = "fst";

        #endregion

        #region Properties

        #endregion

        #region Public methods

        /// <summary>
        /// Get extensions for database files.
        /// </summary>
        [NotNull]
        public static string[] GetExtensions()
        {
            string[] result =
            {
                MasterFileExtension,
                CrossReferenceExtension,
                IndexFileExtension,
                NodeFileExtension,
                LeafFileExtension,
                FileSelectionTableExtension
            };

            return result;
        }

        #endregion
    }
}
