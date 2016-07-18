using System;
using System.Threading;
using ManagedIrbis.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisConnectionTest
    {
        const string ConnectionString 
            = "host=127.0.0.1;port=6666;user=1;password=1;";

        [TestMethod]
        public void TestIrbisConnectionConstructor()
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                Assert.IsNotNull(client);
            }
        }

        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();

            RecordField field = new RecordField("700");
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField("701");
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField("200");
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new RecordField("210");
            field.AddSubField('a', "Иркутск");
            field.AddSubField('d', "2016");
            result.Fields.Add(field);

            field = new RecordField("215");
            field.AddSubField('a', "123");
            result.Fields.Add(field);

            field = new RecordField("300", "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Третье примечание");
            result.Fields.Add(field);

            field = new RecordField("920", "PAZK");
            result.Fields.Add(field);

            return result;
        }

        private void _TestSerialization
            (
                IrbisConnection first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisConnection second = bytes
                .RestoreObjectFromMemory<IrbisConnection>();

            Assert.IsNotNull(second);
        }

        [TestMethod]
        public void TestIrbisConnectionSerialization()
        {
            IrbisConnection client = new IrbisConnection();
            _TestSerialization(client);
        }

        [TestMethod]
        [Ignore]
        public void TestIrbisConnectionConnect()
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                client.ParseConnectionString(ConnectionString);
                client.Connect();

                client.NoOp();

                //Thread.Sleep(10 * 1024);
            }
        }

        [TestMethod]
        [Ignore]
        public void TestIrbisConnectionGetServerVersion()
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                client.ParseConnectionString(ConnectionString);
                client.Connect();

                IrbisVersion version = client.GetServerVersion();
                Assert.IsNotNull(version);

                //Thread.Sleep(10 * 1024);
            }
        }

        [TestMethod]
        [Ignore]
        public void TestIrbisConnectionReadRecord()
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                client.ParseConnectionString(ConnectionString);
                client.Connect();

                MarcRecord record = client.ReadRecord(1);
                Assert.IsNotNull(record);

                //Thread.Sleep(10 * 1024);
            }
        }

        [TestMethod]
        [Ignore]
        public void TestIrbisConnectionReadAndFormatRecord()
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                client.ParseConnectionString(ConnectionString);
                client.Connect();

                MarcRecord record = client.ReadRecord
                    (
                        1,
                        false,
                        "@brief"
                    );
                Assert.IsNotNull(record);
                Assert.IsNotNull(record.Description);

                //Thread.Sleep(10 * 1024);
            }
        }

        [TestMethod]
        [Ignore]
        public void TestIrbisConnectionGetMaxMfn()
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                client.ParseConnectionString(ConnectionString);
                client.Connect();

                int maxMfn = client.GetMaxMfn();
                Assert.IsTrue(maxMfn > 0);

                //Thread.Sleep(10 * 1024);
            }
        }

        [TestMethod]
        [Ignore]
        public void TestIrbisConnectionFormatRecord1()
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                client.ParseConnectionString(ConnectionString);
                client.Connect();

                string actual = client.FormatRecord
                    (
                        "@brief",
                        1
                    );
                Assert.IsNotNull(actual);

                //Thread.Sleep(10 * 1024);
            }
        }

        [TestMethod]
        [Ignore]
        public void TestIrbisConnectionFormatRecord2()
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                client.ParseConnectionString(ConnectionString);
                client.Connect();

                MarcRecord record = _GetRecord();
                string actual = client.FormatRecord
                    (
                        "@brief",
                        record
                    );
                Assert.IsNotNull(actual);

                //Thread.Sleep(10 * 1024);
            }
        }

        [TestMethod]
        [Ignore]
        public void TestIrbisConnectionFormatRecord3()
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                client.ParseConnectionString(ConnectionString);
                client.Connect();

                string[] actual = client.FormatRecords
                    (
                        "@brief",
                        new[] { 1,2,3 }
                    );
                Assert.IsNotNull(actual);
                Assert.AreEqual(3, actual.Length);

                //Thread.Sleep(10 * 1024);
            }
        }

        [TestMethod]
        [Ignore]
        public void TestIrbisConnectionSearch()
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                client.ParseConnectionString(ConnectionString);
                client.Connect();

                int[] found = client.Search("T=А$");
                Assert.IsNotNull(found);

                //Thread.Sleep(10 * 1024);
            }
        }

        [TestMethod]
        [Ignore]
        public void TestIrbisConnectionReadFile()
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                client.ParseConnectionString(ConnectionString);
                client.Connect();

                string text = client.ReadTextFile
                    (
                        IrbisPath.MasterFile,
                        "brief.pft"
                    );
                Assert.IsNotNull(text);

                //Thread.Sleep(10 * 1024);
            }
        }

        [TestMethod]
        [Ignore]
        public void TestIrbisConnectionReadFiles()
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                client.ParseConnectionString(ConnectionString);
                client.Connect();

                string[] texts = client.ReadTextFiles
                    (
                        new []
                        {
                            new IrbisFileSpecification
                                (
                                    IrbisPath.MasterFile,
                                    "IBIS",
                                    "brief.pft"
                                ), 
                            new IrbisFileSpecification
                                (
                                    IrbisPath.MasterFile,
                                    "IBIS",
                                    "briefin.pft"
                                ), 
                        }
                    );
                Assert.IsNotNull(texts);
                Assert.AreEqual(2, texts.Length);

                //Thread.Sleep(10 * 1024);
            }
        }

        [TestMethod]
        [Ignore]
        public void TestIrbisConnectionWriteRecord()
        {
            using (IrbisConnection client = new IrbisConnection())
            {
                client.ParseConnectionString(ConnectionString);
                client.Connect();

                MarcRecord record = _GetRecord();
                client.WriteRecord(record);
                Assert.IsNotNull(record.Database);

                //Thread.Sleep(10 * 1024);
            }
        }

    }
}
