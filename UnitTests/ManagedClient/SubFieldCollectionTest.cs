using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient;

namespace UnitTests
{
    [TestClass]
    public class SubFieldCollectionTest
    {
        [TestMethod]
        public void TestSubFieldCollectionConstruction()
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

        private void _TestSerialization
            (
                params SubField[] subFields
            )
        {
            SubFieldCollection collection1 = new SubFieldCollection();
            collection1.AddRange(subFields);

            //collection1.SaveToFile("collection1.bin");
            //collection1.SaveToZipFile("collection1.biz");

            byte[] bytes = collection1.SaveToMemory();

            SubFieldCollection collection2 = bytes
                    .RestoreObjectFromMemory <SubFieldCollection>();

            Assert.AreEqual(collection1.Count, collection2.Count);

            for (int i = 0; i < collection1.Count; i++)
            {
                Assert.AreEqual
                    (
                        0,
                        SubField.Compare
                        (
                            collection1[i],
                            collection2[i]
                        )
                    );
            }
        }

        [TestMethod]
        public void TestSubFieldCollectionSerialization()
        {
            _TestSerialization();

            _TestSerialization
                (
                    new SubField(),
                    new SubField('a'),
                    new SubField('b', "Hello")
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSubFieldCollectionNotNull()
        {
            SubFieldCollection collection =
                new SubFieldCollection
                {
                    new SubField(),
                    null,
                    new SubField('a')
                };
            Assert.AreEqual(2, collection.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void TestSubFieldCollectionReadOnly()
        {
            RecordField field = new RecordField().AsReadOnly();
            field.SubFields.Add(new SubField());
        }
    }
}
