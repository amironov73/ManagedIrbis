using System;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Marc.Schema;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Marc.Schema
{
    [TestClass]
    public class SubFieldSchemaTest
    {
        private SubFieldSchema _GetSchema()
        {
            return new SubFieldSchema
            {
                Code = 'a',
                Description = "Основное заглавие",
                Display = true,
                Mandatory = true,
                MandatoryText = "Обязательное",
                Name = "Заглавие",
                Repeatable = false,
                RepeatableText = "Не повторяется"
            };
        }

        [TestMethod]
        public void SubFieldSchema_Construction_1()
        {
            SubFieldSchema schema = new SubFieldSchema();
            Assert.AreEqual('\0', schema.Code);
            Assert.AreEqual(" ", schema.CodeString);
            Assert.IsNull(schema.Description);
            Assert.IsFalse(schema.Display);
            Assert.IsFalse(schema.Mandatory);
            Assert.IsNull(schema.MandatoryText);
            Assert.IsNull(schema.Name);
            Assert.IsFalse(schema.Repeatable);
            Assert.IsNull(schema.RepeatableText);
        }

        private void _TestSerialization
            (
                [NotNull] SubFieldSchema first
            )
        {
            byte[] bytes = first.SaveToMemory();
            SubFieldSchema second = bytes.RestoreObjectFromMemory<SubFieldSchema>();
            Assert.AreEqual(first.Code, second.Code);
            Assert.AreEqual(first.CodeString, second.CodeString);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.Display, second.Display);
            Assert.AreEqual(first.Mandatory, second.Mandatory);
            Assert.AreEqual(first.MandatoryText, second.MandatoryText);
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Repeatable, second.Repeatable);
            Assert.AreEqual(first.RepeatableText, second.RepeatableText);
        }

        [TestMethod]
        public void SubFieldSchema_Serialization_1()
        {
            SubFieldSchema schema = new SubFieldSchema();
            _TestSerialization(schema);

            schema.CodeString = "1";
            _TestSerialization(schema);

            schema = _GetSchema();
            _TestSerialization(schema);
        }

        [TestMethod]
        public void SubFieldSchema_CodeString_1()
        {
            SubFieldSchema schema = new SubFieldSchema
            {
                Code = 'a'
            };
            Assert.AreEqual("a", schema.CodeString);

            schema.CodeString = "b";
            Assert.AreEqual('b', schema.Code);
        }

        [TestMethod]
        public void SubFieldSchema_ToXml_1()
        {
            SubFieldSchema schema = new SubFieldSchema();
            Assert.AreEqual("<subfield code=\" \" display=\"false\" mandatory=\"false\" repeatable=\"false\" />", XmlUtility.SerializeShort(schema));

            schema = _GetSchema();
            Assert.AreEqual("<subfield code=\"a\" display=\"true\" mandatory=\"true\" name=\"Заглавие\" repeatable=\"false\"><description>Основное заглавие</description><mandatory-text>Обязательное</mandatory-text><repeatable-text>Не повторяется</repeatable-text></subfield>", XmlUtility.SerializeShort(schema));
        }

        [TestMethod]
        public void SubFieldSchema_ToJson_1()
        {
            SubFieldSchema schema = new SubFieldSchema();
            Assert.AreEqual("{'code':' ','display':false,'mandatory':false,'repeatable':false}", JsonUtility.SerializeShort(schema));

            schema = _GetSchema();
            Assert.AreEqual("{'code':'a','description':'Основное заглавие','display':true,'mandatory':true,'mandatory-text':'Обязательное','name':'Заглавие','repeatable':false,'repeatable-text':'Не повторяется'}", JsonUtility.SerializeShort(schema));
        }

        [TestMethod]
        public void SubFieldSchema_ToString_1()
        {
            SubFieldSchema schema = new SubFieldSchema();
            Assert.AreEqual("' ' : (null)", schema.ToString());

            schema = _GetSchema();
            Assert.AreEqual("'a' : Заглавие", schema.ToString());
        }
    }
}
