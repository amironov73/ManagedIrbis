using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class EmbeddedFieldTest
    {
        private RecordField _GetField()
        {
            RecordField result = new RecordField("461")
                .AddSubField('1', "2001#")
                .AddSubField('a', "Златая цепь")
                .AddSubField('e', "Записки. Повести. Рассказы")
                .AddSubField('f', "Бондарин С. А.")
                .AddSubField('v', "С. 76-132");

            return result;
        }

        [TestMethod]
        public void TestEmbeddedFieldGetEmbeddedFields()
        {
            RecordField field = _GetField();
            RecordField[] embeddedFields = field.GetEmbeddedFields();
            Assert.AreEqual(1, embeddedFields.Length);
        }

        [TestMethod]
        public void TestEmbeddedFieldGetEmbeddedField()
        {
            RecordField field = _GetField();
            
            RecordField[] embeddedFields = field.GetEmbeddedField("200");
            Assert.AreEqual(1, embeddedFields.Length);

            embeddedFields = field.GetEmbeddedField("210");
            Assert.AreEqual(0, embeddedFields.Length);
        }
    }
}
