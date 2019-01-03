// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisCatalog.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace UnsafeIrbis
{
    /// <summary>
    /// Common catalog-related stuff.
    /// </summary>
    [PublicAPI]
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
        public const string NodeFileExtension1 = "n01";

        /// <summary>
        /// Node file.
        /// </summary>
        public const string NodeFileExtension2 = "n02";

        /// <summary>
        /// Leaf node file.
        /// </summary>
        public const string LeafFileExtension1 = "l01";

        /// <summary>
        /// Leaf node file.
        /// </summary>
        public const string LeafFileExtension2 = "l02";

        /// <summary>
        /// File selection table.
        /// </summary>
        public const string FileSelectionTableExtension = "fst";

        /// <summary>
        /// File selection table.
        /// </summary>
        public const string FileSelectionInvertedTableExtension = "ifs";

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
                NodeFileExtension1,
                NodeFileExtension2,
                LeafFileExtension1,
                LeafFileExtension2,
                FileSelectionTableExtension,
                FileSelectionInvertedTableExtension
            };

            return result;
        }

        #endregion
    }
}
