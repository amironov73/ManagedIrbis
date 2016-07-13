using System;
using System.Threading;
using ManagedClient.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient;

namespace UnitTests.ManagedClient
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

                IrbisRecord record = client.ReadRecord(1);
                Assert.IsNotNull(record);

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
        public void TestIrbisConnectionFormatRecord()
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
    }
}
