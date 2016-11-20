/* PftAst.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion


namespace IrbisUI
{
    /// <summary>
    /// TreeView over <see cref="PftNode"/>'s
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class PftTreeView
        : UserControl
    {
        #region Events

        /// <summary>
        /// Fired when current node changed.
        /// </summary>
        public event EventHandler<TreeViewEventArgs> CurrentNodeChanged;

        /// <summary>
        /// Fired when node check state changed.
        /// </summary>
        public event EventHandler<TreeViewEventArgs> NodeChecked;

        #endregion

        #region Properties

        /// <summary>
        /// Current node.
        /// </summary>
        [CanBeNull]
        public PftNodeInfo CurrentNode
        {
            get
            {
                TreeNode currentNode = _tree.SelectedNode;
                if (ReferenceEquals(currentNode, null))
                {
                    return null;
                }

                PftNodeInfo result = currentNode.Tag as PftNodeInfo;

                return result;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Construction.
        /// </summary>
        public PftTreeView()
        {
            InitializeComponent();

            _tree.AfterSelect += _tree_AfterSelect;
            _tree.AfterCheck += _tree_AfterCheck;
        }

        #endregion

        #region Private members

        private static TreeNode _ConvertNode
            (
                PftNodeInfo info
            )
        {
            string text = info.ToString();

            TreeNode result = new TreeNode
            {
                Tag = info,
                Text = text,
                ToolTipText = text
            };

            foreach (PftNodeInfo child in info.Children)
            {
                if (!ReferenceEquals(child, null))
                {
                    TreeNode node = _ConvertNode(child);
                    if (!ReferenceEquals(node, null))
                    {
                        result.Nodes.Add(node);
                    }
                }
            }

            return result;
        }

        private void _tree_AfterCheck
            (
                object sender,
                TreeViewEventArgs e
            )
        {
            NodeChecked.Raise(sender, e);
        }

        void _tree_AfterSelect
            (
                object sender,
                TreeViewEventArgs e
            )
        {
            CurrentNodeChanged.Raise(sender, e);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clear.
        /// </summary>
        public void Clear()
        {
            _tree.Nodes.Clear();
        }

        /// <summary>
        /// Set nodes.
        /// </summary>
        public void SetNodes
            (
                [NotNull] PftNode rootNode
            )
        {
            Code.NotNull(rootNode, "rootNode");

            try
            {
                _tree.BeginUpdate();
                _tree.Nodes.Clear();
                PftNodeInfo rootInfo = rootNode.GetNodeInfo();
                TreeNode treeNode = _ConvertNode(rootInfo);
                _tree.Nodes.Add(treeNode);
                _tree.ExpandAll();
                _tree.SelectedNode = treeNode;
                treeNode.EnsureVisible();
            }
            finally
            {
                _tree.EndUpdate();
            }
        }

        #endregion
    }
}

