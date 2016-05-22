using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class IrbisDateTest
    {
        [TestMethod]
        public void TestIrbisDateConstruction()
        {
            string date1 = "20160101";
            IrbisDate date2 = date1;
            string date3 = date2;
            Assert.AreEqual(date1, date3);
        }

        private void _TestSerialization
            (
                IrbisDate first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisDate second = bytes
                .RestoreObjectFromMemory<IrbisDate>();

            Assert.AreEqual(first.Text, second.Text);
            Assert.AreEqual(first.Date, second.Date);
        }

        [TestMethod]
        public void TestIrbisDateSerialization()
        {
            IrbisDate date = "20121212";
            _TestSerialization(date);
        }
    }
}
