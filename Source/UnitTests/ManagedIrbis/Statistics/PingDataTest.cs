using System;
using System.IO;

using AM.Json;
using AM.Xml;

using ManagedIrbis.Statistics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Statistics
{
    [TestClass]
    public class PingDataTest
    {
        private PingData _GetData()
        {
            return new PingData
            {
                Moment = new DateTime(2017, 10, 9, 10, 0, 0),
                Success = true,
                RoundTripTime = 84
            };
        }

        [TestMethod]
        public void PingData_Construction_1()
        {
            PingData data = new PingData();
            Assert.AreEqual(default(DateTime), data.Moment);
            Assert.IsFalse(data.Success);
            Assert.AreEqual(0, data.RoundTripTime);
        }

        private void _TestSerialization
            (
                PingData first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            first.SaveToStream(writer);
            byte[] bytes = stream.ToArray();
            PingData second = new PingData();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            second.RestoreFromStream(reader);
            Assert.AreEqual(first.Moment, second.Moment);
            Assert.AreEqual(first.Success, second.Success);
            Assert.AreEqual(first.RoundTripTime, second.RoundTripTime);
        }

        [TestMethod]
        public void PingData_Serialization_1()
        {
            PingData data = new PingData();
            _TestSerialization(data);

            data = _GetData();
            _TestSerialization(data);
        }

        [TestMethod]
        public void PingData_ToXml_1()
        {
            PingData data = new PingData();
            Assert.AreEqual("<ping><moment>0001-01-01T00:00:00</moment><roundtrip>0</roundtrip></ping>", XmlUtility.SerializeShort(data));

            data = _GetData();
            Assert.AreEqual("<ping><moment>2017-10-09T10:00:00</moment><success>true</success><roundtrip>84</roundtrip></ping>", XmlUtility.SerializeShort(data));
        }

        [TestMethod]
        public void PingData_ToJson_1()
        {
            PingData data = new PingData();
            Assert.AreEqual("{'moment':'0001-01-01T00:00:00','roundtrip':0}", JsonUtility.SerializeShort(data));

            data = _GetData();
            Assert.AreEqual("{'moment':'2017-10-09T10:00:00','success':true,'roundtrip':84}", JsonUtility.SerializeShort(data));
        }

        [TestMethod]
        public void PingData_ToString_1()
        {
            PingData data = new PingData();
            Assert.AreEqual("00:00:00 False 0", data.ToString());

            data = _GetData();
            Assert.AreEqual("10:00:00 True 84", data.ToString());
        }
    }
}
