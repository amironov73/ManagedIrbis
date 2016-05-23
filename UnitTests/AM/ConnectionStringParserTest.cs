using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class ConnectionStringParserTest
    {
        [TestMethod]
        public void TestConnectionStringParserConstructor()
        {
            ConnectionStringParser parser = new ConnectionStringParser();
            Assert.AreEqual(ConnectionStringParser.DefaultKeyDelimiter, parser.KeyDelimiter.Value);
            Assert.AreEqual(ConnectionStringParser.DefaultValueDelimiter, parser.ValueDelimiter.Value);
        }
    }
}
