using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Readers;

namespace UnitTests.ManagedClient.Readers
{
    [TestClass]
    public class VisitInfoTest
    {
        private void _TestSerialization
            (
                VisitInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            VisitInfo second = bytes
                .RestoreObjectFromMemory<VisitInfo>();

            Assert.AreEqual(first.DateGivenString, second.DateGivenString);
            Assert.AreEqual(first.Department, second.Department);
            Assert.AreEqual(first.Description, second.Description);
        }

        [TestMethod]
        public void TestVisitInfoSerialization()
        {
            VisitInfo visitInfo = new VisitInfo();
            _TestSerialization(visitInfo);

            visitInfo.DateGivenString = "20160529";
            visitInfo.Department = "АБ";
            visitInfo.Description = "Книга";
            _TestSerialization(visitInfo);
        }
    }
}
