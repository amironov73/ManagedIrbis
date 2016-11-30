// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StandardDatabases.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
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
