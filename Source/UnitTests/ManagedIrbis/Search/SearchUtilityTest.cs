using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class SearchUtilityTest
    {
        [TestMethod]
        public void SearchUtility_EscapeQuotation_1()
        {
            Assert.AreEqual
                (
                    null, 
                    SearchUtility.EscapeQuotation(null)
                );
            Assert.AreEqual
                (
                    string.Empty,
                    SearchUtility.EscapeQuotation(string.Empty)
                );
            Assert.AreEqual
                (
                    "Hello",
                    SearchUtility.EscapeQuotation("Hello")
                );
            Assert.AreEqual
                (
                    "<.>Hello<.>",
                    SearchUtility.EscapeQuotation("\"Hello\"")
                );
            Assert.AreEqual
                (
                    "<.><.>",
                    SearchUtility.EscapeQuotation("\"\"")
                );
        }

        [TestMethod]
        public void SearchUtility_UnescapeQuotation_1()
        {
            Assert.AreEqual
                (
                    null,
                    SearchUtility.UnescapeQuotation(null)
                );
            Assert.AreEqual
                (
                    string.Empty,
                    SearchUtility.UnescapeQuotation(string.Empty)
                );
            Assert.AreEqual
                (
                    "Hello",
                    SearchUtility.UnescapeQuotation("Hello")
                );
            Assert.AreEqual
                (
                    "\"Hello\"",
                    SearchUtility.UnescapeQuotation("<.>Hello<.>")
                );
            Assert.AreEqual
                (
                    "\"\"",
                    SearchUtility.UnescapeQuotation("<.><.>")
                );
        }
    }
}
