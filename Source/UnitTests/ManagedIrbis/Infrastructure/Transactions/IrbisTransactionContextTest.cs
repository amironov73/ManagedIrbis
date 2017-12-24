using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Transactions;

namespace UnitTests.ManagedIrbis.Infrastructure.Transactions
{
    [TestClass]
    public class IrbisTransactionContextTest
    {
        [TestMethod]
        public void IrbisTransactionContext_Construction_1()
        {
            IrbisTransactionContext context = new IrbisTransactionContext();
            Assert.IsNotNull(context.Items);
            Assert.AreEqual(0, context.Items.Count);
            Assert.IsNull(context.Name);
            Assert.IsNull(context.ParentContext);
        }

        [TestMethod]
        public void IrbisTransactionContext_Construction_2()
        {
            string name = "name";
            IrbisTransactionContext context = new IrbisTransactionContext(name);
            Assert.IsNotNull(context.Items);
            Assert.AreEqual(0, context.Items.Count);
            Assert.AreEqual(name, context.Name);
            Assert.IsNull(context.ParentContext);
        }

        [TestMethod]
        public void IrbisTransactionContext_Construction_3()
        {
            IrbisTransactionContext parent = new IrbisTransactionContext();
            IrbisTransactionContext context = new IrbisTransactionContext(parent);
            Assert.IsNotNull(context.Items);
            Assert.AreEqual(0, context.Items.Count);
            Assert.IsNull(context.Name);
            Assert.AreEqual(parent, context.ParentContext);
        }

        [TestMethod]
        public void IrbisTransactionContext_Construction_4()
        {
            string name = "name";
            IrbisTransactionContext parent = new IrbisTransactionContext();
            IrbisTransactionContext context = new IrbisTransactionContext(name, parent);
            Assert.IsNotNull(context.Items);
            Assert.AreEqual(0, context.Items.Count);
            Assert.AreEqual(name, context.Name);
            Assert.AreEqual(parent, context.ParentContext);
        }
    }
}
