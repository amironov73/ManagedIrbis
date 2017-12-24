using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Transactions;

// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.ManagedIrbis.Infrastructure.Transactions
{
    [TestClass]
    public class IrbisTransactionItemTest
    {
        [TestMethod]
        public void IrbisTransactionItem_Construction_1()
        {
            IrbisTransactionItem item = new IrbisTransactionItem();
            Assert.AreEqual(DateTime.MinValue, item.Moment);
            Assert.AreEqual((IrbisTransactionAction)0, item.Action);
            Assert.IsNull(item.Database);
            Assert.AreEqual(0, item.Mfn);
        }

        [TestMethod]
        public void IrbisTransactionItem_Properties_1()
        {
            IrbisTransactionItem item = new IrbisTransactionItem();
            DateTime moment = new DateTime(2017, 12, 24, 13, 54, 0);
            item.Moment = moment;
            Assert.AreEqual(moment, item.Moment);
            IrbisTransactionAction action = IrbisTransactionAction.ModifyRecord;
            item.Action = action;
            Assert.AreEqual(action, item.Action);
            string database = "IBIS";
            item.Database = database;
            Assert.AreEqual(database, item.Database);
            int mfn = 1234;
            item.Mfn = mfn;
            Assert.AreEqual(mfn, item.Mfn);
        }
    }
}
