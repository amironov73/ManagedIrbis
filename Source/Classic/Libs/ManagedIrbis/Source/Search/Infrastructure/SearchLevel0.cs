// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchLevel0.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// Term, reference or parenthesis.
    /// </summary>
    sealed class SearchLevel0
        : ISearchTree
    {
        #region Properties

        /// <summary>
        /// Term.
        /// </summary>
        [CanBeNull]
        public SearchTerm Term { get; set; }

        /// <summary>
        /// Reference.
        /// </summary>
        [CanBeNull]
        public SearchReference Reference { get; set; }

        /// <summary>
        /// Parenthesis.
        /// </summary>
        [CanBeNull]
        public SearchLevel7 Parenthesis { get; set; }

        #endregion

        #region ISearchTree members

        public ISearchTree[] Children
        {
            get
            {
                if (!ReferenceEquals(Term, null))
                {
                    return new ISearchTree[] { Term };
                }

                if (!ReferenceEquals(Reference, null))
                {
                    return new ISearchTree[] { Reference };
                }

                return new ISearchTree[] { Parenthesis };
            }
        }

        public string Value
        {
            get { return null; }
        }

        /// <inheritdoc cref="ISearchTree.Find"/>
        public TermLink[] Find
            (
                SearchContext context
            )
        {
            TermLink[] result;

            if (!ReferenceEquals(Term, null))
            {
                result = Term.Find(context);
            }
            else if (!ReferenceEquals(Reference, null))
            {
                result = Reference.Find(context);
            }
            else if (!ReferenceEquals(Parenthesis, null))
            {
                result = Parenthesis.Find(context);
            }
            else
            {
                throw new IrbisException("Unexpected SearchLevel0");
            }

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return
                (
                    Term
                    ?? (object)Reference
                    ?? Parenthesis
                )
                .NullableToVisibleString();
        }

        #endregion
    }
}
