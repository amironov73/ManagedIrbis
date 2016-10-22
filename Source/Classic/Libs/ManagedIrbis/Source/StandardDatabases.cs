/* StandardDatabases.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Стандартные базы данных, входящие
    /// в дистрибутив ИРБИС64.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class StandardDatabases
    {
        #region Constants

        /// <summary>
        /// Электронный каталог.
        /// </summary>
        public const string ElectronicCatalog = "IBIS";

        /// <summary>
        /// Комплектование.
        /// </summary>
        public const string Acquisition = "CMPL";

        /// <summary>
        /// Читатели.
        /// </summary>
        public const string Readers = "RDR";

        /// <summary>
        /// Заказы на литературу.
        /// </summary>
        public const string Requests = "RQST";

        #endregion
    }
}
