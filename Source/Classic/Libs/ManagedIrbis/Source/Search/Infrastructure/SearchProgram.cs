// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
        : ISearchTree
    {
        #region Properties

        /// <summary>
        /// Program entry point - root of syntax tree.
        /// </summary>
        [CanBeNull]
        internal SearchLevel6 EntryPoint { get; set; }

        #endregion

        #region ISearchTree members

        ISearchTree[] ISearchTree.Children
        {
            get
            {
                ISearchTree[] result = ReferenceEquals(EntryPoint, null)
                    ? new ISearchTree[0]
                    : EntryPoint.Children;

                return result;
            }
        }

        string ISearchTree.Value { get { return null; } }

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
