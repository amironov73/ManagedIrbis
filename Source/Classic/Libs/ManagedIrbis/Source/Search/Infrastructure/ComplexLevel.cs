// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ComplexLevel.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Collections;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    class ComplexLevel<T>
        : ISearchTree
        where T: class, ISearchTree
    {
        #region Properties

        /// <summary>
        /// Is complex expression?
        /// </summary>
        public bool IsComplex
        {
            get
            {
                return Items.Count > 1;
            }
        }

        /// <summary>
        /// Item separator.
        /// </summary>
        [CanBeNull]
        public string Separator { get; private set; }

        /// <summary>
        /// Items.
        /// </summary>
        [NotNull]
        public NonNullCollection<T> Items { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ComplexLevel
            (
                [CanBeNull] string separator
            )
        {
            Separator = separator;
            Items = new NonNullCollection<T>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add item.
        /// </summary>
        [NotNull]
        public ComplexLevel<T> AddItem
            (
                [NotNull] T item
            )
        {
            Code.NotNull(item, "item");

            Items.Add(item);

            return this;
        }

        #endregion

        #region ISearchTree members

        /// <inheritdoc cref="ISearchTree.Parent"/>
        public ISearchTree Parent { get; set; }

        /// <inheritdoc cref="ISearchTree.Children" />
        public ISearchTree[] Children
        {
            // ReSharper disable CoVariantArrayConversion
            get { return Items.ToArray(); }
            // ReSharper restore CoVariantArrayConversion
        }

        /// <inheritdoc cref="ISearchTree.Value" />
        public string Value { get { return null; } }

        /// <inheritdoc cref="ISearchTree.Find"/>
        public virtual TermLink[] Find
            (
                SearchContext context
            )
        {
            return TermLink.EmptyArray;
        }

        /// <inheritdoc cref="ISearchTree.ReplaceChild"/>
        public void ReplaceChild
            (
                ISearchTree fromChild,
                ISearchTree toChild
            )
        {
            Code.NotNull(fromChild, "fromChild");

            T item = (T) fromChild;

            int index = Items.IndexOf(item);
            if (index < 0)
            {
                Log.Error
                    (
                        "ComplexLevel::ReplaceChild: "
                        + "child not found: "
                        + fromChild.ToVisibleString()
                    );

                throw new KeyNotFoundException();
            }

            if (ReferenceEquals(toChild, null))
            {
                Items.RemoveAt(index);
            }
            else
            {
                Items[index] = item;
                toChild.Parent = this;
            }

            fromChild.Parent = this;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            string result = StringUtility.Join(Separator, Items);

            if (IsComplex)
            {
                result = " ( " + result + " ) ";
            }

            return result;
        }

        #endregion
    }
}
