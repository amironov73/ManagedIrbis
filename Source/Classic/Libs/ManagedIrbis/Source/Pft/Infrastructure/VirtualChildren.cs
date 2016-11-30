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

        /// <inheritdoc />
        public IEnumerator<PftNode> GetEnumerator()
        {
            Enumeration.Raise(this);

            foreach (PftNode node in _children)
            {
                yield return node;
            }
        }

        /// <inheritdoc />
        public void Add(PftNode item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool Contains
            (
                PftNode item
            )
        {
            return _children.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo
            (
                PftNode[] array,
                int arrayIndex
            )
        {
            _children.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove
            (
                PftNode item
            )
        {
            throw new NotImplementedException();
        }


        /// <inheritdoc />
        public int Count { get { return _children.Length; } }

        /// <inheritdoc />
        public bool IsReadOnly { get { return true; } }

        /// <inheritdoc />
        public int IndexOf
            (
                PftNode item
            )
        {
            return Array.IndexOf(_children, item);
        }

        /// <inheritdoc />
        public void Insert
            (
                int index,
                PftNode item
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void RemoveAt
            (
                int index
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public PftNode this[int index]
        {
            get { return _children[index]; }
            set { throw new NotImplementedException(); }
        }

        #endregion
    }
}
