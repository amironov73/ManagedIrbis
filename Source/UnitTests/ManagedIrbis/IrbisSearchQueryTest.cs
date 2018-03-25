using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisSearchQueryTest
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
        public void IrbisSearchQuery_ForbiddenCharacters_1()
        {
            Assert.IsNotNull(IrbisSearchQuery.ForbiddenCharacters);
        }

        [TestMethod]
        public void IrbisSearchQuery_PrepareQuery_1()
        {
            _TestQuery(null, null);
            _TestQuery("", "");
            _TestQuery(" ", " ");
            _TestQuery("v100:v200", "v100:v200");
            _TestQuery("\tv100\r\n", " v100  ");
        }
    }
}
