/* TreeNode.cs -- generic tree node
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// Generic tree node.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class TreeNode<T>
    {
        #region Properties

        /// <summary>
        /// Children nodes.
        /// </summary>
        [NotNull]
        public NonNullCollection<TreeNode<T>> Children
        {
            get;
            private set;
        }

        /// <summary>
        /// User defined value.
        /// </summary>
        public T Value { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TreeNode()
        {
            Children = new NonNullCollection<TreeNode<T>>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TreeNode
            (
                T value
            )
            : this()
        {
            Value = value;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Add child with a specified value.
        /// </summary>
        public TreeNode<T> AddChild
            (
                T value
            )
        {
            TreeNode<T> child = new TreeNode<T>(value);
            Children.Add(child);

            return child;
        }

        /// <summary>
        /// Get descendants of the node.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public NonNullCollection<TreeNode<T>> GetDescendants()
        {
            NonNullCollection<TreeNode<T>> result
                = new NonNullCollection<TreeNode<T>>();

            foreach (TreeNode<T> child in Children)
            {
                result.Add(child);
                result.AddRange(child.GetDescendants());
            }

            return result;
        }

        /// <summary>
        /// Walk through the tree starting the current node.
        /// </summary>
        public void Walk
            (
                [NotNull] Action<TreeNode<T>> action
            )
        {
            Code.NotNull(action, "action");

            action(this);
            foreach (TreeNode<T> child in Children)
            {
                child.Walk(action);
            }
        }

        #endregion
    }
}
