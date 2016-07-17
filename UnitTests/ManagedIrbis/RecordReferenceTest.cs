using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class RecordReferenceTest
    {
        [TestMethod]
        public void TestRecordReferenceConstructor()
        {
            RecordReference reference = new RecordReference();
            Assert.AreEqual(null, reference.HostName);
            Assert.AreEqual(null, reference.Database);
            Assert.AreEqual(0, reference.Mfn);
            Assert.AreEqual(null, reference.Index);
        }

        private void _TestSerialization
            (
                RecordReference first
            )
        {
            byte[] bytes = first.SaveToMemory();

            RecordReference second = bytes
                    .RestoreObjectFromMemory <RecordReference>();

            Assert.AreEqual(first.HostName, second.HostName);
            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Index, second.Index);
        }

        [TestMethod]
        public void TestRecordReferenceSerialization()
        {
            RecordReference reference = new RecordReference();
            _TestSerialization(reference);

            reference.Mfn = 1234;
            _TestSerialization(reference);
        }
    }
}
