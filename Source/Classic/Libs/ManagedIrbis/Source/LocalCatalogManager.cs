// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LocalCatalogManager.cs -- creates, copies local catalogs
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#if !WIN81 && !PORTABLE

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.IO;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Creates, copies, removes local catalogs.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class LocalCatalogManager
    {
        #region Properties

        /// <summary>
        /// Text output.
        /// </summary>
        [NotNull]
        public AbstractOutput Output { get; private set; }

        /// <summary>
        /// Root path for catalogs.
        /// </summary>
        [NotNull]
        public string RootPath { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalCatalogManager
            (
                [NotNull] string rootPath,
                [NotNull] AbstractOutput output
            )
        {
            Code.NotNullNorEmpty(rootPath, "rootPath");
            Code.NotNull(output, "output");

            if (!Directory.Exists(rootPath))
            {
                throw new DirectoryNotFoundException
                    (
                        rootPath
                    );
            }

            RootPath = rootPath;
            Output = output;
        }

        #endregion

        #region Private members

        private void _CopyDatabaseOnly
            (
                string sourcePath,
                string targetPath,
                string catalogName
            )
        {
            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException
                    (
                        sourcePath
                    );
            }

            string[] extensions = IrbisCatalog.GetExtensions();
            foreach (string extension in extensions)
            {
                string sourceFile = PathUtility.Combine
                    (
                        sourcePath,
                        catalogName,
                        extension
                    );
                string targetFile = PathUtility.Combine
                    (
                        targetPath,
                        catalogName,
                        extension
                    );
                File.Copy
                    (
                        sourceFile,
                        targetFile,
                        true
                    );
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Backup catalog database to the given path.
        /// </summary>
        public void BackupDatabase
            (
                [NotNull] string backupPath,
                [NotNull] string catalogName
            )
        {
            Code.NotNullNorEmpty(backupPath, "backupPath");
            Code.NotNull(catalogName, "catalogName");

#if SILVERLIGHT

            throw new NotImplementedException();

#else

            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
            }
            DirectoryUtility.ClearDirectory(backupPath);

            _CopyDatabaseOnly
                (
                    RootPath,
                    backupPath,
                    catalogName
                );

#endif
        }

        /// <summary>
        /// Restore catalog database from the given path.
        /// </summary>
        public void RestoreDatabase
            (
                [NotNull] string backupPath,
                [NotNull] string catalogName
            )
        {
            Code.NotNullNorEmpty(backupPath, "backupPath");
            Code.NotNull(catalogName, "catalogName");

            _CopyDatabaseOnly
                (
                    backupPath,
                    RootPath,
                    catalogName
                );
        }

        #endregion
    }
}

#endif

