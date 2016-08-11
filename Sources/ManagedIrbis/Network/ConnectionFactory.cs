/* ConnectionFactory.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network
{
    /// <summary>
    /// Connection factory.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ConnectionFactory
    {
        #region Properties

        /// <summary>
        /// Default factory.
        /// </summary>
        [NotNull]
        public static ConnectionFactory Default
        {
            get { return _default; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ConnectionFactory()
        {
            _default = new ConnectionFactory();
        }

        #endregion

        #region Private members

        private static ConnectionFactory _default;

        #endregion

        #region Public methods

        /// <summary>
        /// Get <see cref="IrbisConnection"/>
        /// </summary>
        [NotNull]
        public virtual IrbisConnection GetConnection()
        {
            return new IrbisConnection();
        }

        /// <summary>
        /// Set default factory.
        /// </summary>
        [NotNull]
        public static ConnectionFactory SetDefaultFactory
            (
                [NotNull] ConnectionFactory factory
            )
        {
            Code.NotNull(factory, "factory");

            ConnectionFactory result = Default;
            _default = factory;

            return result;
        }

        #endregion
    }
}
