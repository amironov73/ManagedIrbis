// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ComplexLevel.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Linq;
using AM;
using AM.Collections;
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

        public ISearchTree[] Children
        {
            // ReSharper disable CoVariantArrayConversion
            get { return Items.ToArray(); }
            // ReSharper restore CoVariantArrayConversion
        }

        public string Value { get { return null; } }

        public int[] Find
            (
                IrbisProvider provider
            )
        {
            return new int[0];
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
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
