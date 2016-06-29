using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient.Marc.Schema;

namespace UnitTests.ManagedClient.Marc.Schema
{
    [TestClass]
    public class MarcSchemaTest
        : Common.CommonUnitTest
    {
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
        }
    }
}
