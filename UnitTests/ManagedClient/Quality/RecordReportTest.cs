using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient.Quality;

namespace UnitTests.ManagedClient.Quality
{
    [TestClass]
    public class RecordReportTest
    {
        private void _TestSerialization
            (
                RecordReport first
            )
        {
            byte[] bytes = first.SaveToMemory();

            RecordReport second = bytes
                .RestoreObjectFromMemory<RecordReport>();

            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.Defects.Count, second.Defects.Count);
            Assert.AreEqual(first.Index, second.Index);
            Assert.AreEqual(first.Mfn, second.Mfn);
        }

        [TestMethod]
        public void TestRecordReportSerialization()
        {
            RecordReport recordReport = new RecordReport();
            _TestSerialization(recordReport);
        }
    }
}
