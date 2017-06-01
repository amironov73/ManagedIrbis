// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ProviderManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using AM.Configuration;
using AM.Logging;
using AM.Parameters;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ProviderManager
    {
        #region Constants

        /// <summary>
        /// Connected client (IrbisConnection).
        /// </summary>
        public const string Connected = "Connected";

        /// <summary>
        /// Local provider.
        /// </summary>
        public const string Local = "Local";

        /// <summary>
        /// Null provider.
        /// </summary>
        public const string Null = "Null";

        #endregion

        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        [NotNull]
        public static Dictionary<string, Type> Registry
        {
            get; private set;
        }

        #endregion

        #region Construction

        static ProviderManager()
        {
            Registry = new Dictionary<string, Type>
            {
                {Null, typeof(NullProvider)},
                {Local, typeof(LocalProvider)},
                {Connected, typeof(ConnectedClient)}
            };
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get <see cref="IrbisProvider" /> and configure it.
        /// </summary>
        [NotNull]
        public static IrbisProvider GetAndConfigureProvider
            (
                [NotNull] string configurationString
            )
        {
            Code.NotNullNorEmpty(configurationString, "configurationString" );

            Parameter[] parameters = ParameterUtility.ParseString
                (
                    configurationString
                );
            string name = parameters.GetParameter("Provider", null);
            if (string.IsNullOrEmpty(name))
            {
                Log.Error
                    (
                        "ProviderManager::GetAndConfigureProvider: "
                        + "provider name not specified"
                    );

                throw new IrbisException
                    (
                        "Provider name not specified"
                    );
            }

            IrbisProvider result = GetProvider(name, true)
                .ThrowIfNull();
            result.Configure(configurationString);

            return result;
        }

        /// <summary>
        /// Get <see cref="IrbisProvider"/> by name.
        /// </summary>
        [CanBeNull]
        public static IrbisProvider GetProvider
            (
                [NotNull] string name,
                bool throwOnError
            )
        {
            Code.NotNull(name, "name");

            Type type;
            if (!Registry.TryGetValue(name, out type))
            {
                Log.Error
                    (
                        "ProviderManager::GetProvider: "
                        + "provider not found: "
                        + name
                    );

                if (throwOnError)
                {
                    throw new IrbisException
                        (
                            "Provider not found: " + name
                        );
                }
            }

            if (ReferenceEquals(type, null))
            {
                Log.Error
                    (
                        "ProviderManager::GetProvider: "
                        + "can't find type: "
                        + name
                    );

                throw new IrbisException
                    (
                        "Can't find type: " + name
                    );
            }

            IrbisProvider result
                = (IrbisProvider)Activator.CreateInstance(type);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public static IrbisProvider GetPreconfiguredProvider()
        {
            string configurationString = ConfigurationUtility.GetString
                (
                    "IrbisProvider"
                );
            if (string.IsNullOrEmpty(configurationString))
            {
                Log.Error
                    (
                        "ProviderManager::GetPreconfiguredProvider: "
                        + "IrbisProvider configuration key not specified"
                    );

                throw new IrbisException
                    (
                        "IrbisProvider configuration key not specified"
                    );
            }

            IrbisProvider result
                = GetAndConfigureProvider(configurationString);

            return result;
        }

        #endregion
    }
}
