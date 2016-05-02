using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;

namespace UnitTests
{
    [TestClass]
    public class SubFieldCollectionTest
    {
        [TestMethod]
        public void TestSubFieldCollection()
        {
            SubFieldCollection collection =
                new SubFieldCollection
                {
                    new SubField(),
                    new SubField('a'),
                    new SubField('b', "Value")
                };
            Assert.AreEqual(3, collection.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSubFieldCollectionException()
        {
            SubFieldCollection collection =
                new SubFieldCollection
                {
                    new SubField(),
                    null,
                    new SubField('a')
                };
        }
    }
}
