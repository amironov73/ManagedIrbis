// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Singleton.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC || NETCORE

#region Using directives

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

#endregion

namespace AM.Runtime
{
    /// <summary>
    /// Хранилище объектов по принципу "каждого типа по одной штуке".
    /// </summary>
    [PublicAPI]
    public static class Singleton
    {
        #region Private members

        private static Dictionary<Type, object> _dictionary
            = new Dictionary<Type, object>();

        #endregion

        #region Public methods

        /// <summary>
        /// Clear the dictionary.
        /// </summary>
        public static void Clear()
        {
            lock (_dictionary)
            {
                _dictionary.Clear();
            }
        }

        /// <summary>
        /// Do we have instance of specified type?
        /// </summary>
        [Pure]
        public static bool HaveInstance<T>()
            where T : class
        {
            Type type = typeof(T);

            lock (_dictionary)
            {
                return _dictionary.ContainsKey(type);
            }
        }

        /// <summary>
        /// Get the instance of the specified object type.
        /// </summary>
        public static T Instance<T>()
                where T : class
        {
            T result;
            Type type = typeof(T);

            lock (_dictionary)
            {
                object value;
                if (_dictionary.TryGetValue(type, out value))
                {
                    result = (T)value;
                }
                else
                {
                    result = Activator.CreateInstance<T>();
                    _dictionary[type] = result;
                }
            }

            return result;
        }

        /// <summary>
        /// Remove the instance of the specified object type, if any.
        /// </summary>
        public static bool RemoveInstance<T>()
            where T : class
        {
            Type type = typeof(T);

            lock (_dictionary)
            {
                object value;
                if (_dictionary.TryGetValue(type, out value))
                {
                    IDisposable disposable = value as IDisposable;
                    if (!ReferenceEquals(disposable, null))
                    {
                        disposable.Dispose();
                    }
                }

                return _dictionary.Remove(type);
            }
        }

        #endregion
    }
}

#endif
