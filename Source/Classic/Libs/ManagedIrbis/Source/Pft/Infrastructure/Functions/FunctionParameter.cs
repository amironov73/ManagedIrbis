// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FunctionParameter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Function parameter.
    /// </summary>
    public enum FunctionParameter
    {
        /// <summary>
        /// String.
        /// </summary>
        String = (int)'s',

        /// <summary>
        /// Required string.
        /// </summary>
        RequiredString = (int)'S',

        /// <summary>
        /// Numeric.
        /// </summary>
        Numeric = (int)'n',

        /// <summary>
        /// Required numeric.
        /// </summary>
        RequiredNumeric = (int)'N',

        /// <summary>
        /// Boolean.
        /// </summary>
        Boolean = (int)'b',

        /// <summary>
        /// Required boolean.
        /// </summary>
        RequiredBoolean = (int)'B'
    }
}
