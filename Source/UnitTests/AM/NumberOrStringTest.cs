using System;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.Json;
using AM.Runtime;
using AM.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

namespace UnitTests.AM
{
    [TestClass]
    public class NumberOrStringTest
    {
        [XmlRoot("dummy")]
        public class DummyClass
        {
            [XmlElement("zero")]
            [JsonProperty("zero")]
            public int Zero { get; set; }

            [XmlElement("first")]
            [JsonProperty("first")]
            public NumberOrString First { get; set; }

            [XmlElement("second")]
            [JsonProperty("second")]
            public NumberOrString Second { get; set; }

            [XmlElement("third")]
            [JsonProperty("third")]
            public string Third { get; set; }
        }


        [TestMethod]
        public void NumberOrString_Construction_1()
        {
            NumberOrString number = new NumberOrString();
            Assert.IsNull(number.AsString);
        }

        [TestMethod]
        public void NumberOrString_Construction_2()
        {
            NumberOrString number = new NumberOrString("Hello");
            Assert.AreEqual("Hello", number.AsString);
        }

        [TestMethod]
        public void NumberOrString_Construction_3()
        {
            NumberOrString number = new NumberOrString(1);
            Assert.AreEqual("1", number.AsString);
        }

        [TestMethod]
        public void NumberOrString_Construction_4()
        {
            NumberOrString number = new NumberOrString(1.0);
            Assert.AreEqual("1", number.AsString);
        }

        [TestMethod]
        public void NumberOrString_Construction_5()
        {
            NumberOrString number = new NumberOrString(1.0m);
            Assert.AreEqual("1.0", number.AsString);
        }

        [TestMethod]
        public void NumberOrString_IsNull_1()
        {
            NumberOrString number = new NumberOrString();
            Assert.IsTrue(number.IsNull);

            number.AsString = string.Empty;
            Assert.IsFalse(number.IsNull);

            number.AsString = "Hello";
            Assert.IsFalse(number.IsNull);
        }

        [TestMethod]
        public void NumberOrString_IsNullOrEmpty_1()
        {
            NumberOrString number = new NumberOrString();
            Assert.IsTrue(number.IsNullOrEmpty);

            number.AsString = string.Empty;
            Assert.IsTrue(number.IsNullOrEmpty);

            number.AsString = "Hello";
            Assert.IsFalse(number.IsNullOrEmpty);
        }

        [TestMethod]
        public void NumberOrString_AsInt16_1()
        {
            NumberOrString number = new NumberOrString();
            Assert.AreEqual((short)0, number.AsInt16);

            number = (short) 0;
            Assert.AreEqual((short)0, number.AsInt16);

            number = (short) 1;
            Assert.AreEqual((short)1, number.AsInt16);
        }

