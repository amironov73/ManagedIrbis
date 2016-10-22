using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class TreeNodeTest
    {
        [TestMethod]
        public void TreeNode_Construction()
        {
            TreeNode<char> node = new TreeNode<char>('a');
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual('a', node.Value);
        }

        [TestMethod]
        public void TreeNode_AddChild()
        {
            TreeNode<char> root = new TreeNode<char>('a');
            TreeNode<char> level1 = root.AddChild('b');
            TreeNode<char> level2 = level1.AddChild('c');
            TreeNode<char> level3 = level2.AddChild('d');
            Assert.AreEqual(1, root.Children.Count);
            Assert.AreEqual(1, level1.Children.Count);
            Assert.AreEqual(1, level2.Children.Count);
            Assert.AreEqual(0, level3.Children.Count);
        }

        [TestMethod]
        public void TreeNode_GetDescendants()
        {
            TreeNode<char> root = new TreeNode<char>('a');
            TreeNode<char> level1 = root.AddChild('b');
            TreeNode<char> level2 = level1.AddChild('c');
            TreeNode<char> level3 = level2.AddChild('d');

            NonNullCollection<TreeNode<char>> descendants
                = root.GetDescendants();
            Assert.AreEqual(3, descendants.Count);
            Assert.IsTrue(descendants.Contains(level1));
            Assert.IsTrue(descendants.Contains(level2));
            Assert.IsTrue(descendants.Contains(level3));
        }

        [TestMethod]
        public void TreeNode_Walk()
        {
            TreeNode<char> root = new TreeNode<char>('a');
            TreeNode<char> level1 = root.AddChild('b');
            TreeNode<char> level2 = level1.AddChild('c');
            TreeNode<char> level3 = level2.AddChild('d');

            StringBuilder builder = new StringBuilder();
            root.Walk(node => builder.Append(node.Value));
            Assert.AreEqual("abcd", builder.ToString());
        }
    }
}
