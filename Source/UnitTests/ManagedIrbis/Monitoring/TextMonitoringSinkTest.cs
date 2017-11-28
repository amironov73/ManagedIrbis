using System;
using System.IO;

using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Monitoring;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Monitoring
{
    [TestClass]
    public class TextMonitoringSinkTest
    {
        [NotNull]
        private MonitoringData _GetData()
        {
            return new MonitoringData
            {
                Moment = new DateTime(2017, 11, 28, 9, 39, 0),
                Clients = 10,
                Commands = 1234,
                Databases = new[]
                {
                    new DatabaseData
                    {
                        Name = "IBIS",
                        DeletedRecords = 100,
                        LockedRecords = new[] {1, 2, 3}
                    },
                    new DatabaseData
                    {
                        Name = "RDR",
                        DeletedRecords = 10,
                        LockedRecords = new[] {4, 5, 6}
                    }
                }
            };
        }

        [TestMethod]
        public void TextMonitoringSink_Construction_1()
        {
            StringWriter writer = new StringWriter();
            TextMonitoringSink sink = new TextMonitoringSink(writer);
            Assert.AreSame(writer, sink.Writer);
        }

        [TestMethod]
        public void TextMonitoringSink_WriteData_1()
        {
            StringWriter writer = new StringWriter();
            TextMonitoringSink sink = new TextMonitoringSink(writer);
            MonitoringData data = _GetData();
            sink.WriteData(data);
            string actual = writer.ToString().DosToUnix();
            Assert.AreEqual("{\n  \"moment\": \"2017-11-28T09:39:00\",\n  \"clients\": 10,\n  \"commands\": 1234,\n  \"databases\": [\n    {\n      \"name\": \"IBIS\",\n      \"deletedRecords\": 100,\n      \"lockedRecords\": [\n        1,\n        2,\n        3\n      ]\n    },\n    {\n      \"name\": \"RDR\",\n      \"deletedRecords\": 10,\n      \"lockedRecords\": [\n        4,\n        5,\n        6\n      ]\n    }\n  ]\n}\n", actual);
        }

        [TestMethod]
        public void TextMonitoringSink_WriteData_2()
        {
            Mock<TextWriter> mock = new Mock<TextWriter>();
            mock.Setup(w => w.WriteLine(It.IsAny<string>()))
                .Throws(new IrbisException());
            TextWriter writer = mock.Object;
            TextMonitoringSink sink = new TextMonitoringSink(writer);
            MonitoringData data = _GetData();
            Assert.IsFalse(sink.WriteData(data));
        }
    }
}
