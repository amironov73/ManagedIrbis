// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MethodResult.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// Result of external method call.
    /// </summary>
    [ExcludeFromCodeCoverage]
    sealed class MethodResult
    {
        #region Properties

        /// <summary>
        /// Return code.
        /// </summary>
        public int ReturnCode { get; set; }

        /// <summary>
        /// Input.
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Output.
        /// </summary>
        public string Output { get; set; }

        #endregion
    }
}
