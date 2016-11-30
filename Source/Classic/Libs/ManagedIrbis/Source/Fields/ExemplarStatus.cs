// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExemplarStatus.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Status of book exemplar.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ExemplarStatus
    {
        #region Constants

        /// <summary>
        /// Free.
        /// </summary>
        public const string Free = "0";

        /// <summary>
        /// Loan.
        /// </summary>
        public const string Loan = "1";

        /// <summary>
        /// Wait.
        /// </summary>
        public const string Wait = "2";

        /// <summary>
        /// In bindery.
        /// </summary>
        public const string Bindery = "3";

        /// <summary>
        /// Lost.
        /// </summary>
        public const string Lost = "4";

        /// <summary>
        /// Not available.
        /// </summary>
        public const string NotAvailable = "5";

        /// <summary>
        /// Written off.
        /// </summary>
        public const string WrittenOff = "6";

        /// <summary>
        /// On the way.
        /// </summary>
        public const string OnTheWay = "8";

        /// <summary>
        /// Reserved.
        /// </summary>
        public const string Reserved = "9";

        /// <summary>
        /// BiblioNet.
        /// </summary>
        public const string BiblioNet = "C";

        /// <summary>
        /// Bound.
        /// </summary>
        public const string Bound = "P";

        /// <summary>
        /// Reproduction.
        /// </summary>
        public const string Reproduction = "R";

        /// <summary>
        /// Summary.
        /// </summary>
        public const string Summary = "U";

        #endregion
    }
}
