using System;
using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis.Monitoring;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Monitoring
{
    [TestClass]
    public class MonitoringDataTest
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
        public void MonitoringData_Construction_1()
        {
            MonitoringData data = new MonitoringData();
            Assert.AreEqual(DateTime.MinValue, data.Moment);
            Assert.AreEqual(0, data.Clients);
            Assert.AreEqual(0, data.Commands);
            Assert.IsNull(data.Databases);
        }

        private void _TestSerialization
            (
                [NotNull] MonitoringData first
            )
        {
            byte[] bytes = first.SaveToMemory();
            MonitoringData second = bytes.RestoreObjectFromMemory<MonitoringData>();
            Assert.AreEqual(first.Moment, second.Moment);
            Assert.AreEqual(first.Clients, second.Clients);
            Assert.AreEqual(first.Commands, second.Commands);
            if (ReferenceEquals(first.Databases, null))
            {
                Assert.IsNull(second.Databases);
            }
            else
            {
                Assert.IsNotNull(second.Databases);
                for (int i = 0; i < first.Databases.Length; i++)
                {
                    Assert.AreEqual(first.Databases[i].Name, second.Databases[i].Name);
                    Assert.AreEqual(first.Databases[i].DeletedRecords, second.Databases[i].DeletedRecords);
                    if (ReferenceEquals(first.Databases[i].LockedRecords, null))
                    {
                        Assert.IsNull(second.Databases[i].LockedRecords);
                    }
                    else
                    {
                        Assert.IsNotNull(second.Databases[i].LockedRecords);
                        for (int j = 0; j < first.Databases[i].LockedRecords.Length; j++)
                        {
                            Assert.AreEqual(first.Databases[i].LockedRecords[j],
                                second.Databases[i].LockedRecords[j]);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void MonitoringData_Serialization_1()
        {
            MonitoringData data = new MonitoringData();
            _TestSerialization(data);

            data = _GetData();
            _TestSerialization(data);
        }

        [TestMethod]
        public void MonitoringData_ToXml_1()
        {
            MonitoringData data = new MonitoringData();
            Assert.AreEqual("<monitoring moment=\"0001-01-01T00:00:00\" clients=\"0\" commands=\"0\" />", XmlUtility.SerializeShort(data));

            data = _GetData();
            Assert.AreEqual("<monitoring moment=\"2017-11-28T09:39:00\" clients=\"10\" commands=\"1234\"><database name=\"IBIS\" deletedRecords=\"100\"><locked><mfn>1</mfn><mfn>2</mfn><mfn>3</mfn></locked></database><database name=\"RDR\" deletedRecords=\"10\"><locked><mfn>4</mfn><mfn>5</mfn><mfn>6</mfn></locked></database></monitoring>", XmlUtility.SerializeShort(data));
        }

        [TestMethod]
        public void MonitoringData_ToJson_1()
        {
            MonitoringData data = new MonitoringData();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(data));

            data = _GetData();
            Assert.AreEqual("{'moment':'2017-11-28T09:39:00','clients':10,'commands':1234,'databases':[{'name':'IBIS','deletedRecords':100,'lockedRecords':[1,2,3]},{'name':'RDR','deletedRecords':10,'lockedRecords':[4,5,6]}]}", JsonUtility.SerializeShort(data));
        }

        [TestMethod]
        public void MonitoringData_ToString_1()
        {
            MonitoringData data = new MonitoringData();
            Assert.AreEqual("01.01.0001 0:00:00", data.ToString());

            data = _GetData();
            Assert.AreEqual("28.11.2017 9:39:00:IBIS,RDR", data.ToString());
        }
    }
}
