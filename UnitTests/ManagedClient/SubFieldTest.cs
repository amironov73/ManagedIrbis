using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;

namespace UnitTests
{
    [TestClass]
    public class SubFieldTest
    {
        [TestMethod]
        public void TestSubField()
        {
            SubField subField = new SubField();
            Assert.AreEqual(SubField.NoCode, subField.Code);
            Assert.AreEqual(SubField.NoCodeString, subField.CodeString);
            Assert.AreEqual(null,subField.Value);

            subField = new SubField('A', "The value");
            Assert.AreEqual('A', subField.Code);
            Assert.AreEqual("A", subField.CodeString);
            Assert.AreEqual("The value", subField.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSubFieldException()
        {
            SubField subField = new SubField('a')
            {
                Value = "Wrong^Value"
            };
            Assert.AreEqual("Wrong", subField.Value);
        }
    }
}
