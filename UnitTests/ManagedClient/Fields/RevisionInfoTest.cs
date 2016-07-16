using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Fields;

namespace UnitTests.ManagedClient.Fields
{
    [TestClass]
    public class RevisionInfoTest
    {
        private void _TestSerialization
            (
                RevisionInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            RevisionInfo second = bytes
                    .RestoreObjectFromMemory<RevisionInfo>();
            Assert.AreEqual(first.Date, second.Date);
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Stage, second.Stage);
        }

        [TestMethod]
        public void TestRevisionInfoSerialization()
        {
            var revisionInfo = new RevisionInfo();
            _TestSerialization(revisionInfo);
        }
    }
}
