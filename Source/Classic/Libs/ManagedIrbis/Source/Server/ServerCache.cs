// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerCache.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable NotAccessedField.Local

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Simple cache for files.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ServerCache
    {
        #region Constants

        /// <summary>
        /// Default memory limit.
        /// </summary>
        public const int DefaultLimit = 100 * 1024;

        #endregion

        #region Nested classes

        class Entry
        {
            public byte[] Content;

            public DateTime ModificationTime;

            public DateTime AccessTime;

            public int AccessCount;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Memory usage limit, bytes.
        /// </summary>
        public int MemoryLimit { get; private set; }

        /// <summary>
        /// Memory usage, bytes.
        /// </summary>
        public int MemoryUsage { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor
        /// </summary>
        public ServerCache()
        {
            _sync = new object();
            _dictionary = new Dictionary<string, Entry>();
            MemoryLimit = DefaultLimit;
            MemoryUsage = 0;
        }

        #endregion

        #region Private members

        private readonly object _sync;

        private readonly Dictionary<string, Entry> _dictionary;

        #endregion

        #region Public methods

        /// <summary>
        /// Clear the cache.
        /// </summary>
        public void Clear()
        {
            lock (_sync)
            {
                _dictionary.Clear();
                MemoryUsage = 0;
            }
        }

        /// <summary>
        /// Get file content.
        /// </summary>
        [NotNull]
        public byte[] GetFile
            (
                [NotNull] string fileName
            )
        {
            Code.FileExists(fileName, "fileName");

            lock (_sync)
            {
                DateTime modified = File.GetLastWriteTime(fileName);
                Entry entry;
                if (!_dictionary.TryGetValue(fileName, out entry))
                {
                    byte[] content = File.ReadAllBytes(fileName);
                    entry = new Entry
                    {
                        Content = content,
                        ModificationTime = modified
                    };
                    int newUsage = MemoryUsage + content.Length;
                    if (newUsage < MemoryLimit)
                    {
                        _dictionary[fileName] = entry;
                        MemoryUsage = newUsage;
                    }
                }
                else
                {
                    if (entry.ModificationTime < modified)
                    {
                        MemoryUsage -= entry.Content.Length;
                        entry.Content = File.ReadAllBytes(fileName);
                        MemoryUsage += entry.Content.Length;
                        entry.ModificationTime = modified;
                    }
                }

                entry.AccessTime = DateTime.Now;
                entry.AccessCount++;

                return entry.Content;
            }
        }

        #endregion
    }
}
