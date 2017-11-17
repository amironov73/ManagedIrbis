// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DictionaryUtility.cs -- dictionary manipulation helpers
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// <see cref="Dictionary{Key,Value}" /> manipulation
    /// helper methods.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class DictionaryUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get value for the key or the default value.
        /// </summary>
        [CanBeNull]
        public static TValue GetValueOrDefault<TKey, TValue>
            (
                [NotNull] this IDictionary<TKey,TValue> dictionary,
                [NotNull] TKey key,
                TValue defaultValue
            )
        {
            Code.NotNull(dictionary, "dictionary");

            TValue result;
            if (!dictionary.TryGetValue(key, out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Get value for the key or default value.
        /// </summary>
        [CanBeNull]
        public static TValue GetValueOrDefault<TKey, TValue>
            (
                [NotNull] this IDictionary<TKey, TValue> dictionary,
                [NotNull] TKey key
            )
        {
            Code.NotNull(dictionary, "dictionary");

            TValue result;
            if (!dictionary.TryGetValue(key, out result))
            {
                result = default(TValue);
            }

            return result;
        }

        /// <summary>
        /// Merges the specified dictionaries.
        /// </summary>
        /// <param name="dictionaries">Dictionaries to merge.</param>
        /// <returns>Merged dictionary.</returns>
        /// <exception cref="ArgumentNullException">
        /// One or more dictionaries is <c>null</c>.
        /// </exception>
        public static Dictionary<TKey, TValue> MergeWithConflicts<TKey, TValue>
            (
                params Dictionary<TKey, TValue>[] dictionaries
            )
        {
            foreach (Dictionary<TKey, TValue> dictionary in dictionaries)
            {
                if (ReferenceEquals(dictionary, null))
                {
                    Log.Error
                        (
                            "DictionaryUtility::MergeWithConflicts: "
                            + "dictionary is null"
                        );

                    throw new ArgumentNullException("dictionaries");
                }
            }

            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            for (int i = 0; i < dictionaries.Length; i++)
            {
                Dictionary<TKey, TValue> dic = dictionaries[i];
                foreach (KeyValuePair<TKey, TValue> pair in dic)
                {
                    result.Add(pair.Key, pair.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Merges the specified dictionaries.
        /// </summary>
        /// <param name="dictionaries">Dictionaries to merge.</param>
        /// <returns>Merged dictionary.</returns>
        /// <exception cref="ArgumentNullException">
        /// One or more dictionaries is <c>null</c>.
        /// </exception>
        public static Dictionary<TKey, TValue> MergeFirstValues<TKey, TValue>
            (
                params Dictionary<TKey, TValue>[] dictionaries
            )
        {
            foreach (Dictionary<TKey, TValue> dictionary in dictionaries)
            {
                if (ReferenceEquals(dictionary, null))
                {
                    Log.Error
                        (
                            "DictionaryUtility::MergeWithoutConflicts: "
                            + "dictionary is null"
                        );

                    throw new ArgumentNullException("dictionaries");
                }
            }

            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            for (int i = 0; i < dictionaries.Length; i++)
            {
                Dictionary<TKey, TValue> dic = dictionaries[i];
                foreach (KeyValuePair<TKey, TValue> pair in dic)
                {
                    if (!result.ContainsKey(pair.Key))
                    {
                        result.Add(pair.Key, pair.Value);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Merges the specified dictionaries.
        /// </summary>
        /// <param name="dictionaries">Dictionaries to merge.</param>
        /// <returns>Merged dictionary.</returns>
        /// <exception cref="ArgumentNullException">
        /// One or more dictionaries is <c>null</c>.
        /// </exception>
        public static Dictionary<TKey, TValue> MergeLastValues<TKey, TValue>
            (
                params Dictionary<TKey, TValue>[] dictionaries
            )
        {
            foreach (Dictionary<TKey, TValue> dictionary in dictionaries)
            {
                if (ReferenceEquals(dictionary, null))
                {
                    Log.Error
                        (
                            "DictionaryUtility::MergeLastValues: "
                            + "dictionary is null"
                        );

                    throw new ArgumentNullException("dictionaries");
                }
            }

            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            for (int i = 0; i < dictionaries.Length; i++)
            {
                Dictionary<TKey, TValue> dic = dictionaries[i];
                foreach (KeyValuePair<TKey, TValue> pair in dic)
                {
                    result[pair.Key] = pair.Value;
                }
            }
            return result;
        }

        #endregion
    }
}
