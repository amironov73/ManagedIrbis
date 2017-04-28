// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServiceRepository.cs --
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

namespace AM.IOC
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ServiceRepository
        : IDisposable
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ServiceRepository()
        {
            Log.Trace("ServiceRepository::Constructor");

            _lock = new object();
            _dictionary = new Dictionary<Type, object>();
        }

        #endregion

        #region Private members

        private readonly object _lock;

        private readonly Dictionary<Type, object> _dictionary;

        #endregion

        #region Public methods

        /// <summary>
        /// Clear the repository.
        /// </summary>
        public ServiceRepository Clear()
        {
            Log.Trace("ServiceRepository::Clear");

            lock (_lock)
            {
                _dictionary.Clear();
            }

            return this;
        }

        /// <summary>
        /// Register service of given type.
        /// </summary>
        [NotNull]
        public ServiceRepository Register
            (
                [NotNull] Type type,
                [NotNull] object service
            )
        {
            Code.NotNull(type, "type");
            Code.NotNull(service, "service");

            Log.Trace("ServiceRepository::Register");

#if !NETCORE
            if (type.IsValueType)
            {
                throw new ArsMagnaException("type.IsValueType");
            }

            if (!type.IsInstanceOfType(service))
            {
                throw new ArsMagnaException
                    (
                        "!type.IsInstanceOfType"
                    );
            }
#endif

            lock (_lock)
            {
                _dictionary[type] = service;
            }

            return this;
        }

        /// <summary>
        /// Register service of given type.
        /// </summary>
        [NotNull]
        public ServiceRepository Register<T>
            (
                [NotNull] T service
            )
            where T: class
        {
            return Register(typeof(T), service);
        }

        /// <summary>
        /// Unregister service of given type.
        /// </summary>
        [NotNull]
        public ServiceRepository Unregister
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            Log.Trace("ServiceRepository::Unregister");

            lock (_lock)
            {
                _dictionary.Remove(type);
            }

            return this;
        }

        /// <summary>
        /// Unregister service of given type.
        /// </summary>
        [NotNull]
        public ServiceRepository Unregister<T>()
            where T: class
        {
            return Unregister(typeof(T));
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Log.Trace("ServiceRepository::Dispose");

            lock (_lock)
            {
                foreach (var pair in _dictionary)
                {
                    IDisposable service 
                        = pair.Value as IDisposable;
                    if (!ReferenceEquals(service, null))
                    {
                        service.Dispose();
                    }
                }
            }
        }

        #endregion
    }
}
