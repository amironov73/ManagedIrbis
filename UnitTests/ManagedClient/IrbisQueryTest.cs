using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class IrbisQueryTest
    {
        private void _TestQuery
            (
                string text,
                string expected
            )
        {
            string actual = IrbisSearchQuery.PrepareQuery(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIrbisQueryPrepareQuery()
        {
            _TestQuery("", "");
            _TestQuery(" ", " ");
            _TestQuery("v100:v200", "v100:v200");
            _TestQuery("\tv100\r\n", " v100  ");
        }
    }
}
