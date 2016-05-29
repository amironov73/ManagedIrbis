using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient.Readers;

namespace UnitTests.ManagedClient.Readers
{
    [TestClass]
    public class ChairInfoTest
    {
        private void _TestSerialization
            (
                ChairInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            ChairInfo second = bytes
                .RestoreObjectFromMemory<ChairInfo>();

            Assert.AreEqual(first.Code, second.Code);
            Assert.AreEqual(first.Title, second.Title);
        }

        [TestMethod]
        public void TestChairInfoSerialization()
        {
            ChairInfo chairInfo = new ChairInfo();
            _TestSerialization(chairInfo);

            chairInfo.Code = "АБ";
            chairInfo.Title = "Абонемент";
            _TestSerialization(chairInfo);
        }
    }
}
