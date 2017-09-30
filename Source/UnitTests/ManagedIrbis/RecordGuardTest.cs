using System;
using System.Linq;
using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class RecordGuardTest
    {
        [TestMethod]
        public void RecordGuard_Commit_1()
        {
            NullProvider provider = new NullProvider();
            MarcRecord record1 = new MarcRecord();
            MarcRecord record2 = new MarcRecord();
            bool eventCalled = false;

            using (RecordGuard guard = new RecordGuard(provider, record1))
            {
                guard.AddRecord(record2);
                Assert.AreEqual(2, guard.Records.Count());

                guard.RemoveRecord(record2);
                Assert.AreEqual(1, guard.Records.Count());

                record1.Modified = true;

                guard.CommitChanges += (sender, args) =>
                {
                    eventCalled = true;
                };
            }

            Assert.IsTrue(eventCalled);
        }

        [TestMethod]
        public void RecordGuard_Verify_1()
        {
            NullProvider provider = new NullProvider();
            MarcRecord record = new MarcRecord();

            using (RecordGuard guard = new RecordGuard(provider, record))
            {
                Assert.IsTrue(guard.Verify(false));
            }
        }

        [TestMethod]
        public void RecordGuard_ToString_1()
        {
            NullProvider provider = new NullProvider();
            MarcRecord record = new MarcRecord
            {
                Mfn = 123,
                Database = "IBIS"
            };

            using (RecordGuard guard = new RecordGuard(provider, record))
            {
                Assert.AreEqual("IBIS 123 True", guard.ToString());
            }
        }
    }
}
