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
                if (throwOnError)
                {
                    throw new IrbisException
                        (
                            "Provider not found: " + name
                        );
                }
            }

            IrbisProvider result
                = (IrbisProvider)Activator.CreateInstance(type);

            return result;
        }

        #endregion
    }
}
