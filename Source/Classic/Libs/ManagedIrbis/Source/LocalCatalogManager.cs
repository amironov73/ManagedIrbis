// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LocalCatalogManager.cs -- creates, copies local catalogs
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

using AM;
using AM.IO;
using AM.Logging;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Direct;

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
        /// Root path for catalogs (DATAI folder).
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
                Log.Error
                    (
                        "LocalCatalogManager::Constructor: "
                        + "directory not exist: "
                        + rootPath.ToVisibleString()
                    );

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
                Log.Error
                    (
                        "LocalCatalogManager::_CopyDatabaseOnly: "
                        + "directory not found: "
                        + sourcePath.ToVisibleString()
                    );

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
        }

        /// <summary>
        /// Create database from the blank.
        /// </summary>
        public void CreateCatalog
            (
                [NotNull] string ibisPath,
                [NotNull] string targetPath
            )
        {
            Code.NotNullNorEmpty(ibisPath, "ibisPath");
            Code.NotNullNorEmpty(targetPath, "targetPath");

            if (!Directory.Exists(ibisPath))
            {
                Log.Error
                    (
                        "LocalCatalogManager::CreateCatalog: "
                        + "ibisPath doesn't exist: "
                        + ibisPath.ToVisibleString()
                    );

                throw new IrbisException("ibisPath doesn't exist");
            }

            string directory = Path.GetDirectoryName(ibisPath);
            if (string.IsNullOrEmpty(directory))
            {
                Log.Error
                    (
                        "LocalCatalogManager::CreateCatalog: "
                        + "ibisPath must be full: "
                        + ibisPath.ToVisibleString()
                    );

                throw new IrbisException("must be full path");
            }

            string ibisName = Path.GetFileName(directory);
            if (string.IsNullOrEmpty(ibisName))
            {
                Log.Error
                    (
                        "LocalCatalogManager::CreateCatalog: "
                        + "ibisPath must be full: "
                        + ibisPath.ToVisibleString()
                    );

                throw new IrbisException("must be full path");
            }

            directory = Path.GetDirectoryName(targetPath);
            if (string.IsNullOrEmpty(directory))
            {
                Log.Error
                    (
                        "LocalCatalogManager::CreateCatalog: "
                        + "targetPath must be full: "
                        + targetPath.ToVisibleString()
                    );

                throw new IrbisException("must be full path");
            }

            string targetName = Path.GetFileName(directory);
            if (string.IsNullOrEmpty(ibisName))
            {
                Log.Error
                    (
                        "LocalCatalogManager::CreateCatalog: "
                        + "targetPath must be full: "
                        + targetPath.ToVisibleString()
                    );

                throw new IrbisException("must be full path");
            }

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            DirectoryUtility.ClearDirectory(targetPath);

            string[] sourceFiles = Directory.GetFiles(ibisPath);
            string[] extensions = IrbisCatalog.GetExtensions();
            foreach (string sourceFile in sourceFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(sourceFile)
                    .ThrowIfNull("Path.GetFileName");
                if (fileName.SameString(ibisPath))
                {
                    // don't copy ibis.* database files

                    string extension = Path.GetExtension(sourceFile)
                        .ThrowIfNull("Path.GetExtension");
                    if (extensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    // change file name
                    fileName = targetName + extension;
                }
                else
                {
                    fileName = Path.GetFileName(sourceFile)
                        .ThrowIfNull("Path.GetFileName");
                }

                string targetFile = Path.Combine
                    (
                        targetPath,
                        fileName
                    );
                File.Copy(sourceFile, targetFile);
            }

            DirectUtility.CreateDatabase64
                (
                    targetPath
                );

            string parName = Path.Combine
                (
                    RootPath,
                    targetName + ".par"
                );
            if (!File.Exists(parName))
            {
                ParFile parFile = new ParFile(targetPath);
                parFile.WriteFile(parName);
            }
        }

        /// <summary>
        /// Replicate catalog
        /// </summary>
        public void ReplicateCatalog
            (
                [NotNull] string sourcePath,
                [NotNull] string tagetPath
            )
        {
            Code.NotNullNorEmpty(sourcePath, "sourcePath");
            Code.NotNullNorEmpty(tagetPath, "tagetPath");

            Log.Error
                (
                    "LocalCatalogManager::ReplicateCatalog: "
                    + "not implemented"
                );

            throw new NotImplementedException();
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

