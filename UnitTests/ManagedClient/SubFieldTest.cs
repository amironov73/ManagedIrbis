using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient;

namespace UnitTests
{
    [TestClass]
    public class SubFieldTest
    {
        [TestMethod]
        public void TestSubFieldConstructor()
        {
            SubField subField = new SubField();
            Assert.AreEqual(SubField.NoCode, subField.Code);
            Assert.AreEqual(SubField.NoCodeString, subField.CodeString);
            Assert.AreEqual(null, subField.Value);
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
            Assert.AreEqual(0, SubField.Compare(subField, clone));

            subField.SetValue("New value");
            Assert.AreEqual("New value", subField.Value);
            subField.SetValue(null);
            Assert.AreEqual(null, subField.Value);
        }

        private void _TestSerialization
            (
                params SubField[] subFields
            )
        {
            SubField[] array1 = subFields;
            byte[] bytes = array1.SaveToMemory();

            SubField[] array2 = bytes
                    .RestoreArrayFromMemory<SubField>();

            Assert.AreEqual(array1.Length, array2.Length);
            for (int i = 0; i < array1.Length; i++)
            {
                Assert.AreEqual
                    (
                        0,
                        SubField.Compare(array1[i], array2[i])
                    );
            }
        }

        [TestMethod]
        public void TestSubFieldSerialization()
        {
            _TestSerialization(new SubField[0]);
            _TestSerialization(new SubField());
            _TestSerialization(new SubField(), new SubField());
            _TestSerialization(new SubField('a'), new SubField('b'));
            _TestSerialization(new SubField('a', "Hello"),
                new SubField('b', "World"));
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
