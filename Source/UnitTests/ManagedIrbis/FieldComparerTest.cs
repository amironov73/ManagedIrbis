using System.Collections.Generic;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class FieldComparerTest
    {
        [TestMethod]
        public void FieldComparer_ByTag_1()
        {
            Comparer<RecordField> comparer = FieldComparer.ByTag();

            RecordField left = new RecordField(100);
            RecordField right = new RecordField(101);
            Assert.IsTrue(comparer.Compare(left, right) < 0);

            right = new RecordField(99);
            Assert.IsTrue(comparer.Compare(left, right) > 0);

            right = new RecordField(100);
            Assert.IsTrue(comparer.Compare(left, right) == 0);
        }
    }
}
