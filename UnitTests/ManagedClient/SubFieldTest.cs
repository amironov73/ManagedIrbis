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
            Assert.AreEqual("^\0", subField.ToString());

            subField = new SubField('A', "The value");
            Assert.AreEqual('A', subField.Code);
            Assert.AreEqual("A", subField.CodeString);
            Assert.AreEqual("The value", subField.Value);
            Assert.AreEqual("^AThe value", subField.ToString());

            SubField clone = subField.Clone();
            Assert.AreEqual(subField.Code, clone.Code);
            Assert.AreEqual(subField.CodeString, clone.CodeString);
            Assert.AreEqual(subField.Value, clone.Value);
            Assert.AreEqual("^AThe value", clone.ToString());
            Assert.AreEqual(0,SubField.Compare(subField,clone));

            subField.SetValue("New value");
            Assert.AreEqual("New value", subField.Value);
            subField.SetValue(null);
            Assert.AreEqual(null, subField.Value);
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
