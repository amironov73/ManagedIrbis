using System.Linq;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class RecordGuardTest
    {
        [TestMethod]
        public void RecordGuard_AddRecord_1()
        {
            NullProvider provider = new NullProvider();
            MarcRecord record = new MarcRecord();

            using (RecordGuard guard = new RecordGuard(provider, record))
            {
                guard.AddRecord(record);
                Assert.AreEqual(1, guard.Records.Count());
            }
        }

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
        public void RecordGuard_Commit_2()
        {
            MarcRecord record = new MarcRecord();
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(conn => conn.WriteRecord(record, false, true, false));
            IIrbisConnection connection = mock.Object;

            using (RecordGuard guard = new RecordGuard(connection, record))
            {
                record.Modified = true;
                guard.Commit();
            }

            mock.Verify(conn => conn.WriteRecord(record, false, true, false));
        }

        [TestMethod]
        public void RecordGuard_Construction_1()
        {
            IrbisConnection connection = new IrbisConnection();
            MarcRecord record = new MarcRecord();

            using (RecordGuard guard = new RecordGuard(connection, record))
            {
                Assert.AreEqual(1, guard.Records.Count());
            }
            connection.Dispose();
        }

        [TestMethod]
        public void RecordGuard_Construction_2()
        {
            IrbisConnection connection = new IrbisConnection();
            MarcRecord[] records = { new MarcRecord(), new MarcRecord() };

            using (RecordGuard guard = new RecordGuard(connection, records))
            {
                Assert.AreEqual(2, guard.Records.Count());
            }
            connection.Dispose();
        }

        [TestMethod]
        public void RecordGuard_Construction_3()
        {
            IrbisProvider provider = new NullProvider();
            MarcRecord record = new MarcRecord();

            using (RecordGuard guard = new RecordGuard(provider, record))
            {
                Assert.AreEqual(1, guard.Records.Count());
            }
            provider.Dispose();
        }

        [TestMethod]
        public void RecordGuard_Construction_4()
        {
            IrbisProvider provider = new NullProvider();
            MarcRecord[] records = { new MarcRecord(), new MarcRecord() };

            using (RecordGuard guard = new RecordGuard(provider, records))
            {
                Assert.AreEqual(2, guard.Records.Count());
            }
            provider.Dispose();
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
