// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchLevel0.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Logging;

using CodeJam;

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

        /// <inheritdoc cref="ISearchTree.Parent"/>
        public ISearchTree Parent { get; set; }

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

        /// <inheritdoc cref="ISearchTree.Children" />
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

        /// <inheritdoc cref="ISearchTree.Value" />
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
                Log.Error
                    (
                        "SearchLevel0::Find: "
                        + "unexpected situation"
                    );

                throw new IrbisException("Unexpected SearchLevel0");
            }

            return result;
        }

        /// <inheritdoc cref="ISearchTree.ReplaceChild"/>
        public void ReplaceChild
            (
                ISearchTree fromChild,
                ISearchTree toChild
            )
        {
            Code.NotNull(fromChild, "fromChild");

            fromChild.Parent = null;

            SearchTerm term = fromChild as SearchTerm;
            if (!ReferenceEquals(term, null))
            {
                Term = (SearchTerm) toChild;
            }

            SearchReference reference = fromChild as SearchReference;
            if (!ReferenceEquals(reference, null))
            {
                Reference = (SearchReference) toChild;
            }

            SearchLevel7 level7 = fromChild as SearchLevel7;
            if (!ReferenceEquals(level7, null))
            {
                Parenthesis = (SearchLevel7) toChild;
            }

            if (!ReferenceEquals(toChild, null))
            {
                toChild.Parent = this;
            }
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
                .ToVisibleString();
        }

        #endregion
    }
}
