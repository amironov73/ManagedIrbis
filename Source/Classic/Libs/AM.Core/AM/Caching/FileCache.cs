﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FileCache.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WINMOBILE && !PocketPC

#region Using directives

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Caching
{
    /// <summary>
    /// Cache with file-based item storage.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FileCache<TKey, TValue>
        : AbstractCache<TKey, TValue>,
            IDisposable
        where TValue : class, IHandmadeSerializable, new()
    {
        #region Properties

        /// <summary>
        /// Path to store items.
        /// </summary>
        [NotNull]
        public string CachePath { get; private set; }

        /// <summary>
        /// Use compression.
        /// </summary>
        public bool UseCompression { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileCache()
            : this(false)
        {
            _ownPath = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileCache
            (
                bool useCompression
            )
            : this(_UniquePath(), useCompression)
        {
            _ownPath = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileCache
            (
                [NotNull] string cachePath,
                bool useCompression
            )
        {
            Code.NotNullNorEmpty(cachePath, "cachePath");

            CachePath = cachePath;
            UseCompression = useCompression;
            if (!Directory.Exists(cachePath))
            {
                Directory.CreateDirectory(cachePath);
            }
            _dictionary = new ConcurrentDictionary<TKey, string>();
        }

        #endregion

        #region Private members

        private readonly ConcurrentDictionary<TKey, string> _dictionary;

        private readonly bool _ownPath;

        private byte[] _Compress(TValue value)
        {
            byte[] result = UseCompression
                ? value.SaveToZipMemory()
                : value.SaveToMemory();

            return result;
        }

        private TValue _Decomress(byte[] bytes)
        {
            TValue result = UseCompression
                ? bytes.RestoreObjectFromZipMemory<TValue>()
                : bytes.RestoreObjectFromMemory<TValue>();

            return result;
        }

        private static string _UniquePath()
        {
            string result = Path.Combine
                (
                    Path.GetTempPath(),
                    Guid.NewGuid().ToString("N")
                );

            return result;
        }

        private string _GetPath(string item)
        {
            string result = Path.Combine
                (
                    CachePath,
                    item
                );

            return result;
        }

        #endregion

        #region Public methods

        #endregion

        #region AbstractCache members

        /// <summary>
        /// Add or update item with given key.
        /// </summary>
        public override AbstractCache<TKey, TValue> Add
            (
                TKey key,
                TValue value
            )
        {
            Code.NotNull(value, "value");

            string itemName = Guid.NewGuid().ToString("N");
            _dictionary[key] = itemName;
            string path = _GetPath(itemName);
            byte[] bytes = _Compress(value);
            File.WriteAllBytes(path, bytes);

            return this;
        }

        /// <summary>
        /// Adds or update item in async way.
        /// </summary>
        public Task AddAsync
            (
                TKey key,
                TValue value
            )
        {
            return Task.Factory.StartNew
                (
                    () => Add(key, value)
                );
        }

        /// <summary>
        /// Clear.
        /// </summary>
        public override void Clear()
        {
            foreach (string item in _dictionary.Values)
            {
                string path = _GetPath(item);
                File.Delete(path);
            }
            _dictionary.Clear();
        }

        /// <summary>
        /// Clear in the async way.
        /// </summary>
        public Task ClearAsync()
        {
            return Task.Factory.StartNew
                (
                // For FW 3.5
                // ReSharper disable ConvertClosureToMethodGroup
                    () => Clear()
                // ReSharper restore ConvertClosureToMethodGroup
                );
        }

        /// <summary>
        /// Contains given key?
        /// </summary>
        public override bool ContainsKey
            (
                TKey key
            )
        {
            string itemName;
            return _dictionary.TryGetValue(key, out itemName);
        }

        /// <summary>
        /// Get item for given key.
        /// </summary>
        public override TValue Get
            (
                TKey key
            )
        {
            string itemName;
            if (!_dictionary.TryGetValue(key, out itemName))
            {
                return default(TValue);
            }

            string path = _GetPath(itemName);
            byte[] bytes = File.ReadAllBytes(path);
            TValue result = _Decomress(bytes);

            return result;
        }

        /// <summary>
        /// Remove item for specified key.
        /// </summary>
        public override void Remove
            (
                TKey key
            )
        {
            string itemName;
            if (!_dictionary.TryRemove(key, out itemName))
            {
                return;
            }

            string path = _GetPath(itemName);
            File.Delete(path);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Clear();
            if (_ownPath)
            {
                Directory.Delete(CachePath, true);
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}

#endif
