using System;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis.Marc.Schema;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Marc.Schema
{
    [TestClass]
    public class RelatedFieldTest
    {
        private RelatedField _GetField()
        {
            return new RelatedField
            {
                Description = "см. также",
                Name = "Параллельное заглавие",
                Tag = 517
            };
        }

        [TestMethod]
        public void RelatedField_Construction_1()
        {
            RelatedField field = new RelatedField();
            Assert.IsNull(field.Description);
            Assert.IsNull(field.Field);
            Assert.IsNull(field.Name);
            Assert.AreEqual(0, field.Tag);
        }

        private void _TestSerialization
            (
                [NotNull] RelatedField first
            )
        {
            byte[] bytes = first.SaveToMemory();
            RelatedField second = bytes.RestoreObjectFromMemory<RelatedField>();
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Tag, second.Tag);
            Assert.IsNull(second.Field);
        }

        [TestMethod]
        public void RelatedField_Serialization_1()
        {
            RelatedField field = new RelatedField();
            _TestSerialization(field);

            field.Field = new FieldSchema();
            _TestSerialization(field);

            field = _GetField();
            _TestSerialization(field);
        }

        [TestMethod]
        public void RelatedField_ToXml_1()
        {
            RelatedField field = new RelatedField();
            Assert.AreEqual("<related tag=\"0\" />", XmlUtility.SerializeShort(field));

            field = _GetField();
            Assert.AreEqual("<related name=\"Параллельное заглавие\" tag=\"517\"><description>см. также</description></related>", XmlUtility.SerializeShort(field));
        }

        [TestMethod]
        public void RelatedField_ToJson_1()
        {
            RelatedField field = new RelatedField();
            Assert.AreEqual("{'tag':0}", JsonUtility.SerializeShort(field));

            field = _GetField();
            Assert.AreEqual("{'description':'см. также','name':'Параллельное заглавие','tag':517}", JsonUtility.SerializeShort(field));
        }

        [TestMethod]
        public void RelatedField_ToString_1()
        {
            RelatedField field = new RelatedField();
            Assert.AreEqual("[0]: (null)", field.ToString());

            field = _GetField();
            Assert.AreEqual("[517]: Параллельное заглавие", field.ToString());
        }
    }
}
