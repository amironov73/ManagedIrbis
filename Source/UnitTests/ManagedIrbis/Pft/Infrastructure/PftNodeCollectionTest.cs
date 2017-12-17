using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM.Collections;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Walking;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftNodeCollectionTest
    {
        class MyNode
            : PftNode
        {
            public override PftNode Optimize()
            {
                return new PftC();
            }
        }

        [TestMethod]
        public void PftNodeCollection_Construction_1()
        {
            PftNode node = new PftNode();
            PftNodeCollection collection = new PftNodeCollection(node);
            Assert.AreSame(node, collection.Parent);
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void PftNodeCollection_Construction_2()
        {
            PftNode node = null;
            PftNodeCollection collection = new PftNodeCollection(node);
            Assert.AreSame(node, collection.Parent);
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void PftNodeCollection_Optimize_1()
        {
            PftNodeCollection collection = new PftNodeCollection(null)
            {
                new PftComma()
            };
            collection.Optimize();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void PftNodeCollection_Optimize_2()
        {
            PftNodeCollection collection = new PftNodeCollection(null)
            {
                new MyNode()
            };
            collection.Optimize();
            Assert.AreEqual(1, collection.Count);
            Assert.IsInstanceOfType(collection[0], typeof(PftC));
        }

        [TestMethod]
        public void PftNode_ClearItems_1()
        {
            PftNode parent = new PftNode();
            PftNodeCollection collection = new PftNodeCollection(parent);
            PftNode child = new PftNode();
            collection.Add(child);
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
            Assert.IsNull(child.Parent);
        }

        [TestMethod]
        public void PftNode_InsertItem_1()
        {
            PftNode parent = new PftNode();
            PftNodeCollection collection = new PftNodeCollection(parent);
            PftNode child = new PftNode();
            collection.Insert(0, child);
            Assert.AreSame(parent, child.Parent);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftNode_InsertItem_2()
        {
            PftNode parent1 = new PftNode();
            PftNode parent2 = new PftNode();
            PftNodeCollection collection = new PftNodeCollection(parent1);
            PftNode child = new PftNode();
            parent2.Children.Add(child);
            collection.Insert(0, child);
            Assert.AreSame(parent1, child.Parent);
        }

        [TestMethod]
        public void PftNode_SetItem_1()
        {
            PftNode parent = new PftNode();
            PftNodeCollection collection = new PftNodeCollection(parent)
            {
                new PftNode()
            };
            PftNode child = new PftNode();
            collection[0] = child;
            Assert.AreSame(parent, child.Parent);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftNode_SetItem_2()
        {
            PftNode parent1 = new PftNode();
            PftNode parent2 = new PftNode();
            PftNodeCollection collection = new PftNodeCollection(parent1)
            {
                new PftNode()
            };
            PftNode child = new PftNode();
            parent2.Children.Add(child);
            collection[0] = child;
            Assert.AreSame(parent1, child.Parent);
        }

        [TestMethod]
        public void PftNode_RemoveItem_1()
        {
            PftNode parent = new PftNode();
            PftNodeCollection collection = new PftNodeCollection(parent);
            PftNode child = new PftNode();
            collection.Add(child);
            collection.Remove(child);
            Assert.IsNull(child.Parent);
        }

        [TestMethod]
        public void PftNode_ToString_1()
        {
            PftNodeCollection collection = new PftNodeCollection(null)
            {
                new PftA("v100"),
                new PftBreak(),
                new PftC(10)
            };
            string expected = "a(v100) break c10";
            string actual = collection.ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}
