using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Fields;

namespace UnitTests.ManagedClient.Fields
{
    [TestClass]
    public class ExemplarInfoTest
    {
        private void _TestSerialization
            (
                ExemplarInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            ExemplarInfo second = bytes
                .RestoreObjectFromMemory<ExemplarInfo>();
            Assert.AreEqual(first.Number, second.Number);
            Assert.AreEqual(first.Barcode, second.Barcode);
        }

        [TestMethod]
        public void TestExemplarInfoSerialization()
        {
            ExemplarInfo exemplarInfo = new ExemplarInfo();
            _TestSerialization(exemplarInfo);

            exemplarInfo.Number = "1";
            exemplarInfo.Barcode = "2";
            _TestSerialization(exemplarInfo);
        }
    }
}
