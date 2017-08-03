// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExternalPftCache.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC || NETCORE

#region Using directives

using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

using AM.Reflection;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ExternalPftCache
    {
        #region Constants

        /// <summary>
        /// Serialized AST file extension.
        /// </summary>
        public const string AST = ".ast";

        /// <summary>
        /// DLL file extension.
        /// </summary>
        public const string DLL = ".dll";

        #endregion

        #region Properties

        /// <summary>
        /// Root directory.
        /// </summary>
        [NotNull]
        public string RootDirectory { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public ExternalPftCache()
        {
            _hasp = new object();
            SetRootDirectory(GetDefaultRootDirectory());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExternalPftCache
            (
                [NotNull] string rootDirectory
            )
        {
            Code.NotNullNorEmpty(rootDirectory, "rootDirectory");

            _hasp = new object();
            RootDirectory = rootDirectory;
        }

        #endregion

        #region Private members

        private readonly object _hasp;

        #endregion

        #region Public methods

        /// <summary>
        /// Add DLL with compiled PFT.
        /// </summary>
        public void AddDll
            (
                [NotNull] string scriptText,
                [NotNull] byte[] image
            )
        {
            Code.NotNull(scriptText, "scriptText");
            Code.NotNull(image, "image");

            lock (_hasp)
            {
                string path = ComputePath(scriptText) + DLL;
                File.WriteAllBytes(path, image);
            }
        }

        /// <summary>
        /// Add serialized PFT.
        /// </summary>
        public void AddSerializedPft
            (
                [NotNull] string scriptText,
                [NotNull] byte[] image
            )
        {
            Code.NotNull(scriptText, "scriptText");
            Code.NotNull(image, "image");

            lock (_hasp)
            {
                string path = ComputePath(scriptText) + AST;
                File.WriteAllBytes(path, image);
            }
        }

        /// <summary>
        /// Clear the cache.
        /// </summary>
        public void Clear()
        {
            lock (_hasp)
            {
                string[] files = Directory.GetFiles
                    (
                        RootDirectory,
                        "*.*",
                        SearchOption.AllDirectories
                    );

                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// Compute file name for given script.
        /// </summary>
        [NotNull]
        public string ComputeFileName
            (
                [NotNull] string scriptText
            )
        {
            Code.NotNullNorEmpty(scriptText, "scriptText");

            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = IrbisEncoding.Utf8.GetBytes(scriptText);
                byte[] hash = md5.ComputeHash(bytes);
                StringBuilder result = new StringBuilder(hash.Length * 2);
                foreach (byte one in hash)
                {
                    result.AppendFormat(one.ToString("X2"));
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Compute full file name for given script.
        /// </summary>
        [NotNull]
        public string ComputePath
            (
                [NotNull] string scriptText
            )
        {
            Code.NotNull(scriptText, scriptText);

            string fileName = ComputeFileName(scriptText);
            string result = Path.Combine
                (
                    RootDirectory,
                    fileName
                );

            return result;
        }

        /// <summary>
        /// </summary>
        /// Get path of default cache root directory.
        [NotNull]
        public string GetDefaultRootDirectory()
        {
#if NETCORE

            // TODO implement properly

            return Path.Combine
                (
                    Directory.GetCurrentDirectory(),
                    "PftCatch"
                );

#else

            string result = Path.Combine
                (
                    Path.Combine
                    (
                        Environment.GetFolderPath
                            (
                                Environment.SpecialFolder.ApplicationData
                            ),
                        "ManagedIrbis"
                    ),
                    "PftCache"
                );

            return result;

#endif
        }

        /// <summary>
        /// Get DLL for specified script.
        /// </summary>
        [CanBeNull]
        public Func<PftContext, PftPacket> GetDll
            (
                [NotNull] string scriptText
            )
        {
            Code.NotNullNorEmpty(scriptText, "scriptText");

            lock (_hasp)
            {
                string path = GetPath(scriptText, DLL);
                if (ReferenceEquals(path, null))
                {
                    return null;
                }

                // TODO choose the right method

                Assembly assembly = AssemblyUtility.LoadFile(path);
                //Assembly assembly = Assembly.LoadFrom(path);

                Func<PftContext, PftPacket> result =
                    CompilerUtility.GetEntryPoint(assembly);

                return result;
            }
        }

        /// <summary>
        /// Get supposed path for specified script.
        /// </summary>
        [CanBeNull]
        public string GetPath
            (
                [NotNull] string scriptText,
                [NotNull] string extension
            )
        {
            Code.NotNullNorEmpty(scriptText, "scriptText");
            Code.NotNull(extension, "extension");

            lock (_hasp)
            {
                string result = ComputePath(scriptText) + extension;
                if (File.Exists(result))
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Get serialized PFT for the script.
        /// </summary>
        [CanBeNull]
        public PftNode GetSerializedPft
            (
                [NotNull] string scriptText
            )
        {
            Code.NotNullNorEmpty(scriptText, "scriptText");

            lock (_hasp)
            {
                string path = GetPath(scriptText, AST);
                if (ReferenceEquals(path, null))
                {
                    return null;
                }

                PftNode result = PftSerializer.Read(path);
                return result;
            }
        }

        /// <summary>
        /// Set root directory.
        /// </summary>
        public void SetRootDirectory
            (
                [NotNull] string path
            )
        {
            Code.NotNullNorEmpty(path, "path");

            lock (_hasp)
            {
                path = Path.GetFullPath(path);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                RootDirectory = path;
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return RootDirectory;
        }

        #endregion
    }
}

#endif
