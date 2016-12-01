// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Singleton.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC

#region Using directives

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Runtime
{
    /// <summary>
    /// Хранилище объектов по принципу "каждого типа по одной штуке".
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class Singleton
    {
        #region Private members

        private static Dictionary<Type, object> _dictionary
            = new Dictionary<Type, object>();

        #endregion

        #region Public methods

        /// <summary>
        /// Do we have instance of specified type?
        /// </summary>
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
        /// Выдает объект указанного типа.
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
                }
            }

            return result;
        }

        #endregion
    }
}

#endif

