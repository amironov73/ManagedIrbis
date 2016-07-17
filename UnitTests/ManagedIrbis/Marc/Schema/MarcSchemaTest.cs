using System;
using System.IO;
using AM.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Marc.Schema;

namespace UnitTests.ManagedIrbis.Marc.Schema
{
    [TestClass]
    public class MarcSchemaTest
        : Common.CommonUnitTest
    {
        private void _TestSerialization
            (
                MarcSchema first
            )
        {
            byte[] bytes = first.SaveToMemory();

            MarcSchema second = bytes
                .RestoreObjectFromMemory<MarcSchema>();

            Assert.AreEqual(first.Fields.Count, second.Fields.Count);
        }

        [TestMethod]
        public void TestMarcSchemaParseLocalXml()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "RUSMARC20151123.xml"
                );

            MarcSchema schema = MarcSchema.ParseLocalXml(fileName);
            Assert.IsNotNull(schema);

            string actual = schema.ToJson();
            Assert.IsNotNull(actual);

            _TestSerialization(schema);
        }
    }
}
