using System.IO;

using AM.Json;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class RecordStateTest
        : Common.CommonUnitTest
    {
        private RecordState _GetState()
        {
            return new RecordState
            {
                Id = 123,
                Mfn = 234,
                Status = RecordStatus.Last,
                Version = 345
            };
        }

        [TestMethod]
        public void RecordState_Construction_1()
        {
            RecordState state = new RecordState();
            Assert.AreEqual(0, state.Id);
            Assert.AreEqual(0, state.Mfn);
            Assert.AreEqual(0, (int)state.Status);
            Assert.AreEqual(0, state.Version);
        }

        [TestMethod]
        public void RecordState_ParseServerAnswer_1()
        {
            string line = "0 161608#0 0#1 101#";
            RecordState state = RecordState.ParseServerAnswer(line);
            Assert.AreEqual(0, state.Id);
            Assert.AreEqual(161608, state.Mfn);
            Assert.AreEqual(0, (int)state.Status);
            Assert.AreEqual(1, state.Version);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void RecordState_ParseServerAnswer_2()
        {
            string line = "0 161608#0 0";
            RecordState.ParseServerAnswer(line);
        }

        private void _TestSerialization
            (
                RecordState first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            first.SaveToStream(writer);
            byte[] bytes = stream.ToArray();

            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            RecordState second = new RecordState();
            second.RestoreFromStream(reader);

            Assert.AreEqual(first.Id, second.Id);
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Status, second.Status);
            Assert.AreEqual(first.Version, second.Version);
        }

        [TestMethod]
        public void RecordState_Serialization_1()
        {
            RecordState state = new RecordState();
            _TestSerialization(state);

            state = _GetState();
            _TestSerialization(state);
        }

        [TestMethod]
        public void RecordState_ToXml_1()
        {
            RecordState state = new RecordState();
            Assert.AreEqual("<record />", XmlUtility.SerializeShort(state));

            state = _GetState();
            Assert.AreEqual("<record mfn=\"234\" status=\"Last\" version=\"345\" />", XmlUtility.SerializeShort(state));
        }

        [TestMethod]
        public void RecordState_ToJson_1()
        {
            RecordState state = new RecordState();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(state));

            state = _GetState();
            Assert.AreEqual("{'mfn':234,'status':32,'version':345}", JsonUtility.SerializeShort(state));
        }

        [TestMethod]
        public void RecordState_ToString_1()
        {
            RecordState state = new RecordState();
            Assert.AreEqual("0:0:0", state.ToString());

            state = _GetState();
            Assert.AreEqual("234:32:345", state.ToString());
        }
    }
}
