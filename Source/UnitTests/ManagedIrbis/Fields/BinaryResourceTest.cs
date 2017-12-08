using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Fields;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class BinaryResourceTest
    {
        [TestMethod]
        public void BinaryResource_Constructor_1()
        {
            BinaryResource resource = new BinaryResource();
            Assert.IsNull(resource.Kind);
            Assert.IsNull(resource.Resource);
            Assert.IsNull(resource.Title);
            Assert.IsNull(resource.View);
        }

        [TestMethod]
        public void BinaryResource_Encode_1()
        {
            BinaryResource resource = new BinaryResource();
            byte[] bytes = {1, 2, 3, 4, 5};
            string actual = resource.Encode(bytes);
            const string expected = "%01%02%03%04%05";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BinaryResource_Decode_1()
        {
            BinaryResource resource = new BinaryResource
            {
                Resource = "%01%02%03%04%05"
            };
            byte[] expected = {1, 2, 3, 4, 5};
            byte[] actual = resource.Decode();
            CollectionAssert.AreEqual(expected, actual);
        }

        private RecordField _GetField()
        {
            RecordField result = new RecordField("953")
                .AddSubField('a', "jpg")
                .AddSubField('b', "%01%02%03%04%05")
                .AddSubField('t', "Cover");

            return result;
        }

        [TestMethod]
        public void BinaryResource_Parse_1()
        {
            RecordField field = _GetField();
            BinaryResource resource 
                = BinaryResource.Parse(field);
            Assert.AreEqual("jpg", resource.Kind);
            Assert.AreEqual("%01%02%03%04%05", resource.Resource);
            Assert.AreEqual("Cover", resource.Title);
        }

        [TestMethod]
        public void BinaryResource_Parse_2()
        {
            MarcRecord record = new MarcRecord();
            record.Fields.Add(_GetField());
            BinaryResource[] resources
                = BinaryResource.Parse(record, 953);
            Assert.AreEqual(1, resources.Length);
            Assert.AreEqual("jpg", resources[0].Kind);
            Assert.AreEqual("%01%02%03%04%05", resources[0].Resource);
            Assert.AreEqual("Cover", resources[0].Title);
        }

        [TestMethod]
        public void BinaryResource_Parse_3()
        {
            MarcRecord record = new MarcRecord();
            record.Fields.Add(_GetField());
            BinaryResource[] resources
                = BinaryResource.Parse(record, 953);
            Assert.AreEqual(1, resources.Length);
            Assert.AreEqual("jpg", resources[0].Kind);
            Assert.AreEqual("%01%02%03%04%05", resources[0].Resource);
            Assert.AreEqual("Cover", resources[0].Title);
        }
    }
}