        [TestMethod]
        public void NumberOrString_AsInt16_2()
        {
            NumberOrString number = new NumberOrString();
            number.AsInt16 = 1;
            Assert.AreEqual((short)1, number.AsInt16);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NumberOrString_AsInt16_Exception_1()
        {
            NumberOrString number = "Hello";
            short value = number.AsInt16;
        }

        [TestMethod]
        public void NumberOrString_AsInt32_1()
        {
            NumberOrString number = new NumberOrString();
            Assert.AreEqual(0, number.AsInt32);

            number = 0;
            Assert.AreEqual(0, number.AsInt32);

            number = 1;
            Assert.AreEqual(1, number.AsInt32);
        }

        [TestMethod]
        public void NumberOrString_AsInt32_2()
        {
            NumberOrString number = new NumberOrString();
            number.AsInt32 = 1;
            Assert.AreEqual(1, number.AsInt32);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NumberOrString_AsInt32_Exception_1()
        {
            NumberOrString number = "Hello";
            int value = number.AsInt32;
        }

        [TestMethod]
        public void NumberOrString_AsInt64_1()
        {
            NumberOrString number = new NumberOrString();
            Assert.AreEqual(0L, number.AsInt64);

            number = 0L;
            Assert.AreEqual(0L, number.AsInt64);

            number = 1L;
            Assert.AreEqual(1L, number.AsInt64);
        }

        [TestMethod]
        public void NumberOrString_AsInt64_2()
        {
            NumberOrString number = new NumberOrString();
            number.AsInt64 = 1;
            Assert.AreEqual(1L, number.AsInt64);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NumberOrString_AsInt64_Exception_1()
        {
            NumberOrString number = "Hello";
            long value = number.AsInt64;
        }

        [TestMethod]
        public void NumberOrString_AsDecimal_1()
        {
            NumberOrString number = new NumberOrString();
            Assert.AreEqual(decimal.Zero, number.AsDecimal);

            number = decimal.Zero;
            Assert.AreEqual(0m, number.AsDecimal);

            number = decimal.One;
            Assert.AreEqual(decimal.One, number.AsDecimal);
        }

        [TestMethod]
        public void NumberOrString_AsDecimal_2()
        {
            NumberOrString number = new NumberOrString();
            number.AsDecimal = decimal.One;
            Assert.AreEqual(decimal.One, number.AsDecimal);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NumberOrString_AsDecimal_Exception_1()
        {
            NumberOrString number = "Hello";
            decimal value = number.AsDecimal;
        }

        [TestMethod]
        public void NumberOrString_AsDouble_1()
        {
            NumberOrString number = new NumberOrString();
            Assert.AreEqual(0.0, number.AsDouble);

            number = 0.0;
            Assert.AreEqual(0.0, number.AsDouble);

            number = 1.0;
            Assert.AreEqual(1.0, number.AsDouble);
        }

        [TestMethod]
        public void NumberOrString_AsDouble_2()
        {
            NumberOrString number = new NumberOrString();
            number.AsDouble = 1.0;
            Assert.AreEqual(1.0, number.AsDouble);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NumberOrString_AsDouble_Exception_1()
        {
            NumberOrString number = "Hello";
            double value = number.AsDouble;
        }

        [TestMethod]
        public void NumberOrString_ToVisibleString_1()
        {
            NumberOrString number = new NumberOrString();
            Assert.AreEqual("(null)", number.ToVisibleString());

            number = string.Empty;
            Assert.AreEqual("(empty)", number.ToVisibleString());

            number = "Hello";
            Assert.AreEqual("Hello", number.ToVisibleString());
        }

        [TestMethod]
        public void NumberOrString_IsInt16_1()
        {
            NumberOrString number = "Hello";
            Assert.IsFalse(number.IsInt16);

            number = 1;
            Assert.IsTrue(number.IsInt16);
        }

        [TestMethod]
        public void NumberOrString_IsInt32_1()
        {
            NumberOrString number = "Hello";
            Assert.IsFalse(number.IsInt32);

            number = 1;
            Assert.IsTrue(number.IsInt32);
        }

        [TestMethod]
        public void NumberOrString_IsInt64_1()
        {
            NumberOrString number = "Hello";
            Assert.IsFalse(number.IsInt64);

            number = 1;
            Assert.IsTrue(number.IsInt64);
        }

        [TestMethod]
        public void NumberOrString_IsDecimal_1()
        {
            NumberOrString number = "Hello";
            Assert.IsFalse(number.IsDecimal);

            number = 1;
            Assert.IsTrue(number.IsDecimal);
        }

        [TestMethod]
        public void NumberOrString_IsDouble_1()
        {
            NumberOrString number = "Hello";
            Assert.IsFalse(number.IsDouble);

            number = 1;
            Assert.IsTrue(number.IsDouble);
        }

        [TestMethod]
        public void NumberOrString_Operator_Int16_1()
        {
            NumberOrString number = (short) 1;
            short actual = number;
            Assert.AreEqual((short)1, actual);
        }

        [TestMethod]
        public void NumberOrString_Operator_Int32_1()
        {
            NumberOrString number = 1;
            int actual = number;
            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void NumberOrString_Operator_Int64_1()
        {
            NumberOrString number = 1L;
            long actual = number;
            Assert.AreEqual(1L, actual);
        }

        [TestMethod]
        public void NumberOrString_Operator_Decimal_1()
        {
            NumberOrString number = decimal.One;
            decimal actual = number;
            Assert.AreEqual(decimal.One, actual);
        }

        [TestMethod]
        public void NumberOrString_Operator_Double_1()
        {
            NumberOrString number = 1.0;
            double actual = number;
            Assert.AreEqual(1.0, actual);
        }

        [TestMethod]
        public void NumberOrString_Operator_String_1()
        {
            NumberOrString number = "Hello";
            string actual = number;
            Assert.AreEqual("Hello", actual);
        }

        [TestMethod]
        public void NumberOrString_ToString_1()
        {
            NumberOrString number = new NumberOrString();
            Assert.IsNull(number.ToString());

            number = string.Empty;
            Assert.AreEqual(string.Empty, number.ToString());

            number = "Hello";
            Assert.AreEqual("Hello", number.ToString());
        }

        [TestMethod]
        public void NumberOrString_Equals_1()
        {
            NumberOrString first = 1, second = 2;
            Assert.IsFalse(first.Equals(second));

            second = 1;
            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void NumberOrString_Equals_2()
        {
            NumberOrString number = 1;
            Assert.IsFalse(number.Equals(new object()));
        }

        [TestMethod]
        public void NumberOrString_Equals_3()
        {
            NumberOrString number = 1;
            Assert.IsFalse(number.Equals((object)null));
        }

        [TestMethod]
        public void NumberOrString_ToXml_1()
        {
            DummyClass dummy = new DummyClass();
            string xml = XmlUtility.SerializeShort(dummy);
            Assert.AreEqual("<dummy><zero>0</zero><first>(null)</first><second>(null)</second></dummy>", xml);

            dummy = new DummyClass
            {
                First = 1,
                Second = "Hello"
            };
            Assert.AreEqual("<dummy><zero>0</zero><first>1</first><second>Hello</second></dummy>", XmlUtility.SerializeShort(dummy));
        }

        [TestMethod]
        public void NumberOrString_FromXml_1()
        {
            string xml = "<dummy><zero>0</zero><first>(null)</first><second>(null)</second></dummy>";
            DummyClass dummy = XmlUtility.DeserializeString<DummyClass>(xml);
            Assert.IsTrue(dummy.First.IsNull);
            Assert.IsTrue(dummy.Second.IsNull);

            xml = "<dummy><zero>0</zero><first>1</first><second>Hello</second></dummy>";
            dummy = XmlUtility.DeserializeString<DummyClass>(xml);
            Assert.AreEqual(1, dummy.First.AsInt32);
            Assert.AreEqual("Hello", dummy.Second.AsString);
        }

        [TestMethod]
        public void NumberOrString_ToJson_1()
        {
            DummyClass dummy = new DummyClass();
            string json = JsonUtility.SerializeShort(dummy);
            Assert.AreEqual("{'zero':0,'first':null,'second':null}", json);

            dummy = new DummyClass
            {
                First = 1,
                Second = "Hello"
            };
            Assert.AreEqual("{'zero':0,'first':'1','second':'Hello'}", JsonUtility.SerializeShort(dummy));
        }

        [TestMethod]
        public void NumberOrString_FromJson_1()
        {
            string json = "{'zero':0,'first':null,'second':null}";
            DummyClass dummy = JsonConvert.DeserializeObject<DummyClass>(json);
            Assert.IsTrue(dummy.First.IsNull);
            Assert.IsTrue(dummy.Second.IsNull);

            json = "{'zero':0,'first':'1','second':'Hello'}";
            dummy = JsonConvert.DeserializeObject<DummyClass>(json);
            Assert.AreEqual(1, dummy.First.AsInt32);
            Assert.AreEqual("Hello", dummy.Second.AsString);
        }

        private void _TestSerialization
            (
                NumberOrString first
            )
        {
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                first.SaveToStream(writer);
            }
            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            NumberOrString second;
            using (BinaryReader reader = new BinaryReader(stream))
            {
                second.RestoreFromStream(reader);
            }
            Assert.AreEqual(first.AsString, second.AsString);
        }

        [TestMethod]
        public void NumberOrString_Serialization_1()
        {
            NumberOrString number = new NumberOrString();
            _TestSerialization(number);

            number = "Hello";
            _TestSerialization(number);

            number = 1;
            _TestSerialization(number);

            number = 1.0;
            _TestSerialization(number);

            number = decimal.One;
            _TestSerialization(number);
        }
    }
}
