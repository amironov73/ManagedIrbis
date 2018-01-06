using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fields;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class GlobalCounterTest
    {
        [NotNull]
        private MarcRecord _GetRecord()
        {
            return new MarcRecord()
                .AddField(new RecordField(1, "01"))
                .AddField(new RecordField(2, "СЧ000011/1"))
                .AddField(new RecordField(3, "СЧ******/1"));
        }

        [NotNull]
        private GlobalCounter _GetCounter()
        {
            return new GlobalCounter
            {
                Index = "01",
                Value = "СЧ000011/1",
                Template = "СЧ******/1"
            };
        }

        private void _Compare
            (
                [NotNull] GlobalCounter first,
                [NotNull] GlobalCounter second
            )
        {
            Assert.AreEqual(first.Index, second.Index);
            Assert.AreEqual(first.Value, second.Value);
            Assert.AreEqual(first.Template, second.Template);
        }

        [TestMethod]
        public void GlobalCounter_Construction_1()
        {
            GlobalCounter counter = new GlobalCounter();
            Assert.IsNull(counter.Index);
            Assert.IsNull(counter.Value);
            Assert.IsNull(counter.Template);
            Assert.IsNull(counter.Record);
            Assert.IsNull(counter.UserData);
        }

        [TestMethod]
        public void GlobalCounter_Parse_1()
        {
            MarcRecord record = _GetRecord();
            GlobalCounter counter = GlobalCounter.Parse(record);
            Assert.AreSame(record, counter.Record);
            Assert.AreEqual(record.FM(1), counter.Index);
            Assert.AreEqual(record.FM(2), counter.Value);
            Assert.AreEqual(record.FM(3), counter.Template);
            Assert.IsNull(counter.UserData);
        }

        [TestMethod]
        public void GlobalCounter_ToRecord_1()
        {
            GlobalCounter counter = _GetCounter();
            MarcRecord record = counter.ToRecord();
            Assert.AreEqual(counter.Index, record.FM(1));
            Assert.AreEqual(counter.Value, record.FM(2));
            Assert.AreEqual(counter.Template, record.FM(3));
        }

        [TestMethod]
        public void GlobalCounter_ApplyTo_1()
        {
            GlobalCounter counter = _GetCounter();
            MarcRecord record = new MarcRecord()
                .AddField(1, "Hello")
                .AddField(2, "333")
                .AddField(3, "???");
            counter.ApplyTo(record);
            Assert.AreEqual(counter.Index, record.FM(1));
            Assert.AreEqual(counter.Value, record.FM(2));
            Assert.AreEqual(counter.Template, record.FM(3));
        }

        [TestMethod]
        public void GlobalCounter_NumericValue_1()
        {
            GlobalCounter counter = new GlobalCounter();
            Assert.AreEqual(0, counter.NumericValue);

            counter.NumericValue = 123;
            Assert.AreEqual("123", counter.Value);
        }

        [TestMethod]
        public void GlobalCounter_NumericValue_2()
        {
            GlobalCounter counter = _GetCounter();
            Assert.AreEqual(11, counter.NumericValue);

            counter.NumericValue = 123;
            Assert.AreEqual("СЧ000123/1", counter.Value);
        }

        [TestMethod]
        public void GlobalCounter_Increment_1()
        {
            GlobalCounter counter = new GlobalCounter();
            Assert.AreSame(counter, counter.Increment(1));
            Assert.AreEqual("1", counter.Value);
            Assert.AreEqual(1, counter.NumericValue);

            counter = _GetCounter();
            Assert.AreSame(counter, counter.Increment(1));
            Assert.AreEqual("СЧ000012/1", counter.Value);
            Assert.AreEqual(12, counter.NumericValue);
        }

        [TestMethod]
        public void GlobalCounter_ToXml_1()
        {
            GlobalCounter counter = new GlobalCounter();
            Assert.AreEqual("<counter />", XmlUtility.SerializeShort(counter));

            counter = _GetCounter();
            Assert.AreEqual("<counter index=\"01\" value=\"СЧ000011/1\" template=\"СЧ******/1\" />", XmlUtility.SerializeShort(counter));
        }

        [TestMethod]
        public void GlobalCounter_ToJson_1()
        {
            GlobalCounter counter = new GlobalCounter();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(counter));

            counter = _GetCounter();
            Assert.AreEqual("{'index':'01','value':'СЧ000011/1','template':'СЧ******/1'}", JsonUtility.SerializeShort(counter));
        }

        private void _TestSerialization
            (
                [NotNull] GlobalCounter first
            )
        {
            byte[] bytes = first.SaveToMemory();
            GlobalCounter second = bytes.RestoreObjectFromMemory<GlobalCounter>();
            _Compare(first, second);
            Assert.IsNull(second.Record);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void GlobalCounter_Serialization_1()
        {
            GlobalCounter counter = new GlobalCounter();
            _TestSerialization(counter);

            counter = _GetCounter();
            counter.UserData = "User data";
            _TestSerialization(counter);
        }

        [TestMethod]
        public void GlobalCounter_Verify_1()
        {
            GlobalCounter counter = new GlobalCounter();
            Assert.IsFalse(counter.Verify(false));

            counter = _GetCounter();
            Assert.IsTrue(counter.Verify(false));
        }

        [TestMethod]
        public void GlobalCounter_ToString_1()
        {
            GlobalCounter counter = new GlobalCounter();
            Assert.AreEqual("(null):(null)", counter.ToString());

            counter = _GetCounter();
            Assert.AreEqual("01:СЧ000011/1", counter.ToString());
        }
    }
}
