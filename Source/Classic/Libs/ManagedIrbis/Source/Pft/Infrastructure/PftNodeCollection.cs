// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftNodeCollection.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.ObjectModel;

using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftNodeCollection
        : NonNullCollection<PftNode>
    {
        #region Properties

        /// <summary>
        /// Parent node.
        /// </summary>
        [CanBeNull]
        public PftNode Parent { get; private set; }

        #endregion

        #region Construction

        ///// <summary>
        ///// Constructor.
        ///// </summary>
        //public PftNodeCollection()
        //{
        //}

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNodeCollection
            (
                [CanBeNull] PftNode parent
            )
        {
            Parent = parent;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Collection<T> members

        /// <inheritdoc cref="Collection{T}.ClearItems" />
        protected override void ClearItems()
        {
            foreach (PftNode node in this)
            {
                node.Parent = null;
            }

            base.ClearItems();
        }

        /// <inheritdoc cref="Collection{T}.InsertItem" />
        protected override void InsertItem
            (
                int index,
                PftNode item
            )
        {
            Code.NotNull(item, "item");

            if (!ReferenceEquals(item.Parent, null))
            {
                if (!ReferenceEquals(item.Parent, Parent))
                {
                    throw new IrbisException();
                }
            }

            item.Parent = Parent;
            base.InsertItem(index, item);
        }

        /// <inheritdoc cref="Collection{T}.SetItem" />
        protected override void SetItem
            (
                int index,
                PftNode item
            )
        {
            Code.NotNull(item, "item");

            if (!ReferenceEquals(item.Parent, null))
            {
                if (!ReferenceEquals(item.Parent, Parent))
                {
                    throw new IrbisException();
                }
            }

            item.Parent = Parent;
            base.SetItem(index, item);
        }

        /// <inheritdoc cref="Collection{T}.RemoveItem" />
        protected override void RemoveItem
            (
                int index
            )
        {
            PftNode node = this[index];
            node.Parent = null;

            base.RemoveItem(index);
        }

        #endregion

        #region Object members

        #endregion
    }
}
