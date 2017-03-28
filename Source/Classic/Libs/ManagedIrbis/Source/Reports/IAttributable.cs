// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IAttributable.cs -- object with attributes
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Object with attributes.
    /// </summary>
    public interface IAttributable
    {
        /// <summary>
        /// Attributes.
        /// </summary>
        ReportAttributes Attributes { get; }
    }
}
