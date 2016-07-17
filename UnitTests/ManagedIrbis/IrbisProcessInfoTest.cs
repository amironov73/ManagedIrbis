using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisProcessInfoTest
    {
        [TestMethod]
        public void TestIrbisProcessInfoConstructor()
        {
            IrbisProcessInfo info = new IrbisProcessInfo();
            Assert.AreEqual(null, info.ClientID);
            Assert.AreEqual(null, info.CommandNumber);
            Assert.AreEqual(null, info.IPAddress);
            Assert.AreEqual(null, info.LastCommand);
            Assert.AreEqual(null, info.Name);
            Assert.AreEqual(null, info.Number);
            Assert.AreEqual(null, info.ProcessID);
            Assert.AreEqual(null, info.Started);
            Assert.AreEqual(null, info.State);
            Assert.AreEqual(null, info.Workstation);
        }

        private void _TestSerialization
            (
                IrbisProcessInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisProcessInfo second = bytes
                .RestoreObjectFromMemory<IrbisProcessInfo>();

            Assert.AreEqual(first.ClientID, second.ClientID);
            Assert.AreEqual(first.CommandNumber, second.CommandNumber);
            Assert.AreEqual(first.IPAddress, second.IPAddress);
            Assert.AreEqual(first.LastCommand, second.LastCommand);
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Number, second.Number);
            Assert.AreEqual(first.ProcessID, second.ProcessID);
            Assert.AreEqual(first.Started, second.Started);
            Assert.AreEqual(first.State, second.State);
            Assert.AreEqual(first.Workstation, second.Workstation);
        }

        [TestMethod]
        public void TestIrbisProcessInfoSerialization()
        {
            IrbisProcessInfo info = new IrbisProcessInfo();
            _TestSerialization(info);

            info.Name = "abc";
            _TestSerialization(info);
        }
    }
}
