using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable CollectionNeverQueried.Local

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class VirtualChildrenTest
    {
        [NotNull]
        private VirtualChildren _GetChildren()
        {
            VirtualChildren result = new VirtualChildren();
            PftNode[] children = 
            {
                new PftComma(),
                new PftBang()
            };
            result.SetChildren(children);

            return result;
        }

        [TestMethod]
        public void VirtualChildren_Construction_1()
        {
            VirtualChildren children = new VirtualChildren();
            Assert.AreEqual(0, children.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void VirtualChildren_Add_1()
        {
            VirtualChildren children = new VirtualChildren();
            children.Add(new PftComma());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void VirtualChildren_Clear_1()
        {
            VirtualChildren children = new VirtualChildren();
            children.Clear();
        }

        [TestMethod]
        public void VirtualChildren_Contains_1()
        {
            VirtualChildren children = new VirtualChildren();
            Assert.IsFalse(children.Contains(new PftNode()));
        }

        [TestMethod]
        public void VirtualChildren_CopyTo_1()
        {
            VirtualChildren children = _GetChildren();
            PftNode[] array = new PftNode[2];
            children.CopyTo(array, 0);
            Assert.IsInstanceOfType(array[0], typeof(PftComma));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void VirtualChildren_Remove_1()
        {
            VirtualChildren children = new VirtualChildren();
            children.Remove(new PftNode());
        }

        [TestMethod]
        public void VirtualChildren_Count_1()
        {
            VirtualChildren children = _GetChildren();
            Assert.AreEqual(2, children.Count);
        }

        [TestMethod]
        public void VirtualChildren_IsReadOnly_1()
        {
            VirtualChildren children = _GetChildren();
            Assert.IsTrue(children.IsReadOnly);
        }

        [TestMethod]
        public void VirtualChildren_IndexOf_1()
        {
            VirtualChildren children = _GetChildren();
            Assert.AreEqual(-1, children.IndexOf(new PftNode()));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void VirtualChildren_Insert_1()
        {
            VirtualChildren children = new VirtualChildren();
            children.Insert(0, new PftNode());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void VirtualChildren_RemoveAt_1()
        {
            VirtualChildren children = _GetChildren();
            children.RemoveAt(0);
        }

        [TestMethod]
        public void VirtualChildren_Indexer_1()
        {
            VirtualChildren children = _GetChildren();
            PftNode node = children[0];
            Assert.IsInstanceOfType(node, typeof(PftComma));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void VirtualChildren_Indexer_2()
        {
            VirtualChildren children = _GetChildren();
            children[0] = new PftNode();
        }

        [TestMethod]
        public void VirtualChildren_ToString_1()
        {
            VirtualChildren children = _GetChildren();
            Assert.AreEqual("2", children.ToString());
        }

        [TestMethod]
        public void VirtualChildren_SetChildren_1()
        {
            VirtualChildren children = new VirtualChildren();
            PftNode[] nodes =
            {
                new PftComma()
            };
            children.SetChildren(nodes);
            Assert.AreEqual(1, children.Count);
            PftNode node = children[0];
            Assert.IsInstanceOfType(node, typeof(PftComma));
        }

        [TestMethod]
        public void VirtualChildren_GetEnumerator_1()
        {
            bool flag = false;
            VirtualChildren children = _GetChildren();
            children.Enumeration += (sender, args) => { flag = true; };
            IEnumerator<PftNode> enumerator = children.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.IsTrue(enumerator.MoveNext());
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void VirtualChildren_GetEnumerator_2()
        {
            bool flag = false;
            VirtualChildren children = _GetChildren();
            children.Enumeration += (sender, args) => { flag = true; };
            IEnumerator enumerator = ((IEnumerable)children).GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.IsTrue(enumerator.MoveNext());
            Assert.IsFalse(enumerator.MoveNext());
            Assert.IsTrue(flag);
        }
    }
}
