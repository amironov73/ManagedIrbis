using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;

using Moq;
// ReSharper disable ConvertToLocalFunction


namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class RecordReferenceTest
    {
        [NotNull]
        public MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord
            {
                HostName = "host",
                Database = "db",
                Mfn = 123,
                Index = "Index"
            };
            result.Fields.Add(RecordField.Parse(200, "^aSubA^bSubB"));
            result.Fields.Add(RecordField.Parse(300, "Field300"));

            return result;
        }

        [TestMethod]
        public void RecordReference_Construction_1()
        {
            RecordReference reference = new RecordReference();
            Assert.IsNull(reference.HostName);
            Assert.IsNull(reference.Database);
            Assert.AreEqual(0, reference.Mfn);
            Assert.IsNull(reference.Index);
            Assert.IsNull(reference.Record);
        }

        [TestMethod]
        public void RecordReference_Construction_2()
        {
            MarcRecord record = _GetRecord();
            RecordReference reference = new RecordReference(record);
            Assert.AreEqual("host", reference.HostName);
            Assert.AreEqual("db", reference.Database);
            Assert.AreEqual(123, reference.Mfn);
            Assert.AreEqual("Index", reference.Index);
            Assert.AreSame(record, reference.Record);
        }

        [TestMethod]
        public void RecordReference_Compare_1()
        {
            RecordReference first = new RecordReference();
            RecordReference second = new RecordReference();
            Assert.IsFalse(RecordReference.Compare(first, second));

            first = new RecordReference
            {
                HostName = "host",
                Database = "db",
                Mfn = 123
            };
            second = new RecordReference
            {
                HostName = "host",
                Database = "db",
                Mfn = 123
            };
            Assert.IsTrue(RecordReference.Compare(first, second));

            second.Mfn = 124;
            Assert.IsFalse(RecordReference.Compare(first, second));

            first = new RecordReference
            {
                HostName = "host1",
                Database = "db",
                Mfn = 123
            };
            second = new RecordReference
            {
                HostName = "host2",
                Database = "db",
                Mfn = 123
            };
            Assert.IsFalse(RecordReference.Compare(first, second));

            first = new RecordReference
            {
                HostName = "host",
                Database = "db",
                Index = "Index"
            };
            second = new RecordReference
            {
                HostName = "host",
                Database = "db",
                Index = "Index"
            };
            Assert.IsTrue(RecordReference.Compare(first, second));

            second.Index = "Index2";
            Assert.IsFalse(RecordReference.Compare(first, second));

            first = new RecordReference
            {
                HostName = "host",
                Database = "db",
                Mfn = 123
            };
            second = new RecordReference
            {
                HostName = "host",
                Database = "db",
                Index = "Index"
            };
            Assert.IsFalse(RecordReference.Compare(first, second));
        }

        private void _TestSerialization
            (
                RecordReference first
            )
        {
            byte[] bytes = first.SaveToMemory();

            RecordReference second = bytes
                    .RestoreObjectFromMemory<RecordReference>();

            Assert.AreEqual(first.HostName, second.HostName);
            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Index, second.Index);
        }

        [TestMethod]
        public void RecordReference_Serialization_1()
        {
            RecordReference reference = new RecordReference();
            _TestSerialization(reference);

            reference.Mfn = 1234;
            _TestSerialization(reference);
        }

        [TestMethod]
        public void RecordReference_ZipFile_1()
        {
            string fileName = Path.GetTempFileName();
            RecordReference[] first =
            {
                new RecordReference(_GetRecord())
            };
            RecordReference.SaveToZipFile(first, fileName);

            RecordReference[] second
                = RecordReference.LoadFromZipFile(fileName);
            Assert.AreEqual(first.Length, second.Length);
            for (int i = 0; i < first.Length; i++)
            {
                Assert.AreEqual(first[i].HostName, second[i].HostName);
                Assert.AreEqual(first[i].Database, second[i].Database);
                Assert.AreEqual(first[i].Mfn, second[i].Mfn);
                Assert.AreEqual(first[i].Index, second[i].Index);
            }
        }

        [TestMethod]
        public void RecordReference_ReadRecord_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadRecord(It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(_GetRecord());

            IIrbisConnection connection = mock.Object;
            RecordReference reference = new RecordReference
            {
                HostName = "host",
                Database = "db",
                Mfn = 123
            };
            MarcRecord record = reference.ReadRecord(connection);
            Assert.IsNotNull(record);
            Assert.AreEqual("host", record.HostName);
            Assert.AreEqual("db", record.Database);
            Assert.AreEqual(123, record.Mfn);

            mock.Verify(c => c.ReadRecord(It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()),
                Times.Once);
        }

        [TestMethod]
        public void RecordReference_ReadRecord_2()
        {
            byte[] rawAnswer = new byte[0], rawRequest = new byte[0];
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            Action<SearchReadCommand> callback = command =>
            {
                command.Records = new MarcRecord[1];
                command.Records[0] = _GetRecord();
            };
            IIrbisConnection connection = mock.Object;
            ServerResponse response = new ServerResponse
                (
                    connection, 
                    rawAnswer,
                    rawRequest,
                    true
                );
            mock.SetupGet(c => c.CommandFactory)
                .Returns(new CommandFactory(connection));
            mock.Setup(c => c.ExecuteCommand(It.IsAny<SearchReadCommand>()))
                .Returns(response)
                .Callback(callback);

            RecordReference reference = new RecordReference
            {
                HostName = "host",
                Database = "db",
                Index = "Index"
            };
            MarcRecord record = reference.ReadRecord(connection);
            Assert.IsNotNull(record);
            Assert.AreEqual("host", record.HostName);
            Assert.AreEqual("db", record.Database);
            Assert.AreEqual("Index", record.Index);

            mock.VerifyGet(c => c.CommandFactory, Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<SearchReadCommand>()),
                Times.Once);
        }

        [TestMethod]
        public void RecordReference_ReadRecords_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadRecord(It.IsAny<string>(),
                    It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(_GetRecord());

            IIrbisConnection connection = mock.Object;
            RecordReference[] references = 
            {
                new RecordReference
                {
                    HostName = "host",
                    Database = "db",
                    Mfn = 123
                }
            };
            List<MarcRecord> records = RecordReference.ReadRecords
                (
                    connection,
                    references,
                    false
                );
            Assert.AreEqual(1, records.Count);
            Assert.AreEqual("host", records[0].HostName);
            Assert.AreEqual("db", records[0].Database);
            Assert.AreEqual(123, records[0].Mfn);

            mock.Verify(c => c.ReadRecord(It.IsAny<string>(),
                    It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()),
                Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void RecordReference_ReadRecords_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadRecord(It.IsAny<string>(),
                    It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns((MarcRecord)null);

            IIrbisConnection connection = mock.Object;
            RecordReference[] references = 
            {
                new RecordReference
                {
                    HostName = "host",
                    Database = "db",
                    Mfn = 123
                }
            };
            RecordReference.ReadRecords
                (
                    connection,
                    references,
                    true
                );
        }

        [TestMethod]
        public void RecordReference_ReadRecords_3()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadRecord(It.IsAny<string>(),
                    It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns((MarcRecord)null);

            IIrbisConnection connection = mock.Object;
            RecordReference[] references = 
            {
                new RecordReference
                {
                    HostName = "host",
                    Database = "db",
                    Mfn = 123
                }
            };
            List<MarcRecord> records = RecordReference.ReadRecords
                (
                    connection,
                    references,
                    false
                );
            Assert.AreEqual(0, records.Count);
        }

        [TestMethod]
        public void RecordReference_Verify_1()
        {
            RecordReference reference = new RecordReference();
            Assert.IsFalse(reference.Verify(false));

            reference = new RecordReference
            {
                Database = "db",
                Mfn = 123
            };
            Assert.IsTrue(reference.Verify(false));

            reference = new RecordReference
            {
                Database = "db",
                Index = "Index"
            };
            Assert.IsTrue(reference.Verify(false));

            reference = new RecordReference
            {
                HostName = "host",
                Database = "db"
            };
            Assert.IsFalse(reference.Verify(false));
        }

        [TestMethod]
        public void RecordReference_ToString_1()
        {
            RecordReference reference = new RecordReference();
            Assert.AreEqual("(null)#0#(null)", reference.ToString());

            reference = new RecordReference
            {
                HostName = "host",
                Database = "db",
                Mfn = 123,
                Index = "Index"
            };
            Assert.AreEqual("db#123#Index", reference.ToString());
        }

        [TestMethod]
        public void RecordReference_ToString_2()
        {
            MarcRecord record = _GetRecord();
            RecordReference reference = new RecordReference(record);
            Assert.AreEqual("db123#00#0200#^aSubA^bSubB300#Field300", reference.ToString());
        }
    }
}
