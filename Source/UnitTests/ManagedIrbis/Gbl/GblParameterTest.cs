using System;
using System.IO;
using AM.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl;

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public class GblParameterTest
    {
        [TestMethod]
        public void GblParameter_Construction()
        {
            GblParameter parameter = new GblParameter();
            Assert.AreEqual(null, parameter.Name);
            Assert.AreEqual(null, parameter.Value);
        }

        [TestMethod]
        public void GblParameter_ParseStream()
        {
            const string text = "Value\r\nName";
            TextReader reader = new StringReader(text);
            GblParameter parameter = GblParameter.ParseStream(reader);
            Assert.AreEqual("Name", parameter.Name);
            Assert.AreEqual("Value", parameter.Value);
        }

        private void _TestSerialization
            (
                GblParameter first
            )
        {
            byte[] bytes = first.SaveToMemory();

            GblParameter second = bytes
                .RestoreObjectFromMemory<GblParameter>();

            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Value, second.Value);
        }

        [TestMethod]
        public void GblParameter_Serialization()
        {
            GblParameter parameter = new GblParameter();
            _TestSerialization(parameter);

            parameter = new GblParameter
            {
                Name = "Name",
                Value = "Value"
            };
            _TestSerialization(parameter);
        }

        [TestMethod]
        public void GblParameter_Verify()
        {
            GblParameter parameter = new GblParameter();
            Assert.AreEqual(false, parameter.Verify(false));

            parameter = new GblParameter
            {
                Name = "Name",
                Value = "Value"
            };
            Assert.AreEqual(true, parameter.Verify(false));
        }

        [TestMethod]
        public void GblParameter_ToString()
        {
            GblParameter parameter = new GblParameter
            {
                Name = "Start",
                Value = "Now"
            };
            Assert.AreEqual
                (
                    "Name: Start, Value: Now",
                    parameter.ToString()
                );
        }
    }
}
