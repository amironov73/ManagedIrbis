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
    public class IrbisTransactionEventArgsTest
    {
        [TestMethod]
        public void IrbisTransactionEventArgs_Construction_1()
        {
            IrbisProvider provider = new NullProvider();
            IrbisTransactionContext context = new IrbisTransactionContext();
            IrbisTransactionItem item = new IrbisTransactionItem();
            IrbisTransactionEventArgs args = new IrbisTransactionEventArgs
                (
                    provider,
                    context,
                    item
                );
            Assert.AreEqual(provider, args.Provider);
            Assert.AreEqual(context, args.Context);
            Assert.AreEqual(item, args.Item);
        }
    }
}
