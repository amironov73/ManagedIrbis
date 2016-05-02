using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;

namespace UnitTests
{
    [TestClass]
    public class RecordFieldTest
    {
        [TestMethod]
        public void TestRecordField()
        {
            RecordField field = new RecordField();
            Assert.AreEqual(RecordField.NoTag, field.Tag);
            Assert.AreEqual(null, field.Value);
            Assert.AreEqual(0,field.SubFields.Count);
        }
    }
}
