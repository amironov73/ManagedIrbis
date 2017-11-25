// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConnectionFactory.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ConnectionFactory
    {
        #region Properties

        /// <summary>
        /// Creates <see cref="IrbisConnection"/>
        /// from the connection string.
        /// </summary>
        [NotNull]
        public static Func<string, IIrbisConnection> ConnectionCreator { get; set; }

        #endregion

        #region Construction

        static ConnectionFactory()
        {
            ConnectionCreator = _ConnectionCreator;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create <see cref="IIrbisConnection"/>
        /// from given connection string.
        /// </summary>
        [NotNull]
        public static IIrbisConnection CreateConnection
            (
                [NotNull] string connectionString
            )
        {
            Code.NotNull(connectionString, "connectionString");

            if (ReferenceEquals(ConnectionCreator, null))
            {
                throw new IrbisException("ConnectionCreator not set");
            }
            IIrbisConnection result = ConnectionCreator(connectionString);

            return result;
        }

        #endregion

        #region Private members

        [NotNull]
        private static IIrbisConnection _ConnectionCreator
            (
                [NotNull] string connectionString
            )
        {
            Code.NotNull(connectionString, "connectionString");

            IIrbisConnection result = new IrbisConnection(connectionString);

            return result;
        }

        #endregion
    }
}
