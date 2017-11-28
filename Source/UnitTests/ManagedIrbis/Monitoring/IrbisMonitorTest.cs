using System;
using System.IO;
using System.Threading;
using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Monitoring;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

// ReSharper disable ObjectCreationAsStatement

namespace UnitTests.ManagedIrbis.Monitoring
{
    [TestClass]
    public class IrbisMonitorTest
    {
        class ErrorSink : MonitoringSink
        {
            public override bool WriteData(MonitoringData data)
            {
                return false;
            }
        }

        [NotNull]
        private ServerStat _GetStat()
        {
            return new ServerStat
            {
                ClientCount = 10,
                RunningClients = new ClientInfo[10],
                TotalCommandCount = 123
            };
        }

        [NotNull]
        private DatabaseInfo _GetDatabaseInfo(string databaseName)
        {
            DatabaseInfo result = new DatabaseInfo
            {
                Name = databaseName,
            };

            switch (databaseName)
            {
                case "IBIS":
                    result.LockedRecords = new[] { 1, 2, 3 };
                    break;

                case "RDR":
                    result.LockedRecords = new[] { 4, 5, 6 };
                    result.LogicallyDeletedRecords = new[] { 7, 8 };
                    break;
            }

            return result;
        }

        [NotNull]
        private Mock<IIrbisConnection> _GetMock()
        {
            Mock<IIrbisConnection> result = new Mock<IIrbisConnection>();

            result.Setup(c => c.GetServerStat())
                .Returns(_GetStat);

            result.Setup(c => c.GetDatabaseInfo(It.IsAny<string>()))
                .Returns((string dbName) => _GetDatabaseInfo(dbName));

            return result;
        }


        [TestMethod]
        public void IrbisMonitor_Construction_1()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            string[] databases = { "IBIS", "RDR" };
            IrbisMonitor monitor = new IrbisMonitor(connection, databases);
            Assert.AreSame(connection, monitor.Connection);
            Assert.IsNotNull(monitor.Databases);
            Assert.AreEqual(databases.Length, monitor.Databases.Count);
            for (int i = 0; i < databases.Length; i++)
            {
                Assert.AreEqual(databases[i], monitor.Databases[i]);
            }
            Assert.AreEqual(IrbisMonitor.DefaultInterval, monitor.Interval);
            Assert.IsFalse(monitor.Active);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IrbisMonitor_Interval_1()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            string[] databases = { "IBIS", "RDR" };
            new IrbisMonitor(connection, databases)
            {
                Interval = 0
            };
        }

        [TestMethod]
        public void IrbisMonitor_GetDataPortion_1()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            string[] databases = { "IBIS", "RDR" };
            IrbisMonitor monitor = new IrbisMonitor(connection, databases);
            MonitoringData data = monitor.GetDataPortion();
            Assert.IsNotNull(data.Databases);
            Assert.AreEqual(2, data.Databases.Length);

            mock.Verify(c => c.GetServerStat(), Times.Once);
            mock.Verify(c => c.GetDatabaseInfo(It.IsAny<string>()),
                Times.Exactly(2));
        }

        [TestMethod]
        public void IrbisMonitor_Monitoring_1()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            string[] databases = { "IBIS", "RDR" };
            IrbisMonitor monitor = new IrbisMonitor(connection, databases)
            {
                Interval = 100
            };
            monitor.StartMonitoring();
            Assert.IsTrue(monitor.Active);
            monitor.StartMonitoring();
            Assert.IsTrue(monitor.Active);
            Thread.Sleep(300);
            monitor.StopMonitoring();
            Assert.IsFalse(monitor.Active);
            monitor.StopMonitoring();
            Assert.IsFalse(monitor.Active);

            mock.Verify(c => c.GetServerStat(), Times.AtLeastOnce);
            mock.Verify(c => c.GetDatabaseInfo(It.IsAny<string>()),
                Times.AtLeast(2));
        }

        [TestMethod]
        public void IrbisMonitor_Monitoring_2()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            string[] databases = { "IBIS", "RDR" };
            IrbisMonitor monitor = new IrbisMonitor(connection, databases)
            {
                Interval = 100,
                Sink = new ErrorSink()
            };
            monitor.StartMonitoring();
            Assert.IsTrue(monitor.Active);
            Thread.Sleep(300);
            Assert.IsFalse(monitor.Active);

            mock.Verify(c => c.GetServerStat(), Times.AtLeastOnce);
            mock.Verify(c => c.GetDatabaseInfo(It.IsAny<string>()),
                Times.AtLeast(2));
        }
    }
}
