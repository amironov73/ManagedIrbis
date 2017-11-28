using System;
using System.Collections.Generic;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Monitoring;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable EqualExpressionComparison

namespace UnitTests.ManagedIrbis.Monitoring
{
    [TestClass]
    public class BlockedRecordTest
    {
        [NotNull]
        private BlockedRecord _GetRecord()
        {
            return new BlockedRecord
            {
                Database = "IBIS",
                Mfn = 123,
                Count = 5,
                Since = new DateTime(2017, 11, 28, 8, 24, 00)
            };
        }

        [TestMethod]
        public void BlockedRecord_Construction_1()
        {
            BlockedRecord record = new BlockedRecord();
            Assert.IsNull(record.Database);
            Assert.AreEqual(0, record.Mfn);
            Assert.AreEqual(0, record.Count);
            Assert.AreEqual(DateTime.MinValue, record.Since);
            Assert.IsFalse(record.Marked);
        }

        [TestMethod]
        public void BlockedRecord_Merge_1()
        {
            List<BlockedRecord> list = new List<BlockedRecord>();
            DatabaseInfo[] databases =
            {
                new DatabaseInfo
                {
                    Name = "IBIS",
                    LockedRecords = new[] {1, 2, 3}
                },
                new DatabaseInfo
                {
                    Name = "RDR",
                    LockedRecords = new[] {4, 5, 6}
                }
            };
            list = BlockedRecord.Merge(list, databases);
            Assert.AreEqual(6, list.Count);

            databases = new[]
            {
                new DatabaseInfo
                {
                    Name = "IBIS",
                    LockedRecords = new[] {2, 3, 4}
                },
                new DatabaseInfo
                {
                    Name = "RDR",
                    LockedRecords = new[] {5, 6, 7}
                }
            };
            list = BlockedRecord.Merge(list, databases);
            Assert.AreEqual(6, list.Count);

            databases = new[]
            {
                new DatabaseInfo
                {
                    Name = "IBIS",
                    LockedRecords = null
                },
                new DatabaseInfo
                {
                    Name = "RDR",
                    LockedRecords = new[] {5, 6, 7}
                }
            };
            list = BlockedRecord.Merge(list, databases);
            Assert.AreEqual(3, list.Count);
        }

        private void _TestSerialization
            (
                [NotNull] BlockedRecord first
            )
        {
            byte[] bytes = first.SaveToMemory();
            BlockedRecord second = bytes.RestoreObjectFromMemory<BlockedRecord>();
            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Count, second.Count);
            Assert.AreEqual(first.Since, second.Since);
            Assert.IsFalse(second.Marked);
        }

        [TestMethod]
        public void BlockedRecord_Serialization_1()
        {
            BlockedRecord record = new BlockedRecord();
            _TestSerialization(record);

            record = _GetRecord();
            record.Marked = true;
            _TestSerialization(record);
        }

        [TestMethod]
        public void BlockedRecord_Verify_1()
        {
            BlockedRecord record = new BlockedRecord();
            Assert.IsFalse(record.Verify(false));

            record = _GetRecord();
            Assert.IsTrue(record.Verify(false));
        }

        [TestMethod]
        public void BlockedRecord_Equals_1()
        {
            BlockedRecord first = new BlockedRecord
            {
                Database = "IBIS",
                Mfn = 123,
                Count = 5
            };
            BlockedRecord second = new BlockedRecord
            {
                Database = "IBIS",
                Mfn = 123,
                Count = 6
            };
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(second.Equals(first));
            Assert.IsTrue(first.Equals(first));
            Assert.IsTrue(second.Equals(second));

            second.Mfn = 124;
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));

            second = null;
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void BlockedRecord_Equals_2()
        {
            BlockedRecord first = new BlockedRecord
            {
                Database = "IBIS",
                Mfn = 123
            };
            object second = new BlockedRecord
            {
                Database = "IBIS",
                Mfn = 123
            };
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(second.Equals(first));
            Assert.IsTrue(second.Equals(second));

            second = null;
            Assert.IsFalse(first.Equals(second));

            second = "second";
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));
        }

        [TestMethod]
        public void BlockedRecord_GetHashCode_1()
        {
            BlockedRecord record = new BlockedRecord();
            Assert.AreEqual(0, record.GetHashCode());

            record.Mfn = 123;
            Assert.AreEqual(123, record.GetHashCode());

            record.Database = "IBIS";
            Assert.AreEqual(1577697023, record.GetHashCode());

            record.Mfn = 124;
            Assert.AreEqual(1577697040, record.GetHashCode());
        }

        [TestMethod]
        public void BlockedRecord_ToXml_1()
        {
            BlockedRecord record = new BlockedRecord();
            Assert.AreEqual("<blocked mfn=\"0\" count=\"0\" since=\"0001-01-01T00:00:00\" />", XmlUtility.SerializeShort(record));

            record = _GetRecord();
            Assert.AreEqual("<blocked database=\"IBIS\" mfn=\"123\" count=\"5\" since=\"2017-11-28T08:24:00\" />", XmlUtility.SerializeShort(record));
        }

        [TestMethod]
        public void BlockedRecord_ToJson_1()
        {
            BlockedRecord record = new BlockedRecord();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(record));

            record = _GetRecord();
            Assert.AreEqual("{'database':'IBIS','mfn':123,'count':5,'since':'2017-11-28T08:24:00'}", JsonUtility.SerializeShort(record));
        }

        [TestMethod]
        public void BlockedRecord_ToString_1()
        {
            BlockedRecord record = new BlockedRecord();
            Assert.AreEqual("(null):0:0", record.ToString());

            record = _GetRecord();
            Assert.AreEqual("IBIS:123:5", record.ToString());
        }
    }
}
