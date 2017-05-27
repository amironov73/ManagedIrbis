// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* VirtualChildren.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Virtual children for <see cref="PftNode"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class VirtualChildren
        : IList<PftNode>
    {
        #region Events

        /// <summary>
        /// Fired on <see cref="GetEnumerator()"/> call.
        /// </summary>
        public event EventHandler Enumeration;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public VirtualChildren()
        {
            _children = new PftNode[0];
        }

        #endregion

        #region Private members

        private PftNode[] _children;

        #endregion

        #region Public methods

        /// <summary>
        /// Set children array.
        /// </summary>
        public void SetChildren
            (
                [NotNull] IEnumerable<PftNode> children
            )
        {
            Code.NotNull(children, "children");

            _children = children.ToArray();
        }

        #endregion

        #region IList<PftNode> members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<PftNode> GetEnumerator()
        {
            Enumeration.Raise(this);

            foreach (PftNode node in _children)
            {
                yield return node;
            }
        }

        /// <inheritdoc cref="ICollection{T}.Add" />
        public void Add
            (
                PftNode item
            )
        {
            Log.Error
                (
                    "VirtualChildren::Add: "
                    + "not applicable"
                );

            throw new NotSupportedException();
        }

        /// <inheritdoc cref="ICollection{T}.Clear" />
        public void Clear()
        {
            Log.Error
                (
                    "VirtualChildren::Clear: "
                    + "not applicable"
                );

            throw new NotSupportedException();
        }

        /// <inheritdoc cref="ICollection{T}.Contains" />
        public bool Contains
            (
                PftNode item
            )
        {
            return _children.Contains(item);
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo" />
        public void CopyTo
            (
                PftNode[] array,
                int arrayIndex
            )
        {
            _children.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc cref="ICollection{T}.Remove" />
        public bool Remove
            (
                PftNode item
            )
        {
            Log.Error
                (
                    "VirtualChildren::Remove: "
                    + "not applicable"
                );

            throw new NotSupportedException();
        }


        /// <inheritdoc cref="ICollection{T}.Count" />
        public int Count
        {
            get { return _children.Length; }
        }

        /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <inheritdoc cref="IList{T}.IndexOf" />
        public int IndexOf
            (
                PftNode item
            )
        {
            return Array.IndexOf(_children, item);
        }

        /// <inheritdoc cref="IList{T}.Insert" />
        public void Insert
            (
                int index,
                PftNode item
            )
        {
            Log.Error
                (
                    "VirtualChildren::Insert: "
                    + "not applicable"
                );

            throw new NotSupportedException();
        }

        /// <inheritdoc cref="IList{T}.RemoveAt" />
        public void RemoveAt
            (
                int index
            )
        {
            Log.Error
                (
                    "VirtualChildren::RemoveAt: "
                    + "not applicable"
                );

            throw new NotSupportedException();
        }

        /// <inheritdoc cref="IList{T}.this" />
        public PftNode this[int index]
        {
            get
            {
                return _children[index];
            }
            set
            {
                Log.Error
                    (
                        "VirtualList::Indexer: "
                        + "set value="
                        + value.NullableToVisibleString()
                    );

                throw new NotSupportedException();
            }
        }

        #endregion
    }
}
