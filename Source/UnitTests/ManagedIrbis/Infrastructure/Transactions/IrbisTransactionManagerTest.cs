using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Transactions;

namespace UnitTests.ManagedIrbis.Infrastructure.Transactions
{
    [TestClass]
    public class IrbisTransactionManagerTest
    {
        [TestMethod]
        public void IrbisTransactionManager_Construction_1()
        {
            IrbisProvider provider = new NullProvider();
            using (IrbisTransactionManager manager = new IrbisTransactionManager(provider))
            {
                Assert.AreSame(provider, manager.Provider);
                Assert.IsNotNull(manager.Context);
                Assert.IsNull(manager.Context.Name);
                Assert.IsFalse(manager.InTransactionNow);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void IrbisTransactionManager_CommitTransaction_1()
        {
            IrbisProvider provider = new NullProvider();
            using (IrbisTransactionManager manager = new IrbisTransactionManager(provider))
            {
                string name = "name";
                manager.BeginTransaction(name);
                Assert.AreEqual(name, manager.Context.Name);
                manager.CommitTransaction();
                Assert.IsNull(manager.Context.Name);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void IrbisTransactionManager_RollbackTransaction_1()
        {
            IrbisProvider provider = new NullProvider();
            using (IrbisTransactionManager manager = new IrbisTransactionManager(provider))
            {
                string name = "name";
                manager.BeginTransaction(name);
                Assert.AreEqual(name, manager.Context.Name);
                manager.RollbackTransaction();
                Assert.IsNull(manager.Context.Name);
            }
        }

        [TestMethod]
        public void IrbisTransactionManager_Dispose_1()
        {
            IrbisProvider provider = new NullProvider();
            using (IrbisTransactionManager manager = new IrbisTransactionManager(provider))
            {
                Assert.IsNotNull(manager.Context);
                Assert.IsNull(manager.Context.Name);
            }
        }
    }
}
