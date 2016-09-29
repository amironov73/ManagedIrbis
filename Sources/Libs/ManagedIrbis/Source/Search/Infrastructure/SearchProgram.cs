/* SearchProgram.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// Root of the syntax tree.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SearchProgram
    {
        #region Properties

        /// <summary>
        /// Program entry point - root of syntax tree..
        /// </summary>
        [CanBeNull]
        internal SearchLevel6 EntryPoint { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            if (ReferenceEquals(EntryPoint, null))
            {
                return string.Empty;
            }

            string result = EntryPoint.ToString()
                .Trim();

            return result;
        }

        #endregion
    }
}
