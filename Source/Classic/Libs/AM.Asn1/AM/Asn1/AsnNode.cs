// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsnNode.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Asn1
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public class AsnNode
    {
        #region Properties

        /// <summary>
        /// Parent node.
        /// </summary>
        [CanBeNull]
        public AsnNode Parent { get; internal set; }

        /// <summary>
        /// Breakpoint.
        /// </summary>
        public bool Breakpoint { get; set; }

        /// <summary>
        /// Список потомков. Может быть пустым.
        /// </summary>
        public virtual IList<AsnNode> Children
        {
            get
            {
                if (ReferenceEquals(_children, null))
                {
                    _children = new AsnNodeCollection(this);
                }

                return _children;
            }
            protected set
            {
                AsnNodeCollection collection = (AsnNodeCollection)value;
                collection.Parent = this;
                collection.EnsureParent();
                _children = collection;
            }
        }

        /// <summary>
        /// Column number.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Номер строки, на которой в скрипте расположена
        /// соответствующая конструкция языка.
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        [CanBeNull]
        public virtual string Text { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnNode()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnNode
            (
                [NotNull] AsnToken token
            )
        {
            Code.NotNull(token, "token");

            LineNumber = token.Line;
            Column = token.Column;
            Text = token.Text;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnNode
            (
                params AsnNode[] children
            )
        {
            foreach (AsnNode child in children)
            {
                Children.Add(child);
            }
        }

        #endregion

        #region Private members

        private AsnNodeCollection _children;

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object other
            )
        {
            return ReferenceEquals(this, other);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Column;
                hashCode = (hashCode * 397) ^ LineNumber;
                hashCode = (hashCode * 397) ^
                    (
                        Text != null
                        ? Text.GetHashCode()
                        : 0
                    );

                return hashCode;
            }
        }

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (AsnNode child in Children)
            {
                result.Append(child);
            }

            return result.ToString();
        }

        #endregion
    }
}
